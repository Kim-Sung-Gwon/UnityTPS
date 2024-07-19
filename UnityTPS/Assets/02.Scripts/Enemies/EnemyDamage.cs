using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] private readonly string bulletTag = "BULLET";
    [SerializeField] private GameObject bloodEffect;
    private float hp = 100f;
    void Start()
    {
        bloodEffect = Resources.Load("Effects/BulletImpactFleshBigEffect") as GameObject;
    }
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag(bulletTag))
        {
            col.gameObject.SetActive(false);

            ShowBloodEffect(col);

            hp -= col.gameObject.GetComponent<Bullet>().Damage;
            hp = Mathf.Clamp(hp, 0f, 100f);
            if (hp <= 0f)
                Die();
        }
    }
    void Die()
    {
        Debug.Log("���!");
        GetComponent<EnemyAI>().state = EnemyAI.State.DIE;
    }

    private void ShowBloodEffect(Collision col)
    {
        // ���� ��ġ�� �ѱ� Collision ����ü �ȿ� contacts �迭�� �ִ�.
        Vector3 pos = col.contacts[0].point;      // point : ��ġ

        // ���� ������ �ѱ�
        Vector3 _normal = col.contacts[0].normal; // noraml : ����

        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, _normal);

        GameObject blood = Instantiate(bloodEffect, pos, rot);
        Destroy(blood, 1.0f);
    }
}
