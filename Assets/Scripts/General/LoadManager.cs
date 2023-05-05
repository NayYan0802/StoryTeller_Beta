using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class LoadManager : MonoBehaviour
{
    public Texture2D[] texs;
    public string[] paths;
    public RawImage[] img;

    void Start()
    {
        paths[0] = @"C:\Story Studio\resources\Book_JPG\Part 1\1.jpg";
    }

    void Update()
    {
    }

    //void OnGUI()
    //{
    //    if (GUI.Button(new Rect(0, 0, 100, 50), ""))
    //    {
    //        for (int i = 0; i < paths.Length; i++)
    //        {
    //            Texture2D tex = new Texture2D(1, 1);
    //            byte[] rawJPG = File.ReadAllBytes(paths[i]);
    //            tex.LoadImage(rawJPG);
    //            texs[i] = tex;
    //            img[i].texture = texs[i];
    //        }
    //        //Resources.UnloadUnusedAssets
    //    }
    //}

    public void LoadImage()
    {
        for (int i = 0; i < paths.Length; i++)
        {
            Texture2D tex = new Texture2D(1, 1);
            byte[] rawJPG = File.ReadAllBytes(paths[i]);
            tex.LoadImage(rawJPG);
            texs[i] = tex;
            img[i].texture = texs[i];
        }
    }
}