using UnityEngine;

public class Shop : MonoBehaviour
{
    #region Inspector
    public TurretBlueprint standradTurret;
    public TurretBlueprint missileTurret;
    public TurretBlueprint laserTurret;
    public TurretBlueprint mineFactoryAir;
    public TurretBlueprint mineFactoryGround;
    public TurretBlueprint bFG;

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
        Toggle();
    }
    //call comes from GUI
    public void SelectMissileTurret()
    {
        buildManager.SelectTurretToBuild(missileTurret);
        Debug.Log("Missile");
        Toggle();
    }

    //call comes from GUI
    public void SelectLaserTurret()
    {
        buildManager.SelectTurretToBuild(laserTurret);
        Debug.Log("Laser");
        Toggle();
    }

    public void SelectMineFactoryAir()
    {
        buildManager.SelectTurretToBuild(mineFactoryAir);
        Debug.Log("Air MineFactory");
        Toggle();
    }

    public void SelectMineFactoryGround()
    {
        buildManager.SelectTurretToBuild(mineFactoryGround);
        Debug.Log("Ground MineFactory");
        Toggle();
    }

    private void Toggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

}
