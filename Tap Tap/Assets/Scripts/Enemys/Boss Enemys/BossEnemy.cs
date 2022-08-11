using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossEnemy : Enemy, ITouchInput
{
    [SerializeField] private IndicatorBar healthBar;

    [SerializeField] private int lives;
    [SerializeField] private ParticleSystem looseLiveParticle;

    private SpriteRenderer spriteRenderer;
    private bool switchColor;
    private Color mainColor;
    [SerializeField] private Color secondColor;

    private Vector3 startScale;

    // Start is called before the first frame update
    void Start()
    {
        startScale = transform.localScale;
        base.Setup();
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainColor = spriteRenderer.color;

        healthBar.SetMaxValue(lives);
        healthBar.SetValue(lives);
    }

#if UNITY_EDITOR
    private void OnMouseDown()
    {
        OnTouchDown();
    }
#endif

    public void OnTouchDown()
    {
        lives--;
        healthBar.SetValue(lives);
        if (lives <= 0)
        {
            Kill();
        }
        else
        {
            if (switchColor)
            {
                spriteRenderer.color = mainColor;
                switchColor = false;
            }
            else
            {
                spriteRenderer.color = secondColor;
                switchColor = true;
            }

            LeanTween.cancel(gameObject);
            gameObject.LeanScale(startScale, 0);
            Vector3 targetScale = new Vector3(startScale.x - 0.2f, startScale.y - 0.2f, startScale.z - 0.2f);
            LeanTween.scale(gameObject, targetScale, 0.5f).setEasePunch();

            ParticleSystem particle = Instantiate(looseLiveParticle);
            particle.transform.position = transform.position;
        }
    }

    public void OnTouchHold()
    {
        
    }

    public void OnTouchUp()
    {
        
    }
}
