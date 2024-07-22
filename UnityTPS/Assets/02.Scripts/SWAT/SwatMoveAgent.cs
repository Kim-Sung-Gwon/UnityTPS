using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]

public class SwatMoveAgent : MonoBehaviour
{
    public List<Transform> WayPointList;
    public int nexIdx = 0;
    [SerializeField] private NavMeshAgent agent;
    private readonly float patrolSpeed = 1.5f;
    private readonly float traceSpeed = 4.0f;
    private float damping = 1.0f;
    private Transform swatTr;
    private bool _patrolling;
    public bool patrolling
    {
        get { return _patrolling; }
        set
        {
            _patrolling = value;
            if (_patrolling)
            {
                agent.speed = patrolSpeed;
                damping = 1.0f;
            }
        }
    }
    private Vector3 _traceTarget;
    public Vector3 traceTarget
    {
        get { return _traceTarget; }
        set
        {
            _traceTarget = value;
            agent.speed = traceSpeed;
            damping = 7.0f;
            TraceTarget(_traceTarget);
        }
    }
    public float speed
    {
        get { return agent.velocity.magnitude; }
    }
    void Start()
    {
        swatTr = GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        agent.updatePosition = false;
        var group = GameObject.Find("WalkPointGroup");
        if (group != null)
        {
            group.GetComponentsInChildren<Transform>(WayPointList);
            WayPointList.RemoveAt(0);
        }
        MovewayPoint();
    }
    void Update()
    {
        if (agent.isStopped == false)
        {
            Quaternion rot = Quaternion.LookRotation(agent.desiredVelocity);
            swatTr.rotation = Quaternion.Slerp(swatTr.rotation, rot, Time.deltaTime * damping);
        }
        if (_patrolling == false) return;
        if (agent.remainingDistance <= 0.5f)
        {
            nexIdx = ++nexIdx % WayPointList.Count;
            MovewayPoint();
        }
    }
    void MovewayPoint()
    {
        if (agent.isPathStale) return;
        agent.destination = WayPointList[nexIdx].position;
        agent.isStopped = false;
    }
    void TraceTarget(Vector3 pos)
    {
        if (agent.isPathStale) return;
        agent.destination = pos;
        agent.isStopped = false;
    }
    public void Stop()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        _patrolling = false;
    }
}
