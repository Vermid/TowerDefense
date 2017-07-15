using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingsHealth : MonoBehaviour
{
    #region Inspector
    [Header("Attributes")]
    [SerializeField]
    private int health = 10;

    [SerializeField]
    private int money = 100;
    #endregion
    public static BuildingsHealth instance;

    void Awake()
    {
        if (instance != null)
            return;
        instance = this;
    }

    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
            PlayerStarts.money += money;
        }
    }
}
