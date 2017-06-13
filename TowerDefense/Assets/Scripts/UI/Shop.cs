using UnityEngine;

public class Shop : MonoBehaviour
{
    #region Inspector
    public TurretBlueprint standradTurret;
    public TurretBlueprint missileTurret;
    public TurretBlueprint laserTurret;
    public TurretBlueprint mineFactory;
    #endregion

    private BuildManager buildManager;
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

    //call comes from GUI
    public void SelectLaserTurret()
    {
        buildManager.SelectTurretToBuild(laserTurret);
        Debug.Log("Laser");
    }

    public void SelectMineFactory()
    {
        buildManager.SelectTurretToBuild(mineFactory);
        Debug.Log("MineFactory");
    }

}
