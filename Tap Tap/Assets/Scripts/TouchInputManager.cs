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

    private bool isRecognizingTouches;

    private GameManager gameManager;
    private EnemySpawnFunctions enemySpawnFunctions;

    private bool allowedToMiss = true;
    private bool currentlyAllowedToMiss;

    [SerializeField] private GameObject tapParticle;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        enemySpawnFunctions = GameObject.FindObjectOfType<EnemySpawnFunctions>();

        touchingObjectsText.SetText("");

        ResetCurrentlyAllowedToMiss();
    }

    public void StartRecognizingTouches()
    {
        holdingTouchingObjects.Clear();
        isRecognizingTouches = true;
    }

    public void StopRecognizingTouches()
    {
        isRecognizingTouches = false;
    }

    public void SetCurrentlyAllowedToMissTrue(){
        currentlyAllowedToMiss = true;
    }

    public void ResetCurrentlyAllowedToMiss(){
        currentlyAllowedToMiss = allowedToMiss;
    }

    public void SetAllowedToMiss(bool isAllowedToMiss){
        allowedToMiss = isAllowedToMiss;
        ResetCurrentlyAllowedToMiss();
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

            if(touch.phase == TouchPhase.Began)
            {
                Instantiate(tapParticle, new Vector3(touchPosWorld2D.x, touchPosWorld2D.y, 10), Quaternion.identity);
            }

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
            else if(!currentlyAllowedToMiss)
            {
                //Missed
                if (touch.phase == TouchPhase.Began && enemySpawnFunctions.CheckIfPointIsInSpawnField(touchPosWorld2D))
                {
                    gameManager.GameOver(GameOverInfoTextType.EnemyMissed);
                }
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
}
