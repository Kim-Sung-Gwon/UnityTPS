using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform tr;
    [SerializeField] private TrailRenderer trRenderer;
    public float Speed = 1500f;
    public float Damage = 25;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        trRenderer = GetComponent<TrailRenderer>();
        //Destroy(this.gameObject, 3.0f);
        Invoke("BulletDisable", 2.0f);
    }
    void BulletDisable()
    {
        this.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        rb.AddForce(transform.forward * Speed);
    }
    private void OnDisable()
    {
        trRenderer.Clear();
        //tr.position = Vector3.zero;
        tr.rotation = Quaternion.identity;
        rb.Sleep();
    }
}
