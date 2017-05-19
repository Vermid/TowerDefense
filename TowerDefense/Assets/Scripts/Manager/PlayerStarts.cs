using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStarts : MonoBehaviour
{
    public static int money;
    public int startMoney = 1000;

    public static int lives;
    public int startLives = 20;

    void Start()
    {
        money = startMoney;
        lives = startLives;
    }
}
