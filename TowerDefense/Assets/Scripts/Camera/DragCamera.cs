using UnityEngine;

public class DragCamera : MonoBehaviour
{

    private Vector3 screenPoint;
    private Vector3 offset;

    [SerializeField]
    private BoxCollider mapBox;

    private float MIN_X;
    private float MAX_X;
    private float MIN_Y;
    private float MAX_Y;
    private float MIN_Z;
    private float MAX_Z;

    void Start()
    {
        MIN_X = mapBox.bounds.min.x;
        MAX_X = mapBox.bounds.max.x;
        MIN_Z = mapBox.bounds.min.z;
        MAX_Z = mapBox.bounds.max.z;
    }
    private void Update()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, MIN_X, MAX_X),
                                 0,
                                 Mathf.Clamp(transform.position.z, MIN_Z, MAX_Z));
    }


    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    void OnMouseDrag()
    {
        Vector3 currentScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenPoint) + offset;

        float roundedX = Mathf.Round(currentPosition.x);
        float roundedZ = Mathf.Round(currentPosition.z);

        Vector3 customVector = new Vector3(roundedX, transform.position.y, roundedZ);

        transform.position = customVector;
    }

}


