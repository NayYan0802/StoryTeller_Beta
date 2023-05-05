using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationOutput : MonoBehaviour
{
    onAssetInWindow onAsset;


    //Init sub
    private Subscription<FlyInEvent> FlyInEventSub;
    private Subscription<FlyOutEvent> FlyOutEventSub;

    public Vector2 pos;
    public Vector2 dir;
    public Vector2 des;
    public bool needInit=false;



    // Start is called before the first frame update
    public void Start()
    {
        //onAsset = this.GetComponent<onAssetInWindow>();
        //Assign sub event
        //FlyInEventSub = EventBus.Subscribe<FlyInEvent>(_FlyInEvent);
        //FlyOutEventSub = EventBus.Subscribe<FlyOutEvent>(_FlyOutEvent);
        if (!needInit)
        {
            pos = this.GetComponent<RectTransform>().localPosition;
            //pos = this.GetComponent<onAssetInWindow>().originPos;
            dir.x = pos.x / Vector2.Distance(pos, Vector2.zero);
            dir.y = pos.y / Vector2.Distance(pos, Vector2.zero);
            if (Vector2.Distance(pos, Vector2.zero) == 0)
                dir = Vector2.right;
            dir=dir.normalized;
            des = pos + dir * 1000;
            //Debug.Log(des);
        }
        //if (this.GetComponent<onAssetInWindow>().data.hasAnimation&& 
        //    this.GetComponent<onAssetInWindow>().data.thisAnimation== Assets.DataManager.AnimationType.In)
        //{
        //    this.GetComponent<RectTransform>().position = des;
        //}
    }

    public void InitPos()
    {
        Prepare();
        //Debug.Log(des + "      " + pos);
        //Debug.Log(des);
        this.GetComponent<RectTransform>().localPosition = des;
        Debug.Log(des);

    }

    public void Prepare()
    {
        pos = this.GetComponent<RectTransform>().localPosition;
        Debug.Log("InitPos"+pos);
        dir.x = pos.x / Vector2.Distance(pos, Vector2.zero);
        dir.y = pos.y / Vector2.Distance(pos, Vector2.zero);
        if (Vector2.Distance(pos, Vector2.zero) == 0)
            dir = Vector2.right;
        des = pos + dir * 1000;
        dir = dir.normalized;
        Debug.Log("InitDES" + des);
    }

    // Update is called once per frame
    void Update()
    {
        //pos = this.GetComponent<RectTransform>().localPosition;
    }

    public void _FlyInEvent()
    {
        //if(onAsset.isPlayMode)
        StartCoroutine(AnimationMove(des, pos, 1));
        Debug.Log(des + "      " + pos);
    }

    public void _FlyOutEvent()
    {
        //if(onAsset.isPlayMode)
            StartCoroutine(AnimationMove(pos, des, 1));
    }

    IEnumerator AnimationMove(Vector2 start, Vector2 end, float time)
    {
        float timeSum=0;
        while (timeSum < time)
        {
            //Debug.Log(timeSum);
            timeSum += Time.deltaTime;
            this.GetComponent<RectTransform>().localPosition = start + (end - start) * timeSum / time;
            yield return null;
        }
    }
}
