using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

/*
 * Select File
 */
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
public class SelectFile
{
    public IntPtr hwndOwner = IntPtr.Zero;
    public IntPtr pidlRoot = IntPtr.Zero;
    public String pszDisplayName = null;
    public String lpszTitle = null;
    public UInt32 ulFlags = 0;
    public IntPtr lpfn = IntPtr.Zero;
    public IntPtr lParam = IntPtr.Zero;
    public int iImage = 0;
}

public class SelectFileLog
{

    [DllImport("shell32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern IntPtr SHBrowseForFolder([In, Out] SelectFile ofn);


    [DllImport("shell32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool SHGetPathFromIDList([In] IntPtr pidl, [In, Out] char[] fileName);

    public static string GetSelectFileName()
    {
        SelectFile selectFileName = new SelectFile();
        selectFileName.pszDisplayName = new string(new char[2000]);
        selectFileName.lpszTitle = "Select File";
        IntPtr intPtr = SHBrowseForFolder(selectFileName);
        char[] chArray = new char[2000];
        for (int i = 0; i < chArray.Length; i++)
        {
            chArray[i] = '\0';
        }
        SHGetPathFromIDList(intPtr, chArray);
        string fullPath = new string(chArray);
        fullPath = fullPath.Substring(0, fullPath.IndexOf('\0'));
        return fullPath;
    }
}

/*
 * Open chosen file/folder
 */
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
public class OpenFileName
{
    public int structSize = 0;
    public IntPtr dlgOwner = IntPtr.Zero;
    public IntPtr instance = IntPtr.Zero;
    public String filter = null;
    public String customFilter = null;
    public int maxCustFilter = 0;
    public int filterIndex = 0;
    public String file = null;
    public int maxFile = 0;
    public String fileTitle = null;
    public int maxFileTitle = 0;
    public String initialDir = null;
    public String title = null;
    public int flags = 0;
    public short fileOffset = 0;
    public short fileExtension = 0;
    public String defExt = null;
    public IntPtr custData = IntPtr.Zero;
    public IntPtr hook = IntPtr.Zero;
    public String templateName = null;
    public IntPtr reservedPtr = IntPtr.Zero;
    public int reservedInt = 0;
    public int flagsEx = 0;    
}


public class OpenFileLog:MonoBehaviour
{
    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool GetOpenFileName([In, Out] OpenFileName ofn);

    public static bool openFileName([In, Out] OpenFileName ofn)
    {
        return GetOpenFileName(ofn);
    }

    //Connecting system functions
    //"Save as" window
    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool GetSaveFileName([In, Out] OpenFileName ofn);
    public static bool SaveFileName([In, Out] OpenFileName ofn)
    {
        return GetSaveFileName(ofn);
    }



    public static void OpenFileName_(GameObject prefab)
    {
        OpenFileName openFile = new OpenFileName();
        openFile.structSize = Marshal.SizeOf(openFile);
        openFile.filter = "Image Files(*.jpg*.png)\0*.jpg;*.png";
        openFile.file = new string(new char[256]);
        openFile.maxFile = openFile.file.Length;
        openFile.fileTitle = new string(new char[64]);
        openFile.maxFileTitle = openFile.fileTitle.Length;
        //Set default file path
        string path = Application.streamingAssetsPath;
        path = path.Replace('/', '\\');
        openFile.initialDir = path;
        openFile.title = "Open File";
        openFile.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;

        if (GetOpenFileName(openFile))
        {            
            FileStream fileStream = new FileStream(openFile.file, FileMode.Open, FileAccess.Read);
            fileStream.Seek(0, SeekOrigin.Begin);
            //Create a file length buffer
            byte[] bytes = new byte[fileStream.Length];
            //Read File
            fileStream.Read(bytes, 0, (int)fileStream.Length);            
            fileStream.Close();
            fileStream.Dispose();
            fileStream = null;

            //Create Texture           
            Texture2D texture2D = new Texture2D(100,100);
            texture2D.LoadImage(bytes);

            //Create Prefab in Scene
            GameObject newPicInLib = Instantiate(prefab);
            newPicInLib.transform.SetParent(GameObject.Find("ImageContent").transform);
            newPicInLib.transform.localScale =Vector3.one;
            RawImage image = newPicInLib.GetComponent<RawImage>();


            image.gameObject.SetActive(true);
            if (image==null)
            {
                Debug.LogError("Fail Loading RawImages");
                return;
            }
            image.texture = texture2D;
            image.SetNativeSize();
        }
    }
    

    public static void SetBG()
    {
        OpenFileName openFile = new OpenFileName();
        openFile.structSize = Marshal.SizeOf(openFile);
        openFile.filter = "Image Files(*.jpg*.png)\0*.jpg;*.png";
        openFile.file = new string(new char[256]);
        openFile.maxFile = openFile.file.Length;
        openFile.fileTitle = new string(new char[64]);
        openFile.maxFileTitle = openFile.fileTitle.Length;
        //Set default file path
        string path = Application.streamingAssetsPath;
        path = path.Replace('/', '\\');
        openFile.initialDir = path;
        openFile.title = "Open File";
        openFile.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;

        if (GetOpenFileName(openFile))
        {            
            FileStream fileStream = new FileStream(openFile.file, FileMode.Open, FileAccess.Read);
            fileStream.Seek(0, SeekOrigin.Begin);
            //Create a file length buffer
            byte[] bytes = new byte[fileStream.Length];
            //Read File
            fileStream.Read(bytes, 0, (int)fileStream.Length);            
            fileStream.Close();
            fileStream.Dispose();
            fileStream = null;

            //Create Texture           
            Texture2D texture2D = new Texture2D(100,100);
            texture2D.LoadImage(bytes);

            GameManager.Instance.currentPage.GetComponent<RawImage>().texture = texture2D;


            ////Create Prefab in Scene
            //GameObject newPicInLib = Instantiate(prefab);
            //newPicInLib.transform.SetParent(GameObject.Find("ImageContent").transform);
            //newPicInLib.transform.localScale =Vector3.one;
            //RawImage image = newPicInLib.GetComponent<RawImage>();


            //image.gameObject.SetActive(true);
            //if (image==null)
            //{
            //    Debug.LogError("Fail Loading RawImages");
            //    return;
            //}
            //image.texture = texture2D;
            //image.SetNativeSize();
        }
    }
    
    public static string ReadData()
    {
        OpenFileName openFile = new OpenFileName();
        openFile.structSize = Marshal.SizeOf(openFile);
        openFile.filter = "Json Files(*.json)\0*.json";
        openFile.file = new string(new char[256]);
        openFile.maxFile = openFile.file.Length;
        openFile.fileTitle = new string(new char[64]);
        openFile.maxFileTitle = openFile.fileTitle.Length;
        //Set default file path
        string path = Application.streamingAssetsPath;
        path = path.Replace('/', '\\');
        openFile.initialDir = path;
        openFile.title = "Open File";
        openFile.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;

        if (GetOpenFileName(openFile))
        {
            return openFile.file;
            //FileStream fileStream = new FileStream(openFile.file, FileMode.Open, FileAccess.Read);
            //fileStream.Seek(0, SeekOrigin.Begin);
            ////Create a file length buffer
            //byte[] bytes = new byte[fileStream.Length];
            ////Read File
            //fileStream.Read(bytes, 0, (int)fileStream.Length);            
            //fileStream.Close();
            //fileStream.Dispose();
            //fileStream = null;

            ////Create Texture           
            //Texture2D texture2D = new Texture2D(100,100);
            //texture2D.LoadImage(bytes);

            //GameManager.Instance.currentPage.GetComponent<RawImage>().texture = texture2D;


            ////Create Prefab in Scene
            //GameObject newPicInLib = Instantiate(prefab);
            //newPicInLib.transform.SetParent(GameObject.Find("ImageContent").transform);
            //newPicInLib.transform.localScale =Vector3.one;
            //RawImage image = newPicInLib.GetComponent<RawImage>();


            //image.gameObject.SetActive(true);
            //if (image==null)
            //{
            //    Debug.LogError("Fail Loading RawImages");
            //    return;
            //}
            //image.texture = texture2D;
            //image.SetNativeSize();
        }
        return "";
    }


    public static void SaveFileName()
    {
        OpenFileName openFile = new OpenFileName();
        openFile.structSize = Marshal.SizeOf(openFile);
        openFile.filter = "Json File(*.json)\0*.json";
        openFile.file = new string(new char[256]);
        openFile.maxFile = openFile.file.Length;
        //openFile.fileTitle = new string(new char[64]);
        openFile.fileTitle = new string(new char[64]);
        openFile.maxFileTitle = openFile.fileTitle.Length;
        //Set defauly file path
        string path = Application.streamingAssetsPath;
        path = path.Replace('/', '\\');
        openFile.initialDir = path;
        openFile.title = "Save File";
        openFile.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;

        if (GetSaveFileName(openFile))
        {                      
            FileInfo file = new FileInfo(openFile.file);
            StreamWriter sw = file.CreateText();
            sw.Write(GameManager.Instance.DataSaver.GetSavedData(openFile.file));
            //Debug.Log(openFile.file + "       1");
            sw.Dispose();
            //Debug.Log(openFile.file + "       2");
            sw.Close();
            //Debug.Log(openFile.file + "       3");
        }
    }


}





