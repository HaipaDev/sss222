using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Contains the "dimensions" of an animation curve as a rectangle that covers the whole curve.
/// Can also provide information on the highest and lowest points of the curve.
/// </summary>

[System.Serializable]
public class AnimCurveRect {

	// accessors for highest and lowest x and y values of the curve
	[SerializeField]
	float minX;
	public float MinX {	get { return minX; } }
	[SerializeField]
	float maxX;
	public float MaxX {	get { return maxX; } }
	[SerializeField]
	float minY;
	public float MinY {	get { return minY; } }
	[SerializeField]
	float maxY;
	public float MaxY {	get { return maxY; } }

	// accessor for the rectangle that covers the whole curve
	[SerializeField]
	Rect rectangle;
	public Rect Rectangle {	get { return rectangle;	} }

	// constructor, find 
	// extremaFindingPrexision tells how many recursions to run when approximating highest / lowest point of the curve
	public AnimCurveRect(AnimationCurve curve, int extremaFindingRecursions = 20) {
		Keyframe[] keys = curve.keys;

		// make sure the curve has some keys
		if (keys.Length == 0) Debug.LogError("Trying to create AnimCurveRect for AnimationCurve that has no keyframes");

		// find min and max x values, those are trivial
		minX = keys[0].time;
		maxX = keys[keys.Length - 1].time;


		// finding highest and lowest y values
		// init lists for potential candidates
		List<float> potentiallyHighest = new List<float>();
		List<float> potentiallyLowest = new List<float>();

		// go through all keyframes
		int i = 0;
		int maxI = keys.Length - 2;
		foreach(Keyframe k in keys) {

			// the keyframes themselves couldbe highest or lowest points, who knows.
			potentiallyHighest.Add(k.value);
			potentiallyLowest.Add (k.value);

			// if this is not the last keyframe
			if (i <= maxI) {
				// approximate maxima and minima between this keyframe and the next
				// Could be optimized still by only running the approximation recursions if needed, can tell by intangent and outtangent
				float localMaximum = ApproximateLocalMaximum(curve, k, keys[i+1], extremaFindingRecursions);
				float localMinimum = ApproximateLocalMinimum(curve, k, keys[i+1], extremaFindingRecursions);
				potentiallyHighest.Add(localMaximum);
				potentiallyLowest.Add(localMinimum);
			}
			i++;
		}

		// now find lowest and highest y values from the potentials
		maxY = Mathf.Max (potentiallyHighest.ToArray());
		minY = Mathf.Min (potentiallyLowest.ToArray());
		//Debug.Log ("Maximum: " + maxY + " Minimum: " + minY);

		// use the min and max info to calculate the rectangle
		rectangle = new Rect(minX, maxY, (maxX - minX), (maxY - minY));
		//Debug.Log (rectangle);
	}

	// approximate the highest point between two keyframes and returns its y coordinate
	float ApproximateLocalMaximum(AnimationCurve curve, Keyframe key1, Keyframe key2, int recursions) {
		return ApproximateLocalExtremum(curve, key1.time, key2.time, recursions, true);
	}
	// approximate the lowest point between two keyframes and returns its y coordinate
	float ApproximateLocalMinimum(AnimationCurve curve, Keyframe key1, Keyframe key2, int recursions) {
		return ApproximateLocalExtremum(curve, key1.time, key2.time, recursions, false);
	}

	// Approximates the y coordinate of the highest or lowest point on an Animationcurve between two keyframes, specifiy whether max or min is needed
	float ApproximateLocalExtremum(AnimationCurve curve, float lowX, float highX, int recursions, bool lookingForMaximum) {

		//Debug.Log ("Going from low x " + lowX + " high x " + highX);
		float result;

		// calc midpoint between the two x, then two more midpoints between those three
		float midX = Midpoint(lowX, highX);
		float center1 = Midpoint(lowX, midX);
		float center2 = Midpoint(midX, highX);

		// evaluate which of the two "center" midpoints is better (higher or lower for maximun or minimum)
		float value1 = curve.Evaluate(center1);
		float value2 = curve.Evaluate(center2);
		if ((value1 > value2) == lookingForMaximum) { // if value 1 is better
			result = value1;
			highX = midX;
			//Debug.Log ("Value 1 better, new result " + result);
		}
		else { // if value 2 is better
			result = value2;
			lowX = midX;
			//Debug.Log ("Value 2 better, new result " + result);
		}

		// dec number of recursions left and call method another time if needed
		recursions--;
		if (recursions > 0) result = ApproximateLocalExtremum (curve, lowX, highX, recursions, lookingForMaximum);

		// return
		return result;
	}

	// returns the midpoint of two points on the x-axis
	float Midpoint(float lowX, float highX) {
		return lowX + ((highX - lowX) / 2f);;
	}
}
