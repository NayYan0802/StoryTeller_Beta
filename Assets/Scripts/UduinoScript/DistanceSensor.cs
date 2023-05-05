using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Uduino;

public class DistanceSensor : MonoBehaviour
{
    UduinoDevice MyBoard;

    public float Distance, ServoPos;
    public float ServoPosMax=180;
    public string DistanceRead;

    void Start()
    {
        MyBoard = UduinoManager.Instance.GetBoard("StoryStudio");
    }

    void Update()
    {
        DistanceRead = UduinoManager.Instance.Read(MyBoard, "Distance");
        float.TryParse(DistanceRead, out Distance);

        if (Distance > 0f && Distance < 20f)
        {
            ServoPos = (ServoPosMax / 20) * Distance;
            //StartCoroutine(DelayedSendCommand("ServoRun", 0.01f, ServoPos));
            //Send data to servo
            EventBus.Publish(new RotateServoData(0));
        }
    }
}
