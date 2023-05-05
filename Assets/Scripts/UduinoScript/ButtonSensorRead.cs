using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Uduino;

public class ButtonSensorRead : MonoBehaviour
{
    UduinoDevice ButtonBoard;

    public string ButtonRead;
    public float ButtonInt;

    // Start is called before the first frame update
    void Start()
    {
        ButtonBoard = UduinoManager.Instance.GetBoard("ButtonBoard");
    }

    // Update is called once per frame
    void Update()
    {
        ButtonRead = UduinoManager.Instance.Read(ButtonBoard, "Button");
        float.TryParse(ButtonRead, out ButtonInt);

        if(ButtonInt == 1)
        {
            EventBus.Publish(new RotateServoData(0));
            ButtonInt = 0;
        }
    }
}
