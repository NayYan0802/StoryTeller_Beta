using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Uduino;

public class UltrasonicToServo : MonoBehaviour
{
    UduinoDevice MyBoard;

    public float Distance, ServoPosMax, ServoPos;
    public string DistanceRead;

    IEnumerator DelayedSendCommand(string command, float delayTime, float Num1)
    {
        yield return new WaitForSeconds(delayTime);
        UduinoManager.Instance.sendCommand(MyBoard, command, Num1);
    }

    // Start is called before the first frame update
    void Start()
    {
        MyBoard = UduinoManager.Instance.GetBoard("StoryStudio");
    }

    // Update is called once per frame
    void Update()
    {
        DistanceRead = UduinoManager.Instance.Read(MyBoard, "Distance");
        float.TryParse(DistanceRead, out Distance);

        if (Distance > 0f && Distance < 20f)
        {
            ServoPos = (ServoPosMax / 20) * Distance;
            StartCoroutine(DelayedSendCommand("ServoRun", 0.01f, ServoPos));
        }
    }
}
