using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
//TODO: check if you should use a pool for the turrets or not

public class BuildManager : MonoBehaviour
{
    #region Inspector
    [SerializeField]
    public GameObject buildEffect;

    public static BuildManager instance;
    public NodeUI nodeUi;
    public GameObject sellEffect;
    #endregion
    private TurretBlueprint turretToBuild;
    private Node selectedNode;


    void Awake()
    {
        if (instance != null)
            return;
        instance = this;
    }

    public bool CanBuild
    {
        //if turrettobuild is null there is no turret on this position
        get { return turretToBuild != null; }
    }

    public bool HasMoney
    {
        //if turrettobuild is null there is no turret on this position
        get { return PlayerStarts.money >= turretToBuild.cost; }
    }


    public void SelectNode(Node node)
    {
        if (selectedNode == node)
        {
            DeselectNode();
            return;
        }

        selectedNode = node;
        turretToBuild = null;

        nodeUi.SetTarget(node);
    }

    public void DeselectNode()
    {
        selectedNode = null;
        nodeUi.Hide();
    }

    public void SelectTurretToBuild(TurretBlueprint turret)
    {
        //set the turret to buils
        turretToBuild = turret;
        DeselectNode();
    }
    public TurretBlueprint GetTurretToBuild()
    {
        return turretToBuild;
    }
}
