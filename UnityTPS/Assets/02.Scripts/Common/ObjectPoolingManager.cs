using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour
{
    public static ObjectPoolingManager poolingManager;
    [Header("Bullet")]
    [SerializeField] private GameObject bulletPrefab;   // �Ѿ� ������
    [SerializeField] private GameObject E_bullet;
    public int maxPool = 10;     // ������Ʈ Ǯ�� ���� ����
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
            poolingManager = this; // �ؿ� �Լ��� ����
                                   // poolingManager = GetComponent<ObjectPoolingManager>();
        else if (poolingManager != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        bulletPrefab = Resources.Load("Bullet") as GameObject;
        E_bullet = Resources.Load("E_Bullet") as GameObject;
        EnemyPrefab = Resources.Load<GameObject>("Enemy");

        CreateBulletPool();  // ������Ʈ Ǯ�� ���� �Լ�
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
            enemyObj.name = $"{(i + 1).ToString()} ��";
            enemyObj.SetActive(false);
            EnemyPoolList.Add(enemyObj);
        }
    }

    void CreateBulletPool()
    {  // ���� ������Ʈ ����
        GameObject playerBulletGroup = new GameObject("PlayerBulletGroup");
        for (int i = 0; i < maxPool; i++)
        {
            var _buttet = Instantiate(bulletPrefab, playerBulletGroup.transform);
            _buttet.name = $"{(i + 1).ToString()} ��";
            _buttet.SetActive(false);
            bulletPoolList.Add(_buttet);
        }
    }
    public GameObject GetBulletPool()
    {
        for (int i = 0; i < bulletPoolList.Count; i++)
        {    // ��Ȱ��ȭ �Ǿ��ٸ� activeSelf�� Ȱ��ȭ ��Ȱ��ȭ ���θ� �˷���
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
            E_buttet.name = $"{(i + 1).ToString()} ��";
            E_buttet.SetActive(false);
            E_bulletPoolList.Add(E_buttet);
        }
    }
    public GameObject GetE_BulletPool()
    {
        for (int i = 0; i < E_bulletPoolList.Count; i++)
        {    // ��Ȱ��ȭ �Ǿ��ٸ� activeSelf�� Ȱ��ȭ ��Ȱ��ȭ ���θ� �˷���
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
            // ������ ���� �Ǹ� �ڷ�ƾ�� �����ؼ� ���� ��ƾ�� ���� ���� ����
            foreach (GameObject _enemy in EnemyPoolList)
            {
                if (_enemy.activeSelf == false)
                {
                    int idx = Random.Range(0, EnemyPoolList.Count-1);
                    _enemy.transform.position = SpawnPointsList[idx].position;// �¾�� ��ġ
                    _enemy.transform.rotation = SpawnPointsList[idx].rotation;
                    _enemy.gameObject.SetActive(true);
                    break;
                }
            }
        }
    }
}
