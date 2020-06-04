using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public bool devMode;

    public Wave[] waves;
    public Enemy enemy;

    LivingEntity playerEntity;
    Transform playerT;

    Wave currentWave;
    int currentWaveNumber;

    int enemiesRemainingToSpawn;
    int enemiesRemainingAlive;
    float nextSpawnTime;

    MapGenerator map;

    float timeBtwCampingChecks = 2f;
    float campThresholdDist = 1.5f;
    float nextCampCheckTime;
    Vector3 campPosOld;
    bool isCamping;

    bool isDisabled;

    //public Material skinC;

    public event System.Action<int> OnNewWave;


    private void Start()
    {
        playerEntity = FindObjectOfType<Player>();
        playerT = playerEntity.transform;

        nextCampCheckTime = timeBtwCampingChecks + Time.time;
        campPosOld = playerT.position;
        playerEntity.OnDeath += OnPlayerDeath;

        map = FindObjectOfType<MapGenerator>();
        NextWave();
    }

    private void Update()
    {
        if (!isDisabled)
        {
            if (Time.time > nextCampCheckTime)
            {
                nextCampCheckTime = Time.time + timeBtwCampingChecks;

                isCamping = (Vector3.Distance(playerT.position, campPosOld) < campThresholdDist);
                campPosOld = playerT.position;
            }

            if ((enemiesRemainingToSpawn > 0 || currentWave.infinite) && Time.time > nextSpawnTime)
            {
                enemiesRemainingToSpawn--;
                nextSpawnTime = Time.time + currentWave.timeBtwSpawns;
                StartCoroutine("SpawnEnemy");
            }
        }
        //skinC.color = currentWave.skinColor;

        if (devMode)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                StopCoroutine("SpawnEnemy");
                foreach (Enemy enemy in FindObjectsOfType<Enemy>())
                {
                    GameObject.Destroy(enemy.gameObject);
                }
                NextWave();
            }
        }
    }

    IEnumerator SpawnEnemy()
    {
        float spawnDelay = 1;
        float tileFlashSpeed = 0;

        if (currentWaveNumber % 2 == 0)
        {
            tileFlashSpeed = currentWaveNumber;
        }
        else
        {
            tileFlashSpeed = currentWaveNumber + 1;
        }

        Transform spawnTile = map.GetRandomOpenTile();
        if (isCamping)
        {
            spawnTile = map.GetTileFromPos(playerT.position);
        }

        Transform randomTile = map.GetRandomOpenTile();
        Material tileMat = randomTile.GetComponent<Renderer>().material;
        Color initialColor = tileMat.color;
        Color flashColor = Color.red;
        float spawnTimer = 0;

        while(spawnTimer < spawnDelay)
        {
            tileMat.color = Color.Lerp(initialColor, flashColor, Mathf.PingPong(spawnTimer * tileFlashSpeed, 1));

            spawnTimer += Time.deltaTime;
            yield return null;
        }

        Enemy spawnedEnemy = Instantiate(enemy, randomTile.position + Vector3.up, Quaternion.identity) as Enemy;
        spawnedEnemy.OnDeath += OnEnemyDeath;
        spawnedEnemy.SetCharacteristics(currentWave.moveSpeed, currentWave.hitsToKill, currentWave.enemyHealth, currentWave.skinColor);
    }

    void OnPlayerDeath()
    {
        isDisabled = true;
    }

    void OnEnemyDeath()
    {
        enemiesRemainingAlive--;

        if(enemiesRemainingAlive == 0)
        {
            NextWave();
        }
    }

    void ResetPlayerPos()
    {
        playerT.position = map.GetTileFromPos(Vector3.zero).position + Vector3.up * 1.4f ;
    }

    void NextWave()
    {
        bool canCheer = (PlayerPrefs.GetInt("Full Screen") == 1) ? true : false;
        if (canCheer)
        {
            if (currentWaveNumber > 0)
            {
                AudioManager.instance.PlaySound2D("Level Complete");
            }
        }

        currentWaveNumber++;

        if (currentWaveNumber - 1 < waves.Length)
        {
            currentWave = waves[currentWaveNumber - 1];
        }

        enemiesRemainingToSpawn = currentWave.enemyCount;
        enemiesRemainingAlive = enemiesRemainingToSpawn;

        OnNewWave?.Invoke(currentWaveNumber);

        ResetPlayerPos();
    }

    [System.Serializable]
    public class Wave
    {
        public bool infinite;
        public int enemyCount;
        public float timeBtwSpawns;
        public float moveSpeed;
        public int hitsToKill;
        public float enemyHealth;
        public Color skinColor;
    }
}
