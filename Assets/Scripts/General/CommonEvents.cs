using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPlay
{
    public StartPlay() { }
}

public class FlyInEvent
{
    public FlyInEvent() { }
}

public class FlyOutEvent
{
    public FlyOutEvent() { }
}

//=========================================Uduino==========================================

public class RotateServoData
{
    public int idx;
    public RotateServoData(int _idx)
    {
        idx = _idx;
    }
}
