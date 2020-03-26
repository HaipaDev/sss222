using UnityEngine;
using System.Collections;

/// <summary>
/// Add this script to a camera to make the camera's viewport resize to an assigned rect transform
/// </summary>

public class CamToRectTransformResizer : MonoBehaviour {

	public RectTransform resizeTo;
	public bool resizeOnStart;

	// Use this for initialization
	void Start () {
		if (resizeOnStart) ResizeToRectTransform(resizeTo);
	}

	// resizes the camera this is 
	public void ResizeToRectTransform(RectTransform rectTransform) {
		Rect rectOnScreen = RectTransformToScreenSpace(rectTransform);
		Rect normalizedRect = NormalizeScreenSpaceRect(rectOnScreen);
		GetComponent<Camera>().rect = normalizedRect;
	}

	// normalize a rect that represents something in screen space
	Rect NormalizeScreenSpaceRect(Rect rect) {
		return new Rect(rect.x / Screen.width,
		                rect.y / Screen.height,
		                rect.width / Screen.width,
		                rect.height / Screen.height);
	}

	// returns a rect with the screen space coordinates of a rect transform
	Rect RectTransformToScreenSpace(RectTransform rectTransform) {
		Vector3[] worldCorners = new Vector3[4];
		rectTransform.GetWorldCorners(worldCorners);
		
		Vector3 bottomLeft = worldCorners[0];
		Vector3 topLeft = worldCorners[1];
		Vector3 topRight = worldCorners[2];
		//Vector3 bottomRight = worldCorners[3];

		float width = topRight.x - topLeft.x;
		float height = topLeft.y - bottomLeft.y;

		// use bottom left y cause y axis is inverted
		return new Rect(topLeft.x, bottomLeft.y, width, height);
	}

}
