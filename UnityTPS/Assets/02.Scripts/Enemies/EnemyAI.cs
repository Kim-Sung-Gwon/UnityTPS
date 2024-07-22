using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Animator))]
public class EnemyAI : MonoBehaviour
{ // 현재 상태를 알기 위해서 열거형 상수 제작
    public enum State  // 열거형 상수
    {
        PTROL = 0, TRACE, ATTACK, DIE
    }
    public State state = State.PTROL;
    [SerializeField] private Transform playerTr; // 거리를 재기위해 선언
    [SerializeField] private Transform enemyTr;  // 거리를 재기위해 선언
    [SerializeField] private Animator animator;
    // 공격 거리, 추적 거리
    public float attackDist = 5.0f;  // 공격 선언 5미터 안으로 들어왔을때
    public float traceDist = 10f;    // 추적 선언 10미터 안으로 들어왔을떄
    public bool isDie = false;       // 사망 여부 판단
    private WaitForSeconds ws;
    private EnemiyMoveAgent moveAgent;
    private EnemyFire enemyFire;
    // 애니메이터 컨트롤로에 정의 한 파라미터의 해시값을 정수로 미리 추출
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
    private void OnEnable()  // 오브젝트가 활성화 될때마다 호출
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
            // 사망 상태이면 코루틴 함수를 종료 시킴

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
        GetComponent<Rigidbody>().isKinematic = true;     // 물리제거
        GetComponent<CapsuleCollider>().enabled = false;  // 캡슐 콜라이더 비활성화
        gameObject.tag = "Untagged";                      // 사망시 태그 제거
        state = State.DIE;
        StartCoroutine(ObjectPoolPush());
    }
    IEnumerator ObjectPoolPush()
    {
        yield return new WaitForSeconds(3f);
        isDie = false;
        GetComponent<Rigidbody>().isKinematic = false;   // 물리가 있게
        GetComponent<CapsuleCollider>().enabled = true;  // 캡슐 콜라이더 활성화
        gameObject.tag = "ENEMY";                        // 오브젝트 활성화 되기전 태그이름을 원래대로
        gameObject.SetActive(false);
        state = State.PTROL;
    }

    void Update()
    {
        animator.SetFloat(hashSpeed, moveAgent.speed);
    }
}
