using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Animator))]
public class EnemyAI : MonoBehaviour
{ // ���� ���¸� �˱� ���ؼ� ������ ��� ����
    public enum State  // ������ ���
    {
        PTROL = 0, TRACE, ATTACK, DIE
    }
    public State state = State.PTROL;
    [SerializeField] private Transform playerTr; // �Ÿ��� ������� ����
    [SerializeField] private Transform enemyTr;  // �Ÿ��� ������� ����
    [SerializeField] private Animator animator;
    // ���� �Ÿ�, ���� �Ÿ�
    public float attackDist = 5.0f;  // ���� ���� 5���� ������ ��������
    public float traceDist = 10f;    // ���� ���� 10���� ������ ��������
    public bool isDie = false;       // ��� ���� �Ǵ�
    private WaitForSeconds ws;
    private EnemiyMoveAgent moveAgent;
    private EnemyFire enemyFire;
    // �ִϸ����� ��Ʈ�ѷο� ���� �� �Ķ������ �ؽð��� ������ �̸� ����
    private readonly int hashMove = Animator.StringToHash("IsMove");
    private readonly int hashSpeed = Animator.StringToHash("MoveSpeed");
    private readonly int hashDie = Animator.StringToHash("DieTrigger");
    private readonly int hashDieIdx = Animator.StringToHash("DieIdxInt");
    private readonly int hashOffset = Animator.StringToHash("Offset");
    private readonly int hashWalkSpeed = Animator.StringToHash("WarkSpeed");
    void Awake()
    {
        animator = GetComponent<Animator>();
        moveAgent = GetComponent<EnemiyMoveAgent>();
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerTr = player.GetComponent<Transform>();
        enemyTr = GetComponent<Transform>();
        enemyFire = GetComponent<EnemyFire>();
        ws = new WaitForSeconds(0.3f);
    }
    private void OnEnable()  // ������Ʈ�� Ȱ��ȭ �ɶ����� ȣ��
    {
        animator.SetFloat(hashOffset, Random.Range(0.0f, 1.0f));
        animator.SetFloat(hashWalkSpeed, Random.Range(1.0f, 2.0f));
        StartCoroutine(CheckState());
        StartCoroutine(Action());
    }
    IEnumerator CheckState()
    {
        while (!isDie)
        {
            if (state == State.DIE) yield break;
            // ��� �����̸� �ڷ�ƾ �Լ��� ���� ��Ŵ

            float dist = (playerTr.position - enemyTr.position).magnitude;
            if (dist <= attackDist)
                state = State.ATTACK;
            else if (dist <= traceDist)
                state = State.TRACE;
            else
                state = State.PTROL;

            yield return ws;
        }
    }
    IEnumerator Action()
    {
        while (!isDie)
        {
            yield return ws;

            switch (state)
            {
                case State.PTROL:
                    enemyFire.isFire = false;
                    moveAgent.patrolling = true;
                    animator.SetBool(hashMove, true);
                    break;

                case State.ATTACK:
                    if (enemyFire.isFire == false)
                        enemyFire.isFire = true;
                    moveAgent.Stop();
                    animator.SetBool(hashMove, false);
                    break;

                case State.TRACE:
                    enemyFire.isFire = false;
                    moveAgent.traceTarget = playerTr.position;
                    animator.SetBool(hashMove, true);
                    break;

                case State.DIE:
                    EnemyDie();
                    break;
            }
        }
    }

    private void EnemyDie()
    {
        isDie = true;
        moveAgent.Stop();
        enemyFire.isFire = false;
        animator.SetTrigger(hashDie);
        animator.SetInteger(hashDieIdx, Random.Range(0, 3));
        GetComponent<Rigidbody>().isKinematic = true;     // ��������
        GetComponent<CapsuleCollider>().enabled = false;  // ĸ�� �ݶ��̴� ��Ȱ��ȭ
        gameObject.tag = "Untagged";                      // ����� �±� ����
        state = State.DIE;
        StartCoroutine(ObjectPoolPush());
    }
    IEnumerator ObjectPoolPush()
    {
        yield return new WaitForSeconds(3f);
        isDie = false;
        GetComponent<Rigidbody>().isKinematic = false;   // ������ �ְ�
        GetComponent<CapsuleCollider>().enabled = true;  // ĸ�� �ݶ��̴� Ȱ��ȭ
        gameObject.tag = "ENEMY";                        // ������Ʈ Ȱ��ȭ �Ǳ��� �±��̸��� �������
        gameObject.SetActive(false);
        state = State.PTROL;
    }

    void Update()
    {
        animator.SetFloat(hashSpeed, moveAgent.speed);
    }
}
