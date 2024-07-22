using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelCtfl : MonoBehaviour
{
    [SerializeField] private GameObject Effect; // ���� ����Ʈ
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
    #region ������Ÿ�� ����� �浹 ����
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
        Vector3 FirePos = (Vector3)_params[0];   // �߻� ��ġ
        Vector3 hitPos = (Vector3)_params[1];    // ���� ��ġ
        // �Ÿ��� ����       = ���� ��ǥ���� �߻� ��ġ�� ��
        Vector3 incomeVector = hitPos - FirePos; // Ray�� ������ ���ϱ� ����
        // ���� ���δ� �Ի� ���Ͷ�� �Ѵ�.
        incomeVector = incomeVector.normalized;  // �Ի� ���͸� ����ȭ ���ͷ� ����
        GetComponent<Rigidbody>().AddForceAtPosition(incomeVector * 1500f, hitPos);
        // Ray�� hit ��ǥ�� �Ի� ������ ������ ���� ����
        // � ������ ���� ��Ƽ� ������ �����ǰ� �Ϸ��� �Ҷ� ȣ�� �Ǵ� �޼���
        if (++hitCount == 5)
            ExplosionBarrel();
    }
    void ExplosionBarrel()
    {
        GameObject eff = Instantiate(Effect, transform.position, Quaternion.identity);
        Destroy(eff, 2.0f);
        Collider[] Cols = Physics.OverlapSphere(transform.position, 20f, 1 << 7);
        // �跲 �ڱ��ڽ� ��ġ���� 20�ݰ濡 �跲 ���̾ Cols��� �迭�� ��´�.
        foreach (Collider col in Cols)
        {
            Rigidbody rigidbody = col.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.mass = 1.0f; // �跲 ���Ը� ������
                rigidbody.AddExplosionForce(1000, transform.position, 20f, 1200f);
                // �������ٵ� Ŭ���� ���� �Լ��� AddExplosionForce
                // (���ķ�, ��ġ, �ݰ�, ���� �ڱ�ġ�� ��)
            }
            else
            {
                Invoke("BarrelMassChange()", 3.0f);
            }
            SoundManager.S_instance.PlaySound(transform.position, FireClip);
            // ��׷��� �޽��� ����
            int idx = Random.Range(0, meshes.Length);
            meshFilter.sharedMesh = meshes[idx];
            GetComponent<MeshCollider>().sharedMesh = meshes[Random.Range(0, meshes.Length)];
        }
    }
    void BarrelMassChange()
    {
        Collider[] Cols = Physics.OverlapSphere(transform.position, 20f, 1 << 7);
        // �跲 �ڱ��ڽ� ��ġ���� 20�ݰ濡 �跲 ���̾ Cols��� �迭�� ��´�.
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
