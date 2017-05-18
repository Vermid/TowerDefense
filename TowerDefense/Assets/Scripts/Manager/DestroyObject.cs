using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{

    [SerializeField]
    private float destroyTimer = 2;
    void OnEnable()
    {
        Invoke(ConstNames.Destroy, destroyTimer);
    }

    void Destroy()
    {
        gameObject.SetActive(false);
    }

    void OnDisable()
    {
        CancelInvoke();
    }
}
