using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WaitTillAllEnemysClearedStage : MonoBehaviour, IStage
{
    [SerializeField] private EnemyEntry[] enemys;
    private EnemySpawnFunctions enemySpawnFunctions;

    [SerializeField] private AmountOfEnemysChance[] amountChances;

    [SerializeField] private float timeBetweenEnemys;
    private float timeTillNextSpawn;

    [SerializeField] private bool isEndless;
    private bool hasSpawnedOneEnemy;
    private bool isOver;

    private void Start()
    {
        enemySpawnFunctions = GameObject.FindObjectOfType<EnemySpawnFunctions>();
    }

    public void Reset()
    {
        hasSpawnedOneEnemy = false;
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
            if (hasSpawnedOneEnemy && noEnemy)
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
            enemySpawnFunctions.SpawnRandomEnemy(enemys, amountChances);
            hasSpawnedOneEnemy = true;
            timeTillNextSpawn = timeBetweenEnemys;
        }
    }
}
