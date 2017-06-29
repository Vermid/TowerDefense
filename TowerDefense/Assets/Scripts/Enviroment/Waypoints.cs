using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public static Transform[] GroundPoints;
    public static Transform[] AirPoints;

    void Awake()
    {
        GameObject groundUnit = GameObject.FindGameObjectWithTag("Ground");
        GameObject airUnit = GameObject.FindGameObjectWithTag("Air");

        // counts all children that he have
        GroundPoints = new Transform[groundUnit.transform.childCount];
        for (int i = 0; i < GroundPoints.Length; i++)
        {
            //save the children into the points tranform []
            GroundPoints[i] = groundUnit.transform.GetChild(i);
        }

        // counts all children that he have
        AirPoints = new Transform[airUnit.transform.childCount];
        for (int i = 0; i < AirPoints.Length; i++)
        {
            //save the children into the points tranform []
            AirPoints[i] = airUnit.transform.GetChild(i);
        }

    }
}
