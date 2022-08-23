using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ReactionBossEnemy : Enemy, ITouchInput
{
    [SerializeField] private IndicatorBar healthBar;

    [SerializeField] private int lives;
    [SerializeField] private ParticleSystem looseLiveParticle;

    private SpriteRenderer spriteRenderer;
    private Color normalColor;
    [SerializeField] private Color reactionModeColor;

    private Vector3 startScale;

    // Start is called before the first frame update
    void Start()
    {
        startScale = transform.localScale;
        base.Setup();

        spriteRenderer = GetComponent<SpriteRenderer>();
        normalColor = spriteRenderer.color;

        healthBar.SetMaxValue(lives);
        healthBar.SetValue(lives);

        StopReactionMode();
    }

#if UNITY_EDITOR
    private void OnMouseDown()
    {
        OnTouchDown();
    }
#endif

    [SerializeField] private float minTimeTillNextReaction;
    [SerializeField] private float maxTimeTillNextReaction;
    private float currentTimeTillNextReaction;

    private float currentTimeInsideReaction;
    private bool isInReaction;

    [SerializeField] private float maxTimeInsideReaction;

    [SerializeField] private TextMeshProUGUI numberText;

    private void Update()
    {
        //base.Update();
        if (isInReaction)
        {
            currentTimeInsideReaction += Time.deltaTime;
        }
        else
        {
            if (currentTimeTillNextReaction <= 0)
            {
                StartReactionMode();
            }
            else
            {
                currentTimeTillNextReaction -= Time.deltaTime;
            }
        }
    }

    public void StartReactionMode()
    {
        spriteRenderer.color = reactionModeColor;
        currentTimeInsideReaction = 0;
        isInReaction = true;
    }

    public void StopReactionMode()
    {
        spriteRenderer.color = normalColor;
        currentTimeTillNextReaction = Random.Range(minTimeTillNextReaction, maxTimeTillNextReaction);
        isInReaction = false;
    }

    public void OnTouchDown()
    {
        if (!isInReaction)
        {
            GameObject.FindObjectOfType<GameManager>().GameOver(GameOverInfoTextType.ReactionBossEnemyClickedOnRed);
            return;
        }
        else if(currentTimeInsideReaction >= maxTimeInsideReaction)
        {
            float timeTooSlow = currentTimeInsideReaction - maxTimeInsideReaction;
            string timeTooSlowFormattet = ((int) timeTooSlow + "," + (int) ((timeTooSlow - (int) timeTooSlow) * 1000)) + "s";

            GameObject.FindObjectOfType<GameManager>().GameOver("You were " + timeTooSlowFormattet + " too slow.");
            return;
        }

        lives -= 1;

        numberText.SetText((int) currentTimeInsideReaction + "," + (int) ((currentTimeInsideReaction - (int) currentTimeInsideReaction) * 1000));
        healthBar.SetValue(lives);
        if (lives <= 0)
        {
            Kill();
        }
        else
        {
            PlayHitAnimation();
            StopReactionMode();
        }
    }

    /*[SerializeField] private int maxDamageOnHit;
    [SerializeField] private int minDamageOnHit;

    private int CalculateHitDamage()
    {
        int damage = (int) ((1 - Mathf.Clamp(currentTimeInsideReaction, 0, 1)) * (maxDamageOnHit - minDamageOnHit)) + minDamageOnHit;
        return damage;
    }*/

    private void PlayHitAnimation()
    {
        LeanTween.cancel(gameObject);
        gameObject.LeanScale(startScale, 0);
        Vector3 targetScale = new Vector3(startScale.x - 0.2f, startScale.y - 0.2f, startScale.z - 0.2f);
        LeanTween.scale(gameObject, targetScale, 0.5f).setEasePunch();

        ParticleSystem particle = Instantiate(looseLiveParticle);
        particle.transform.position = transform.position;
    }

    public void OnTouchHold()
    {

    }

    public void OnTouchUp()
    {

    }
}
