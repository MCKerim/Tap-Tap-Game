using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WaitTillAllEnemysClearedStage : MonoBehaviour, IStage
{
    [SerializeField] private EnemyEntry[] enemys;
    private EnemySpawnFunctions enemySpawnFunctions;

    [SerializeField] private AmountChances[] amountOfEnemysChances;

    [SerializeField] private float timeBetweenEnemys;
    private float timeTillNextSpawn;
    
    [SerializeField] private bool isEndless;
    private int amountOfWaves;
    [SerializeField] private AmountChances[] amountOfWavesChances;
    private bool isOver;

    private void Start()
    {
        enemySpawnFunctions = GameObject.FindObjectOfType<EnemySpawnFunctions>();
    }

    public void Reset()
    {
        if (!isEndless)
        {
            amountOfWaves = enemySpawnFunctions.SelectRandomAmount(amountOfWavesChances);
        }
        isOver = false;
        timeTillNextSpawn = 0;
    }

    public bool IsOver()
    {
        return isOver;
    }
    
    public void UpdateStage()
    {
        if (IsOver())
        {
            return;
        }

        bool noEnemy = GameObject.FindObjectsOfType<Enemy>().Length == 0;
        bool timerAboveZero = timeTillNextSpawn > 0;

        if (!isEndless)
        {
            if (amountOfWaves <= 0 && noEnemy)
            {
                isOver = true;
                return;
            }
        }

        if (noEnemy && timerAboveZero)
        {
            timeTillNextSpawn -= Time.deltaTime;
        }
        else if(noEnemy && !timerAboveZero)
        {
            enemySpawnFunctions.SpawnRandomEnemy(enemys, amountOfEnemysChances);
            if (!isEndless)
            {
                amountOfWaves--;
            }
            timeTillNextSpawn = timeBetweenEnemys;
        }
    }
}
