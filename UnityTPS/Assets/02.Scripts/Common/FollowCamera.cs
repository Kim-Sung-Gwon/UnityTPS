using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform target;          // 따라 다닐 대상
    [SerializeField] private float Height = 5.0f;       // 카메라 높이
    [SerializeField] private float distance = 7.0f;     // 타겟광의 거리
    [SerializeField] private float movedamping = 10f;
    // 카메라가 이동 회전시 떨림을 부드럽게 완하 하는 값
    [SerializeField] private float rotdamping = 15f;
    [SerializeField] private Transform CamTr;           // 자기자신 위치
    [SerializeField] private float targetOffset = 2.0f; // 타겟에서의 카메라 높이
    void Start()
    {
        CamTr = transform;
    }
    void LateUpdate()
    {                      // 타겟 포지션에서 distance만큼 뒤에 위치 + // Height 높이 만큼 위에 위치
        var CamPos = target.position - (target.forward * distance) + (target.up * Height);
        CamTr.position = Vector3.Slerp(CamTr.position, CamPos, Time.deltaTime * movedamping);
                  // 곡면 보간 (자기 자신 위치에서, CamPos까지, damping 시간만큼 부드럽게 움직임)
        CamTr.rotation = Quaternion.Slerp(CamTr.rotation, target.rotation, Time.deltaTime * rotdamping);
        // 곡면 보간으로 회전 (자기 자신 로테이션에서, 타겟 로테이션까지, damping 시간만큼 부드럽게 회전)
        CamTr.LookAt(target.position + (target.up * targetOffset));
             // 타겟 포지션에서 2만큼 위로 올림
    }
    private void OnDrawGizmos()  // 싼 화면에서 색상이나 선을 그려주는 함수 콜백함수
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(target.position + (target.up * targetOffset), 0.1f);
               // 색상
        Gizmos.DrawLine(target.position + (target.up * targetOffset), CamTr.position);
               // 선
    }
}
