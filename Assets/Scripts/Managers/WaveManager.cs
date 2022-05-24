using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveEnemyTypes
{
    public string name;
    public Transform enemy;
    public int count;
    public float rate;
    public float waitToSpawn;
}

[System.Serializable]
public class Wave
{
    public string name;
    public List<WaveEnemyTypes> EnemyTypes;
}

public class WaveManager : MonoBehaviour
{
    public enum SpawnState { SPAWNING, WAITING, COUNTING, FINISHED};

    public Wave[] waves;
    private int nextWave = 0;

    public Transform[] spawnPoints;

    public float timeBetweenWaves = 5f;
    private float waveCountDown;

    private float searchCountDown = 1f;

    private SpawnState state = SpawnState.COUNTING;

    private void Start()
    {
        waveCountDown = timeBetweenWaves;
    }

    private void Update()
    {
        if(state == SpawnState.FINISHED)
        {
            return;
        }
        if(state == SpawnState.WAITING)
        {
            // Check if any Enemies remain
            if (!EnemyIsAlive())
            {
                // Start new wave                // Adjust Later to allow player to start wave early

                WaveComplete();
            }
            else
            {
                return;
            }
        }
        if (waveCountDown <= 0f)
        {
            if(state != SpawnState.SPAWNING)
            {
                // Spawn Wave
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        }
        else
        {
            waveCountDown -= Time.deltaTime;
        }
    }

    private void WaveComplete()
    {
        Debug.Log("Wave Complete!");

        state = SpawnState.COUNTING;
        waveCountDown = timeBetweenWaves;

        if(nextWave + 1 > waves.Length - 1)
        {
            // If there are no waves left
            state = SpawnState.FINISHED;
            Debug.Log("Victory!");        
        }
        else
        {
            nextWave++;
        }    
    }

    private bool EnemyIsAlive()
    {
        searchCountDown -= Time.deltaTime;
        if (searchCountDown <= 0f)
        {
            searchCountDown = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false;
            }
        }
        return true;
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        Debug.Log($"Spawning Wave: {_wave.name}");
        state = SpawnState.SPAWNING;
        IEnumerator AllEnemyTypes(WaveEnemyTypes _type)
        {
           yield return new WaitForSeconds(_type.waitToSpawn);
           for (int i = 0; i < _type.count; i++)
           {
               SpawnEnemies(_type.enemy);
               yield return new WaitForSeconds(1f * _type.rate);
           }
           yield break;         
        }
        foreach(WaveEnemyTypes E in _wave.EnemyTypes)
        {
            StartCoroutine(AllEnemyTypes(E));
        }

        state = SpawnState.WAITING;

        yield break;
    }

    private void SpawnEnemies (Transform _enemy)
    {
        // Spawn Enenmy
        if (spawnPoints.Length == 0)
        {
            Debug.Log("No spawn points set! Dummy");
        }

        Transform _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(_enemy, _sp.position, _sp.rotation);
        Debug.Log($"Spawning Enemy: {_enemy.name}");
    }

}
