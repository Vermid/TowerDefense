using UnityEngine;
using System.Collections;

[System.Serializable]
public class TurretBlueprint
{
    #region Inspector
    [Header("Start Object")]
    public GameObject prefab;
    public int cost;

    [Header("Upgrade Object")]
    public GameObject upgradedPrefab;
    public int upgradeCost;
    #endregion
    public int GetSellAmount()
    {
        return cost / 2;
    }

}
