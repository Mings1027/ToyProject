using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkController : MonoBehaviour
{
    [SerializeField] float lifeTime;

    private void OnEnable()
    {
        Invoke(nameof(DestroyObject), lifeTime);
    }
    private void OnDisable()
    {
        ObjectPooler.ReturnToPool(gameObject);
        CancelInvoke();
    }
    private void DestroyObject() => gameObject.SetActive(false);
}
