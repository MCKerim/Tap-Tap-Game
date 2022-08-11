using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneTapEnemy : Enemy, ITouchInput
{
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
        Kill();
    }

    public void OnTouchHold()
    {
        
    }

    public void OnTouchUp()
    {

    }
}
