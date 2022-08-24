using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerStage : MonoBehaviour, IStage
{
    private EnemySpawnFunctions settings;

    [SerializeField] private EnemyEntry[] enemys;

    [SerializeField] private AmountChances[] amountChances;

    [SerializeField] private float minSpawnIntervall;
    [SerializeField] private float maxSpawnIntervall;
    private float timeTillNextSpawn;

    [SerializeField] private float minTimeStageEnds;
    [SerializeField] private float maxTimeStageEnds;
    private float timeTillStageIsOver;
    private bool isEndless;

    private bool isOver;

    private void Start()
    {
        settings = GameObject.FindObjectOfType<EnemySpawnFunctions>();
    }

    public void SetIsEndless(bool isEndless){
        this.isEndless = isEndless;
    }

    public bool IsOver()
    {
        return isOver;
    }

    public void Reset()
    {
        timeTillNextSpawn = 0;
        timeTillStageIsOver = Random.Range(minTimeStageEnds, maxTimeStageEnds);
        isOver = false;
    }

    public void UpdateStage()
    {
        if(!isOver && timeTillStageIsOver <= 0 && !isEndless)
        {
            isOver = true;
            return;
        }

        if(!isEndless){
            timeTillStageIsOver -= Time.deltaTime;
        }
        

        if (timeTillNextSpawn <= 0)
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
