using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class onStopPoint : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    private RectTransform parentRectTransform;
    private Vector2 originalLocalPointerPosition;
    private Vector3 originalLocalPosition;


    public void OnPointerDown(PointerEventData data)
    {
        parentRectTransform = transform.parent as RectTransform;
        originalLocalPosition = this.transform.localPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, data.position, data.pressEventCamera, out originalLocalPointerPosition);
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
    }
}
