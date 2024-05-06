using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;

    [Header("Attributes")]
    [SerializeField] private int baseEnemies = 8;
    [SerializeField] private float enemiesPerSecond = 0.5f;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private float difficultyScalingFactor = 0.75f;
    [SerializeField] private float enemiesPerSecoundCap = 15f;
    [SerializeField] private int[] allowedEnemyIndices;


    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();

    private int currentWave = 1;
    private float timeSinceLastSpawn;
    private int enemiesAlive;
    private int enemiesLeftToSpawn;
    private float eps;
    private bool isSpawning = false;

    public int GetCurrentWave()
    {
        return currentWave;
    }
    private void Awake()
    {
        onEnemyDestroy.AddListener(EnemyDestroyed);
    }

    private void Start()
    {
        StartCoroutine(WaitForInputToStartWave());
    }
    private IEnumerator WaitForInputToStartWave()
    {
        // Czekamy na naciœniêcie klawisza "Space"
        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }

        // Po naciœniêciu "Space" rozpoczynamy falê
        StartCoroutine(StartWave());
    }

    private void Update()
    {
        if (!isSpawning) return;
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= (1f / eps) && enemiesLeftToSpawn > 0)
        {
            SpawnEnemy();
            enemiesLeftToSpawn--;
            enemiesAlive++;
            timeSinceLastSpawn = 0f;
        }

        if (enemiesAlive == 0 && enemiesLeftToSpawn == 0)
        {
            EndWave();
        }
    }

    private void EnemyDestroyed()
    {
        enemiesAlive--;
    }

    private void SpawnEnemy()
    {
        int allowedIndex = Random.Range(0, allowedEnemyIndices.Length);
        int prefabIndex = allowedEnemyIndices[allowedIndex];

        GameObject prefabToSpawn = enemyPrefabs[prefabIndex];
        Instantiate(prefabToSpawn, LevelManager.main.startPoint.position, Quaternion.identity);
    }

    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);

        isSpawning = true;
        enemiesLeftToSpawn = EnemiesPerWave();
        eps = EnemiesPerSecound();
        UpdateAllowedEnemies();
        //Debug.Log("Wave: " + currentWave);
    }

    private void EndWave()
    {
        isSpawning = false;
        timeSinceLastSpawn = 0f;
        currentWave++;
        StartCoroutine(StartWave());
    }

    private int EnemiesPerWave()
    {
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultyScalingFactor));
    }
    private float EnemiesPerSecound()
    {
        return Mathf.Clamp(enemiesPerSecond * Mathf.Pow(currentWave, difficultyScalingFactor), 0f, enemiesPerSecoundCap);
    }
    private void UpdateAllowedEnemies()
    {

        if (currentWave < 3)
        {
            allowedEnemyIndices = new int[] { 0 }; 
        }
        else if (currentWave <= 5 && currentWave >= 3)
        {
            allowedEnemyIndices = new int[] { 0, 2,4,5,0 }; 
        }
        else if (currentWave > 5 && currentWave <= 8)
        {
            allowedEnemyIndices = new int[] { 0,1,2,0,0 };
        }
        else if (currentWave >= 15 && currentWave < 18)
        {
            allowedEnemyIndices = new int[] {3,1};
        }
        else if (currentWave >= 18)
        {
            allowedEnemyIndices = new int[] { 3  };
        }
        else
        {
            allowedEnemyIndices = new int[] { 0, 1, 2, 3 };
        }
    }
}
