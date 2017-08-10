using UnityEngine;

[System.Serializable]
public class WaveGenerator
{
    #region Inspector
    [SerializeField]
    private GameObject airMonster;
    [SerializeField]
    private int airCounter;

    [SerializeField]
    private GameObject groundMonster;
    [SerializeField]
    private int groundCounter;

    [SerializeField]
    private GameObject bossMonster;
    [SerializeField]
    private int bossCounter;
    #endregion
    public GameObject AirMonster { get { return airMonster; }  }
    public int AirCounter { get { return airCounter; } }

    public GameObject GroundMonster { get { return groundMonster; } }
    public int GroundCounter { get { return groundCounter; } }

    public GameObject BossMonster { get { return bossMonster; } }
    public int BossCounter { get { return bossCounter; } }
}
