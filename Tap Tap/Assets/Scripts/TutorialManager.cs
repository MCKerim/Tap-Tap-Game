using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private string[] tutorialTexts;
    [SerializeField] private Enemy[] enemyTypes;

    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private GameObject tutorialCircle;
    [SerializeField] private TextMeshProUGUI tutorialText;

    private EnemySpawner enemySpawner;
    [SerializeField] private TutorialTextManager tutorialTextManager;
    [HideInInspector] public bool isPlayingTutorial;

    // Start is called before the first frame update
    void Start()
    {
        enemySpawner = GameObject.FindObjectOfType<EnemySpawner>();
        circleScale = tutorialCircle.transform.localScale;
    }

    public void ResetTutorialProgress()
    {
        foreach(Enemy e in enemyTypes)
        {
            PlayerPrefs.SetInt(e.GetType().ToString(), 0);
        }
    }

    public bool CheckIfEnemyNeedsTutorial(GameObject enemy)
    {
        if (isPlayingTutorial)
        {
            enemy.GetComponent<Enemy>().livesEndless = true;
            return false;
        }

        if (enemy.tag == "Boss")
        {
            Debug.Log("Boss Enemy has no tutorial");
            return false;
        }

        Enemy tutorialEnemy = enemy.GetComponent<Enemy>();
        return PlayerPrefs.GetInt(tutorialEnemy.GetType().ToString(), 0) == 0;
    }

    private Vector3 circleScale;

    public void PlayTutorial(GameObject enemy)
    {
        isPlayingTutorial = true;
        enemySpawner.StopSpawning();
        foreach (Enemy e in GameObject.FindObjectsOfType<Enemy>())
        {
            e.livesEndless = true;
        }

        Enemy tutorialEnemy = enemy.GetComponent<Enemy>();

        tutorialEnemy.SetTutorialManagerToUpdateWhenKilled(this);

        tutorialPanel.SetActive(true);
        tutorialCircle.transform.position = enemy.transform.position;
        tutorialTextManager.UpdatePos();

        tutorialCircle.transform.localScale = new Vector3(0.1f, 0.1f, 1);
        LeanTween.scale(tutorialCircle, circleScale, 0.25f);
        
        for (int i=0; i < enemyTypes.Length; i++)
        {
            if (tutorialEnemy.GetType().Equals(enemyTypes[i].GetType()))
            {
                tutorialText.SetText(tutorialTexts[i]);
                break;
            }
        }

        PlayerPrefs.SetInt(tutorialEnemy.GetType().ToString(), 1);
    }

    public void StopTutorial()
    {
        tutorialPanel.SetActive(false);
        isPlayingTutorial = false;
        LeanTween.delayedCall(0.5f, enemySpawner.ResumeSpawning);
    }
}
