using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//TODO: check if you should use a pool for the turrets or not

public class BuildManager : MonoBehaviour
{
    private TurrretBluePrint turretToBuild;
    private Node selectedNode;

    [SerializeField]
    public GameObject buildEffect;

    public static BuildManager instance;
    public NodeUI nodeUi;
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


    public void BuildTurretOn(Node node)
    {
        //if the player has the money to buy the turret
        if (PlayerStarts.money < turretToBuild.cost)
        {
            Debug.Log("Not enough money");
            return;
        }

        PlayerStarts.money -= turretToBuild.cost;
        //should we use the pool here? 
        GameObject turret = (GameObject)Instantiate(turretToBuild.prefab, node.GetBuildPosition(), Quaternion.identity);

        node.turret = turret;

        Debug.Log("Turret build!! Money left: " + PlayerStarts.money);

        GameObject gobj = ObjectPool_Zaim.current.GetPoolObject(buildEffect.name);

        if (gobj == null)
            return;

        gobj.transform.position = node.transform.position;
        gobj.transform.rotation = node.transform.rotation;
        gobj.SetActive(true);

        turretToBuild = null;
        selectedNode = null; //may delete this
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

    public void SelectTurretToBuild(TurrretBluePrint turret)
    {
        //set the turret to buils
        turretToBuild = turret;
        DeselectNode();
    }
}
