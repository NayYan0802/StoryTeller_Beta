using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Uduino;

public class AngleServo : MonoBehaviour
{
    UduinoDevice ServoBoard;

    public float MaxAngle;

    //Init sub
    private Subscription<RotateServoData> RotateServoSub;

    // Start is called before the first frame update
    void Start()
    {
        ServoBoard = UduinoManager.Instance.GetBoard("ServoBoard");
        //Assign sub event
        RotateServoSub = EventBus.Subscribe<RotateServoData>(_RotateServo);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void _RotateServo(RotateServoData e)
    {
        StartCoroutine(DelayedSendCommand("ServoAngle", MaxAngle));
    }

    IEnumerator DelayedSendCommand(string command, float parameter1)
    {
        yield return new WaitForSeconds(0.1f);
        UduinoManager.Instance.sendCommand(ServoBoard, command, parameter1);
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe(RotateServoSub);
    }

}
