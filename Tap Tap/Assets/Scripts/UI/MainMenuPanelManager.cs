using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPanelManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;

    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
    }

    public void HideMainMenu()
    {
        mainMenuPanel.SetActive(false);
    }
}
