using Assets.EventsManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class onEventButton : MonoBehaviour
{
    public void OnSelect()
    {
        GameManager.Instance.hasSelectEvent = true;
        GameManager.Instance.currentEventIdx =int.Parse(this.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text)-1;
        GameManager.Instance.EventsManager.currentEventIdx = GameManager.Instance.currentEventIdx;

        //Run at update instead
        //GameManager.Instance.LoadCurrentObjectData();

        List<EventOnOnePage.thisEvent> EventList = GameManager.Instance.EventsManager.allEvents[GameManager.Instance.pageIdx].EventsOnThisPage;
        for (int i = 0; i < EventList.Count; i++)
        {
            EventList[i].UI.transform.GetChild(3).GetChild(0).GetComponent<RawImage>().enabled = false;
        }
        //EventList[GameManager.Instance.currentEventIdx].UI.transform.GetChild(3).GetChild(0).GetComponent<RawImage>().enabled = true;
        GameManager.Instance.LoadCurrentObjectData();
        GameManager.Instance.EventsManager.UpdateEventList();
    }
}
