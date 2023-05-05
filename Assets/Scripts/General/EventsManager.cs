using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Assets.DataManager;

namespace Assets.EventsManager
{
    public enum InputType {None, MouseClick, Sensor }
    public enum OutputType {None, Animation, Audio, Servo }
    [System.Serializable]
    public class EventOnOnePage
    {
        [System.Serializable]
        public struct thisEvent
        {
            public int EventIdx;
            public GameObject Object;
            public GameObject UI;
            public GameObject UICountCorner;
            public AssetData data;
        }
        public GameObject thisPage;
        public List<thisEvent> EventsOnThisPage= new List<thisEvent>();
    }

    public class EventsManager : MonoBehaviour
    {
        public PlayManager PlayManager;
        public List<EventOnOnePage> allEvents;
        public int currentPageIdx;
        public int currentEventIdx = 0;
        public GameObject eventPrefeb;
        public GameObject eventCountCornerPrefeb;
        public Transform eventContent;
        public EventOnOnePage.thisEvent currentEvent;
        public EventOnOnePage.thisEvent currentPlayingEvent;
        public AssetData data;

        //Playmode
        public bool isPlaying;
        public int currentPlayingIdx;

        private void Update()
        {
            if (GameManager.Instance.isPlaymode)
            {
                //Run the events
                //PlayModeUpdate();
                return;
            }
            //Debug.Log(allEvents.Count + "      " + GameManager.Instance.pageIdx);
            //Debug.Log(allEvents[GameManager.Instance.pageIdx] != null);
            List<EventOnOnePage.thisEvent> EventList = allEvents[GameManager.Instance.pageIdx].EventsOnThisPage;
            if (EventList.Count>0)
            {
                //only data changes
                //currentEvent = EventList[currentEventIdx];
                //currentEvent.data = data;
                //EventList[currentEventIdx] = currentEvent;

                /*
                 * It's very dangerous to have this running on everyframe cause it may cover the data in current event
                 * Instead we need to call this UpdateEventList() at the right moment
                 */
                EventUpdate();
                UpdateEventList();
            }
        }

        public void AddEvent()
        {
            //if (!GameManager.Instance.currentObjectValidationCheck())
            //{
            //    return;
            //}
            //Get the EventList for this page
            currentPageIdx = GameManager.Instance.pageIdx;
            List<EventOnOnePage.thisEvent> EventList = allEvents[currentPageIdx].EventsOnThisPage;
            for (int i = 0; i < EventList.Count; i++)
            {
                if(EventList[i].Object== GameManager.Instance.currentObjectInWindow.gameObject)
                {
                    return;
                }
            }
            allEvents[GameManager.Instance.pageIdx].thisPage = GameManager.Instance.currentPage;
            EventOnOnePage.thisEvent newEvent = new EventOnOnePage.thisEvent();
            //newEvent.data = new AssetData();
            newEvent.Object = GameManager.Instance.currentObjectInWindow.gameObject;
            //Debug.Log("AddUI");
            newEvent.UI = Instantiate(eventPrefeb, eventContent);
            newEvent.UICountCorner = Instantiate(eventCountCornerPrefeb, newEvent.Object.transform.GetChild(1));
            newEvent.EventIdx = EventList.Count;
            newEvent.UI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (newEvent.EventIdx+1).ToString();
            newEvent.UICountCorner.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (newEvent.EventIdx+1).ToString();
            EventList.Add(newEvent);            
        }

        public void DeleteEvent()
        {
            Destroy(currentEvent.UI);
            Destroy(currentEvent.UICountCorner);
            List<EventOnOnePage.thisEvent> EventList = allEvents[GameManager.Instance.pageIdx].EventsOnThisPage;
            EventList.Remove(currentEvent);
            currentEventIdx = 0;
            GameManager.Instance.hasSelectEvent = false;
        }

        private List<EventOnOnePage.thisEvent> GetEventListOnThisPage()
        {
            return allEvents[GameManager.Instance.pageIdx].EventsOnThisPage;
        }

