using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageManager : MonoBehaviour
{
    public GameObject newPage;
    public GameObject AddButton;
    public GameObject newEditWindow;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddPage()
    {
        GameObject newOne = Instantiate(newPage, GameObject.Find("PageContent").transform);
        GameObject newWindow = Instantiate(newEditWindow, GameObject.Find("Editer").transform);
        newOne.transform.localScale = Vector3.one;
        AddButton.transform.SetAsLastSibling();
        GameManager.Instance.Pages.Add(newWindow);
        newWindow.SetActive(false);
        GameManager.Instance.EventsManager.AddPage();
    }

    public void DeletePage()
    {
        int deleteIdx= GameManager.Instance.pageIdx;
        bool isLast;
        if (GameManager.Instance.Pages.Count <= 1)
        {
            AddPage();
        }
        if (GameManager.Instance.pageIdx == GameManager.Instance.Pages.Count - 1)
        {
            GameManager.Instance.pageIdx--;
            isLast = true;
        }
        else
        {
            isLast = false;
        }
        for (int i = 0; i < GameManager.Instance.Pages.Count; i++)
        {
            GameManager.Instance.Pages[i].SetActive(false);
            GameObject.Find("PageContent").transform.GetChild(i).GetChild(0).GetComponent<RawImage>().enabled = false;
        }

        GameManager.Instance.Pages.Remove(GameManager.Instance.currentPage);
        Destroy(GameManager.Instance.currentPage);
        GameManager.Instance.Pages[GameManager.Instance.pageIdx].SetActive(true);
        if (isLast)
        {
            this.transform.GetChild(GameManager.Instance.pageIdx).GetChild(0).GetComponent<RawImage>().enabled = true;
        }
        else
        {
            this.transform.GetChild(GameManager.Instance.pageIdx+1).GetChild(0).GetComponent<RawImage>().enabled = true;
        }
        Debug.Log(this.transform.GetChild(GameManager.Instance.pageIdx).gameObject.name);
        Destroy(GameObject.Find("PageContent").transform.GetChild(deleteIdx).gameObject);
        GameManager.Instance.currentPage = GameManager.Instance.Pages[GameManager.Instance.pageIdx];
        GameManager.Instance.EventsManager.DeletePage();
    }

}
