using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System;
using Assets.EventsManager;

[System.Serializable]
public class SavedData
{
    [System.Serializable]
    public struct imageData
    {
        public Vector2 RelativePos;
        public Vector3 Rotation;
        public Vector2 Scale;
        public Vector2 size;
        public int ImageFileIdx;
        public byte[] imageBytes;
    }
    [System.Serializable]
    public struct thisEvent
    {
        public int EventIdx;
        //Index of the target object so that we can reload it
        public int ObjectIdx;


        public bool hasAnimation;
        public bool hasAudio;

        public AudioClip thisAudio;
        public bool isFlyIn;
    }
    public List<imageData> ImageOnthisPage = new List<imageData>();
    public List<thisEvent> EventsOnThisPage = new List<thisEvent>();
    public byte[] bgBytes;
}

[System.Serializable]
public class AllData
{
    public List<SavedData> ListOfAllData= new List<SavedData>();
}


public class DataSaver : MonoBehaviour
{
    public AllData data;
    public bool hasLoad;

    [Header("Play")]
    public int currentPageIdx;
    public int currentPlayingIdx;
    public bool isPlaying;
    SavedData.thisEvent currentPlayingEvent;
    public List<GameObject> PageList = new List<GameObject>();
    public Transform PlayingParent;

    public List<GameObject>[] ObjectList = new List<GameObject>[100];

    public Text text;

    [Header("Prefab")]
    public GameObject Page;
    public GameObject ObjectInScene;

    void Start()
    {
        hasLoad = false;
        isPlaying = false;
    }

    private void Update()
    {
        if (isPlaying)
        {
            PlayUpdate();
        }
    }

    public void StartPlay()
    {
        isPlaying = true;
        currentPlayingIdx = -1;
        currentPageIdx = 0;
    }

    public void PlayUpdate()
    {
        List<SavedData.thisEvent> EventList =data.ListOfAllData[currentPageIdx].EventsOnThisPage;

        if (isPlaying)
        {
            //Listen to the input
            if (Input.GetMouseButtonDown(0))
            {
                currentPlayingIdx++;
                if (currentPlayingIdx > EventList.Count - 1)
                {
                    //Turn Page
                    //If this is the last page, exit playmode
                    if (currentPageIdx >= PageList.Count - 1)
                    {
                        ExitPlayMode();
                        return;
                    }
                    //If not, turn to next page
                    else
                    {
                        //GameManager.Instance.pageContent.transform.GetChild(GameManager.Instance.pageIdx + 1).GetComponent<OnPageSelect>().SelectPage();

                        PageList[currentPageIdx + 1].SetActive(true);
                        PageList[currentPageIdx].SetActive(false);
                        currentPageIdx++;

                        currentPlayingIdx = -1;
                        return;
                    }
                }
                currentPlayingEvent = EventList[currentPlayingIdx];
                var EventD = currentPlayingEvent;
                //Audio
                Debug.Log(EventD.thisAudio.name);
                //text.text = EventD.thisAudio.name;
                if (EventD.hasAudio && EventD.thisAudio != null)
                {
                    this.GetComponent<AudioSource>().PlayOneShot(EventD.thisAudio);
                }
                //PresetAnimation
                if (EventD.hasAnimation)
                {
                    //currentPlayingEvent.Object.AddComponent<AnimationOutput>();
                    //currentPlayingEvent.Object.GetComponent<AnimationOutput>().Prepare();
                    if (EventD.isFlyIn)
                    {
                        //currentPlayingEvent.Object.GetComponent<AnimationOutput>()._FlyInEvent();
                        PageList[currentPageIdx].transform.GetChild(EventD.ObjectIdx).GetComponent<AnimationOutput>()._FlyInEvent();
                    }
                    else
                    {
                        //currentPlayingEvent.Object.GetComponent<AnimationOutput>()._FlyOutEvent();
                        PageList[currentPageIdx].transform.GetChild(EventD.ObjectIdx).GetComponent<AnimationOutput>()._FlyOutEvent();
                    }
                }
            }
        }
    }