        public void EventUpdate()
        {
            List<EventOnOnePage.thisEvent> EventList = GetEventListOnThisPage();
            //Get the "Pointer" of current event info
            if (currentEvent.EventIdx == currentEventIdx)
            {
                //Debug.Log(true);
                //Same Event

                currentEvent = EventList[currentEventIdx];
                //the data here might be the old data
                currentEvent.data = data;
                EventList[currentEventIdx] = currentEvent;
            }
            else
            {
                //Debug.Log(false);
                //New Event
                data = EventList[currentEventIdx].data;
                GameManager.Instance.LoadCurrentObjectData();
                currentEvent = EventList[currentEventIdx];
                currentEvent.data = data;
                EventList[currentEventIdx] = currentEvent;
            }

            //data = new AssetData();

            //List<EventOnOnePage.thisEvent> EventList = GameManager.Instance.EventsManager.allEvents[GameManager.Instance.pageIdx].EventsOnThisPage;
            //EventOnOnePage.thisEvent thisEvent = EventList[currentEventIdx];
            ////thisEvent.data = GameManager.Instance.currentObjectInWindow.data;
            //EventList[currentEventIdx] = thisEvent;
            //EventList[currentEventIdx].data.hasAnimation = GameManager.Instance.currentObjectInWindow.data.hasAnimation;
            //EventList[currentEventIdx].data.hasAudio = GameManager.Instance.currentObjectInWindow.data.hasAudio;
            //EventList[currentEventIdx].data.hasMouseClick = GameManager.Instance.currentObjectInWindow.data.hasMouseClick;
            //EventList[currentEventIdx].data.hasSensor = GameManager.Instance.currentObjectInWindow.data.hasSensor;
            //EventList[currentEventIdx].data.hasServo = GameManager.Instance.currentObjectInWindow.data.hasServo;
        }

        public void UpdateEventList()
        {
            List<EventOnOnePage.thisEvent> EventList = allEvents[GameManager.Instance.pageIdx].EventsOnThisPage;
            //currentEvent = EventList[currentEventIdx];
            //currentEvent.data = data;
            //EventList[currentEventIdx] = currentEvent;
            //while (eventContent.childCount > 0)
            //{
            //    Destroy(eventContent.transform.GetChild(0).gameObject);
            //    Debug.Log("111");
            //}

            //Deactive all the event in UI
            for (int i = eventContent.childCount - 1; i >= 0; i--)
            {
                //Destroy(eventContent.transform.GetChild(i).gameObject);
                eventContent.transform.GetChild(i).gameObject.SetActive(false);
            }
            currentPageIdx = GameManager.Instance.pageIdx;
            //Debug.Log(EventList.Count);



            for (int i = 0; i < EventList.Count; i++)
            {
                //Debug.Log("222");
                GameObject newUI = EventList[i].UI;
                GameObject newUICorner = EventList[i].UICountCorner;
                newUI.SetActive(true);
                newUICorner.SetActive(true);
                newUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (i + 1).ToString();
                newUICorner.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (i + 1).ToString();
                //Input
                if (EventList[i].data.hasMouseClick || EventList[i].data.hasSensor)
                {
                    newUI.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                    //if (EventList[i].Inputs[0] != InputType.None)
                    if (EventList[i].data.hasMouseClick)
                    {
                        //for (int j = 0; j < EventList[i].Inputs.Count; j++)
                        //{
                        //    //Debug.Log("333");
                        //    newUI.transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>().text += EventList[i].Inputs[j].ToString();
                        //    newUI.transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>().text += "  ";
                        //}
                        newUI.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text += "Mouse Click  ";
                    }
                    if (EventList[i].data.hasSensor)
                    {
                        newUI.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text += "Sensor  ";
                    }
                }
                //Output
                //if (EventList[i].Outputs[0] != OutputType.None)
                //{
                //    newUI.transform.GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                //    for (int j = 0; j < EventList[i].Outputs.Count; j++)
                //    {
                //        //Debug.Log("444");
                //        newUI.transform.GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>().text += EventList[i].Outputs[j].ToString();
                //        newUI.transform.GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>().text += "  ";
                //    }
                //}
                if (EventList[i].data.hasAnimation || EventList[i].data.hasAudio || EventList[i].data.hasServo)
                {
                    newUI.transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                    if (EventList[i].data.hasAnimation)
                    {
                        newUI.transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>().text += "Animation  ";
                    }
                    if (EventList[i].data.hasAudio)
                    {
                        newUI.transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>().text += "Audio  ";
                    }
                    if (EventList[i].data.hasServo)
                    {
                        newUI.transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>().text += "Servo  ";
                    }
                }
            }
        }

