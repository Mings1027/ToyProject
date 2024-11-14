using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour
{
    public ParticleSystem ps;
    public Transform target;
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ObjectPooler.SpawnFromPool("PlayerMeleeAttack", target.transform.position);
            ps.Play();
        }
    }
}
