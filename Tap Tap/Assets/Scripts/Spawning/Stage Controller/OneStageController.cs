using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OneStageController : MonoBehaviour, IStageController
{
    [SerializeField] private GameObject stage;
    [SerializeField] private bool isEndless;
    [SerializeField] private bool playerIsNotAllowedToMiss;
    private TouchInputManager touchInputManager;
    private bool isOver;

    private void Start() {
        touchInputManager = GameObject.FindObjectOfType<TouchInputManager>();
    }

    public void Reset()
    {
        isOver = false;
        stage.GetComponent<IStage>().SetIsEndless(isEndless);

        touchInputManager.SetAllowedToMiss(!playerIsNotAllowedToMiss);
    }

    public IStage GetStage()
    {
        isOver = true;
        return stage.GetComponent<IStage>();
    }

    public bool IsOver()
    {
        return isOver;
    }
}
