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
        #region Profectile MoveMent 방식
        //// 오브젝트 풀링이 아닐때
        ////Instantiate(bulletPrefab, FirePos.position, FirePos.rotation);
        //// 오브젝트 풀링 방실 일때 로직
        //var _bullet = ObjectPoolingManager.poolingManager.GetBulletPool();
        //if (_bullet != null)
        //{
        //    _bullet.transform.position = FirePos.position;
        //    _bullet.transform.rotation = FirePos.rotation;
        //    _bullet.SetActive(true);
        //}
        #endregion
        RaycastHit hit;  // 광선이 오브젝트에 맞으면 충돌 지점이나
                         // 거리등을 알려주는 광선 구조체
                         // 광선을 쏘았을 때 맞았는 가의 여부
        if (Physics.Raycast(FirePos.position, FirePos.forward, out hit, 15f))
        {      // 태그 검사
            if (hit.collider.gameObject.tag == EnemyTag)
            {
                Debug.Log("맞았나?");
                object[] _params = new object[2];
                _params[0] = hit.point;  // 첫번째 배열에은 맞은 위치를 전달
                _params[1] = 25f;        // 데미지 값을 배열에 전달
                // 광선에 맞은 오브젝트의 함수를 호출 하면서 매개변수 값을 전달
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
                _params[0] = FirePos.position;  // 발사 위치
                _params[1] = hit.point;         // 맞은 위치
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
