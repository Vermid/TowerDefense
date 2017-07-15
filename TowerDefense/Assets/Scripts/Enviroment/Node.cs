using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    #region Inspector
    [HideInInspector]
    public bool isUpgraded = false;

    [SerializeField]
    private Color hoverColor;

    [SerializeField]
    private Color notEnoughMoneyColor;

    [SerializeField]
    private Vector3 positionOffset;
    // [Header("This is needed for the Player Turret")]
    [HideInInspector]
    public GameObject turret;
    public GameObject startTurret;

    [HideInInspector]
    public TurretBlueprint turretBlueprint;
    #endregion
    private BuildManager buildManager;

    private Renderer rend;
    private Color startColor;
    private GameObject objectHolder;
    private GameObject turretHolder;
    void Start()
    {
        buildManager = BuildManager.instance;
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
        objectHolder = GameObject.FindGameObjectWithTag(ConstNames.ObjectPool);
        objectHolder = GameObject.FindGameObjectWithTag(ConstNames.TurretHolder);


        if (startTurret != null)
        {
            GameObject tmp_turret = (GameObject)Instantiate(startTurret, GetBuildPosition(), Quaternion.identity, objectHolder.transform);

            turret = tmp_turret;
            buildManager.DeselectNode();
        }
    }
    //change all the mouse action for Touch actions (android)
    void OnMouseDown()
    {
        //if shop ui is over a node you cant build you will click on the shopUi turret 
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        CameraScript.current.LastPosition();

        if (turret != null)
        {
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
            return;

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
        turret.GetComponent<Turret>().RejectAmmo();
        GameObject upgrade = turret.GetComponent<Turret>().GetUpgrade();
        Destroy(turret);

 
        //should we use the pool here? 
        GameObject tmp_turret = (GameObject)Instantiate(upgrade, GetBuildPosition(), Quaternion.identity, objectHolder.transform);

        turret = tmp_turret;

        isUpgraded = true;

        Debug.Log("Turret Upgraded");

        GameObject gobj = ObjectPool_Zaim.current.GetPoolObject(buildManager.buildEffect.name);

        if (gobj == null)
            return;

        gobj.transform.position = transform.position;
        gobj.transform.rotation = transform.rotation;

        gobj.SetActive(true);
    }

    public void SellTurret()
    {
        if (turret.gameObject.GetComponent<Turret>() != null)
            turret.gameObject.GetComponent<Turret>().RejectAmmo();

        PlayerStarts.money += turretBlueprint.GetSellAmount();
        GameObject effect = (GameObject)Instantiate(buildManager.sellEffect, GetBuildPosition(), Quaternion.identity, objectHolder.transform);
        Destroy(effect, 5f);

        Destroy(turret);
        turretBlueprint = null;
        isUpgraded = false;
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
