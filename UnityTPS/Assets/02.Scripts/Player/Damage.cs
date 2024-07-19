using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField] private readonly string E_bulletTag = "E_BULLET";
    [SerializeField] private GameObject bloodEffect;
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
        }
    }
}
