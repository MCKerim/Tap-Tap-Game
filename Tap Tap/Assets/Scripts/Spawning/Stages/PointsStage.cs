using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsStage : MonoBehaviour, IStage
{
    private EnemySpawnFunctions settings;
    private ScoreManager scoreManager;

    [SerializeField] private EnemyEntry[] enemys;

    [SerializeField] private AmountChances[] amountChances;

    [SerializeField] private int endScore;
    [SerializeField] private bool isEndless;

    [SerializeField] private float minSpawnIntervall;
    [SerializeField] private float maxSpawnIntervall;

    private float timeTillNextSpawn;

    private void Start()
    {
        settings = GameObject.FindObjectOfType<EnemySpawnFunctions>();
        scoreManager = GameObject.FindObjectOfType<ScoreManager>();
    }

    public void Reset()
    {
        timeTillNextSpawn = 0;
    }

    public bool IsOver()
    {
        if (isEndless)
        {
            return false;
        }
        else
        {
            return scoreManager.GetCurrentScore() > endScore;
        }
    }

    public void UpdateStage()
    {
        if(timeTillNextSpawn <= 0)
        {
            settings.SpawnRandomEnemy(enemys, amountChances);
            timeTillNextSpawn = GetNextSpawnTime();
        }
        else
        {
            timeTillNextSpawn -= Time.deltaTime;
        }
    }

    private float GetNextSpawnTime()
    {
        return Random.Range(minSpawnIntervall, maxSpawnIntervall);
    }
}
