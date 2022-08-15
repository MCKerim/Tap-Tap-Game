using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RandomStageController : MonoBehaviour, IStageController
{
    [SerializeField] private StageEntry[] stages;
    private EnemySpawnFunctions enemySpawnFunctions;

    private void Start()
    {
        enemySpawnFunctions = GameObject.FindObjectOfType<EnemySpawnFunctions>();
    }

    public IStage GetStage()
    {
        return enemySpawnFunctions.GetRandomStage(stages).GetComponent<IStage>();
    }

    public bool IsOver()
    {
        return false;
    }

    public void Reset()
    {
        
    }
}

[Serializable]
public class StageEntry
{
    public GameObject stage;
    public float chance;
}