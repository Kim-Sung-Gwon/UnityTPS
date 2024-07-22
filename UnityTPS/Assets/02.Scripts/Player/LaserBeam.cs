using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    private Transform tr;
    private LineRenderer line;
    [SerializeField] private Transform fireFos;
    void Start()
    {
        tr = transform;
        line = GetComponent<LineRenderer>();
        line.useWorldSpace = false;
        line.enabled = false;
        fireFos = transform.GetComponentsInParent<Transform>()[1];
    }
    void Update()
    {   // 광선을 미리 생성 (위치, 방향)
        Ray ray = new Ray(fireFos.position, tr.forward);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.blue);
        if (Input.GetMouseButton(0))
        {   // 라인 랜더러의 첫번째 점의 위치 설정
            line.SetPosition(0, tr.InverseTransformPoint(ray.origin));
            // 월드 좌표 포지션을 로컬 좌표로 포지션으로 변경
            // 어떤 물체에 광선이 맞았을 때의 위치를 Line Renderer의 끝점으로 설정
            if (Physics.Raycast(ray, out hit, 100f))
            {
                line.SetPosition(1, tr.InverseTransformPoint(hit.point));
            }
            else // 맞지 않았을 때 끝점을 100으로 잡는다.
            {
                line.SetPosition(1, tr.InverseTransformPoint(ray.GetPoint(100f)));
            }
            StartCoroutine(ShowRaserBaem());
        }
    }

    IEnumerator ShowRaserBaem()
    {
        line.enabled = true;
        yield return new WaitForSeconds(0.1f);
        line.enabled = false;
    }
}
