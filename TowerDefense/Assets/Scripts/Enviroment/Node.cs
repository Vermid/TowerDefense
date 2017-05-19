using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    [SerializeField]
    private Color hoverColor;
    [SerializeField]
    private Color notEnoughMoneyColor;

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
        //if shop ui is over a node you cant build you will click on the shopUi turret 
        if (EventSystem.current.IsPointerOverGameObject())
            return;


        if (turret != null)
        {
            buildManager.SelectNode(this);
            return;
        }

        if (!buildManager.CanBuild)
            return;

        buildManager.BuildTurretOn(this);
    }

    void OnMouseEnter()
    {
        //if shop ui is over a node you cant build you will click on the shopUi turret 
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (!buildManager.CanBuild)
            return;
        if (buildManager.HasMoney)
        {
            //change the color from the node
            rend.material.color = hoverColor;
        }
        else
        {
            rend.material.color = notEnoughMoneyColor;
        }
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
