using UnityEngine;

[System.Serializable]
public class TurretBlueprint
{
    #region Inspector
    [Header("Start Object")]
    [SerializeField]
    private GameObject prefab;

    [Header("Upgrade Object")]
    [SerializeField]
    private int upgradeCost;
    #endregion

    public int GetSellAmount()
    {
        return prefab.GetComponent<Turret>().Price / 2;
    }
    public int GetCostAmount()
    {
        return prefab.GetComponent<Turret>().Price;
    }
    public int GetUpgradeCostAmount()
    {
        return prefab.GetComponent<Turret>().UpgradePrice;
    }

    public GameObject GetPrefab()
    {
        return prefab;
    }
}
