using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Animator))]
public class SwatAI : MonoBehaviour
{
    public enum State
    {
        PTROL = 0, TRACE, ATTACK, DIE
    }
    public State state = State.PTROL;
    [SerializeField] private Transform playerTr;
    [SerializeField] private Transform swatTr;
    [SerializeField] private Animator animator;

    public float attackDist = 5.0f;
    public float traceDist = 10f;
    public bool isDie = false;
    private WaitForSeconds ws;
    private SwatMoveAgent moveAgent;

    private readonly int hashMove = Animator.StringToHash("IsMove");
    private readonly int hashSpeed = Animator.StringToHash("MoveSpeed");
    void Awake()
    {
        animator = GetComponent<Animator>();
        moveAgent = GetComponent<SwatMoveAgent>();
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerTr = playerTr.GetComponent<Transform>();
        swatTr = GetComponent<Transform>();
        ws = new WaitForSeconds(0.3f);
    }
    private void OnEnable()
    {
        StartCoroutine(CheckState());
        StartCoroutine(Action());
    }
    IEnumerator CheckState()
    {
        while (!isDie)
        {
            if (state == State.DIE) yield break;
            float dist = (playerTr.position - swatTr.position).magnitude;
            if (dist < attackDist)
                state = State.ATTACK;
            else if (dist > traceDist)
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
                    moveAgent.patrolling = true;
                    break;
                case State.ATTACK:
                    moveAgent.Stop();
                    break;
                case State.TRACE:
                    moveAgent.traceTarget = playerTr.position;
                    break;
                case State.DIE:
                    break;
            }
        }
    }
}
