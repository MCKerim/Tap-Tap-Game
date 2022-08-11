using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SelectModePanelManager : MonoBehaviour
{
    [SerializeField] private GameObject selectModePanel;
    [SerializeField] private ModeButton modeButtonPrefab;
    [SerializeField] private GameObject moreSoonButtonPrefab;
    [SerializeField] private Transform contentObject;


    private GameManager gameManager;
    private GameUIPanelManager gameUIPanelManager;

    private void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        gameUIPanelManager = GameObject.FindObjectOfType<GameUIPanelManager>();

        EnemySpawner enemySpawner = GameObject.FindObjectOfType<EnemySpawner>();

        string[] modeNames = enemySpawner.GetModeNames();

        int spaceBetween = 150;
        int startY = 50;
        for(int i=0; i < modeNames.Length; i++)
        {
            ModeButton button = Instantiate(modeButtonPrefab, contentObject);
            button.transform.LeanSetLocalPosY(-(i * spaceBetween + startY));

            button.SetModeName(modeNames[i]);
        }

        GameObject moreSoonButton = Instantiate(moreSoonButtonPrefab, contentObject);
        moreSoonButton.transform.LeanSetLocalPosY(-(modeNames.Length * spaceBetween + startY));

        RectTransform rt = contentObject.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, (modeNames.Length + 1) * spaceBetween + startY);
    }

    public void SelectAndStartMode(string mode)
    {
        gameManager.SelectModeAndStartGame(mode);
        HideSelectModePanel();
        gameUIPanelManager.ShowGameUIPanel();
    }

    public void ShowSelectModePanel()
    {
        selectModePanel.SetActive(true);
    }

    public void HideSelectModePanel()
    {
        selectModePanel.SetActive(false);
    }
}
