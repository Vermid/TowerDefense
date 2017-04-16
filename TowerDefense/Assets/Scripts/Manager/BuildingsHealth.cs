using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingsHealth : MonoBehaviour
{

    [SerializeField]
    private int health = 10;

    [SerializeField]
    private int money = 100;

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
