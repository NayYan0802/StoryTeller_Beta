using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Assets.DataManager;
using TMPro;

public class onAssetInWindow : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    public AssetData data;
    public bool isPlayMode = false;

    private Vector2 originalLocalPointerPosition;
    private Vector3 originalLocalPosition;
    private RectTransform parentRectTransform;

    public Vector2 originPos;

    private void Awake()
    {
        data.hasAnimation = false;
        data.hasAudio = false;
        data.hasServo = false;
    }

    public void OnPointerDown(PointerEventData data)
    {
        parentRectTransform = transform.parent as RectTransform;
        originalLocalPosition = this.transform.localPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, data.position, data.pressEventCamera, out originalLocalPointerPosition);
        if (this.GetComponent<TextMeshProUGUI>())
        {
            GameManager.Instance.currentObjectInWindow = this;
            GameManager.Instance.currentTextInWindow = this.GetComponent<TextMeshProUGUI>();
            GameManager.Instance.inputField.text = this.GetComponent<TextMeshProUGUI>().text;
            GameManager.Instance.PopUpInputField();
        }
        else
        {
            OpenSetting();
        }
        GameManager.Instance.currentObjectUpdate();
    }

    public void OnDrag(PointerEventData data)
    {
        Vector2 localPointerPosition;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, data.position, data.pressEventCamera, out localPointerPosition))
        {
            GameObject window = GameManager.Instance.currentPage;
            RectTransform windowBounds = window.GetComponent<RectTransform>();
            if (!RectTransformUtility.RectangleContainsScreenPoint(windowBounds, GameObject.Find("MousePos").transform.position))
            {
                return;
            }
            Vector3 offsetToOriginal = localPointerPosition - originalLocalPointerPosition;
            transform.localPosition = originalLocalPosition + offsetToOriginal;
        }
        GameManager.Instance.UpdatePagePreview();
    }

    //Open up the setting window
    public void OpenSetting()
    {
        //Instead of setactive() the object, set the scale from 0 to 1 to achieve the same effect,
        //this way we can use GameObject.Find() cause it's active.
        GameObject.Find("InteractionSetting").transform.localScale = Vector3.one;
        GameManager.Instance.currentObjectInWindow = this;
    }

    public void changeMouseInput(string input)
    {
        switch (input)
        {
            case "Left":
                data.thisMouseClick = MouseClick.Left;
                break;
            case "Right":
                data.thisMouseClick = MouseClick.Right;
                break;
            case "Double":
                data.thisMouseClick = MouseClick.Double;
                break;
            case "None":
                data.thisMouseClick = MouseClick.None;
                break;
            default:
                Debug.LogError("Error MouseClick Type");
                break;
        }
    }

    public void changeSensorInput(string input)
    {
        switch (input)
        {
            case "Distance":
                data.thisSenser = Senser.Distance;
                break;
            case "Sound":
                data.thisSenser = Senser.Sound;
                break;
            case "RFID":
                data.thisSenser = Senser.RFID;
                break;
            case "Motion":
                data.thisSenser = Senser.Motion;
                break;
            case "ArcadeButtonA":
                data.thisSenser = Senser.ArcadeButtonA;
                break;
            case "ArcadeButtonB":
                data.thisSenser = Senser.ArcadeButtonB;
                break;
            default:
                Debug.LogError("Error Sensor Type");
                break;
        }
    }

    public void changeServo(string input)
    {
        switch (input)
        {
            case "Rotate":
                data.thisServo = Assets.DataManager.Servo.Rotate;
                break;
            default:
                Debug.LogError("Error Servo Type");
                break;
        }
    }

    public void changeAnimationOutput(string input)
    {
        switch (input)
        {
            case "In":
                data.thisAnimation = Assets.DataManager.AnimationType.In;
                break;
            case "Out":
                data.thisAnimation = Assets.DataManager.AnimationType.Out;
                break;
            case "Scale":
                data.thisAnimation = Assets.DataManager.AnimationType.Scale;
                break;
            case "Rotate":
                data.thisAnimation = Assets.DataManager.AnimationType.Rotate;
                break;
            default:
                Debug.LogError("Error Animation Type");
                break;
        }
    }


    public void setOriginPos()
    {
        originPos = this.GetComponent<RectTransform>().position;
    }
}
