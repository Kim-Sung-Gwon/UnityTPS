using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]

public class EnemiyMoveAgent : MonoBehaviour
{ // ��Ʈ�� ������ ��� ���� List ���׸� (�Ϲ���) ����
    public List<Transform> WayPointList;
    public int nexIdx = 0;   // ���� ���� ������ �迭 �ε��� ��
    [SerializeField] private NavMeshAgent agent;
    private readonly float patrolSpeed = 1.5f;
    private readonly float traceSpeed = 4.0f;
    private float damping = 1.0f;  // ȸ�� �� ���� �ӵ��� ���� �ϴ� ���
    private Transform enemyTr;
    // ���� ��Ƽ ���
    private bool _patrolling;
    // ���� ��Ƽ
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
                MovewayPoint();
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
        get { return agent.velocity.magnitude; } // agent �ӵ��� ��ȯ�Ѵ�.
    }
    void Start()
    {
        enemyTr = GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        agent.updateRotation = false;
        // ������Ʈ�� �̿��ؼ� ȸ���ϴ� ����� ��Ȱ��ȭ ���� �ε巴���� ����
        var group = GameObject.Find("WayPointGroup");
        // ���̶�Ű�� �ִ� ������Ʈ ���� WaypointGroup ��(��) ã�Ƽ� ����
        if (group != null) // ��ȿ�� �˻�
        {  // ���� ������Ʈ�� Ʈ�������� WayPointList�� �� ��´�.
            group.GetComponentsInChildren<Transform>(WayPointList);
            WayPointList.RemoveAt(0); // ù��° �ε����� ����
        }
        MovewayPoint();
    }
    void Update()
    {
        if (agent.isStopped == false)
        {   // NavMeshAgent ���� �� ���� ���͸� ���ʹϾ� Ÿ���� ������ ��ȯ
            Quaternion rot = Quaternion.LookRotation(agent.desiredVelocity);
            enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot, Time.deltaTime * damping);
        }

        if (_patrolling == false) return;

        //float dist = (WayPointList[nexIdx].position - transform.position).magnitude;
        //float dist = Vector3.Distance(transform.position, WayPointList[nexIdx].position);
        // ���� ���������� 0.5���� �۰ų� ���ٸ�
        if (agent.remainingDistance <= 0.5f)
        {
            nexIdx = ++nexIdx % WayPointList.Count;
            MovewayPoint();
        }
    }
    void MovewayPoint()
    {   // �ִ� ��� ����� ������ �ʰų� ���� �Ҿ�������
        if (agent.isPathStale) return;
        // ��� ������ ���� ��� = ����Ʈ�� ��Ҵ� Ʈ������
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
