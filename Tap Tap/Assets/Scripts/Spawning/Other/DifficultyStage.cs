using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DifficultyStage
{
    [SerializeField] private string name;
    [SerializeField] private int fromScore;

    [SerializeField] private float minSecInEndlessMode;
    [SerializeField] private float maxSecInEndlessMode;

    [SerializeField] private float intervallMin;
    [SerializeField] private float intervallMax;

    [SerializeField] private float chanceToSpawnMoreEnemysThanMin;
    [SerializeField] private int minNumberOfEnemysToSpawn;
    [SerializeField] private int maxNumberOfEnemysToSpawn;
    
    [SerializeField] private EnemyEntry[] enemys;

    public GameObject GetRandomEnemy()
    {
        float chanceSum = GetChanceSum();

        float randomChance = UnityEngine.Random.Range(0, chanceSum);

        float currentChance = 0;
        foreach(EnemyEntry e in enemys)
        {
            currentChance += e.chanceToSpawn;
            if(currentChance >= randomChance)
            {
                return e.enemy;
            }
        }
        return null;
    }

    private float GetChanceSum()
    {
        float chanceSum = 0;
        foreach (EnemyEntry e in enemys)
        {
            chanceSum += e.chanceToSpawn;
        }
        return chanceSum;
    }

    public int RandomNumberOfEnemysToSpawn()
    {
        if(UnityEngine.Random.Range(0f, 1f) < chanceToSpawnMoreEnemysThanMin)
        {
            return UnityEngine.Random.Range(minNumberOfEnemysToSpawn+1, maxNumberOfEnemysToSpawn);
        }
        return minNumberOfEnemysToSpawn;
    }

    public float GetNextSpawnTime()
    {
        return UnityEngine.Random.Range(intervallMin, intervallMax);
    }

    public int GetFromScore()
    {
        return fromScore;
    }

    public float GetTimeUntilNextStage()
    {
        return UnityEngine.Random.Range(minSecInEndlessMode, maxSecInEndlessMode);
    }

    public string GetName()
    {
        return name;
    }
}

