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
    {   // ������ �̸� ���� (��ġ, ����)
        Ray ray = new Ray(fireFos.position, tr.forward);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.blue);
        if (Input.GetMouseButton(0))
        {   // ���� �������� ù��° ���� ��ġ ����
            line.SetPosition(0, tr.InverseTransformPoint(ray.origin));
            // ���� ��ǥ �������� ���� ��ǥ�� ���������� ����
            // � ��ü�� ������ �¾��� ���� ��ġ�� Line Renderer�� �������� ����
            if (Physics.Raycast(ray, out hit, 100f))
            {
                line.SetPosition(1, tr.InverseTransformPoint(hit.point));
            }
            else // ���� �ʾ��� �� ������ 100���� ��´�.
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
