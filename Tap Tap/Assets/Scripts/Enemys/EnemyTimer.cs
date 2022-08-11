using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTimer : MonoBehaviour
{
    [SerializeField] private GameObject timerSprite;
    private float timeToLife;
    [SerializeField] private float percentWhenToStart;

    public void SetTimeToLife(float time)
    {
        timeToLife = time;
        timerSprite.transform.localScale = new Vector3(0, 0, 0);
    }

    public void UpdateTime(float time)
    {
        float currentValueInPercent = 1 / timeToLife * (timeToLife - time);

        if (percentWhenToStart < currentValueInPercent)
        {
            float timeStart = timeToLife * (1 - percentWhenToStart);
            float currentValue = 1 / timeStart * (timeStart - time);
            timerSprite.transform.localScale = new Vector3(currentValue, currentValue, 1);
        }
    }
}
