using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraAspectController : MonoBehaviour{
    public float targetAspectRatio = 16f / 9f;
    //public bool fitHorizontally = true;
    //public bool fitVertically = true;
    Camera _camera;
    void Start(){
        _camera = GetComponent<Camera>();
        //Debug.Log(Screen.width + ":" + Screen.height);
        //Debug.Log("Aspect ratio: " + ((float)Screen.width / (float)Screen.height));
        //Debug.Log("Orthographic size: " + _camera.orthographicSize);
        UpdateAspectRatio();
        //Debug.Log("Orthographic size: " + _camera.orthographicSize);
        //Debug.Log("Rect: " + _camera.rect);
    }
    
    ///Perfect calculation of Orthographic Size
    void UpdateAspectRatio(){
        float screenAspectRatio = ((float)Screen.width / (float)Screen.height);
        float orthographicSize = _camera.orthographicSize;

        if (screenAspectRatio < targetAspectRatio) {
            float aspectRatioRatio = screenAspectRatio / targetAspectRatio;
            orthographicSize = _camera.orthographicSize / aspectRatioRatio;
            orthographicSize = _camera.orthographicSize / aspectRatioRatio;
        }

        _camera.orthographicSize = orthographicSize;
    }
    /*void UpdateAspectRatio(){
        float screenAspectRatio = ((float)Screen.width / (float)Screen.height);
        float aspectRatioRatio = screenAspectRatio / targetAspectRatio;

        float orthographicSize = _camera.orthographicSize;

        if (fitHorizontally && !fitVertically){
            orthographicSize = _camera.orthographicSize / aspectRatioRatio;
        }else if (fitVertically && !fitHorizontally){
            orthographicSize = _camera.orthographicSize / aspectRatioRatio;
        }else if (fitHorizontally && fitVertically){
            if(screenAspectRatio > targetAspectRatio){
                orthographicSize = _camera.orthographicSize / aspectRatioRatio;
            }else{
                orthographicSize = _camera.orthographicSize / aspectRatioRatio;
            }
        }

        _camera.orthographicSize = orthographicSize;
    }*/
    ///Attempt at making it cut off the excessive vertical space
    /*void UpdateAspectRatio(){
        float screenAspectRatio = ((float)Screen.width / (float)Screen.height);
        float aspectRatioRatio = screenAspectRatio / targetAspectRatio;

        float orthographicSize = _camera.orthographicSize;

        if (fitHorizontally && !fitVertically){
            orthographicSize = _camera.orthographicSize / aspectRatioRatio;
        }else if (fitVertically && !fitHorizontally){
            orthographicSize = _camera.orthographicSize / aspectRatioRatio;
        }else if (fitHorizontally && fitVertically){
            if(screenAspectRatio > targetAspectRatio){
                orthographicSize = _camera.orthographicSize / aspectRatioRatio;
            }else{
                orthographicSize = _camera.orthographicSize / aspectRatioRatio;
            }
        }

        _camera.orthographicSize = orthographicSize;

        float excessHeight = orthographicSize - (orthographicSize / aspectRatioRatio);
        float excessWidth = excessHeight * targetAspectRatio;
        float viewportHeight = 1f - (2f * excessHeight);
        float viewportWidth = 1f - (2f * excessWidth);
        float viewportX = excessWidth;
        float viewportY = excessHeight;

        _camera.rect = new Rect(viewportX, viewportY, viewportWidth, viewportHeight);
    }*/
}