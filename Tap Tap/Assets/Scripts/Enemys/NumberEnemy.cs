using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NumberEnemy : Enemy, ITouchInput
{
    [SerializeField] private TextMeshProUGUI numberText;
    public int number;
    private bool canBeKilled;
    private NumberEnemy nextNumberToKill;

    // Start is called before the first frame update
    void Start()
    {
        base.Setup();
    }

    public void SetNumber(int number)
    {
        this.number = number;
        numberText.SetText(number.ToString());
    }

    public void SetNextEnemy(NumberEnemy nextNumberToKill)
    {
        this.nextNumberToKill = nextNumberToKill;
    }

    public void CanBeKilledNow()
    {
        canBeKilled = true;
    }

#if UNITY_EDITOR
    private void OnMouseDown()
    {
        OnTouchDown();
    }
#endif

    public void OnTouchDown()
    {
        if (canBeKilled)
        {
            if(nextNumberToKill != null)
            {
                nextNumberToKill.CanBeKilledNow();
            }
            Kill();
        }
        else
        {
            GameObject.FindObjectOfType<GameManager>().GameOver();
        }
    }

    public void OnTouchHold()
    {
        
    }

    public void OnTouchUp()
    {
        
    }
}
