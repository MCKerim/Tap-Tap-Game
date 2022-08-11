using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITouchInput
{
    void OnTouchDown();
    void OnTouchHold();
    void OnTouchUp();
}
