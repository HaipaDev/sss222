using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour{
    public MeshRenderer bgSprite;
    // Start is called before the first frame update
    void Start()
    {
        float orthosize = bgSprite.bounds.size.x * Screen.height / Screen.width * 0.5f;

        Camera.main.orthographicSize = orthosize;

        /*float aspectRatio = 9f / 16f;
        float cameraHeight = bgSprite.bounds.size.x / aspectRatio;

        Camera.main.orthographicSize = cameraHeight / 2f;*/
    }
}
