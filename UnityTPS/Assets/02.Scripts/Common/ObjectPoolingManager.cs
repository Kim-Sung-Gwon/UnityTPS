using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour
{
    public static ObjectPoolingManager poolingManager;
    [Header("Bullet")]
    [SerializeField] private GameObject bulletPrefab;   // 총알 프리펩
    [SerializeField] private GameObject E_bullet;
    public int maxPool = 10;     // 오브젝트 풀에 생성 갯수
    public List<GameObject> bulletPoolList;
    public List<GameObject> E_bulletPoolList;
    [Header("EnemyObjectPool")]
    public GameObject EnemyPrefab;
    public List<GameObject> EnemyPoolList;
    public List<Transform> SpawnPointsList;
    void Awake()  // Awake >> OnEnable >> Start
    {
        bulletPrefab = GetComponent<GameObject>();
        if (poolingManager == null)
            poolingManager = this; // 밑에 함수와 같음
                                   // poolingManager = GetComponent<ObjectPoolingManager>();
        else if (poolingManager != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        bulletPrefab = Resources.Load("Bullet") as GameObject;
        E_bullet = Resources.Load("E_Bullet") as GameObject;
        EnemyPrefab = Resources.Load<GameObject>("Enemy");

        CreateBulletPool();  // 오브젝트 풀링 생성 함수
        CreateE_BulletPool();
        CreateEnemyPool();
    }

    private void Start()
    {
        var spawnPoint = GameObject.Find("SpawnPoints");
        if (spawnPoint != null)
            spawnPoint.GetComponentsInChildren<Transform>(SpawnPointsList);
        SpawnPointsList.RemoveAt(0);
        if (SpawnPointsList.Count > 0)
            StartCoroutine(CreateEnemy());
    }

    private void CreateEnemyPool()
    {
        GameObject EnemyGroup = new GameObject("EnemyGroup");
        for (int i = 0; i < maxPool; i++)
        {
            var enemyObj = Instantiate(EnemyPrefab, EnemyGroup.transform);
            enemyObj.name = $"{(i + 1).ToString()} 명";
            enemyObj.SetActive(false);
            EnemyPoolList.Add(enemyObj);
        }
    }

    void CreateBulletPool()
    {  // 게임 오브젝트 생성
        GameObject playerBulletGroup = new GameObject("PlayerBulletGroup");
        for (int i = 0; i < maxPool; i++)
        {
            var _buttet = Instantiate(bulletPrefab, playerBulletGroup.transform);
            _buttet.name = $"{(i + 1).ToString()} 발";
            _buttet.SetActive(false);
            bulletPoolList.Add(_buttet);
        }
    }
    public GameObject GetBulletPool()
    {
        for (int i = 0; i < bulletPoolList.Count; i++)
        {    // 비활성화 되었다면 activeSelf는 활성화 비활성화 여부를 알려줌
            if (bulletPoolList[i].activeSelf == false)
            {
                return bulletPoolList[i];
            }
        }
        return null;
    }
    void CreateE_BulletPool()
    {
        GameObject EnemyBulletGroup = new GameObject("EnemyBulletGroup");
        for (int i = 0; i < 10; i++)
        {
            var E_buttet = Instantiate(E_bullet, EnemyBulletGroup.transform);
            E_buttet.name = $"{(i + 1).ToString()} 발";
            E_buttet.SetActive(false);
            E_bulletPoolList.Add(E_buttet);
        }
    }
    public GameObject GetE_BulletPool()
    {
        for (int i = 0; i < E_bulletPoolList.Count; i++)
        {    // 비활성화 되었다면 activeSelf는 활성화 비활성화 여부를 알려줌
            if (E_bulletPoolList[i].activeSelf == false)
            {
                return E_bulletPoolList[i];
            }
        }
        return null;
    }
    IEnumerator CreateEnemy()
    {
        while (!GameManager.G_instance.isGameOver)
        {
            yield return new WaitForSeconds(3f);
            if(GameManager.G_instance.isGameOver) yield break;
            // 게임이 종료 되면 코루틴을 종료해서 다음 루틴을 진행 하지 않음
            foreach (GameObject _enemy in EnemyPoolList)
            {
                if (_enemy.activeSelf == false)
                {
                    int idx = Random.Range(0, EnemyPoolList.Count-1);
                    _enemy.transform.position = SpawnPointsList[idx].position;// 태어나는 위치
                    _enemy.transform.rotation = SpawnPointsList[idx].rotation;
                    _enemy.gameObject.SetActive(true);
                    break;
                }
            }
        }
    }
}
