using UnityEngine;
using UnityEngine.UI;


public class TurretInfo : MonoBehaviour
{
    [SerializeField]
    private Text range;

    [SerializeField]
    private Text damage;

    [SerializeField]
    private Text fireRate;

    [SerializeField]
    private Text target;

    [SerializeField]
    private Text price;

    [SerializeField]
    private Text weaponType;

    [SerializeField]
    private GameObject turret;
    // Use this for initialization
    void Start()
    {
        Turret t = turret.GetComponent<Turret>();
        damage.text = "Damage: " + t.Damage;
        range.text = "Range: " + t.Range;
        fireRate.text = "FireRate: " + t.FireRate;
        target.text = "Target: " + (t.GroundType ? (t.AirType ? "Both" : "Ground") : "Air");
        price.text = t.Price + "$";
        weaponType.text = "Type: " + t.WeaponType;
    }
}
