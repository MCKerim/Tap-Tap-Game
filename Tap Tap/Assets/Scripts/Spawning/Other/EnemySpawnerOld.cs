using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemySpawnerOld : MonoBehaviour
{
    private float borderLeft;
    private float borderRight;
    private float borderTop;
    private float borderBottom;
    [SerializeField] private Canvas spawnField;
    [SerializeField] private float marginLeft;
    [SerializeField] private float marginRight;
    [SerializeField] private float marginTop;
    [SerializeField] private float marginBottom;

    private float zAxis = 10;

    private float nextSpawnTime;

    private bool isSpawning;

    [SerializeField] private DifficultyStage[] difficultyStages;
    private DifficultyStage currentStage;
    private int currentStageIndex = 0;

    [SerializeField] private int scoreForEndlessMode;
    [SerializeField] private DifficultyStage[] difficultyStagesEndlessMode;
    
    private float currentTimeOnStage;

    private ScoreManager scoreManager;

    [SerializeField] private TextMeshProUGUI currentStageNameText;
    [SerializeField] private TextMeshProUGUI timeUntilNextStageText;

    private TutorialManager tutorialManager;

    // Start is called before the first frame update
    void Start()
    {
        scoreManager = GameObject.FindObjectOfType<ScoreManager>();
        UpdateDifficultyOnStart();
        nextSpawnTime = currentStage.GetNextSpawnTime();

        tutorialManager = GameObject.FindObjectOfType<TutorialManager>();

        UpdateSpawnField();
    }

    void Update()
    {

#if UNITY_EDITOR
        UpdateSpawnField();
#endif

        if (!isSpawning)
        {
            return;
        }

        if (scoreManager.GetCurrentScore() >= scoreForEndlessMode)
        {
            UpdateDifficultyEndlessMode();
        }
        else
        {
            UpdateDifficulty();
        }

        if (nextSpawnTime <= 0)
        {
            for(int i=0; i < currentStage.RandomNumberOfEnemysToSpawn(); i++)
            {
                SpawnRandomEnemy();
            }
            nextSpawnTime = currentStage.GetNextSpawnTime();
        }
        else
        {
            nextSpawnTime -= Time.deltaTime;
        }
    }

    private void SpawnRandomEnemy()
    {
        if (!isSpawning)
        {
            return;
        }
        GameObject type = currentStage.GetRandomEnemy();
        Vector3 spawnPos = GetRandomSpawnPos(type);

        points = new LinkedList<Vector3>();
        points.AddLast(spawnPos);
        scale = type.transform.localScale * 1.2f;

        int currentTrys = 0;
        while (currentTrys <= 10000 && Physics2D.OverlapBox(spawnPos, type.transform.localScale * 1.2f, 0) != null)
        {
            spawnPos = GetRandomSpawnPos(type);
            points.AddLast(spawnPos);
            currentTrys++;
        }

        GameObject spawnedEnemy = GameObject.Instantiate(type, spawnPos, Quaternion.identity);
        if (tutorialManager.CheckIfEnemyNeedsTutorial(spawnedEnemy))
        {
            tutorialManager.PlayTutorial(spawnedEnemy);
        }
    }

    private Vector3 GetRandomSpawnPos(GameObject type)
    {
        float x = Random.Range(borderLeft + type.transform.localScale.x * 1.2f / 2, borderRight - type.transform.localScale.x * 1.2f / 2);
        float y = Random.Range(borderBottom + type.transform.localScale.y * 1.2f / 2, borderTop - type.transform.localScale.y * 1.2f / 2);
        return new Vector3(x, y, zAxis);
    }

    private Vector3 point;
    private Vector3 scale;
    private LinkedList<Vector3> points = new LinkedList<Vector3>();

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach(Vector3 p in points)
        {
            Gizmos.DrawWireCube(p, scale);
        }

        Gizmos.color = Color.green;
        Vector3 upperLeft = new Vector3(borderLeft, borderTop, zAxis);
        Vector3 upperRight = new Vector3(borderRight, borderTop, zAxis);
        Vector3 bottomLeft = new Vector3(borderLeft, borderBottom, zAxis);
        Vector3 bottomRight = new Vector3(borderRight, borderBottom, zAxis);

        Gizmos.DrawLine(upperLeft, upperRight);
        Gizmos.DrawLine(bottomLeft, bottomRight);
        Gizmos.DrawLine(upperLeft, bottomLeft);
        Gizmos.DrawLine(upperRight, bottomRight);
    }
#endif

    private void UpdateDifficulty()
    {
        if(currentStageIndex < difficultyStages.Length - 1 && scoreManager.GetCurrentScore() >= difficultyStages[currentStageIndex + 1].GetFromScore())
        {
            currentStageIndex++;
            currentStage = difficultyStages[currentStageIndex];
            currentStageNameText.SetText(currentStage.GetName());

            if(currentStageIndex + 1 < difficultyStages.Length)
            {
                timeUntilNextStageText.SetText("Next: " + difficultyStages[currentStageIndex + 1].GetFromScore());
            }
            else
            {
                timeUntilNextStageText.SetText("Next: " + scoreForEndlessMode);
            }
            
            return;
        }
        //currentStage = (int) Mathf.Clamp(scoreManager.GetCurrentScore() / 100, 0, difficultyStages.Length-1);
    }

    private void UpdateDifficultyEndlessMode()
    {
        if(currentTimeOnStage <= 0)
        {
            currentStage = difficultyStagesEndlessMode[Random.Range(0, difficultyStagesEndlessMode.Length)];
            currentTimeOnStage = currentStage.GetTimeUntilNextStage();
            currentStageNameText.SetText(currentStage.GetName());
            timeUntilNextStageText.SetText("Next: " + (int)currentTimeOnStage);
        }
        else
        {
            currentTimeOnStage -= Time.deltaTime;
            timeUntilNextStageText.SetText("Next: " + (int)currentTimeOnStage);
        }
    }

    private void UpdateDifficultyOnStart()
    {
        /*currentStageIndex = 0;
        while(scoreManager.GetCurrentScore() > difficultyStages[currentStageIndex+1].GetFromScore())
        {
            currentStageIndex++;
        }
        */

        currentStageIndex = 0;
        currentStage = difficultyStages[currentStageIndex];
        currentStageNameText.SetText(currentStage.GetName());
        if (currentStageIndex + 1 < difficultyStages.Length)
        {
            timeUntilNextStageText.SetText("Next: " + difficultyStages[currentStageIndex + 1].GetFromScore());
        }
        else
        {
            timeUntilNextStageText.SetText("Next: " + scoreForEndlessMode);
        }
    }

    public void DestroyAllEnemys()
    {
        Enemy[] enemys = GameObject.FindObjectsOfType<Enemy>();
        foreach (Enemy e in enemys)
        {
            Destroy(e.gameObject);
        }
    }

    public void StartSpawner()
    {
        isSpawning = true;
        UpdateDifficultyOnStart();
    }

    public void ResumeSpawning()
    {
        if (!tutorialManager.isPlayingTutorial)
        {
            isSpawning = true;
        }
    }

    public void StopSpawning()
    {
        isSpawning = false;
    }

    private void UpdateSpawnField()
    {
        Vector3 topLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelHeight, 0));
        Vector3 bottomRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, 0, 0));

        borderLeft = topLeft.x + marginLeft;
        borderRight = bottomRight.x - marginRight;
        borderTop = topLeft.y - marginTop;
        borderBottom = bottomRight.y + marginBottom;
    }
}
