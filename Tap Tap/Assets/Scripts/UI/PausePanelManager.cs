using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanelManager : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject resumeButton;
    [SerializeField] private GameObject timerUntilResumeText;

    public void ShowPausePanel()
    {
        pausePanel.SetActive(true);
        resumeButton.SetActive(true);
        timerUntilResumeText.SetActive(false);
    }

    public void HidePausePanel()
    {
        pausePanel.SetActive(false);
    }

    public void HideResumeButton()
    {
        resumeButton.SetActive(false);
        timerUntilResumeText.SetActive(true);
    }
}
