using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class EnemySpawner : MonoBehaviour
{
    private bool isSpawning;

    [SerializeField] private GameObject[] stageController;

    private string currentModeName;
    private IStageController currentStageController;
    private IStage currentStage;

    [SerializeField] private TextMeshProUGUI currentStageNameText;
    [SerializeField] private TextMeshProUGUI timeUntilNextStageText;

    private TutorialManager tutorialManager;

    // Start is called before the first frame update
    void Start()
    {
        tutorialManager = GameObject.FindObjectOfType<TutorialManager>();

        currentStageController = stageController[0].GetComponent<IStageController>();
        currentModeName = stageController[0].name;
    }

    void Update()
    {
        if (!isSpawning)
        {
            return;
        }

        if (!currentStage.IsOver())
        {
            currentStage.UpdateStage();
        }
        else
        {
            ChangeStage();
        }
    }

    public void SelectMode(string mode)
    {
        foreach(GameObject s in stageController)
        {
            if (s.name == mode)
            {
                currentModeName = s.name;
                currentStageController = s.GetComponent<IStageController>();
                return;
            }
        }
        Debug.LogError("Mode '" + mode + "' does not exists.");
    }


    public string[] GetModeNames()
    {
        string[] modeNames = new string[stageController.Length];

        for (int i = 0; i < stageController.Length; i++)
        {
            modeNames[i] = stageController[i].name;
        }

        return modeNames;
    }

    public string GetCurrentModeName()
    {
        return currentModeName;
    }

    private void ChangeStage()
    {
        currentStage = currentStageController.GetStage();
        currentStage.Reset();
        currentStageNameText.SetText("Stage: " + currentStage.ToString());
    }

    public void DestroyAllEnemys()
    {
        Enemy[] enemys = GameObject.FindObjectsOfType<Enemy>();
        foreach (Enemy e in enemys)
        {
            Destroy(e.gameObject);
        }
    }

    public void ResumeSpawning()
    {
        if (!tutorialManager.isPlayingTutorial)
        {
            isSpawning = true;
        }
    }

    public void StopSpawning()
    {
        isSpawning = false;
    }

    public void StartSpawner()
    {
        currentStageController.Reset();
        ChangeStage();
        isSpawning = true;
    }
}
