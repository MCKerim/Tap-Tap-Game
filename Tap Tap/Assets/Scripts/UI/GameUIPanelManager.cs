using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIPanelManager : MonoBehaviour
{
    [SerializeField] private GameObject gameUIPanel;

    public void ShowGameUIPanel()
    {
        gameUIPanel.SetActive(true);
    }

    public void HideGameUIPanel()
    {
        gameUIPanel.SetActive(false);
    }
}
