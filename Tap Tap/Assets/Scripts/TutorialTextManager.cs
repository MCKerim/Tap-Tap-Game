using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTextManager : MonoBehaviour
{
    [SerializeField] private GameObject tutorialCircle;
    [SerializeField] private Transform trans1;
    [SerializeField] private Transform trans2;
    [SerializeField] private Transform trans3;

    [SerializeField] private float marginLeft;
    [SerializeField] private float marginTop;
    private float leftScreenBorder;
    private float topScreenBorder;

    private void Awake()
    {
        LeanTween.scale(gameObject, new Vector3(1.5f, 1.5f, 1), 0.5f).setLoopPingPong().setEaseInOutCirc();

        leftScreenBorder = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x;
        topScreenBorder = Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelHeight, 0)).y;
    }

#if UNITY_EDITOR
    private void Update()
    {
        leftScreenBorder = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x;
        topScreenBorder = Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelHeight, 0)).y;

        UpdatePos();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector3 upperLeft = new Vector3(leftScreenBorder + marginLeft, topScreenBorder - marginTop, 10);
        Vector3 upperRight = new Vector3(5, topScreenBorder - marginTop, 10);
        Vector3 bottomLeft = new Vector3(leftScreenBorder + marginLeft, -5, 10);

        Gizmos.DrawLine(upperLeft, upperRight);
        Gizmos.DrawLine(upperLeft, bottomLeft);
    }
#endif

    public void UpdatePos()
    {
        if(tutorialCircle.transform.position.y > topScreenBorder - marginTop)
        {
            transform.position = trans3.position;
            transform.rotation = trans3.rotation;
        }
        else if (tutorialCircle.transform.position.x < leftScreenBorder + marginLeft)
        {
            transform.position = trans2.position;
            transform.rotation = trans2.rotation;
        }
        else
        {
            transform.position = trans1.position;
            transform.rotation = trans1.rotation;
        }
    }
}
