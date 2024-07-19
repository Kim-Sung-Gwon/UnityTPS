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
    void Start()
    {
        player = GetComponent<Player>();
        source = GetComponent<AudioSource>();
        fireClip = Resources.Load("Sounds/GunShot") as AudioClip;
        muzzleFalsh.Stop();
    }

    void Update()
    {
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
        // 오브젝트 풀링이 아닐때
        //Instantiate(bulletPrefab, FirePos.position, FirePos.rotation);
        // 오브젝트 풀링 방실 일때 로직
        var _bullet = ObjectPoolingManager.poolingManager.GetBulletPool();
        if (_bullet != null)
        {
            _bullet.transform.position = FirePos.position;
            _bullet.transform.rotation = FirePos.rotation;
            _bullet.SetActive(true);
        }
        Invoke("muzzleFalshFire", 0.1f);
        muzzleFalsh.Play();
    }
    void muzzleFalshFire()
    {
        muzzleFalsh.Stop();
    }
}
