using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField] private readonly string E_bulletTag = "E_BULLET";
    [SerializeField] private GameObject bloodEffect;
    public float hp = 100f;
    void Start()
    {
        bloodEffect = Resources.Load("Effects/BulletImpactFleshBigEffect") as GameObject;
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag(E_bulletTag))
        {
            col.gameObject.SetActive(false);
            Vector3 pos = col.contacts[0].point;
            Vector3 _normal = col.contacts[0].normal;
            Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, _normal);
            GameObject blood = Instantiate(bloodEffect, pos, rot);
            Destroy(blood, 1.0f);
            hp -= 15f;
            hp = Mathf.Clamp(hp, 0f, 100f);
            if (hp <= 0f)
                PlayerDie();
        }
    }
    void PlayerDie()
    {
        Debug.Log("Player»ç¸Á!");
    }
}
