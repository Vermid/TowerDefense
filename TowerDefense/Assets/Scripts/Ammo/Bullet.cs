#region Using/Import

using System.Collections;
using UnityEngine;

#endregion

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float speed = 70f;

    [SerializeField]
    private float splashRadius = 0;

    [SerializeField]
    private int damage = 50;

    [SerializeField]
    private GameObject impactEffect = null;

    private Transform target;

    public void Seek(Transform _target)
    {
        target = _target;
    }

    // Update is called once per frame
    private void Update()
    {
        if (target == null)
        {
            return;
        }

        //again get the direction where to look/shoot by sub the target.position from your position
        Vector3 dir = target.position - transform.position;
        //get the distance the bullet should get every frame
        float distanceThisFrame = speed * Time.deltaTime;
        //dir is direction vector magnitude is the lengh of it
        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
        }

        //sets the speed from the bullet
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);

        transform.LookAt(target);

    }

    bool CheckTarget()
    {
        //expand this in a functon
        if (!target.gameObject.activeInHierarchy)
        {
            GameObject effect = ObjectPool_Zaim.current.GetPoolObject(impactEffect.name);

            if (effect == null)
                return false;

            effect.transform.position = transform.position;
            effect.transform.rotation = transform.rotation;
            effect.SetActive(true);

            gameObject.SetActive(false);
            return false;
        }
        return true;
    }
    private void HitTarget()
    {
        if (splashRadius > 0f)
        {
            Explode();
        }
        else
        {
            Damage(target);
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
            e.TakeDamage(damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, splashRadius);
    }
}