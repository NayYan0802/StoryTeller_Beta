using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    private void OnMouseUpAsButton()
    {
        Debug.Log("StartPlay");
        EventBus.Publish(new StartPlay());
    }
}
