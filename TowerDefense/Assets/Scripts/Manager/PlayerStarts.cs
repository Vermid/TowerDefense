using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStarts : MonoBehaviour
{
    #region Inspector
    [SerializeField]
    private int startMoney = 1000;
    [SerializeField]
    private int startLives = 20;

    public static int lives;
    public static int money;

    #endregion
    void Start()
    {
        money = startMoney;
        lives = startLives;
    }
    #region You may delete this in further versions
    //public int StartMoney
    //{
    //    get { return startMoney; }
    //}

    //public int StartLives
    //{
    //    get { return startLives; }
    //} 
    #endregion
}