        public void AddPage()
        {
            EventOnOnePage eventOnOne=new EventOnOnePage();
            //eventOnOne.thisPage = GameManager.Instance.currentPage;
            allEvents.Add(eventOnOne);
            GameManager.Instance.hasSelectEvent = false;
        }

        public void DeletePage()
        {
            allEvents.RemoveAt(GameManager.Instance.pageIdx);
            GameManager.Instance.hasSelectEvent = false;
        }

        public void changeMouseInput(string input)
        {
            switch (input)
            {
                case "Left":
                    data.thisMouseClick = MouseClick.Left;
                    break;
                case "Right":
                    data.thisMouseClick = MouseClick.Right;
                    break;
                case "Double":
                    data.thisMouseClick = MouseClick.Double;
                    break;
                case "None":
                    data.thisMouseClick = MouseClick.None;
                    break;
                default:
                    Debug.LogError("Error MouseClick Type");
                    break;
            }
            //currentEvent.data = data;
            //List<EventOnOnePage.thisEvent> EventList = allEvents[GameManager.Instance.pageIdx].EventsOnThisPage;
            //EventList[currentEventIdx] = currentEvent;
        }

        public void changeSensorInput(string input)
        {
            switch (input)
            {
                case "Distance":
                    data.thisSenser = Senser.Distance;
                    break;
                case "Sound":
                    data.thisSenser = Senser.Sound;
                    break;
                case "RFID":
                    data.thisSenser = Senser.RFID;
                    break;
                case "Motion":
                    data.thisSenser = Senser.Motion;
                    break;
                case "ArcadeButtonA":
                    data.thisSenser = Senser.ArcadeButtonA;
                    break;
                case "ArcadeButtonB":
                    data.thisSenser = Senser.ArcadeButtonB;
                    break;
                default:
                    Debug.LogError("Error Sensor Type");
                    break;
            }
            //currentEvent.data = data;
            //List<EventOnOnePage.thisEvent> EventList = allEvents[GameManager.Instance.pageIdx].EventsOnThisPage;
            //EventList[currentEventIdx] = currentEvent;
        }

        public void changeServo(string input)
        {
            switch (input)
            {
                case "Rotate":
                    data.thisServo = Assets.DataManager.Servo.Rotate;
                    break;
                default:
                    Debug.LogError("Error Servo Type");
                    break;
            }
            //currentEvent.data = data;
            //List<EventOnOnePage.thisEvent> EventList = allEvents[GameManager.Instance.pageIdx].EventsOnThisPage;
            //EventList[currentEventIdx] = currentEvent;
        }

        public void changeAnimationOutput(string input)
        {
            switch (input)
            {
                case "In":
                    data.thisAnimation = Assets.DataManager.AnimationType.In;
                    break;
                case "Out":
                    data.thisAnimation = Assets.DataManager.AnimationType.Out;
                    break;
                case "Scale":
                    data.thisAnimation = Assets.DataManager.AnimationType.Scale;
                    break;
                case "Rotate":
                    data.thisAnimation = Assets.DataManager.AnimationType.Rotate;
                    break;
                default:
                    Debug.LogError("Error Animation Type");
                    break;
            }
            //currentEvent.data = data;
            //List<EventOnOnePage.thisEvent> EventList = allEvents[GameManager.Instance.pageIdx].EventsOnThisPage;
            //EventList[currentEventIdx] = currentEvent;
        }

