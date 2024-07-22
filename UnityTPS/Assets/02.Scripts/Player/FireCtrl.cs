using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCtrl : MonoBehaviour
{
    
    public Transform FirePos;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip fireClip;
    [SerializeField] private float fireTime;
    [SerializeField] private Player player;
    [SerializeField] private ParticleSystem muzzleFalsh;
    private readonly string EnemyTag = "ENEMY";
    private readonly string BarrelTag = "Barrel";
    private readonly string WallTag = "Wall";
    void Start()
    {
        player = GetComponent<Player>();
        source = GetComponent<AudioSource>();
        fireClip = Resources.Load("Sounds/GunShot") as AudioClip;
        muzzleFalsh.Stop();
    }

    void Update()
    {
        Debug.DrawRay(FirePos.position, FirePos.forward * 100f, Color.red);

        if (Input.GetMouseButton(0))
        {
            if (Time.time - fireTime > 0.2f)
            {
                if (!player.siRunning)
                    Fire();
                fireTime = Time.time;
                source.PlayOneShot(fireClip, 1.0f);
            }
        }
    }

    private void Fire()
    {
        #region Profectile MoveMent ���
        //// ������Ʈ Ǯ���� �ƴҶ�
        ////Instantiate(bulletPrefab, FirePos.position, FirePos.rotation);
        //// ������Ʈ Ǯ�� ��� �϶� ����
        //var _bullet = ObjectPoolingManager.poolingManager.GetBulletPool();
        //if (_bullet != null)
        //{
        //    _bullet.transform.position = FirePos.position;
        //    _bullet.transform.rotation = FirePos.rotation;
        //    _bullet.SetActive(true);
        //}
        #endregion
        RaycastHit hit;  // ������ ������Ʈ�� ������ �浹 �����̳�
                         // �Ÿ����� �˷��ִ� ���� ����ü
                         // ������ ����� �� �¾Ҵ� ���� ����
        if (Physics.Raycast(FirePos.position, FirePos.forward, out hit, 15f))
        {      // �±� �˻�
            if (hit.collider.gameObject.tag == EnemyTag)
            {
                Debug.Log("�¾ҳ�?");
                object[] _params = new object[2];
                _params[0] = hit.point;  // ù��° �迭���� ���� ��ġ�� ����
                _params[1] = 25f;        // ������ ���� �迭�� ����
                // ������ ���� ������Ʈ�� �Լ��� ȣ�� �ϸ鼭 �Ű����� ���� ����
                hit.collider.gameObject.SendMessage("OnDamage", _params,
                    SendMessageOptions.DontRequireReceiver);
            }
            if (hit.collider.gameObject.tag == WallTag)
            {
                object[] _params = new object[2];
                _params[0] = hit.point;
                hit.collider.gameObject.SendMessage("OnDamage", _params,
                    SendMessageOptions.DontRequireReceiver);
            }
            if (hit.collider.gameObject.tag == BarrelTag)
            {
                object[] _params = new object[2];
                _params[0] = FirePos.position;  // �߻� ��ġ
                _params[1] = hit.point;         // ���� ��ġ
                hit.collider.gameObject.SendMessage("OnDamage", _params,
                    SendMessageOptions.DontRequireReceiver);
            }
        }
        Invoke("muzzleFalshFire", 0.1f);
        muzzleFalsh.Play();
    }
    void muzzleFalshFire()
    {
        muzzleFalsh.Stop();
    }
}
