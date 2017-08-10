using UnityEngine;
using Assets.Scripts;

public class Mine : MonoBehaviour
{
    #region Inspector
    [SerializeField]
    private GameObject impactEffect = null;

    [Header("Attributes")]
    [SerializeField]
    private float splashRadius = 0;
    [SerializeField]
    public static float damage = 50;

    #endregion

    private GameObject parent;
    private Enums.WeaponType wType;
    private Turret turret;

    void Start()
    {
        parent = GameObject.FindGameObjectWithTag(ConstNames.ObjectPool);
    }

    public void SetWeapontType(Enums.WeaponType _wType, Turret _turret)
    {
        wType = _wType;
        turret = _turret;
    }



    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == ConstNames.Enemy)
        {
            HitTarget();

            gameObject.SetActive(false);
            gameObject.transform.parent = parent.transform;
        }
    }
    private void HitTarget()
    {
        if (splashRadius > 0f)
        {
            Explode();
        }
        else
        {
            return;
        }
        GameObject effect = ObjectPool_Zaim.current.GetPoolObject(impactEffect.name);

        if (effect == null)
            return;

        effect.transform.position = transform.position;
        effect.transform.rotation = transform.rotation;
        effect.SetActive(true);

        gameObject.SetActive(false);
    }

    private void Explode()
    {
        //get all the Colliders that are inside the  overlapsphere
        Collider[] hitObjects = Physics.OverlapSphere(transform.position, splashRadius);

        foreach (Collider collider in hitObjects)
        {
            //check in a foreac for every enemy that is inside 
            //this is also needed for a laser later
            if (collider.tag == ConstNames.Enemy)
            {
                // pass the collider that gets the damage
                Damage(collider.transform);
            }
        }
    }

    private void Damage(Transform enemy)
    {
        Enemy e = enemy.GetComponent<Enemy>();
        if (e != null)
        {
            bool kill = e.CalculateDamage(damage, wType);
            if (kill)
            {
                turret.Kill();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, splashRadius);
    }
}