namespace CustomRandomDistributions {

	using UnityEngine;
	using System.Collections;
	using System.Collections.Generic;


	/// <summary>
	/// Takes care of "prebaking" (pregenerating) a set of random numbers
	/// </summary>

	[System.Serializable]
	public class NumberBakery {
		
		// pre-baked random numbers
		[SerializeField]
		List<float> prebakedFloats;
		[SerializeField]
		List<int> prebakedInts;

		// constructor
		public NumberBakery(AnimationCurve curve, AnimCurveRect curveRect, int resolution) {

			// prebake lists of numbers
			PrebakeFloats(curve, curveRect, resolution);
			PrebakeInts(curve, curveRect, resolution);
			
			// give error messages if something failed
			if (prebakedFloats.Count < 1) Debug.LogError("Could not prebake enough floats (" + prebakedFloats.Count + ") from distribution curve.");
			if (prebakedInts.Count < 1) Debug.LogError("Could not prebake enough ints (" + prebakedInts.Count + ") from distribution curve.");
//			Debug.Log ("Pre-baked " + prebakedFloats.Count + " floats.");
//			Debug.Log ("Pre-baked " + prebakedInts.Count + " ints.");
		}

		// prebake the ints
		void PrebakeInts(AnimationCurve curve, AnimCurveRect curveRect, int resolution) {
			prebakedInts = new List<int>();

			//get min and max x values and range
			int minX = Mathf.RoundToInt(curveRect.MinX);
			int maxX = Mathf.RoundToInt(curveRect.MaxX);
			int xRange = maxX - minX;
			// step for stepping from x to x
			int xStep = Mathf.RoundToInt(xRange / resolution);
			if (xStep < 1) xStep = 1; // don't allow zero steps...
			// vertical step / upwards
			float yStep = curveRect.MaxY / resolution; // y step can be float as it's not used directly

			// fill int list
			// for loop that goes through all relevant x coords determined by step
			float currentY = 0;
			for (int currentX = minX; currentX <= maxX; currentX += xStep) {
				while (currentY < curve.Evaluate(currentX)) {
					prebakedInts.Add(currentX);
					currentY += yStep;
				}
				currentY = 0;
			}

			// sometimes, no ints can be generated due to rounding. fix that by adding the closest suitable int value(s)
			if (prebakedInts.Count == 0) {
				if (minX == maxX) prebakedInts.Add(minX); // if minX and maxX are the same when rounded, just always return that value
				else { // otherwise, the curve will have some unusual shape that is especially nasty to ints (and probably not intended to return ints). try to add at least some int.
					// could throw a warning here, but it would get annoying / become spam if curve is not intended for ints
					// instead, try minX, maxX and avgX if they could be added
					if (curve.Evaluate(minX) >= 0) prebakedInts.Add(minX);
					if (curve.Evaluate(maxX) >= 0) prebakedInts.Add(maxX);
					int avg = Mathf.RoundToInt((curveRect.MinX + curveRect.MaxX) / 2f);
					if (curve.Evaluate(avg) >= 0) prebakedInts.Add(avg);
					// if nothing worked, just put the average there.
					if (prebakedInts.Count == 0) prebakedInts.Add(avg);
				}
			}

		}

		// prebake the float numbers
		void PrebakeFloats(AnimationCurve curve, AnimCurveRect curveRect, int resolution) {
			prebakedFloats = new List<float>();

			//get min and max x values and range
			float minX = curveRect.MinX;
			float maxX = curveRect.MaxX;
			float xRange = maxX - minX;
			// step for stepping from x to x
			float xStep = xRange / resolution;
			// vertical step / upwards
			float yStep = curveRect.MaxY / resolution;
			
			// for floats
			// for loop that goes through all relevant x coords determined by step
			float currentY = 0;
			for (float currentX = minX; currentX <= maxX; currentX += xStep) {
				while (currentY < curve.Evaluate(currentX)) {
					prebakedFloats.Add(currentX);
					currentY += yStep;
				}
				currentY = 0;
			}
		}

		public float RandomFloat() {
			int randomIndex = Random.Range (0, prebakedFloats.Count);
			//Debug.Log ("Picked number with index " + randomIndex + " from " + prebakedFloats.Count + " float numbers.");
			return prebakedFloats[randomIndex];
		}

		public int RandomInt() {
			#if UNITY_EDITOR
				if (prebakedInts.Count == 0) Debug.LogError ("No pre-baked ints available");
			#endif

			int randomIndex = Random.Range (0, prebakedInts.Count);
			//Debug.Log ("Picked number with index " + randomIndex + " from " + prebakedInts.Count + " int numbers.");
			return prebakedInts[randomIndex];
		}
	}
}