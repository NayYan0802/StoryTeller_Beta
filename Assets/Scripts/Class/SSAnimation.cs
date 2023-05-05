using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSAnimation : MonoBehaviour
{
    public GameObject AniObject { get; set; }

    public Transform[] PosArray { get; set; }

    ////Positon
    //public Vector2 StartPos { get; set; }
    //public Vector2 EndPos { get; set; }

    ////Scale
    //public Vector3 StartScale { get; set; }
    //public Vector3 EndScale { get; set; }

    ////Rotation
    //public Vector3 StartRotation { get; set; }
    //public Vector3 EndRotation { get; set; }


    //public float TimeSum { get; set; }
    //public float[] TimeArray { get; set; }
    public float speed { get; set; }

    private int AniIdx;

    public void StartAnimation()
    {
        //_gameObject.transform.position = StartPos;
        Debug.Log("StartAnimation");
        //StartCoroutine(PositionAni());
    }

    //public IEnumerator LinearAnimation()
    //{
    //    Debug.Log(TimeSum);
    //    AniObject.transform.position = StartPos;
    //    float time = 0f;

    //    //Check Input Validation
    //    if (TimeSum <= 0)
    //    {
    //        Debug.LogError("Wrong Time Sum!");
    //    }

    //    //Main Loop
    //    while (time < TimeSum)
    //    {
    //        time += Time.deltaTime;
    //        AniObject.transform.position = Vector3.Lerp(StartPos, EndPos, time / TimeSum);
    //        AniObject.transform.localScale = Vector3.Lerp(StartScale, EndScale, time / TimeSum);
    //        AniObject.transform.eulerAngles = Vector3.Lerp(StartRotation, EndRotation, time / TimeSum);
    //        yield return null;
    //    }

    //    //Fixed Frame
    //    AniObject.transform.position = EndPos;
    //    AniObject.transform.localScale = EndScale;
    //    AniObject.transform.eulerAngles = EndRotation;
    //} 
    
    public IEnumerator LinearAnimation()
    {
        while (AniIdx < PosArray.Length-1)
        {
            //Debug.Log(TimeArray[AniIdx]);

            float time = 0f;
            Transform start = PosArray[AniIdx];
            Transform end = PosArray[AniIdx + 1];
            float TimeSum = Vector2.Distance(start.position, end.position) / speed;
            //float TimeSum = TimeArray[AniIdx];

            AniObject.transform.position = start.position;
            //AniObject.transform.localScale = start.localScale;
            AniObject.transform.eulerAngles = start.eulerAngles;

            //Check Input Validation
            //if (TimeArray[AniIdx] <= 0)
            //{
            //    Debug.LogError("Wrong Time Sum!");
            //}

            //Main Loop
            while (time < TimeSum)
            {
                time += Time.deltaTime;
                AniObject.transform.position = Vector3.Lerp(start.position, end.position, time / TimeSum);
                //AniObject.transform.localScale = Vector3.Lerp(start.localScale, end.localScale, time / TimeSum);
                AniObject.transform.eulerAngles = Vector3.Lerp(start.eulerAngles, end.eulerAngles, time / TimeSum);
                yield return null;
            }

            //Fixed Frame
            AniObject.transform.position = end.position;
            //AniObject.transform.localScale = end.localScale;
            AniObject.transform.eulerAngles = end.eulerAngles;

            AniIdx++;
        }
        //if (AniIdx < TimeArray.Length)
        //{
        //    StartCoroutine(LinearAnimation());
        //}
    }


    public SSAnimation()
    { }

    //public SSAnimation(Transform startPos, Transform endPos, float timesum, GameObject aniObject)
    //{
    //    this.StartPos = startPos.position;
    //    this.EndPos = endPos.position;
    //    this.StartScale = startPos.localScale;
    //    this.EndScale = endPos.localScale;
    //    this.StartRotation = startPos.eulerAngles;
    //    this.EndRotation = endPos.eulerAngles;
    //    this.TimeSum = timesum;
    //    this.AniObject = aniObject;
    //}

    //public SSAnimation(Transform[] posArray, float[] timeArray, GameObject aniObject)
    //{
    //    this.PosArray = posArray;
    //    this.TimeArray = timeArray;
    //    this.AniObject = aniObject;
    //    this.AniIdx = 0;
    //}
    public SSAnimation(Transform[] posArray, float speed, GameObject aniObject)
    {
        this.PosArray = posArray;
        this.speed = speed;
        this.AniObject = aniObject;
        this.AniIdx = 0;
    }
}
