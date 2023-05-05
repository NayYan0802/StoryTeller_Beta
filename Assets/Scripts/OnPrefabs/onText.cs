using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Assets.DataManager;
using TMPro;

public class onText : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    public bool isPlayMode = false;

    private Vector2 originalLocalPointerPosition;
    private Vector3 originalLocalPosition;
    private RectTransform parentRectTransform;

    public void OnPointerDown(PointerEventData data)
    {
        parentRectTransform = transform.parent as RectTransform;
        originalLocalPosition = this.transform.localPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, data.position, data.pressEventCamera, out originalLocalPointerPosition);
        GameManager.Instance.currentTextInWindow = this.GetComponent<TextMeshProUGUI>();
        GameManager.Instance.inputField.text = this.GetComponent<TextMeshProUGUI>().text;
    }

    public void OnDrag(PointerEventData data)
    {
        Vector2 localPointerPosition;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, data.position, data.pressEventCamera, out localPointerPosition))
        {
            Vector3 offsetToOriginal = localPointerPosition - originalLocalPointerPosition;
            transform.localPosition = originalLocalPosition + offsetToOriginal;
        }
    }
}
