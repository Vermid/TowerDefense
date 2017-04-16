using UnityEngine;

public class Shop : MonoBehaviour
{
    private BuildManager buildManager;

    public TurrretBluePrint standradTurret;
    public TurrretBluePrint missileTurret;

    void Start()
    {
        buildManager = BuildManager.instance;
    }
    //call comes from GUI
    public void SelectStandardTurret()
    {
        buildManager.SelectTurretToBuild(standradTurret);
        Debug.Log("Standard");
    }
    //call comes from GUI
    public void SelectMissileTurret()
    {
        buildManager.SelectTurretToBuild(missileTurret);
        Debug.Log("Missile");
    }
}
