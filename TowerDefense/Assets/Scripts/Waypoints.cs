using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public static Transform[] Points;

    void Awake()
    {
        // cunts all children that he have
        Points = new Transform[transform.childCount];
        for (int i = 0; i < Points.Length; i++)
        {
            //save the children into the points tranform []
            Points[i] = transform.GetChild(i);
        }
    }
}
