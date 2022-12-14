using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    private GameOverPanelManager gameOverPanelManager;
    private MainMenuPanelManager mainMenuPanelManager;
    private PausePanelManager pausePanelManager;
    private GameUIPanelManager gameUIPanelManager;
    private EnemySpawner enemySpawner;
    private ScoreManager scoreManager;

    private TouchInputManager touchInputManager;

    [SerializeField] private AudioMixerSnapshot pausedSnapshot;
    [SerializeField] private AudioMixerSnapshot unPausedSnapshot;
    [SerializeField] private GameObject[] debugInfoObjects;

    [SerializeField] private TextMeshProUGUI timerUntilResumeText;

    private bool isPaused;

    [SerializeField] private float backgroundMusicPauseTransitionTime;

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
        touchInputManager.StartRecognizingTouches();
        enemySpawner.StartSpawner(1f);
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

    public void GameOver(GameOverInfoTextType type)
    {
        if (cannotLooseGame)
        {
            return;
        }

        gameOverPanelManager.UpdateGameOverInfoText(type);
        ShowGameOverPanelAndEndGame();
    }

    public void GameOver(string gameOverInfoText)
    {
        if (cannotLooseGame)
        {
            return;
        }

        gameOverPanelManager.UpdateGameOverInfoText(gameOverInfoText);
        ShowGameOverPanelAndEndGame();
    }

    private void ShowGameOverPanelAndEndGame(){
        enemySpawner.StopSpawning();
        touchInputManager.StopRecognizingTouches();
        enemySpawner.DestroyAllEnemys();

        if(scoreManager.CheckAndSaveHighscore()){
            gameOverPanelManager.UpdateGameOverInfoText("New Highscore!");
        }

        gameOverPanelManager.ShowGameOverPanel();   
        gameUIPanelManager.HideGameUIPanel();
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
            touchInputManager.StopRecognizingTouches();
            pausePanelManager.ShowPausePanel();
            gameUIPanelManager.HideGameUIPanel();
            pausedSnapshot.TransitionTo(backgroundMusicPauseTransitionTime);
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


    [SerializeField] private float countdownTime;
    [SerializeField] private LeanTweenType easeType;

    IEnumerator TimeUntilResumeTimer()
    {
        pausePanelManager.HideResumeButton();
        Vector3 startScale = timerUntilResumeText.transform.localScale;
        Vector3 targetScale = new Vector3(startScale.x - 0.5f, startScale.y - 0.5f, startScale.z - 0.5f);

        int countdownStart = 3;
        unPausedSnapshot.TransitionTo(countdownStart * countdownTime);

        for(int i = countdownStart; i > 0; i--)
        {
            timerUntilResumeText.SetText(i + "");

            LeanTween.scale(timerUntilResumeText.gameObject, targetScale, countdownTime).setIgnoreTimeScale(true).setEase(easeType);

            yield return new WaitForSecondsRealtime(countdownTime);

            LeanTween.scale(timerUntilResumeText.gameObject, startScale, 0).setIgnoreTimeScale(true);
        }

        enemySpawner.ResumeSpawning();
        touchInputManager.StartRecognizingTouches();
        pausePanelManager.HidePausePanel();
        gameUIPanelManager.ShowGameUIPanel();
        Time.timeScale = 1;
    }
}
