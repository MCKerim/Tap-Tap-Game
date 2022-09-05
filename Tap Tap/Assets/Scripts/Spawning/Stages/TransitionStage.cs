using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class TransitionStage : MonoBehaviour, IStage
{
    [SerializeField] private float secondsToWait;
    private float currentTime;

    [SerializeField] private bool playTransitionAnimation;
    [SerializeField] private bool waitTillAllEnemysAreCleared;
    [SerializeField] private TextMeshProUGUI transitionText;

    [SerializeField] private float waitTillTextTravelStarts;
    [SerializeField] private float textTravelTime;
    [SerializeField] private float waitTextTravelStand;
    [SerializeField] private float textFadeOutTime;
    [SerializeField] private float waitOnBlankStage;

    [SerializeField] private LeanTweenType easeType;

    [SerializeField] private CanvasGroup transitionPanel;

    private bool animationIsPlaying;
    private bool isOver;

    private TouchInputManager touchInputManager;

    private void Start()
    {
        touchInputManager = GameObject.FindObjectOfType<TouchInputManager>();
    }

    public void SetIsEndless(bool isEndless)
    {

    }

    public bool IsOver()
    {
        return isOver;
    }

    public void Reset()
    {
        if (playTransitionAnimation)
        {
            secondsToWait = waitTillTextTravelStarts + textTravelTime + waitTextTravelStand + textFadeOutTime + waitOnBlankStage;
            animationIsPlaying = false;
            transitionText.gameObject.SetActive(false);
            transitionPanel.gameObject.SetActive(false);
        }
        currentTime = secondsToWait;
        isOver = false;
    }

    public void UpdateStage()
    {
        if(waitTillAllEnemysAreCleared && FindObjectsOfType<Enemy>().Length != 0)
        {
            return;
        }

        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
        }
        else
        {
            if (playTransitionAnimation)
            {
                transitionText.gameObject.SetActive(false);
                transitionPanel.gameObject.SetActive(false);
            }
            touchInputManager.StartRecognizingTouches();
            isOver = true;
        }

        if(playTransitionAnimation && !animationIsPlaying)
        {
            touchInputManager.StopRecognizingTouches();
            PlayTransitionAnimation();
        }
    }

    private void PlayTransitionAnimation()
    {
        animationIsPlaying = true;

        transitionText.gameObject.SetActive(true);
        transitionPanel.gameObject.SetActive(true);

        LeanTween.scaleY(transitionText.gameObject, 30, 0);
        LeanTween.alphaCanvas(transitionText.GetComponent<CanvasGroup>(), 1, 0);
        LeanTween.alphaCanvas(transitionPanel, 0, 0);

        LeanTween.scaleY(transitionText.gameObject, 4, textTravelTime).setEase(easeType).setDelay(waitTillTextTravelStarts);

        LeanTween.alphaCanvas(transitionText.GetComponent<CanvasGroup>(), 0, textFadeOutTime).setDelay(waitTillTextTravelStarts + textTravelTime + waitTextTravelStand);

        LeanTween.alphaCanvas(transitionPanel, 1f, waitTillTextTravelStarts - 1f);
        LeanTween.alphaCanvas(transitionPanel, 0f, textFadeOutTime).setDelay(waitTillTextTravelStarts + textTravelTime + waitTextTravelStand);
    }
}