        public void StartPlayMode()
        {
            GameManager.Instance.isPlaymode = true;
            isPlaying = false;
            PlayManager.isPlaying = false;
            currentPlayingIdx = -1;
            PlayManager.currentPlayingIdx = -1;
            PlayManager.getCurrentPlayList();
            PlayManager.GotCurrentInput = false;

            //Camera Work
            GameManager.Instance.mainCam.gameObject.SetActive(false);
            GameManager.Instance.CapCam.targetTexture=null;
            GameManager.Instance.CapCam.GetComponent<AudioListener>().enabled=true;
            GameObject[] ignoreObjects = GameObject.FindGameObjectsWithTag("Ignore");
            foreach(var o in ignoreObjects)
            {
                o.transform.localScale = Vector3.zero;
            }
        }

        public void PlayModeUpdate()
        {
            List<EventOnOnePage.thisEvent> EventList = allEvents[currentPageIdx].EventsOnThisPage;

            if (!isPlaying)
            {
                //Listen to the input
                if (Input.GetMouseButtonDown(0))
                {
                    currentPlayingIdx ++;
                    if (currentPlayingIdx > EventList.Count-1)
                    {
                        //Turn Page
                        //If this is the last page, exit playmode
                        if (GameManager.Instance.pageIdx >= GameManager.Instance.Pages.Count - 1)
                        {
                            GameManager.Instance.ExitPlayMode();
                            return;
                        }
                        //If not, turn to next page
                        else
                        {
                            GameManager.Instance.pageContent.transform.GetChild(GameManager.Instance.pageIdx + 1).GetComponent<OnPageSelect>().SelectPage();
                            currentPlayingIdx = -1;
                            return;
                        }
                    }
                    currentPlayingEvent =EventList[currentPlayingIdx];
                    var data = currentPlayingEvent.data;
                    //Audio
                    if (data.hasAudio&&data.thisAudio!=null)
                    {
                        this.GetComponent<AudioSource>().PlayOneShot(data.thisAudio);
                    }
                    //PresetAnimation
                    if (data.hasAnimation && data.hasPresetAnimation)
                    {
                        currentPlayingEvent.Object.AddComponent<AnimationOutput>();
                        currentPlayingEvent.Object.GetComponent<AnimationOutput>().Prepare();
                        if (data.thisAnimation == Assets.DataManager.AnimationType.In)
                        {
                            currentPlayingEvent.Object.GetComponent<AnimationOutput>()._FlyInEvent();
                        }
                        if (data.thisAnimation == Assets.DataManager.AnimationType.Out)
                        {
                            currentPlayingEvent.Object.GetComponent<AnimationOutput>()._FlyOutEvent();
                        }
                    }
                    //CostumAnimation
                    if (data.hasAnimation && data.hasCustomAnimation)
                    {
                        currentPlayingEvent.Object.AddComponent<AnimationInstance>();
                        currentPlayingEvent.Object.GetComponent<AnimationInstance>().posArray = new Transform[data.thisCostumAnimationStops.Count];
                        for (int i = 0; i < data.thisCostumAnimationStops.Count; i++)
                        {
                            currentPlayingEvent.Object.GetComponent<AnimationInstance>().posArray[i] = data.thisCostumAnimationStops[i];
                        }
                        currentPlayingEvent.Object.GetComponent<AnimationInstance>().speed = data.CostumAniSpeed;
                        currentPlayingEvent.Object.GetComponent<AnimationInstance>().Start();
                        currentPlayingEvent.Object.GetComponent<AnimationInstance>().startAni();
                    }
                }
            }
        }
    }
}


