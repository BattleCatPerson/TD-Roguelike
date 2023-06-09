using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    public static bool doneSpawning;

    [SerializeField] List<EnemyWave> waves;
    [SerializeField] int currentWaveIndex;
    [SerializeField] EnemyWave currentWave;
    [SerializeField] float timeBetweenWaves;
    [SerializeField] float timeBetweenWavesCurrent;

    [SerializeField] List<EnemyPathfinding> currentEnemies;
    [SerializeField] float currentTimeBetweenEnemies;
    [SerializeField] float countdown;
    [SerializeField] int currentEnemyIndex;
    [SerializeField] bool stop;

    [SerializeField] string mapScene;
    [SerializeField] GameObject continuePanel;
    private void Awake()
    {
        doneSpawning = false;
        HealthManager.onDeath += OnDeath;
    }
    void Start()
    {
        continuePanel.SetActive(false);
        transform.position = new(CarvePath.Path[0].position.x, CarvePath.Path[0].position.y + 2, CarvePath.Path[0].position.z);
        ChangeWave();
        timeBetweenWavesCurrent = timeBetweenWaves;
    }

    void Update()
    {
        if (stop || SpawnTower.instance.BuildPhase) return;
        if (currentWaveIndex >= waves.Count && EnemyPathfinding.enemies.Count == 0 && HealthManager.health > 0)
        {
            doneSpawning = true;
            continuePanel.SetActive(true);
            return;
        }
        if (currentEnemyIndex >= currentEnemies.Count)
        {
            if (timeBetweenWavesCurrent <= 0 || currentWaveIndex == waves.Count - 1)
            {
                timeBetweenWavesCurrent = timeBetweenWaves;
                currentWaveIndex++;
                ChangeWave();
            }
            else
            {
                timeBetweenWavesCurrent -= Time.deltaTime;
            }
        }
        if (countdown > 0) countdown -= Time.deltaTime;
        else if (currentEnemyIndex < currentEnemies.Count)
        {
            SpawnEnemy();
            countdown = currentTimeBetweenEnemies;
        }
    }

    public void SpawnEnemy()
    {
        Instantiate(currentEnemies[currentEnemyIndex], transform.position, transform.rotation);
        currentEnemyIndex++;
    }

    public void ChangeWave()
    {
        if (waves.Count <= 0 || currentWaveIndex >= waves.Count) return;
        currentWave = waves[currentWaveIndex];
        currentEnemies = currentWave.enemies;
        currentTimeBetweenEnemies = currentWave.timeBetweenEnemies;
        countdown = currentTimeBetweenEnemies;
        currentEnemyIndex = 0;
    }

    public void OnDeath() => stop = true;
    public void LoadScene()
    {
        SceneManager.LoadScene(mapScene);
        foreach (GameObject g in TowerProjectile.activeProjectiles) Destroy(g);
    }
}
