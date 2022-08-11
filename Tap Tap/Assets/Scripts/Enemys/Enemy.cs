using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int pointsOnDeath;
    [SerializeField] private float timeToKill;
    protected float currentTime;

    [SerializeField] private float rotationSpeed;
    [HideInInspector] public bool isDead;
    public bool livesEndless;

    private TutorialManager tutorialManager;

    [SerializeField] private ParticleSystem deathParticle;

    private EnemyTimer enemyTimer;

    public void Setup()
    {
        SpawnAnimation();
        currentTime = timeToKill;

        enemyTimer = GetComponentInChildren<EnemyTimer>();
        if(enemyTimer != null)
        {
            enemyTimer.SetTimeToLife(timeToKill);
        }
    }

    public void Update()
    {
        if (!isDead)
        {
            UpdateTimeToKill();
        }
        RotateOnZ();
    }

    public void Kill()
    {
        if (isDead)
        {
            return;
        }
        isDead = true;
        GameObject.FindObjectOfType<ScoreManager>().addPoints(pointsOnDeath);
        Destroy(GetComponent<Collider>());
        LeanTween.scale(gameObject, new Vector3(0, 0, 0), 0.2f).setOnComplete(DestroyThis);

        ParticleSystem particle = Instantiate(deathParticle);
        particle.transform.position = transform.position;

        if(tutorialManager != null)
        {
            tutorialManager.StopTutorial();
        }
    }

    public void SetTutorialManagerToUpdateWhenKilled(TutorialManager tutorialManager)
    {
        this.tutorialManager = tutorialManager;
    }

    private void DestroyThis()
    {
        Destroy(gameObject);
    }

    public void UpdateTimeToKill()
    {
        if (livesEndless)
        {
            return;
        }

        if (currentTime <= 0)
        {
            TriggerGameOver();
        }
        else
        {
            currentTime -= Time.deltaTime;
            if(enemyTimer != null)
            {
                enemyTimer.UpdateTime(currentTime);
            }
        }
    }

    public void TriggerGameOver()
    {
        /*//DEBUG PURPOSES
        GameObject d = GameObject.FindWithTag("DEBUG");
        d.GetComponent<SpriteRenderer>().enabled = true;
        d.transform.position = transform.position;
        */
        GameObject.FindObjectOfType<GameManager>().GameOver();
    }

    public void RotateOnZ()
    {
        transform.Rotate(new Vector3(0, 0, -rotationSpeed) * Time.deltaTime);
    }

    public void SpawnAnimation()
    {
        Vector3 targetScale = transform.localScale;
        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        LeanTween.scale(gameObject, targetScale, 0.2f).setEaseOutBack();
    }
}
