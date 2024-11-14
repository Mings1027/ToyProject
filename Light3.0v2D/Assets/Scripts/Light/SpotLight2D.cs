using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotLight2D : MonoBehaviour
{
    private void OnDisable()
    {
        ObjectPooler.ReturnToPool(gameObject);
    }
}
