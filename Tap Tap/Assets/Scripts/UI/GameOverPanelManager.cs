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

    [SerializeField] private Color defaultColor;
    [SerializeField] private Color highscoreColor;

    private ScoreManager scoreManager;

    private EnemySpawner enemySpawner;

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
}
