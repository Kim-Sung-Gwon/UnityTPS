using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFire : MonoBehaviour
{
    [SerializeField] private AudioClip fireClip;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform playerTr;  // ������ ���ϱ� ���ؼ�
    [SerializeField] private Transform enemyTr;   // ������ ���ϱ� ���ؼ�
    [SerializeField] private Transform firePos;
    private readonly int hashFire = Animator.StringToHash("FireTrigger");
    private readonly int hashReload = Animator.StringToHash("ReloedTrigger");
    private float nextFire = 0.0f;  // ���� �ð��� �߻��� �ð� ���� ����
    private readonly float fireRate = 0.1f;  // �Ѿ� �߻� ����
    private readonly float damping = 10.0f;  // �÷��̾ ��� ȸ���� �ӵ�
    public bool isFire = false;
    [Header("Reload")]
    [SerializeField] private readonly float reloadTime = 2.0f;  // ������ �ð�
    [SerializeField] private readonly int maxBullet = 10;       // 10���϶� ������ �ϱ����� Max��
    [SerializeField] private int curBullet = 0;                 // ���� �Ѿ� ��
    [SerializeField] private bool isReload = false;             // ������ ����
    [SerializeField] private WaitForSeconds reloadWs;           // ��ŸƮ �ڷ�ƾ������ �ð��� ���� ����
    [SerializeField] private AudioClip reloadClip;              // ������ ����
    public MeshRenderer mezzleFlash;
    void Start()
    {
        firePos = transform.GetChild(3).GetChild(0).GetChild(0).transform;
        animator = GetComponent<Animator>();
        enemyTr = GetComponent<Transform>();
        playerTr = GameObject.FindGameObjectWithTag("Player").transform;
        fireClip = Resources.Load("Sounds/p_m4_1_1") as AudioClip;
        reloadClip = Resources.Load("Sounds/p_reload_1_1") as AudioClip;
        curBullet = maxBullet;
        reloadWs = new WaitForSeconds(reloadTime);
        mezzleFlash = transform.GetChild(3).GetChild(0).GetChild(0).GetChild(0).
            GetComponent<MeshRenderer>();
        mezzleFlash.enabled = false;
    }
    void Update()
    {
        if (isFire && !isReload)
        {
            if (Time.time >= nextFire)
            {
                Fire();
                nextFire = Time.time + fireRate + Random.Range(0.0f, 0.3f);
            }
            Vector3 playerNoraml = playerTr.position - enemyTr.position;
            Quaternion rot = Quaternion.LookRotation(playerNoraml.normalized);
            enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot,
                damping * Time.deltaTime);
        }
    }
    private void Fire()
    {
        var E_bullet = ObjectPoolingManager.poolingManager.GetE_BulletPool();
        E_bullet.transform.position = firePos.position;
        E_bullet.transform.rotation = firePos.rotation;
        E_bullet.SetActive(true);

        animator.SetTrigger(hashFire);
        SoundManager.S_instance.PlaySound(firePos.position, fireClip);

        isReload = (--curBullet % maxBullet) == 0;
        if (isReload)
        {
            StartCoroutine(Reloading());
        }
        StartCoroutine(ShowMuzzleFlach());
    }
    IEnumerator Reloading()
    {
        animator.SetTrigger(hashReload);  // ������ �ִϸ��̼� ����
        SoundManager.S_instance.PlaySound(transform.position, reloadClip);
                                // ������ ���� ����
        yield return reloadWs;  // ������ �ð���ƴ ����ϴ� ���� ����� �纸
        curBullet = maxBullet;  // ���� �Ѿ��� �ٽ� 10�߷�
        isReload = false;       // ������ �Һ����� false��
    }
    IEnumerator ShowMuzzleFlach()
    {
        mezzleFlash.enabled = true;

        mezzleFlash.transform.localScale = Vector3.one * Random.Range(1.5f, 2.3f);
        Quaternion rot = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
        mezzleFlash.transform.localRotation = rot;
        yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
        mezzleFlash.enabled = false;
    }
}
