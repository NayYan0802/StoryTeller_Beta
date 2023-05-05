using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Magic : MonoBehaviour
{
    GameObject[] magic;
    public GameObject black;
    // Start is called before the first frame update
    void Start()
    {
        magic = GameObject.FindGameObjectsWithTag("Magic");
        if (magic.Length==2)
        {
            Invoke("Later", 0.7f);
        }
        else if(magic.Length == 1)
        {
            Invoke("SLater", 0.3f);
        }
        else
        {
            if(black!=null)
            black.SetActive(false);
        }
        DontDestroyOnLoad(this);
    }

    public void Later()
    {
        SceneManager.LoadScene("StartPage");
    }

    public void SLater()
    {
        SceneManager.LoadScene("Editor");
    }
}
