using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollCamera : MonoBehaviour {
// Just add this script to your camera. It doesn't need any configuration.
    #region Inspector
    [SerializeField]
    private float cameraSpeed = 50;

    [SerializeField]
    private Camera cam;
    [SerializeField]
    private BoxCollider mapBox;
    #endregion
    private float MIN_Y;
    private float MAX_Y;
    private Vector2 oldTouchVector;
    private float oldTouchDistance;

    Vector2?[] oldTouchPositions = {
        null,
        null
    };

    void Start()
    {
        MIN_Y = mapBox.bounds.min.y;
        MAX_Y = mapBox.bounds.max.y;
        transform.position += new Vector3(0, MAX_Y, 0);
    }

    void Update()
    {
        Move_Camera_Destktop();

        Move_Camera_Android();

        transform.position = new Vector3(0,Mathf.Clamp(transform.position.y, MIN_Y, MAX_Y),0);
    }

    private void Move_Camera_Destktop()
    {

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            transform.position += new Vector3(0, -cameraSpeed * Time.deltaTime, 0);
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            transform.position += new Vector3(0, cameraSpeed * Time.deltaTime, 0);
        }
    }

    private void Move_Camera_Android()
    {
        // If there are two touches on the device...
        if (Input.touchCount == 2)
        {
            // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            if (Vector3.Distance(touchZero.position, touchOne.position) < Vector3.Distance(touchZeroPrevPos, touchOnePrevPos))
            {
                transform.position += new Vector3(0, cameraSpeed, 0);
            }
            else if (Vector3.Distance(touchZero.position, touchOne.position) > Vector3.Distance(touchZeroPrevPos, touchOnePrevPos))
            {
                transform.position += new Vector3(0, -cameraSpeed, 0);
            }
        }
    }
}
