using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]  // ��Ʃ����Ʈ : public ����� ����ʵ带
public class PlayerAnimation // �ν����� â�� �����ش�.
{
    public AnimationClip idle;
    public AnimationClip runForward;
    public AnimationClip runBackward;
    public AnimationClip runLeft;
    public AnimationClip runRight;
    public AnimationClip Sprint;
}

public class Player : MonoBehaviour
{
    public PlayerAnimation playerAnimation;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotSpeed = 90f;
    [SerializeField] Rigidbody rboby;
    [SerializeField] CapsuleCollider capCol;
    [SerializeField] Transform tr;
    [SerializeField] Animation _animation;
    float h, v, r;
    public bool siRunning = false;
    void Start()
    { // ���۳�Ʈ ĳ�� ó��
        rboby = GetComponent<Rigidbody>();
        capCol = GetComponent<CapsuleCollider>();
        tr = GetComponent<Transform>();
        _animation = GetComponent<Animation>();
        _animation.Play(playerAnimation.idle.name);
    }
    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        r = Input.GetAxisRaw("Mouse X");
        Vector3 moveDir = (h * Vector3.right) + (v * Vector3.forward);
        tr.Translate(moveDir.normalized * moveSpeed * Time.deltaTime);
        {
            MoveAni();
        }
        tr.Rotate(Vector3.up * r * Time.deltaTime * rotSpeed);
        IsRun();
    }

    private void IsRun()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
        {
            moveSpeed = 10f;
            _animation.CrossFade(playerAnimation.Sprint.name, 0.3f);
            siRunning = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed = 5f;
            siRunning = false;
        }
    }

    private void MoveAni()
    {
        if (h > 0.1f)
            _animation.CrossFade(playerAnimation.runRight.name, 0.3f);
        else if (h < -0.1f)
            _animation.CrossFade(playerAnimation.runLeft.name, 0.3f);
        else if (v > 0.1f)
            _animation.CrossFade(playerAnimation.runForward.name, 0.3f);
        else if (v < -0.1f)
            _animation.CrossFade(playerAnimation.runBackward.name, 0.3f);
        else
            _animation.CrossFade(playerAnimation.idle.name, 0.3f);
    }
}