    private void ExitPlayMode()
    {
        PageList[currentPageIdx].SetActive(false);
        for (int i = PlayingParent.childCount-1; i >= 0; i--)
        {
            Destroy(PlayingParent.GetChild(i));
        }
        PlayingParent.gameObject.SetActive(false);
        isPlaying = false;
    }

    public void ReadData()
    {
        string json;
        //string filePath = Application.streamingAssetsPath + "/playerData.json";
        string filePath = OpenFileLog.ReadData();

        using (StreamReader sr = new StreamReader(filePath))
        {
            json = sr.ReadToEnd();
            sr.Close();
        }
        data = JsonUtility.FromJson<AllData>(json);
        hasLoad = true;
        LoadData();
        //if (data.levelSum > 0)
        //{
        //    StartCoroutine(LoadData());
        //}
    }

    public void LoadData()
    {
        for (int i = 0; i < data.ListOfAllData.Count; i++)
        {
            ObjectList[i] = new List<GameObject>();
        }
        PageList = new List<GameObject>();
        for (int i = 0; i < data.ListOfAllData.Count; i++)
        {
            GameObject newPage = Instantiate(Page, PlayingParent);
            Texture2D textureBG = new Texture2D(968, 541);
            textureBG.LoadImage(data.ListOfAllData[i].bgBytes);
            newPage.GetComponent<RawImage>().texture = textureBG;
            List<SavedData.imageData> imagesOnThisPage = data.ListOfAllData[i].ImageOnthisPage;
            for (int j = 0; j < imagesOnThisPage.Count; j++)
            {
                GameObject newObject = Instantiate(ObjectInScene, newPage.transform);
                RectTransform rect = newObject.transform as RectTransform;
                rect.localPosition = imagesOnThisPage[j].RelativePos;
                rect.eulerAngles = imagesOnThisPage[j].Rotation;
                rect.localScale = imagesOnThisPage[j].Scale;
                rect.sizeDelta = imagesOnThisPage[j].size;

                Texture2D textureObject = new Texture2D(100, 100);
                textureObject.LoadImage(imagesOnThisPage[j].imageBytes);
                rect.GetComponent<RawImage>().texture = textureObject;
                ObjectList[i].Add(newObject);
                for (int k = 0; k < data.ListOfAllData[i].EventsOnThisPage.Count; k++)
                {
                    SavedData.thisEvent thisEvent = data.ListOfAllData[i].EventsOnThisPage[k];
                    if (thisEvent.hasAnimation&& thisEvent.isFlyIn && thisEvent.ObjectIdx == j)
                    {
                        Debug.Log("need");
                        newObject.GetComponent<AnimationOutput>().InitPos();
                        newObject.GetComponent<AnimationOutput>().needInit=true;
                    }
                }
                
            }
            PageList.Add(newPage);
            newPage.SetActive(false);
        }
        PageList[0].SetActive(true);
        StartPlay();
    }

    //IEnumerator LoadData()
    //{
    //    yield return null;
    //}

    public void SaveData()
    {
        //data = TransferDataFromEditorToSaver(GameManager.Instance.EventsManager.allEvents);
        data = TransferDataFromEditorToSaver(GameManager.Instance.layerList, GameManager.Instance.EventsManager.allEvents, Application.streamingAssetsPath);
        string json = JsonUtility.ToJson(data, true);
        string filePath = Application.streamingAssetsPath + "/playerData.json";

        using (StreamWriter sw = new StreamWriter(filePath))
        {
            sw.WriteLine(json);
            sw.Close();
            sw.Dispose();
        }
    }

    public string GetSavedData(string path)
    {
        data = TransferDataFromEditorToSaver(GameManager.Instance.layerList, GameManager.Instance.EventsManager.allEvents, path);
        string json = JsonUtility.ToJson(data, true);
        return json;
    }

