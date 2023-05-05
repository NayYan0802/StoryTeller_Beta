using Assets.EventsManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayManager : MonoBehaviour
{
    public enum PlayMode {Preview, Play };
    public PlayMode playmode;
    public List<EventOnOnePage> allPlayingEvents;
    
    //If we are playing one event, we cannot load next event
    public bool isPlaying;

    //Global parameter
    public bool GotCurrentInput;
    public inputs currentInputs=new inputs();

    public int currentPlayingIdx;
    public EventOnOnePage.thisEvent currentPlayingEvent;


    public struct inputs
    {
        public bool hasMouseClick;
        public Assets.DataManager.MouseClick mouseClick;

        public bool hasSensor;
        public Assets.DataManager.Senser sensor;
    }



    private void Update()
    {
        if (GameManager.Instance.isPlaymode)
        {
            //Run the events

            PlayModeUpdate();
            return;
        }
    }

    public void currentInputMethodUpdate()
    {
        currentInputs.hasMouseClick = currentPlayingEvent.data.hasMouseClick;
        currentInputs.hasSensor = currentPlayingEvent.data.hasSensor;
        currentInputs.mouseClick = currentPlayingEvent.data.thisMouseClick;
        currentInputs.sensor = currentPlayingEvent.data.thisSenser;
    }

    public void PlayModeUpdate()
    {
        List<EventOnOnePage.thisEvent> EventList = allPlayingEvents[GameManager.Instance.pageIdx].EventsOnThisPage;

        if (!isPlaying)
        {
            //Listen to the input
            //if (Input.GetMouseButtonDown(0))
            if (GotCurrentInput|| Input.GetMouseButtonDown(1))
            {
                //Reset
                GotCurrentInput = false;
                //Start Play the event
                currentPlayingIdx++;
                if (currentPlayingIdx > EventList.Count - 1)
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
                currentPlayingEvent = EventList[currentPlayingIdx];
                var data = currentPlayingEvent.data;
                //Audio
                if (data.hasAudio && data.thisAudio != null)
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

    public void getCurrentPlayList()
    {
        allPlayingEvents = GameManager.Instance.EventsManager.allEvents;
    }
}
