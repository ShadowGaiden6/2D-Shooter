using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject[] _powerups;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject _ammoCollectable;
    [SerializeField]
    private GameObject _healthCollectable;
    [SerializeField]
    private GameObject _megaLaserCollectable;
    private bool _stopSpawning = false;
    public int _currentWave = 0;
    [SerializeField]
    private int _enemiesToSpawn = 0;
    public int _enemiesRemaining = 0;
    [SerializeField]
    private bool _startWave = false;
    private UI_Manager _uiManager;
    [SerializeField]
    private GameObject _negativePowerup;
    public Enemy _enemy;
    [SerializeField]
    private GameObject _bossPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        _uiManager = GameObject.Find("UI_Manager").GetComponent<UI_Manager>();
        _currentWave = 0;
        _enemiesToSpawn = 0;
    }
    private void Update()
    {
        if(_enemyContainer.transform.childCount == 0 && _startWave == true)
        {
            _enemiesRemaining = 0;
            StopCoroutine(SpawnEnemyRoutine());
            EndWave();
        }
    }
    public void StartSpawning() //Spawn routines called
    {
        if (_currentWave == 4)
        {
            StartCoroutine(SpawnBossRoutine());
            StartCoroutine(SpawnPowerupRoutine());
            StartCoroutine(SpawnAmmoRoutine());
            StartCoroutine(SpawnHealthRoutine());
            StartCoroutine(SpawnMegaLaserRoutine());
            StartCoroutine(SpawnNegativePowerup());
            StartCoroutine(BossDeathRoutine());
            if (_bossPrefab == null)
            {
                StartCoroutine(BossDeathRoutine());
            }
        }
        else
        {
            StartCoroutine(SpawnEnemyRoutine());
            StartCoroutine(SpawnPowerupRoutine());
            StartCoroutine(SpawnAmmoRoutine());
            StartCoroutine(SpawnHealthRoutine());
            StartCoroutine(SpawnMegaLaserRoutine());
            StartCoroutine(SpawnNegativePowerup());
        }
        _startWave = false;
        _enemiesRemaining = _enemiesToSpawn;
    }

    public void EndWave()
    {
        _currentWave += 1;
        _enemiesToSpawn += 15;
        new WaitForSeconds(3.0f);
        StartWave();
    }
    public void StartWave()
    {
        _uiManager.UpdateWaves(_currentWave);
        new WaitForSeconds(3.0f);
        _stopSpawning = false;
        _enemiesRemaining = _enemiesToSpawn;
        StartSpawning();

    }
    IEnumerator SpawnEnemyRoutine() //Enemy Spawner
    {
        int _rng = _enemy._rng;
        int _floaterChance = _enemy._floaterChance;
        int enemiesSpawned = 0;
        yield return new WaitForSeconds(1.5f); //pause in spawning for asteroid to be destroyed
        while (_stopSpawning == false)
        {
            if (enemiesSpawned != _enemiesToSpawn)
            {
                Vector3 posToSpawn = new Vector3(Random.Range(-9f, 9f), 7f, 0);
                GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
                enemiesSpawned++;
                yield return new WaitForSeconds(3f);
            }
            else if (enemiesSpawned != _enemiesToSpawn && _rng == _floaterChance)
            {
                Vector3 posToSpawn = new Vector3(Random.Range(-9f, 9f), 7f, 0);
                GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
                enemiesSpawned++;
                yield return new WaitForSeconds(3f);
            }
            else 
            {
                _stopSpawning = true;
                _startWave = true;
                enemiesSpawned = 0;
            }
        }
    }

    IEnumerator SpawnBossRoutine()
    {
        if (_currentWave == 4)
        {
            yield return new WaitForSeconds(1.5f);
            Vector3 posToSpawn = new Vector3(0, 7f, 0);
            GameObject newEnemy = Instantiate(_bossPrefab, posToSpawn, Quaternion.identity);
        }
    }
    IEnumerator BossDeathRoutine()
    {
        yield return new WaitForSeconds(1.0f);
        EndWave();
      
    }

    IEnumerator SpawnPowerupRoutine() //Powerup Spawner
    {
        yield return new WaitForSeconds(20f); //pause in spawning for asteroid to be destroyed
        while (_stopSpawning == false)
        {
          Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7f, 0);
          int randomPowerup = Random.Range(0, 4);
          Instantiate(_powerups[randomPowerup], posToSpawn, Quaternion.identity);
          yield return new WaitForSeconds(Random.Range(30, 40));
        }
       
    }

    IEnumerator SpawnAmmoRoutine()
    {
        yield return new WaitForSeconds(5f);
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7f, 0);
            Instantiate(_ammoCollectable, posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(5, 10));
        }
    }

    IEnumerator SpawnHealthRoutine()
    {
        yield return new WaitForSeconds(25f);
        while(_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7f, 0);
            Instantiate(_healthCollectable, posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(20, 30));
        }
    }

    IEnumerator SpawnMegaLaserRoutine()
    {
        yield return new WaitForSeconds(45f);
        while(_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7f, 0);
            Instantiate(_megaLaserCollectable, posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(40, 60));
        }
        
    }
    IEnumerator SpawnNegativePowerup()
    {
        yield return new WaitForSeconds(15f);
        while(_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7f, 0);
            Instantiate(_negativePowerup, posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(10, 25));
        }
    }

    public void Rotate()
    {
        gameObject.transform.Rotate(0, 0, 0.15f, Space.Self);
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

}
