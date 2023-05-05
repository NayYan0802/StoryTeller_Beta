using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.DataManager
{
    public enum MouseClick {None, Left, Right, Double };
    public enum Senser {None, Distance, Sound, ArcadeButtonA, ArcadeButtonB, Motion, RFID };
    public enum Servo {None, Rotate };
    public enum AnimationType {None,In,Out, Scale, Rotate };
    //This Class Saves every pieces of data of Assets in the window;
    [System.Serializable]
    public struct AssetData
    {
        public MouseClick thisMouseClick;
        public Senser thisSenser;
        public AudioClip thisAudio;
        public AnimationType thisAnimation;
        public Servo thisServo;
        public bool hasSensor;
        public bool hasMouseClick;
        public bool hasAnimation;
        public bool hasPresetAnimation;
        public bool hasCustomAnimation;
        public List<Transform> thisCostumAnimationStops;
        public float CostumAniSpeed;
        public bool hasAudio;
        public bool hasServo;
        public int timeOfSpin;
        public int AnglesOfSpin;
    }
}
