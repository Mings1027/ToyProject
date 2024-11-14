using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class RandomMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private Rigidbody rigid;
    public Transform player;
    public float distance;
    [SerializeField] private bool isChase;

    public float range;
    public Transform centrePoint; //if you want enemy move everywhere then centrePoint is child this object
    //but you want fixed range then centrePoint separate this object

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        isChase = Vector3.Distance(transform.position, player.position) < distance ? true : false;
        if (isChase) ChasePlayer();
        else Patrol();
    }
    private void FixedUpdate()
    {
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
    }
    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }
    private void Patrol()
    {
        if (agent.remainingDistance > agent.stoppingDistance) return;
        if (RandomPoint(centrePoint.position, range, out var point))
        {
            // Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
            agent.SetDestination(point);
        }
    }
    private bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        var randomPoint = center + Random.insideUnitSphere * range;
        result = NavMesh.SamplePosition(randomPoint, out var hit, 1.0f, NavMesh.AllAreas) ? hit.position : Vector3.zero;
        return result == hit.position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(centrePoint.position, range);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distance);
    }


}
