using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverPanelManager : MonoBehaviour
{

    [SerializeField] private GameObject gameOverPanel;
    private CanvasGroup gameOverPanelGroup;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highscoreText;

    [SerializeField] private TextMeshProUGUI currentModeText;

    [SerializeField] private TextMeshProUGUI gameOverInfoText;

    [SerializeField] private Color defaultColor;
    [SerializeField] private Color highscoreColor;

    [SerializeField] private Color gameOverInfoTextDefaultColor;

    private ScoreManager scoreManager;

    private EnemySpawner enemySpawner;

    [SerializeField] private string[] enemyNotClickedTexts;
    [SerializeField] private string[] dontTapEnemyClickedTexts;
    [SerializeField] private string[] numberEnemyClickedInWrongOrderTexts;
    [SerializeField] private string[] enemyMissedTexts;
    [SerializeField] private string[] reactionBossEnemyClickedTexts;
    [SerializeField] private string[] winningTexts;

    // Start is called before the first frame update
    void Start()
    {
        gameOverPanelGroup = gameOverPanel.GetComponent<CanvasGroup>();
        enemySpawner = GameObject.FindObjectOfType<EnemySpawner>();
        scoreManager = GameObject.FindObjectOfType<ScoreManager>();
    }

    public void ShowGameOverPanel()
    {
        currentModeText.SetText(enemySpawner.GetCurrentModeName());

        UpdateScoreAndHighscoreText(scoreManager.GetCurrentScore(), scoreManager.GetHighScore());

        gameOverPanelGroup.alpha = 0;
        gameOverPanel.SetActive(true);
        LeanTween.alphaCanvas(gameOverPanelGroup, 1, 2);
    }

    private void UpdateScoreAndHighscoreText(int currentScore, int highscore)
    {
        scoreText.SetText(currentScore.ToString());
        highscoreText.SetText("Best: " + highscore);

        if (currentScore > highscore)
        {
            scoreText.fontStyle = FontStyles.Italic;
            scoreText.color = highscoreColor;
        }
        else
        {
            scoreText.fontStyle = FontStyles.Normal;
            scoreText.color = defaultColor;
        }
    }

    public void HideGameOverPanel()
    {
        scoreManager.StopHighscoreParticles();
        gameOverPanel.SetActive(false);

        //DEBUG PURPOSES
        GameObject d = GameObject.FindWithTag("DEBUG");
        d.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void UpdateGameOverInfoText(GameOverInfoTextType type)
    {
        gameOverInfoText.color = gameOverInfoTextDefaultColor;
        switch (type)
        {
            case GameOverInfoTextType.EnemyNotClicked:
                SelectRandomGameOverInfoText(enemyNotClickedTexts);
                break;

            case GameOverInfoTextType.DontTapEnemyClicked:
                SelectRandomGameOverInfoText(dontTapEnemyClickedTexts);
                break;

            case GameOverInfoTextType.NumberEnemyClickedInWrongOrder:
                SelectRandomGameOverInfoText(numberEnemyClickedInWrongOrderTexts);
                break;

            case GameOverInfoTextType.EnemyMissed:
                SelectRandomGameOverInfoText(enemyMissedTexts);
                break;

            case GameOverInfoTextType.ReactionBossEnemyClicked:
                SelectRandomGameOverInfoText(reactionBossEnemyClickedTexts);
                break;

            case GameOverInfoTextType.Winning:
                gameOverInfoText.color = highscoreColor;
                SelectRandomGameOverInfoText(winningTexts);
                break;

            default:
                gameOverInfoText.SetText("");
                break;
        }
    }

    private void SelectRandomGameOverInfoText(string[] texts)
    {
        if(texts.Length == 0)
        {
            gameOverInfoText.SetText("");
            Debug.Log("No Game Over texts for that scenario");
        }
        else
        {
            int randomText = Random.Range(0, texts.Length);
            gameOverInfoText.SetText(texts[randomText]);
        }
        
    }
}
