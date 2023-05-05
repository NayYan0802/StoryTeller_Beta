using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadEditor : MonoBehaviour
{
    //private void Start()
    //{
    //    DontDestroyOnLoad(this);
    //}
    public void LoadingEditor()
    {
        SceneManager.LoadScene("Editor");
    }

    public void LoadMain()
    {
        SceneManager.LoadScene("StartPage");
    }

    //public void LoadPlayer()
    //{
    //    SceneManager.LoadScene(1);
    //    Invoke("Later", 0.5f);
    //}

    //public void Later()
    //{
    //    SceneManager.LoadScene(0);
    //}
}
