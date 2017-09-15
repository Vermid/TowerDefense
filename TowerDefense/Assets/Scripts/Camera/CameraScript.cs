using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    #region Inspector
    [SerializeField]
    private float panBorderThickness = 50;

    [SerializeField]
    private float cameraSpeed = 50;

    [SerializeField]
    private BoxCollider mapBox;
    #endregion

    #region Privates
    private Vector3 screenPoint;
    private Vector3 offset;
    private float MIN_X;
    private float MAX_X;
    private float MIN_Y;
    private float MAX_Y;
    private float MIN_Z;
    private float MAX_Z;
    private Vector2 oldTouchVector;
    private float oldTouchDistance;
    #endregion

    public static CameraScript current;

    Vector2?[] oldTouchPositions = {
        null,
        null
    };

    private void Awake()
    {
        current = this;
    }

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

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, MIN_X, MAX_X), 0, Mathf.Clamp(transform.position.z, MIN_Z, MAX_Z));
    }

    void OnMouseDown()
    {
        LastPosition();
    }

    public void LastPosition()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    void OnMouseDrag()
    {
        DragObject();
    }

    public void DragObject()
    {
        Vector3 currentScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenPoint) + offset;

        float roundedX = Mathf.Round(currentPosition.x);
        float roundedZ = Mathf.Round(currentPosition.z);

        Vector3 customVector = new Vector3(roundedX, transform.position.y, roundedZ);

        transform.position = customVector;
    }

    private void Move_Camera_Destktop()
    {
        //reverse
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.back * cameraSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.forward * cameraSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.left * cameraSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.right * cameraSpeed * Time.deltaTime, Space.World);
        }
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

                if (newTouchPosition.y >= Screen.height - panBorderThickness)
                {
                    TouchCameraMove(Vector3.forward, transform.position);
                }
                if (newTouchPosition.y <= panBorderThickness)
                {
                    TouchCameraMove(Vector3.back, transform.position);
                }
                if (newTouchPosition.x >= Screen.width - panBorderThickness)
                {
                    TouchCameraMove(Vector3.right, transform.position);
                }
                if (newTouchPosition.x <= panBorderThickness)
                {
                    TouchCameraMove(Vector3.left, transform.position);
                }
                //   oldTouchPositions[0] = newTouchPosition;
            }
        }
    }

    private void TouchCameraMove(Vector2 oldPos, Vector3 currentPos)
    {
        float xvalue = Mathf.Abs(oldPos.x - currentPos.x);
        float yvalue = Mathf.Abs(oldPos.y - currentPos.y);

        Vector2 dir = new Vector2(xvalue, yvalue);
        transform.Translate(oldPos * cameraSpeed * Time.deltaTime, Space.World);
    }
}
