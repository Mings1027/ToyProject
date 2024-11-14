using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    private Rigidbody2D rigid;
    [SerializeField] private Transform target;
    public float thrust, knockbackTime;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        target = col.transform;
    }

    public void Knockback()
    {
        if (rigid == null) return;
        // rigid.isKinematic = false;
        Vector2 difference = rigid.transform.position - target.position;
        difference = difference.normalized * thrust;
        rigid.AddForce(difference, ForceMode2D.Impulse);
        StartCoroutine(KnockbackCo(knockbackTime));
    }

    private IEnumerator KnockbackCo(float knockbackTime)
    {
        var wait = new WaitForSeconds(knockbackTime);
        yield return wait;
        rigid.velocity = Vector2.zero;
        // rigid.isKinematic = true;
    }
}