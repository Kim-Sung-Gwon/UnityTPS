using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelCtfl : MonoBehaviour
{
    [SerializeField] private GameObject Effect; // 폭파 이펙트
    [SerializeField] public AudioSource source;
    [SerializeField] public AudioClip FireClip;
    [SerializeField] private Texture[] textures;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Rigidbody rd;
    [SerializeField] private int hitCount = 0;
    private readonly string bulletTag = "BULLET";
    void Start()
    {
        rd = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        //textures = Resources.LoadAll("BarrelTextures") as Texture[];
        textures = Resources.LoadAll<Texture>("BarrelTextures");
        meshRenderer.material.mainTexture = textures[Random.Range(0, textures.Length)];
        Effect = Resources.Load("ExpEffect") as GameObject;
        FireClip = Resources.Load("Sounds/grenadeSound") as AudioClip;
    }
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag(bulletTag))
        {
            if (++hitCount == 5)
            {
                ExplosionBarrel();
            }
        }
    }
    void ExplosionBarrel()
    {
        GameObject eff = Instantiate(Effect, transform.position, Quaternion.identity);
        Destroy(eff, 2.0f);
        Collider[] Cols = Physics.OverlapSphere(transform.position, 20f, 1 << 7);
        // 배럴 자기자신 위치에서 20반경에 배럴 레이어만 Cols라는 배열에 담는다.
        foreach (Collider col in Cols)
        {
            Rigidbody rigidbody = col.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.mass = 1.0f; // 배럴 무게를 가볍게
                rigidbody.AddExplosionForce(1000, transform.position, 20f, 1200f);
                // 리디지바디 클래스 폭파 함수는 AddExplosionForce
                // (폭파력, 위치, 반경, 위로 솟구치는 힘)
            }
            else
            {
                Invoke("BarrelMassChange()", 3.0f);
            }
            Vector3 HitPos = col.transform.position;
            SoundManager.S_instance.PlaySound(HitPos, FireClip);
        }
    }
    void BarrelMassChange()
    {
        Collider[] Cols = Physics.OverlapSphere(transform.position, 20f, 1 << 7);
        // 배럴 자기자신 위치에서 20반경에 배럴 레이어만 Cols라는 배열에 담는다.
        foreach (Collider col in Cols)
        {
            Rigidbody rigidbody = col.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.mass = 60.0f;
            }
        }
    }
}
