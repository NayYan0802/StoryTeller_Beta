using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickInput : MonoBehaviour
{
    onAssetInWindow onAsset;
    // Start is called before the first frame update
    void Start()
    {
        onAsset = this.GetComponent<onAssetInWindow>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)&&onAsset.isPlayMode)
        {
            if(onAsset.data.hasMouseClick&& onAsset.data.thisMouseClick == Assets.DataManager.MouseClick.Left)
            {
                if (onAsset.data.hasAnimation)
                {
                    if(onAsset.data.thisAnimation== Assets.DataManager.AnimationType.In)
                    {
                        this.GetComponent<AnimationOutput>()._FlyInEvent();
                    }
                    if(onAsset.data.thisAnimation== Assets.DataManager.AnimationType.Out)
                    {
                        this.GetComponent<AnimationOutput>()._FlyOutEvent();
                    }
                }
            }
        }
    }
}
