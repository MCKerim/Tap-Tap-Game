using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TouchInputManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI touchingObjectsText;

    private ArrayList newTouchingObjects = new ArrayList();
    private ArrayList currentFrameTouchingObjects = new ArrayList();
    private ArrayList holdingTouchingObjects = new ArrayList();

    private bool isRecognizingTouches = true;

    // Start is called before the first frame update
    void Start()
    {
        touchingObjectsText.SetText("");
    }

    // Update is called once per frame
    void Update()
    {
        if (!isRecognizingTouches)
        {
            return;
        }
        Touch[] touches = Input.touches;

        newTouchingObjects.Clear();
        currentFrameTouchingObjects.Clear();
        foreach (Touch touch in touches)
        {
            Vector3 touchPosWorld = Camera.main.ScreenToWorldPoint(touch.position);
            Vector2 touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);
            RaycastHit2D hitInformation = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);

            if (hitInformation.collider != null)
            {
                GameObject touchedObject = hitInformation.transform.gameObject;
                ITouchInput touchInterface = touchedObject.GetComponent<ITouchInput>();
                if (touchInterface != null)
                {
                    if(touch.phase == TouchPhase.Began)
                    {
                        //DOWN
                        touchInterface.OnTouchDown();
                        if (!newTouchingObjects.Contains(touchedObject))
                        {
                            newTouchingObjects.Add(touchedObject);
                        }
                    }
                    else if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
                    {
                        if (!currentFrameTouchingObjects.Contains(touchedObject))
                        {
                            currentFrameTouchingObjects.Add(touchedObject);
                        }
                    }
                }
            }
            else
            {

            }
        }

        foreach (GameObject touchedObject in holdingTouchingObjects)
        {
            if (touchedObject != null && !currentFrameTouchingObjects.Contains(touchedObject))
            {
                //UP
                touchedObject.GetComponent<ITouchInput>().OnTouchUp();
                holdingTouchingObjects.Remove(touchedObject);
            }
            else if(touchedObject == null)
            {
                holdingTouchingObjects.Remove(touchedObject);
            }
            else
            {
                //HOLD
                touchedObject.GetComponent<ITouchInput>().OnTouchHold();
            }
        }

        foreach (GameObject touchedObject in newTouchingObjects)
        {
            if(touchedObject != null && !holdingTouchingObjects.Contains(touchedObject))
            {
                holdingTouchingObjects.Add(touchedObject);
            }
        }

        touchingObjectsText.SetText("Touching: " + currentFrameTouchingObjects.Count + " Holding: " + holdingTouchingObjects.Count);
    }

    public void Pause()
    {
        isRecognizingTouches = false;
    }

    public void Resume()
    {
        isRecognizingTouches = true;
    }
}
