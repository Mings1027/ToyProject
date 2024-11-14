using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssistWeaponController : MonoBehaviour
{
    // public Vector3 followPos;
    // public int followDelay;
    // public Transform parent;
    // public Queue<Vector3> parentPos;

    public Transform target;
    private float rotZ;
    public float rotationSpeed;

    public float minRange;
    public float maxRange;
    public float moveSpeed;


    private void Update()
    {
        Follow();
    }
    private void Follow()
    {
        var distance = Vector2.Distance(transform.position, target.position);
        if (distance > minRange && distance < maxRange)
        {
            var moveVec = target.position - transform.position;
            transform.Translate((moveVec.normalized * moveSpeed) * Time.deltaTime);
        }
        else if (distance > maxRange) transform.position = target.position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, minRange);
        Gizmos.DrawWireSphere(transform.position, maxRange);
    }
}
