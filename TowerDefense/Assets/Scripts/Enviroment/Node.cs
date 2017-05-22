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
   // [Header("This is needed for the Player Turret")]
   [HideInInspector]
    public GameObject turret;
    [HideInInspector]
    public TurretBlueprint turretBlueprint;
    [HideInInspector]
    public bool  isUpgraded = false;

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

        BuildTurret(buildManager.GetTurretToBuild());
    }

    void BuildTurret(TurretBlueprint bluePrint)
    {

        //if the player has the money to buy the turret
        if (PlayerStarts.money < bluePrint.cost)
        {
            Debug.Log("Not enough money");
            return;
        }

        PlayerStarts.money -= bluePrint.cost;
        //should we use the pool here? 
        GameObject tmp_turret = (GameObject)Instantiate(bluePrint.prefab, GetBuildPosition(), Quaternion.identity);

        turret = tmp_turret;
        turretBlueprint = bluePrint;
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
        if (PlayerStarts.money < turretBlueprint.upgradeCost)
        {
            Debug.Log("Not enough money");
            return;
        }
        Destroy(turret);
        //destroy or move old turret back

        PlayerStarts.money -= turretBlueprint.upgradeCost;
        //should we use the pool here? 
        GameObject tmp_turret = (GameObject)Instantiate(turretBlueprint.upgradedPrefab, GetBuildPosition(), Quaternion.identity);

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
        PlayerStarts.money += turretBlueprint.GetSellAmount();

        Destroy(turret);
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
