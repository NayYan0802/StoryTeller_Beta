using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Uduino;

public class RFIDSensorRead : MonoBehaviour
{
    UduinoDevice RFIDBoard;

    public string RFIDRead;
    public float RFIDInt;
    // Start is called before the first frame update
    void Start()
    {
        RFIDBoard = UduinoManager.Instance.GetBoard("RFIDBoard");
    }

    // Update is called once per frame
    void Update()
    {
        RFIDRead = UduinoManager.Instance.Read(RFIDBoard, "RFID");
        float.TryParse(RFIDRead, out RFIDInt);

        if(RFIDInt == 1)
        {
            EventBus.Publish(new RotateServoData(0));
        }
    }
}
