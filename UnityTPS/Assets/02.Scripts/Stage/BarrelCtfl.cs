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
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private Mesh[] meshes;
    private readonly string bulletTag = "BULLET";
    void Start()
    {
        rd = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();
        meshes = Resources.LoadAll<Mesh>("Meshes");
        //textures = Resources.LoadAll("BarrelTextures") as Texture[];
        textures = Resources.LoadAll<Texture>("BarrelTextures");
        meshRenderer.material.mainTexture = textures[Random.Range(0, textures.Length)];
        Effect = Resources.Load("ExpEffect") as GameObject;
        FireClip = Resources.Load("Sounds/grenadeSound") as AudioClip;
    }
    #region 프로젝타일 방식의 충돌 감지
    //private void OnCollisionEnter(Collision col)
    //{
    //    if (col.gameObject.CompareTag(bulletTag))
    //    {
    //        if (++hitCount == 5)
    //        {
    //            ExplosionBarrel();
    //        }
    //    }
    //}
    #endregion
    void OnDamage(object[] _params)
    {
        Vector3 FirePos = (Vector3)_params[0];   // 발사 위치
        Vector3 hitPos = (Vector3)_params[1];    // 맞은 위치
        // 거리와 방향       = 맞은 좌표에서 발사 위치의 차
        Vector3 incomeVector = hitPos - FirePos; // Ray의 각도를 구하기 위해
        // 전문 용어로는 입사 벡터라고 한다.
        incomeVector = incomeVector.normalized;  // 입사 벡터를 정교화 벡터로 변경
        GetComponent<Rigidbody>().AddForceAtPosition(incomeVector * 1500f, hitPos);
        // Ray의 hit 좌표에 입사 벡터의 각도로 힘을 생성
        // 어떤 지점에 힘을 모아서 무리가 생성되게 하려고 할때 호출 되는 메서드
        if (++hitCount == 5)
            ExplosionBarrel();
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
            SoundManager.S_instance.PlaySound(transform.position, FireClip);
            // 찌그러진 메쉬를 적용
            int idx = Random.Range(0, meshes.Length);
            meshFilter.sharedMesh = meshes[idx];
            GetComponent<MeshCollider>().sharedMesh = meshes[Random.Range(0, meshes.Length)];
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
