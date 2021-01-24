using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(RandomDistribution))]
public class RandomDistributionEditor : Editor 
{
	// the target script
	RandomDistribution targetScript;
	SerializedObject serializedScript;

	// editable properties
	SerializedProperty distributionCurve;
	SerializedProperty randomizeMode;
	SerializedProperty prebakeResolution;

	// GUIContents to show to label stuff
	GUIContent contentRandomizeMode, contentPrebakeResolution;

	const int minPrebakeResolution = 2;
	const int maxPrebakeResolution = 1000;

	void OnEnable() {

		// get target script
		targetScript = (RandomDistribution)target;
		serializedScript = new SerializedObject(targetScript);

		// get serialized properties
		distributionCurve = serializedScript.FindProperty("distributionCurve");
		randomizeMode = serializedScript.FindProperty("randomizeMode");
		prebakeResolution = serializedScript.FindProperty("prebakeResolution");

		// instantiate the gui contents only once
		contentRandomizeMode = new GUIContent("Algorithm", "Pregenerate will generate a set of numbers on initialization. Brute Force does not do that, but may have worse performance.");
		contentPrebakeResolution = new GUIContent("Pregeneration Resolution");


	}

	public override void OnInspectorGUI()
	{

		//DrawDefaultInspector();

		// Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
		serializedScript.Update ();

		// draw the curve
		EmptyLine();
		EditorGUILayout.PropertyField(distributionCurve, new GUIContent("Distribution Curve"));

		// empty line
		EmptyLine();

		// draw options foldout
		targetScript.showOptions = EditorGUILayout.Foldout(targetScript.showOptions, "Options");
		// draw options
		if (targetScript.showOptions) {
			EditorGUI.indentLevel++;
			// draw randomize mode property
			EditorGUILayout.PropertyField(randomizeMode, contentRandomizeMode);

			// draw prebaking resolution
			if (targetScript.randomizeMode == RandomDistribution.RandomizeMode.Pregenerate) {
				EditorGUILayout.PropertyField(prebakeResolution, contentPrebakeResolution);
			}
			EditorGUI.indentLevel--;

		}

		// empty line
		EmptyLine();
		
		// Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
		serializedScript.ApplyModifiedProperties ();

		// clamp prebake resolution
		targetScript.prebakeResolution = Mathf.Clamp(targetScript.prebakeResolution, minPrebakeResolution, maxPrebakeResolution);
	}

	// helper for empty line
	void EmptyLine() {
		EditorGUILayout.LabelField("");
	}
}