using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleTapEnemy : Enemy, ITouchInput
{
    private int lives = 2;
    [SerializeField] private ParticleSystem looseLiveParticle;

    // Start is called before the first frame update
    void Start()
    {
        base.Setup();
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
        if (lives <= 0)
        {
            Kill();
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.white;

            Vector3 targetScale = new Vector3(transform.localScale.x - 0.2f, transform.localScale.y - 0.2f, transform.localScale.z - 0.2f);
            LeanTween.scale(gameObject, targetScale, 0.75f).setEasePunch();

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
