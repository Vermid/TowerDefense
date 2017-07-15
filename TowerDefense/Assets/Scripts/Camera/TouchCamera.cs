// Just add this script to your camera. It doesn't need any configuration.

using UnityEngine;

//TODO: Rebuild this and understand why this works so well
public class TouchCamera : MonoBehaviour
{
    #region Inspector
    [SerializeField]
    private float panBorderThickness = 10f;

    [SerializeField]
    private float cameraSpeed = 50;

    [SerializeField]
    private BoxCollider mapBox;
    #endregion

    private float MIN_X;
    private float MAX_X;
    private float MIN_Y;
    private float MAX_Y;
    private float MIN_Z;
    private float MAX_Z;
    private Vector2 oldTouchVector;
    private float oldTouchDistance;

    Vector2?[] oldTouchPositions = {
        null,
        null
    };

    void Start()
    {
        MIN_X = mapBox.bounds.min.x;
        MAX_X = mapBox.bounds.max.x;
        MIN_Y = mapBox.bounds.min.y;
        MAX_Y = mapBox.bounds.max.y;
        MIN_Z = mapBox.bounds.min.z;
        MAX_Z = mapBox.bounds.max.z;
    }

    void Update()
    {
        Move_Camera_Destktop();

        Move_Camera_Android();

        //transform.position = new Vector3(Mathf.Clamp(transform.position.x, MIN_X, MAX_X),
        //                                 Mathf.Clamp(transform.position.y, MIN_Y, MAX_Y), 0);
        //                               //  Mathf.Clamp(transform.position.z, MIN_Z, MAX_Z));
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, MIN_X, MAX_X),0,Mathf.Clamp(transform.position.z, MIN_Z, MAX_Z));
        //  Mathf.Clamp(transform.position.z, MIN_Z, MAX_Z));

    }

    private void Move_Camera_Destktop()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * cameraSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * cameraSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * cameraSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * cameraSpeed * Time.deltaTime, Space.World);
        }

        //if (Input.GetAxis("Mouse ScrollWheel") > 0)
        //{
        //    transform.position += new Vector3(0, -cameraSpeed * Time.deltaTime, 0);
        //}

        //if (Input.GetAxis("Mouse ScrollWheel") < 0)
        //{
        //    transform.position += new Vector3(0, cameraSpeed * Time.deltaTime, 0);
        //}
    }

    private void Move_Camera_Android()
    {
        if (Input.touchCount == 0)
        {
            oldTouchPositions[0] = null;
            oldTouchPositions[1] = null;
        }
        if (Input.touchCount == 1)
        {
            if (oldTouchPositions[0] == null || oldTouchPositions[1] != null)
            {
                oldTouchPositions[0] = Input.GetTouch(0).position;
                oldTouchPositions[1] = null;
            }
            else
            {
                Vector2 newTouchPosition = Input.GetTouch(0).position;
                TouchCameraMove(oldTouchVector, newTouchPosition);

                if (newTouchPosition.y >= Screen.height - panBorderThickness)
                {
                    TouchCameraMove(Vector3.forward, newTouchPosition);
                }
                if (newTouchPosition.y <= panBorderThickness)
                {
                    TouchCameraMove(Vector3.back, newTouchPosition);
                }
                if (newTouchPosition.x >= Screen.width - panBorderThickness)
                {
                    TouchCameraMove(Vector3.right, newTouchPosition);
                }
                if (newTouchPosition.x <= panBorderThickness)
                {
                    TouchCameraMove(Vector3.left, newTouchPosition);
                }

                oldTouchPositions[0] = newTouchPosition;
            }
        }

        //// If there are two touches on the device...
        //if (Input.touchCount == 2)
        //{
        //    // Store both touches.
        //    Touch touchZero = Input.GetTouch(0);
        //    Touch touchOne = Input.GetTouch(1);

        //    // Find the position in the previous frame of each touch.
        //    Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
        //    Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

        //    if (Vector3.Distance(touchZero.position, touchOne.position) < Vector3.Distance(touchZeroPrevPos, touchOnePrevPos))
        //    {
        //        transform.position += new Vector3(0, cameraSpeed, 0);
        //    }
        //    else if (Vector3.Distance(touchZero.position, touchOne.position) > Vector3.Distance(touchZeroPrevPos, touchOnePrevPos))
        //    {
        //        transform.position += new Vector3(0, -cameraSpeed, 0);
        //    }
        //}
    }

    private void TouchCameraMove(Vector2 oldPos, Vector2 currentPos)
    {
        float xvalue = Mathf.Abs(oldPos.x - currentPos.x);
        float yvalue = Mathf.Abs(oldPos.y - currentPos.y);

        Vector2 dir = new Vector2(xvalue, yvalue);
        transform.Translate(dir * cameraSpeed * Time.deltaTime, Space.World);
    }
}
