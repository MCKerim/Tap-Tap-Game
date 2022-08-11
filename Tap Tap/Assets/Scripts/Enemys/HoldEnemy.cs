using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldEnemy : Enemy, ITouchInput
{

    [SerializeField] private float timeToHold;
    private float currentLife;
    private Color c;

    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        base.Setup();
        currentLife = timeToHold;

        spriteRenderer = GetComponent<SpriteRenderer>();
        c = spriteRenderer.color;
    }

#if UNITY_EDITOR
    private void OnMouseDrag()
    {
        if(currentLife <= 0)
        {
            Kill();
        }
        else
        {
            currentLife -= Time.deltaTime;
           
            c.a = (1 / timeToHold) * currentLife;
            spriteRenderer.color = c;
        }
    }

    private void OnMouseUp()
    {
        currentLife = timeToHold;
        c.a = 1;
        spriteRenderer.color = c;
    }
#endif

    public void OnTouchDown()
    {
        
    }

    public void OnTouchHold()
    {
        if (isDead)
        {
            return;
        }
        if (currentLife <= 0)
        {
            Kill();
        }
        else
        {
            currentLife -= Time.deltaTime;
            c.a = (1 / timeToHold) * currentLife;
            spriteRenderer.color = c;
        }
    }

    public void OnTouchUp()
    {
        if (isDead)
        {
            return;
        }
        currentLife = timeToHold;
        c.a = (1 / timeToHold) * currentLife;
        spriteRenderer.color = c;
    }
}
