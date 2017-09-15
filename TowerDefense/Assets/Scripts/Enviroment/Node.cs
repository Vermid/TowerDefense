using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{

    #region Inspector
    [HideInInspector]
    public bool lastUpgrade = false;

    [SerializeField]
    private Color hoverColor;

    [SerializeField]
    private Color notEnoughMoneyColor;

    [SerializeField]
    private Vector3 positionOffset;
    [Header("This is needed for Testing turrets on Game Start")]
    [HideInInspector]
    public GameObject turret;
    public GameObject startTurret;

    [HideInInspector]
    public TurretBlueprint turretBlueprint;
    #endregion

    #region Privates
    private BuildManager buildManager;
    private Renderer renderer;
    private Color startColor;
    private GameObject objectHolder;
    private GameObject turretHolder;
    #endregion

    void Start()
    {
        buildManager = BuildManager.instance;
        renderer = GetComponent<Renderer>();
        startColor = renderer.material.color;
        objectHolder = GameObject.FindGameObjectWithTag(ConstNames.ObjectPool);
        objectHolder = GameObject.FindGameObjectWithTag(ConstNames.TurretHolder);

        if (startTurret != null)
        {
            GameObject tmp_turret = (GameObject)Instantiate(startTurret, GetBuildPosition(), Quaternion.identity, objectHolder.transform);

            turret = tmp_turret;
            buildManager.DeselectNode();
        }
    }

    void BuildTurret(TurretBlueprint bluePrint)
    {
        //if the player has the money to buy the turret
        if (PlayerStarts.money < bluePrint.GetCostAmount())
        {
            Debug.Log("Not enough money");
            return;
        }

        PlayerStarts.money -= bluePrint.GetCostAmount();
        //should we use the pool here? 
        GameObject tmp_turret = (GameObject)Instantiate(bluePrint.GetPrefab(), GetBuildPosition(), Quaternion.identity, objectHolder.transform);
        turretBlueprint = bluePrint;

        turret = tmp_turret;
        Debug.Log("Turret build!! Money left: " + PlayerStarts.money);

        GameObject gobj = ObjectPool_Zaim.current.GetPoolObject(buildManager.buildEffect.name);

        if (gobj == null)
        {
            Debug.LogWarning(buildManager.buildEffect.name + " is NULL");
            return;
        }

        gobj.transform.position = transform.position;
        gobj.transform.rotation = transform.rotation;
        gobj.SetActive(true);

        buildManager.DeselectNode();
    }

    public void UpgradeTurret()
    {
        //if the player has the money to buy the turret
        if (PlayerStarts.money < turret.GetComponent<Turret>().UpgradePrice)
        {
            Debug.Log("Not enough money");
            return;
        }

        if (turret.transform.Find(ConstNames.SpawnPoint))
        {
            var children = turret.transform.Find(ConstNames.SpawnPoint);
            for (int i = 0; i < children.childCount; i++)
            {
                children.GetChild(i).gameObject.SetActive(false);
                children.GetChild(i).SetParent(objectHolder.transform);
            }
        }

        PlayerStarts.money -= turret.GetComponent<Turret>().UpgradePrice;
        //destroy or move old turret back
        //reject Ammo before you destroy Turret or Objectpool will run into an ERROR
        turret.GetComponent<Turret>().RejectAmmo();
        GameObject upgrade = turret.GetComponent<Turret>().GetUpgrade();
        Destroy(turret);


        //if player can place more than 10 times the same turret than use ObjectPool 
        GameObject tmp_turret = (GameObject)Instantiate(upgrade, GetBuildPosition(), Quaternion.identity, objectHolder.transform);

        turret = tmp_turret;

        if (upgrade.GetComponent<Turret>().GetUpgrade() == null)
        {
            lastUpgrade = true;
        }

        Debug.Log("Turret Upgraded");

        GameObject gobj = ObjectPool_Zaim.current.GetPoolObject(buildManager.buildEffect.name);

        if (gobj == null)
        {
            Debug.LogWarning(buildManager.buildEffect.name + " is NULL");
            return;
        }

        gobj.transform.position = transform.position;
        gobj.transform.rotation = transform.rotation;

        gobj.SetActive(true);
    }

    public void SellTurret()
    {
        if (turret.gameObject.GetComponent<Turret>() != null)
            turret.gameObject.GetComponent<Turret>().RejectAmmo();

        PlayerStarts.money += turret.gameObject.GetComponent<Turret>().Price / 2;
        GameObject effect = (GameObject)Instantiate(buildManager.sellEffect, GetBuildPosition(), Quaternion.identity, objectHolder.transform);
        Destroy(effect, 5f);

        Destroy(turret);
        turretBlueprint = null;
        lastUpgrade = false;
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
            renderer.material.color = hoverColor;
        }
        else
        {
            renderer.material.color = notEnoughMoneyColor;
        }
    }

    //change all the mouse action for Touch actions (android)
    void OnMouseDown()
    {
        CameraScript.current.LastPosition();

        //if shop ui is over a node you cant build you will click on the shopUi turret 
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (turret != null)
        {
            Debug.Log("TURRET");
            buildManager.SelectNode(this);
            return;
        }

        if (!buildManager.CanBuild)
            return;

        BuildTurret(buildManager.GetTurretToBuild());
    }

    void OnMouseDrag()
    {
        CameraScript.current.DragObject();
    }

    void OnMouseExit()
    {
        //change the color from the node
        renderer.material.color = startColor;
    }

    public Vector3 GetBuildPosition()
    {
        //return the node position + an offset so the turret is not inside the node
        return transform.position + positionOffset;
    }
}
