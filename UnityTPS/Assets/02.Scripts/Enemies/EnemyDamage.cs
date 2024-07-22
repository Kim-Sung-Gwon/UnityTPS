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
    #region 프로젝타일 방식의 충돌 감지 isTrigger 체크된 경우 onTriggerEnter
    //private void OnCollisionEnter(Collision col)
    //{
    //    if (col.gameObject.CompareTag(bulletTag))
    //    {
    //        col.gameObject.SetActive(false);

    //        ShowBloodEffect(col);

    //        hp -= col.gameObject.GetComponent<Bullet>().Damage;
    //        hp = Mathf.Clamp(hp, 0f, 100f);
    //        if (hp <= 0f)
    //            Die();
    //    }
    //}
    #endregion

    void OnDamage(object[] _params)
    {
        ShowBloodEffect((Vector3)_params[0]);
        hp -= (float)_params[1];
        hp = Mathf.Clamp(hp, 0f, 100f);
        if (hp <= 0f)
            Die();
    }

    void Die()
    {
        Debug.Log("사망!");
        GetComponent<EnemyAI>().state = EnemyAI.State.DIE;
    }

    private void ShowBloodEffect(Vector3 col)
    {
        // 맞은 위치를 넘김 Collision 구조체 안에 contacts 배열이 있다.
        Vector3 pos = col;      // point : 위치
        // 맞은 방항을 넘김
        Vector3 _normal = col.normalized; // noraml : 방향
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, _normal);
        GameObject blood = Instantiate(bloodEffect, pos, rot);
        Destroy(blood, 1.0f);
    }
}
