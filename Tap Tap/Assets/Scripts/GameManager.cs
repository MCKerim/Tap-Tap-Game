using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    private GameOverPanelManager gameOverPanelManager;
    private MainMenuPanelManager mainMenuPanelManager;
    private PausePanelManager pausePanelManager;
    private GameUIPanelManager gameUIPanelManager;
    private EnemySpawner enemySpawner;
    private ScoreManager scoreManager;

    private TouchInputManager touchInputManager;

    [SerializeField] private GameObject[] debugInfoObjects;

    [SerializeField] private TextMeshProUGUI timerUntilResumeText;

    private bool isPaused;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;

        gameOverPanelManager = GameObject.FindObjectOfType<GameOverPanelManager>();
        mainMenuPanelManager = GameObject.FindObjectOfType<MainMenuPanelManager>();
        pausePanelManager = GameObject.FindObjectOfType<PausePanelManager>();
        gameUIPanelManager = GameObject.FindObjectOfType<GameUIPanelManager>();
        enemySpawner = GameObject.FindObjectOfType<EnemySpawner>();
        scoreManager = GameObject.FindObjectOfType<ScoreManager>();
        touchInputManager = GameObject.FindObjectOfType<TouchInputManager>();
    }

    private void Update()
    {
        int current = (int)(1f / Time.unscaledDeltaTime);
        debugInfoObjects[2].GetComponent<TextMeshProUGUI>().SetText("FPS: " + current);
    }

    public void StartGame()
    {
        scoreManager.Reset();
        LeanTween.delayedCall(1f, enemySpawner.StartSpawner);
    }

    public void SelectModeAndStartGame(string mode)
    {
        enemySpawner.SelectMode(mode);
        StartGame();
    }

    private bool cannotLooseGame;
    public void CannotLooseGame(bool cannotLooseGame)
    {
        this.cannotLooseGame = cannotLooseGame;
    }

    public void GameOver()
    {
        if (cannotLooseGame)
        {
            return;
        }

        gameOverPanelManager.ShowGameOverPanel();
        gameUIPanelManager.HideGameUIPanel();
        scoreManager.CheckAndSaveHighscore();
        enemySpawner.StopSpawning();
        enemySpawner.DestroyAllEnemys();
    }

    public void ShowDebugInfo(bool show)
    {
        foreach(GameObject g in debugInfoObjects)
        {
            g.SetActive(show);
        }
    }

    public void Pause()
    {
        if (!isPaused)
        {
            isPaused = true;
            enemySpawner.StopSpawning();
            touchInputManager.Pause();
            pausePanelManager.ShowPausePanel();
            gameUIPanelManager.HideGameUIPanel();
            Time.timeScale = 0;
        }
    }

    public void Resume()
    {
        if (isPaused)
        {
            isPaused = false;
            StartCoroutine(TimeUntilResumeTimer());
        }
    }


    [SerializeField] private float timer;
    [SerializeField] private LeanTweenType easeType;

    IEnumerator TimeUntilResumeTimer()
    {
        pausePanelManager.HideResumeButton();
        Vector3 startScale = timerUntilResumeText.transform.localScale;
        Vector3 targetScale = new Vector3(startScale.x - 0.5f, startScale.y - 0.5f, startScale.z - 0.5f);

        for(int seconds = 3; seconds > 0; seconds--)
        {
            timerUntilResumeText.SetText(seconds + "");

            LeanTween.scale(timerUntilResumeText.gameObject, targetScale, timer).setIgnoreTimeScale(true).setEase(easeType);

            yield return new WaitForSecondsRealtime(timer);

            LeanTween.scale(timerUntilResumeText.gameObject, startScale, 0).setIgnoreTimeScale(true);
        }

        enemySpawner.ResumeSpawning();
        touchInputManager.Resume();
        pausePanelManager.HidePausePanel();
        gameUIPanelManager.ShowGameUIPanel();
        Time.timeScale = 1;
    }
}
