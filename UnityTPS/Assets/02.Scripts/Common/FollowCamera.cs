using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform target;          // ���� �ٴ� ���
    [SerializeField] private float Height = 5.0f;       // ī�޶� ����
    [SerializeField] private float distance = 7.0f;     // Ÿ�ٱ��� �Ÿ�
    [SerializeField] private float movedamping = 10f;
    // ī�޶� �̵� ȸ���� ������ �ε巴�� ���� �ϴ� ��
    [SerializeField] private float rotdamping = 15f;
    [SerializeField] private Transform CamTr;           // �ڱ��ڽ� ��ġ
    [SerializeField] private float targetOffset = 2.0f; // Ÿ�ٿ����� ī�޶� ����
    void Start()
    {
        CamTr = transform;
    }
    void LateUpdate()
    {                      // Ÿ�� �����ǿ��� distance��ŭ �ڿ� ��ġ + // Height ���� ��ŭ ���� ��ġ
        var CamPos = target.position - (target.forward * distance) + (target.up * Height);
        CamTr.position = Vector3.Slerp(CamTr.position, CamPos, Time.deltaTime * movedamping);
                  // ��� ���� (�ڱ� �ڽ� ��ġ����, CamPos����, damping �ð���ŭ �ε巴�� ������)
        CamTr.rotation = Quaternion.Slerp(CamTr.rotation, target.rotation, Time.deltaTime * rotdamping);
        // ��� �������� ȸ�� (�ڱ� �ڽ� �����̼ǿ���, Ÿ�� �����̼Ǳ���, damping �ð���ŭ �ε巴�� ȸ��)
        CamTr.LookAt(target.position + (target.up * targetOffset));
             // Ÿ�� �����ǿ��� 2��ŭ ���� �ø�
    }
    private void OnDrawGizmos()  // �� ȭ�鿡�� �����̳� ���� �׷��ִ� �Լ� �ݹ��Լ�
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(target.position + (target.up * targetOffset), 0.1f);
               // ����
        Gizmos.DrawLine(target.position + (target.up * targetOffset), CamTr.position);
               // ��
    }
}
