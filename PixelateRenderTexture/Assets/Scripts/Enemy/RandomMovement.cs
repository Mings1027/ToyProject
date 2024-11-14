using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class RandomMovement : MonoBehaviour
{
    private NavMeshAgent nav;
    private Rigidbody rigid;

    public Transform player;
    public float disToPlayer;

    public float patrolRange;
    public Transform centrePoint; //if you want enemy move everywhere then centrePoint is child this object
                                  //but you want fixed range then centrePoint separate this object

    public Collider atkPoint;
    public float atkDelay;
    public float targetDis, targetRange;

    [SerializeField] private bool isChase, isAttack;

    private void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        isChase = Vector3.Distance(transform.position, player.position) < disToPlayer ? true : false;
        if (isChase) ChasePlayer();
        else Patrol();
    }
    private void FixedUpdate()
    {
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        Targeting();
    }
    private void ChasePlayer()
    {
        nav.SetDestination(player.position);
        nav.isStopped = !isChase;
    }
    private void Patrol()
    {
        if (nav.remainingDistance > nav.stoppingDistance) return;
        if (RandomPoint(centrePoint.position, patrolRange, out var point))
        {
            // Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
            nav.SetDestination(point);
        }
    }
    private void Targeting()
    {
        var ray = Physics.SphereCastAll(atkPoint.transform.position, targetDis, atkPoint.transform.forward, targetRange, LayerMask.GetMask("Player"));
        if (ray.Length > 0 && !isAttack)
        {
            StartCoroutine(AttackCoroutine());
        }
    }
    private IEnumerator AttackCoroutine()
    {
        var wait = new WaitForSeconds(atkDelay);
        isChase = false;
        isAttack = true;

        atkPoint.enabled = true;
        yield return wait;
        atkPoint.enabled = false;
        yield return wait;

        isChase = true;
        isAttack = false;
    }

    private bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        var randomPoint = center + Random.insideUnitSphere * range;
        result = NavMesh.SamplePosition(randomPoint, out var hit, 1.0f, NavMesh.AllAreas) ? hit.position : Vector3.zero;
        return result == hit.position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(centrePoint.position, patrolRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, disToPlayer);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(atkPoint.transform.position, targetDis);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(atkPoint.transform.position, targetRange);
    }


}
