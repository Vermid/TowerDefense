using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//TODO: check if you should use a pool for the turrets or not

public class BuildManager : MonoBehaviour
{
    private TurrretBluePrint turretToBuild;
    [SerializeField]
    public GameObject standartTurretPrefab;
    [SerializeField]
    public GameObject missileTurretPrefab;

    public static BuildManager instance;

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
        GameObject turret = (GameObject) Instantiate(turretToBuild.prefab, node.GetBuildPosition(), Quaternion.identity);

        node.turret = turret;

        Debug.Log("Turret build!! Money left: "+ PlayerStarts.money);
    }

    public void SelectTurretToBuild(TurrretBluePrint turret)
    {
        //set the turret to buils
        turretToBuild = turret;
    }
}
