using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class onDragPoint : MonoBehaviour, IPointerDownHandler, IDragHandler
{
	public Vector2 minSize = new Vector2(100, 100);
	public Vector2 maxSize = new Vector2(400, 400);

	private RectTransform panelRectTransform;
	private Vector2 originalLocalPointerPosition;
	private Vector2 originalSizeDelta;

	void Awake()
	{
		panelRectTransform = transform.parent.GetComponent<RectTransform>();
	}

	public void OnPointerDown(PointerEventData data)
	{
		originalSizeDelta = panelRectTransform.sizeDelta;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(panelRectTransform, data.position, data.pressEventCamera, out originalLocalPointerPosition);
	}

	public void OnDrag(PointerEventData data)
	{
		if (panelRectTransform == null)
			return;

		Vector2 localPointerPosition;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(panelRectTransform, data.position, data.pressEventCamera, out localPointerPosition);
		Vector3 offsetToOriginal = localPointerPosition - originalLocalPointerPosition;

		Vector2 sizeDelta = originalSizeDelta + new Vector2(offsetToOriginal.x, -offsetToOriginal.y);
		sizeDelta = new Vector2(
			Mathf.Clamp(sizeDelta.x, minSize.x, maxSize.x),
			Mathf.Clamp(sizeDelta.y, minSize.y, maxSize.y)
		);
		var lScale = panelRectTransform.localScale;

		if (sizeDelta.x < 0&& lScale.x == 1)
        {
			lScale.x = -1;
			sizeDelta.x = Mathf.Abs(sizeDelta.x);
        }
  //      else
  //      {
		//	lScale.x = 1;
		//}
		if(sizeDelta.x < 0&& lScale.x == -1)
        {
			lScale.x = 1;
			sizeDelta.x = Mathf.Abs(sizeDelta.x);
		}

		if (sizeDelta.y < 0&& lScale.y == 1)
        {
			lScale.y = -1;
			sizeDelta.y = Mathf.Abs(sizeDelta.y);
        }
		//      else
		//      {
		//	lScale.y = 1;
		//}

		if (sizeDelta.y < 0 && lScale.y == -1)
		{
			lScale.y = 1;
			sizeDelta.y = Mathf.Abs(sizeDelta.y);
		}

		panelRectTransform.sizeDelta = sizeDelta;
		panelRectTransform.localScale = lScale;
	}
}