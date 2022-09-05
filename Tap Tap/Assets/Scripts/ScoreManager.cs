using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    private int score = 0;
    private int highscore;

    [SerializeField] private ParticleSystem[] highscoreParticles;

    [SerializeField] private Color aboveHighscoreColor;
    private Color defaultColor;

    private GameOverPanelManager gameOverPanelManager;

    private EnemySpawner enemySpawner;
    private string currentModeName;

    [SerializeField] private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        gameOverPanelManager = GameObject.FindObjectOfType<GameOverPanelManager>();
        enemySpawner = GameObject.FindObjectOfType<EnemySpawner>();

        canvasGroup = scoreText.GetComponent<CanvasGroup>();
        localYOnStart = scoreText.transform.localPosition.y;
        scoreText.SetText("");

        defaultColor = scoreText.color;
    }

    public void addPoints(int points)
    {
        score += points;
        UpdateScoreTextAnimated();
    }

    public int GetCurrentScore()
    {
        return score;
    }

    public int GetHighScore()
    {
        return highscore;
    }

    public void CheckAndSaveHighscore()
    {
        if (score > highscore)
        {
            highscore = score;
            PlayerPrefs.SetInt(currentModeName, highscore);
            StartCoroutine(PlayHighscoreParticles());
        }
    }

    public void Reset()
    {
        score = 0;
        scoreText.color = defaultColor;
        scoreText.fontStyle = FontStyles.Normal;
        scoreText.SetText("");

        currentModeName = enemySpawner.GetCurrentModeName();
        highscore = PlayerPrefs.GetInt(currentModeName, 0);
    }

    public void ResetHighscore()
    {
        highscore = 0;
        PlayerPrefs.SetInt(currentModeName, highscore);
    }

    IEnumerator PlayHighscoreParticles()
    {
        yield return new WaitForSeconds(0.5f);

        while (true)
        {
            int index = Random.Range(0, highscoreParticles.Length);
            if (!highscoreParticles[index].isPlaying)
            {
                highscoreParticles[index].Play();
                audioSource.Play();
            }

            yield return new WaitForSeconds(Random.Range(0f, 1f));
        }
    }

    public void StopHighscoreParticles()
    {
        StopAllCoroutines();
    }

    private void UpdateScoreText()
    {
        scoreText.SetText(score.ToString());
        if (score > highscore)
        {
            scoreText.color = aboveHighscoreColor;
            scoreText.fontStyle = FontStyles.Italic;
        }
    }

    private CanvasGroup canvasGroup;
    private float alphaOnStart = 1f;
    private float localYOnStart;

    private void UpdateScoreTextAnimated()
    {
        LeanTween.moveLocalY(scoreText.gameObject, localYOnStart + 250, 0.1f).setOnComplete(UpdateScoreTextAnimatedOut);
        LeanTween.alphaCanvas(canvasGroup, 0, 0.1f);
    }

    private void UpdateScoreTextAnimatedOut()
    {
        UpdateScoreText();
        transform.LeanSetLocalPosY(localYOnStart - 250);
        LeanTween.moveLocalY(scoreText.gameObject, localYOnStart, 0.1f);
        LeanTween.alphaCanvas(canvasGroup, alphaOnStart, 0.1f);
    }
}
