using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RandomStageController : MonoBehaviour, IStageController
{
    [SerializeField] private StageEntry[] stages;

    //NOT IMPLEMENTET YET
    [SerializeField] private bool isEndless;
    private EnemySpawnFunctions enemySpawnFunctions;

    [SerializeField] private int minNumberOfStageRounds;
    [SerializeField] private int maxNumberOfStageRounds;
    [SerializeField] private bool playerIsNotAllowedToMiss;
    private TouchInputManager touchInputManager;

    private int currentStageRoundsLeft;
    private bool isOver;

    private void Start()
    {
        enemySpawnFunctions = GameObject.FindObjectOfType<EnemySpawnFunctions>();
        touchInputManager = GameObject.FindObjectOfType<TouchInputManager>();
    }

    public IStage GetStage()
    {
        if(!isEndless){
            if(currentStageRoundsLeft-1 > 0){
                currentStageRoundsLeft--;
            }else{
                isOver = true;
            }
        }

        return enemySpawnFunctions.GetRandomStage(stages).GetComponent<IStage>();
    }

    public bool IsOver()
    {
        return isOver;
    }

    public void Reset()
    {
        currentStageRoundsLeft = UnityEngine.Random.Range(minNumberOfStageRounds, maxNumberOfStageRounds + 1);

        foreach(StageEntry stageEntry in stages){
            stageEntry.stage.GetComponent<IStage>().SetIsEndless(false);
        }

        isOver = false;

        touchInputManager.SetAllowedToMiss(!playerIsNotAllowedToMiss);
    }
}

[Serializable]
public class StageEntry
{
    public GameObject stage;
    public float chance;
}