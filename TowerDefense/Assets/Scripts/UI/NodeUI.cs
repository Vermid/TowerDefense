using UnityEngine;
using UnityEngine.UI;

public class NodeUI : MonoBehaviour
{
    #region Inspector
    public GameObject ui;
    public Text upgradeCost;
    public Button upgradeButton;
    public Text sellAmount;
    public Text killCounter;

    [Header("Current")]
    public Text range;
    public Text damage;
    public Text fireRate;

    [Header("Next")]
    public Text nextRange;
    public Text nextDamage;
    public Text nextFireRate;
    #endregion

    private Node target;

    public void SetTarget(Node _target)
    {
        target = _target;

        transform.position = target.GetBuildPosition();

        //sellAmount.text = "$" + target.turretBlueprint.GetSellAmount();
        
        Turret turret = target.turret.GetComponent<Turret>();
        killCounter.text = "Kills: " + turret.GetKillCounter();
        sellAmount.text = "$" + turret.Price / 2;

        if (!target.isUpgraded)
        {
            //  upgradeCost.text = "$" + target.turretBlueprint.GetUpgradeCostAmount();
            upgradeCost.text = "$" + turret.UpgradePrice;

            upgradeButton.interactable = true;

            damage.text = "D:\n" + turret.Damage;
            range.text = "R:\n" + turret.Range;
            fireRate.text = "F:\n" + turret.FireRate;

            Turret upgrade = turret.GetUpgrade().GetComponent<Turret>();
            nextDamage.text = "D:\n" + upgrade.Damage;
            nextRange.text = "R:\n" + upgrade.Range;
            nextFireRate.text = "F:\n" + upgrade.FireRate;
        }
        else
        {
            upgradeCost.text = "Done";
            upgradeButton.interactable = false;
            damage.text = "D:\n" + turret.Damage;
            range.text = "R:\n" + turret.Range;
            fireRate.text = "F:\n" + turret.FireRate;

            nextDamage.text = "D:\n NA";
            nextRange.text = "R:\n NA";
            nextFireRate.text = "F:\n NA";
        }
        ui.SetActive(true);
    }

    private void Update()
    {
        //if (target != null)
          //  killCounter.text = "Kills: " + target.turret.GetComponent<Turret>().GetKillCounter();
    }
    private void Start()
    {
        Hide();
    }

    public void Hide()
    {
        ui.SetActive(false);
    }

    public void Upgrade()
    {
        target.UpgradeTurret();
        BuildManager.instance.DeselectNode();
    }

    public void Sell()
    {
        target.SellTurret();
        BuildManager.instance.DeselectNode();
    }

}