    public AllData TransferDataFromEditorToSaver(List<GameObject>[] layerList, List<EventOnOnePage> allEvents,string path)
    {
        int imageIdx = 0;
        //All the data
        AllData allData=new AllData();

        //ObjectData
        for (int i = 0; i <GameManager.Instance.Pages.Count ; i++)
        {
            //Object on on Page
            SavedData dataOnPage = new SavedData();
            for (int j = 0; j < layerList[i].Count; j++)
            {
                //One Specific Object
                SavedData.imageData newImage = new SavedData.imageData();
                //The origin data of the gameObject
                GameObject sourceObject = layerList[i][j];
                newImage.RelativePos = sourceObject.transform.localPosition;
                newImage.Rotation = sourceObject.transform.eulerAngles;
                newImage.Scale= sourceObject.transform.localScale;
                newImage.size = (sourceObject.transform as RectTransform).sizeDelta;

                //Will save every texture in certain order in assigned File
                Texture2D textureOnThisObject = (Texture2D)sourceObject.GetComponent<RawImage>().texture;
                byte[] bytes = textureOnThisObject.EncodeToPNG();
                newImage.imageBytes = bytes;
                //string filename = Application.streamingAssetsPath + "/" + imageIdx.ToString() + ".png";
                string filename = CutTail(path) + "/" + imageIdx.ToString() + ".png";
                //System.IO.File.WriteAllBytes(filename, bytes);

                newImage.ImageFileIdx = imageIdx;
                imageIdx++;
                dataOnPage.ImageOnthisPage.Add(newImage);

            }
            Texture2D textureOnBG = (Texture2D)GameManager.Instance.Pages[i].GetComponent<RawImage>().texture;
            byte[] BytesBG=textureOnBG.EncodeToPNG();
            dataOnPage.bgBytes = BytesBG;
            allData.ListOfAllData.Add(dataOnPage);
        }



        //EventsData
        for (int i = 0; i < allEvents.Count; i++)
        {
            //Data on one page
            SavedData dataOnPage = allData.ListOfAllData[i];
            for (int j = 0; j < allEvents[i].EventsOnThisPage.Count; j++)
            {
                //One specific event
                SavedData.thisEvent newEvent = new SavedData.thisEvent();
                //The origin data of one event
                EventOnOnePage.thisEvent sourceEvent = allEvents[i].EventsOnThisPage[j];
                newEvent.EventIdx = j;
                //newEvent.RelativePos = sourceEvent.Object.transform.localPosition;
                //newEvent.Rotation = sourceEvent.Object.transform.eulerAngles;
                //newEvent.Scale = sourceEvent.Object.GetComponent<RectTransform>().sizeDelta;
                //Will save every texture in certain order in assigned File
                //Texture2D textureOnThisObject = (Texture2D)sourceEvent.Object.GetComponent<RawImage>().texture;
                //byte[] bytes = textureOnThisObject.EncodeToPNG();
                ////string filename = Application.streamingAssetsPath + "/" + imageIdx.ToString() + ".png";
                //string filename = CutTail(path) + "/" + imageIdx.ToString() + ".png";
                //System.IO.File.WriteAllBytes(filename, bytes);

                //newEvent.ImageFileIdx = imageIdx;
                //imageIdx++;

                newEvent.ObjectIdx = layerList[i].IndexOf(sourceEvent.Object);

                if (sourceEvent.data.hasAudio)
                {
                    newEvent.hasAudio = true;
                    newEvent.thisAudio = sourceEvent.data.thisAudio;
                }
                else
                {
                    newEvent.hasAudio = false;
                }
                if (sourceEvent.data.hasAnimation)
                {
                    newEvent.hasAnimation = true;
                    if (sourceEvent.data.thisAnimation == Assets.DataManager.AnimationType.In)
                    {
                        newEvent.isFlyIn = true;
                    }
                    else
                    {
                        newEvent.isFlyIn = false;
                    }
                }
                else
                {
                    newEvent.hasAnimation = false;
                }
                dataOnPage.EventsOnThisPage.Add(newEvent);
            }
            allData.ListOfAllData[i] = dataOnPage;
        }

        return allData;
    }

    private string CutTail(string input)
    {
        for(int j = input.Length - 1; j >= 0; j--)
        {
            char thisLetter = input[j];
            input = input.Remove(j);
            //Debug.Log(thisLetter);
            if (thisLetter == '\\')
            {
                return input;
            }
        }
        return input;
    }

    private void DeleteData()//DataDelete e)
    {
        string filePath = Application.streamingAssetsPath + "/playerData.json";
        File.Delete(filePath);
    }
}