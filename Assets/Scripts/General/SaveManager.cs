using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    public LoadManager loadManager;

    //Save images to local file
    public static void OutputRt(Texture2D rt, int idx = 0)
    {
        //RenderTexture.active = rt;

        //Texture2D png = new Texture2D(rt.width, rt.height, TextureFormat.ARGB32, false);
        //png.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        byte[] dataBytes = rt.EncodeToPNG();
        string strSaveFile = Application.dataPath + "/StreamingResources/texture/rt_" + System.DateTime.Now.Minute + "_" + System.DateTime.Now.Second + "_" + idx + ".jpg";
        FileStream fs = File.Open(strSaveFile, FileMode.OpenOrCreate);
        fs.Write(dataBytes, 0, dataBytes.Length);
        fs.Flush();
        fs.Close();
        RenderTexture.active = null;
    }

    public void GetImage()
    {
        OutputRt(loadManager.texs[0], 0);
    }
}
