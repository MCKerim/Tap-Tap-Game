using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontTapEnemy : Enemy, ITouchInput
{
    public void Start()
    {
        base.Setup();
    }

    private void Update()
    {
        UpdateTimeToKill();
        RotateOnZ();
    }

    public new void UpdateTimeToKill()
    {
        if (currentTime <= 0)
        {
            Kill();
        }
        else
        {
            currentTime -= Time.deltaTime;
        }
    }

#if UNITY_EDITOR
    private void OnMouseDown()
    {
        OnTouchDown();
    }
#endif

    public void OnTouchDown()
    {
        if (!livesEndless)
        {
            TriggerGameOver();
        }
    }

    public void OnTouchHold()
    {

    }

    public void OnTouchUp()
    {

    }
}
