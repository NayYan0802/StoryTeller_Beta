using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Uduino;

public class UduinoCallServo : MonoBehaviour
{
    UduinoDevice StoryStudio;

    public string DistanceRead;
    public int DistanceInt;


    public string ServoPos;
    //Temporary Servo trigger
    public bool ServoTester = false;

    // Start is called before the first frame update
    void Start()
    {
        StoryStudio = UduinoManager.Instance.GetBoard("StoryStudio");
    }

    // Update is called once per frame
    void Update()
    {
        //Get Distance from Ultrasonic Sensor
        DistanceRead = UduinoManager.Instance.Read(StoryStudio, "Distance");
        int.TryParse(DistanceRead, out DistanceInt);

        //Activate Servo
        if (ServoTester == true)
        {
            UduinoManager.Instance.sendCommand(StoryStudio, "ServoRun");
            ServoTester = false;
        }

    }
}
