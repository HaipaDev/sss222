using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Visualizes statistics (how often a number was returned)
/// </summary>

public class DistributionVisualizer : MonoBehaviour {

	// public settings
	public Color barColor;
	public int useThisManyNumbers;
	public RandomDistribution visualizeDistribution;
	public UnityEngine.UI.Text title;

	// width for the image that'll be generated in pixels.
	// height comes from highest bar in graphics
	public int imageWidth;

	// assign some numbers to those
	UnityEngine.UI.Text[] numberLabels;

	// Use this for initialization
	void Awake () {
		Initialize();
	}
	void Start () {		
		TestDistribution ();
	}
	
	void Initialize() {
		// dirty find distribution to visualize if none is set
		if (visualizeDistribution == null) visualizeDistribution = FindObjectOfType<RandomDistribution>();
		numberLabels = GetComponentsInChildren<Text>();
	}

	// do a test run on the distribution curve
	public void TestDistribution() {
		RawImage setImage = FindObjectOfType<RawImage>();

		// set the title text
		title.text = "Distribution of " + useThisManyNumbers.ToString() + " Random Numbers";

		// old version
		// find out how many "stacks" of numbers are there, create an array for them
//		int highestPossibleX =  Mathf.RoundToInt(visualizeDistribution.CurveRect.MaxX);
//		int lowestPossibleX = Mathf.RoundToInt(visualizeDistribution.CurveRect.MinX);
//		int slotsNeeded = (highestPossibleX - lowestPossibleX) + 1; // zero indexing
//		int[] numbers = new int[slotsNeeded];
//		for (int i = 0; i < useThisManyNumbers; i++) {
//			int r = visualizeDistribution.RandomInt();
//			r -= lowestPossibleX; // move lowest point of curve to zero
//			numbers[r]++;
//		}

		// fill array with ints
		int highestPossibleX =  Mathf.RoundToInt(visualizeDistribution.CurveRect.MaxX);
		int lowestPossibleX = Mathf.RoundToInt(visualizeDistribution.CurveRect.MinX);
		int xRange = highestPossibleX - lowestPossibleX;

		int[] numbers = new int[imageWidth];
		for (int i = 0; i < useThisManyNumbers; i++) {
			float randomFloat = visualizeDistribution.RandomFloat();
			randomFloat -= lowestPossibleX; // move lowest point of curve to zero
			randomFloat /= xRange; // normalize
			randomFloat *= (imageWidth - 1); // then scale to image width. -1 because zero indexing
			int randomInt = Mathf.FloorToInt(randomFloat); // make int

			if (randomInt < imageWidth)
				numbers[randomInt]++; // add to stats array
//			else
//				Debug.Log ("Dropped number " + randomFloat);
		}

		// label the axes
		UpdateNumberLabels(lowestPossibleX, highestPossibleX);

		//visualizer.VisualizeStats(numbers, changeCamSize);
		Texture2D tex = VisualizeStats(numbers);
		setImage.texture = tex;
	}

	// put labels, at least on the x axis for now
	void UpdateNumberLabels(int minValue, int maxValue) {
		int thirdOfRange = (maxValue - minValue) / 3;
		int mid1 = minValue + thirdOfRange;
		int mid2 = minValue + thirdOfRange + thirdOfRange;

		numberLabels[0].text = minValue.ToString();
		numberLabels[1].text = mid1.ToString();
		numberLabels[2].text = mid2.ToString();
		numberLabels[3].text = maxValue.ToString();

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.anyKeyDown) TestDistribution (); // test again when pressing sth
	}


	// create an image that visualizes the stats
	Texture2D VisualizeStats(int[] numbers) {
		int width = numbers.Length;
		int height = Mathf.Max(numbers);
		Texture2D tex = new Texture2D(width, height);
		
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				if (y < numbers[x]) tex.SetPixel(x, y, barColor);
				else tex.SetPixel(x, y, Color.clear);
			}
		}
		tex.Apply();
		// apply some settings to make tex look better
		tex.filterMode = FilterMode.Point;
		tex.wrapMode = TextureWrapMode.Clamp;
		return tex;
	}
}
