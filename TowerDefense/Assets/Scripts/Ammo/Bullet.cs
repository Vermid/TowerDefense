#region Using/Import

using UnityEngine;

#endregion

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private bool canGrow = false;

    [SerializeField]
    private float speed = 70f;

    [SerializeField]
    private float splashRadius = 0;

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
            gameObject.SetActive(false);
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

    private void HitTarget()
    {
        if (splashRadius > 0f)
        {
            Explode();

            GameObject effectExplode = ObjectPool_Zaim.current.GetPoolObject(impactEffect.name, canGrow);

            if (effectExplode == null)
                return;

            effectExplode.transform.position = transform.position;
            effectExplode.transform.rotation = transform.rotation;

            if (effectExplode.transform.childCount != 0)
            {
                for (int i = 0; i < effectExplode.transform.childCount; i++)
                {
                    effectExplode.transform.GetChild(i).gameObject.SetActive(true);
                }
            }
            effectExplode.SetActive(true);
        }
        else
        {
            Damage(target);

            GameObject effectInstance = ObjectPool_Zaim.current.GetPoolObject(impactEffect.name, canGrow);

            if (effectInstance == null)
                return;

            effectInstance.transform.position = transform.position;
            effectInstance.transform.rotation = transform.rotation;
            effectInstance.SetActive(true);
        }
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
            if (collider.tag == "Enemy")
            {
                Damage(collider.transform);
            }
        }
    }

    private void Damage(Transform enemy)
    {
        //kill or damage the enemy
        enemy.gameObject.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, splashRadius);
    }
}