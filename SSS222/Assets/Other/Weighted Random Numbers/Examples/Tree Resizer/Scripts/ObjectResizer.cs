using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Changes the size of all objects with a tag using a random number from a random distribution
/// </summary>

public class ObjectResizer : MonoBehaviour {

	// resize all objects that have this script on them
	public string resizeAllWithTag;
	public float baseSize = 1f; // random number from distribution will tell how much percent of this size an object will be
	public RandomDistribution randomDistribution;
	Transform[] allTransforms;

	// Use this for initialization
	void Awake () {
		// check if string is something
		if (string.IsNullOrEmpty(resizeAllWithTag)) Debug.LogError("Tag of resizeable objets is null or empty");

		// fill all the transforms of game objects with the specified tag into an array
		GameObject[] allGameObjects = GameObject.FindGameObjectsWithTag(resizeAllWithTag);
		List<Transform> transformList = new List<Transform>();
		foreach(GameObject obj in allGameObjects) {
			transformList.Add(obj.transform);
		}
		allTransforms = transformList.ToArray();

		// warn if none were found
		if (allTransforms.Length == 0) Debug.LogWarning("No objects found with name '" + resizeAllWithTag + "'");
	}

	void Start () {
		RandomizeSizes();		
	}

	void Update() {
		if (Input.anyKeyDown) RandomizeSizes();
	}

	// rescale each transform of objects to resize to a random number from RandomDistribution
	public void RandomizeSizes() {
		if (randomDistribution == null) Debug.LogError("No RandomDistribution assigned to ObjectResizer!");

		foreach (Transform t in allTransforms) {
			float size = baseSize;
			float r = randomDistribution.RandomFloat();
			r /= 100f; // r comes in percent
			size *= r;
			t.localScale = new Vector3 (size, size, size);
		}
	}
}
