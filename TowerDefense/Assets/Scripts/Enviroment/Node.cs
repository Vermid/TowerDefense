using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    [SerializeField]
    private Color hoverColor;

    [SerializeField]
    private Vector3 positionOffset;
    [Header("This is needed for the Player Turret")]
    public GameObject turret;
    private BuildManager buildManager;

    private Renderer rend;
    private Color startColor;

    void Start()
    {
        buildManager = BuildManager.instance;
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
    }
    //change all the mouse action for Touch actions (android)
    void OnMouseDown()
    {
        if(!buildManager.CanBuild)
            return;

        if (turret != null)
        {
            Debug.Log("We cant Build here");
            return;
        }
        
        buildManager.BuildTurretOn(this);
        }

    void OnMouseEnter()
    {//hver ui element
        if(EventSystem.current.IsPointerOverGameObject())
            return;
        
        if (!buildManager.CanBuild)
            return;
        //change the color from the node
        rend.material.color = hoverColor;
    }

    void OnMouseExit()
    {
        //change the color from the node
        rend.material.color = startColor;
    }

    public Vector3 GetBuildPosition()
    {
        //return the node position + an offset so the turret is not inside the node
        return transform.position + positionOffset;
    }
}
