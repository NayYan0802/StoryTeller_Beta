using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Uduino;

public class SoundSensor : MonoBehaviour
{

    UduinoDevice MyBoard;

    public float Volume, ServoPos;
    public float ServoPosMax = 180;
    public string VolumeRead;
    // Start is called before the first frame update
    void Start()
    {
        MyBoard = UduinoManager.Instance.GetBoard("StoryStudio");
    }

    // Update is called once per frame
    void Update()
    {
        VolumeRead = UduinoManager.Instance.Read(MyBoard, "Volume");
        float.TryParse(VolumeRead, out Volume);

        if(Volume > 120f && Volume <= 1023f)
        {
            EventBus.Publish(new RotateServoData(0));
        }
    }
}
