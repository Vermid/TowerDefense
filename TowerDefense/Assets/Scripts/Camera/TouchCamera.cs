// Just add this script to your camera. It doesn't need any configuration.

using UnityEngine;

//TODO: Rebuild this and understand why this works so well
public class TouchCamera : MonoBehaviour
{
    Vector2?[] oldTouchPositions = {
        null,
        null
    };
    Vector2 oldTouchVector;
    float oldTouchDistance;


    //mapX, mapY is size of background image

    public GameObject map;
    //private BoxCollider mapCollider;
    public float touchSpeed = 1;
    public float perspectiveZoomSpeed = 0.5f;        // The rate of change of the field of view in perspective mode.
    public float orthoZoomSpeed = 0.5f;        // The rate of change of the orthographic size in orthographic mode.

    private Camera cam;
    void Start()
    {
        cam = GetComponent<Camera>();
      //  mapCollider = map.GetComponentInChildren<BoxCollider>();
    }


    public float panSpeed = 30f;
    public float panBorderThickness = 10f;

    public float scrollSpeed = 5f;
    public float minY = 10f;
    public float maxY = 80f;
    void Update()
    {
        //if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness)
        //{
        //    transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.World);
        //}
        //if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness)
        //{
        //    transform.Translate(Vector3.back * panSpeed * Time.deltaTime, Space.World);
        //}
        //if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness)
        //{
        //    transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World);
        //}
        //if (Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness)
        //{
        //    transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World);
        //}
        //if (moveVertical != 0 && moveHorizontal != 0)
        //{
        //    mapMaxX = mapCollider.bounds.max.x;
        //    mapMaxY = mapCollider.bounds.max.y;
        //    mapMinX = mapCollider.bounds.min.x;
        //    mapMinY = mapCollider.bounds.min.y;

        //    Vector3 v3 = transform.position;
        //    v3.x = Mathf.Clamp(v3.x, mapMinX, mapMaxX);
        //    v3.y = Mathf.Clamp(v3.y, mapMinY, mapMaxY);
        //    transform.position = v3;

        //    if (transform.position.x > mapMinX && transform.position.x < mapMaxX && transform.position.y > mapMinY &&
        //        transform.position.y < mapMaxY)
        //    {
        //        transform.Translate(newMovement, Space.World);
        //    }
        //}

                Move_Camera();

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
                //TouchCameraMove(oldTouchVector, newTouchPosition);

                //if (newTouchPosition.y >= Screen.height - panBorderThickness)
                //{
                //    TouchCameraMove(Vector3.forward, newTouchPosition);
                //    //transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.World);
                //}
                //if (newTouchPosition.y <= panBorderThickness)
                //{
                //    TouchCameraMove(Vector3.back, newTouchPosition);
                //    //transform.Translate(Vector3.back * panSpeed * Time.deltaTime, Space.World);
                //}
                //if (newTouchPosition.x >= Screen.width - panBorderThickness)
                //{
                //    TouchCameraMove(Vector3.right, newTouchPosition);
                //    //                    transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World);
                //}
                //if (newTouchPosition.x <= panBorderThickness)
                //{
                //    TouchCameraMove(Vector3.left, newTouchPosition);
                //    //                  transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World);
                //}
                var element = transform.TransformDirection(
                        (Vector3)
                            ((oldTouchPositions[0] - newTouchPosition) * cam.orthographicSize / cam.pixelHeight * 2f *
                             touchSpeed));
                if (element.x < 112)
                    transform.position += element;

                oldTouchPositions[0] = newTouchPosition;
            }
        }

        // If there are two touches on the device...
        if (Input.touchCount == 2)
        {
            // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            // If the camera is orthographic...
            if (cam.orthographic)
            {
                // ... change the orthographic size based on the change in distance between the touches.
                cam.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;

                // Make sure the orthographic size never drops below zero.
                cam.orthographicSize = Mathf.Max(cam.orthographicSize, 0.1f);
            }
            else
            {
                // Otherwise change the field of view based on the change in distance between the touches.
                cam.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;

                // Clamp the field of view to make sure it's between 10 and 30.
                cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, 10, 30);
            }
        }
    }

    private void TouchCameraMove(Vector2 oldPos, Vector2 currentPos)
    {
        Debug.Log(currentPos.x + " " + currentPos.y);

        float xvalue = Mathf.Abs(oldPos.x - currentPos.x);
        float yvalue = Mathf.Abs(oldPos.y - currentPos.y);
        Vector2 dir = new Vector2(xvalue, yvalue);
        //   if(currentPos.x >-32 && currentPos.x < 112)
        transform.Translate(dir * panSpeed * Time.deltaTime, Space.World);

        if (currentPos.x < 1200)
        {
            Debug.Log("TOCUH");
        }

        //Mathf.Clamp(element.x, -32.5651F + (camera.orthographicSize + 4), 112.3136F - (camera.orthographicSize + 4)),
        //Mathf.Clamp(element.z, -10, 100),
        //Mathf.Clamp(element.y, 108, 122));

    }
    public void Move_Camera()
    {
        //mapMaxX = mapCollider.bounds.max.x;
        //mapMaxY = mapCollider.bounds.max.y;
        //mapMinX = mapCollider.bounds.min.x;
        //mapMinY = mapCollider.bounds.min.y;

        transform.position += new Vector3(Input.GetAxisRaw("Horizontal") * 5, 0, Input.GetAxisRaw("Vertical") * 5);
        //transform.position = new Vector3(
        //     Mathf.Clamp(transform.position.x, -32.5651F + (camera.orthographicSize + 4), 112.3136F - (camera.orthographicSize + 4)),
        //    Mathf.Clamp(transform.position.y, -10, 100),
        //    Mathf.Clamp(transform.position.z, 108, 122));
        //87.778F , -0.626F ));
    }
}

