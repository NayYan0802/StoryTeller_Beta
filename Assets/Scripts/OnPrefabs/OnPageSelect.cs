using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnPageSelect : MonoBehaviour
{
    public void SelectPage()
    {
        GameManager.Instance.UpdatePagePreview();
        GameManager.Instance.pageIdx = this.transform.GetSiblingIndex();
        for(int i=0;i< GameManager.Instance.Pages.Count; i++)
        {
            GameManager.Instance.Pages[i].SetActive(false);
            GameObject.Find("PageContent").transform.GetChild(i).GetChild(0).GetComponent<RawImage>().enabled = false;
        }
        GameManager.Instance.Pages[GameManager.Instance.pageIdx].SetActive(true);
        this.transform.GetChild(0).GetComponent<RawImage>().enabled = true;
        GameManager.Instance.currentPage = GameManager.Instance.Pages[GameManager.Instance.pageIdx];
        GameManager.Instance.EventsManager.UpdateEventList();
        GameManager.Instance.EventsManager.currentEventIdx = 0;
    }
}
