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
    private bool _stopSpawning = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void StartSpawning() //Spawn routines called
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    IEnumerator SpawnEnemyRoutine() //Enemy Spawner
    {
        yield return new WaitForSeconds(1.5f); //pause in spawning for asteroid to be destroyed
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9f, 9f), 7f, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5f);
        }
    }
    IEnumerator SpawnPowerupRoutine() //Powerup Spawner
    {
        yield return new WaitForSeconds(1.5f); //pause in spawning for asteroid to be destroyed
        while (_stopSpawning == false)
        {
          Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7f, 0);
            int randomPowerup = Random.Range(0, 3);
          Instantiate(_powerups[randomPowerup], posToSpawn, Quaternion.identity);
          yield return new WaitForSeconds(Random.Range(4, 9));
        }
       
    }
    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

}
