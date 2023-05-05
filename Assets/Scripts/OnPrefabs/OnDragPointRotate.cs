using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class onDragPointRotate : MonoBehaviour, IPointerDownHandler, IDragHandler
{

	private RectTransform panelRectTransform;
	private Vector2 originalLocalPointerPosition;
	//private Vector2 originalSizeDelta;
	private Vector3 originalAngleDelta;
	private Vector2 Center;

	void Awake()
	{
		panelRectTransform = transform.parent.GetComponent<RectTransform>();
	}

	public void OnPointerDown(PointerEventData data)
	{
		//originalSizeDelta = panelRectTransform.sizeDelta;
		originalAngleDelta = panelRectTransform.eulerAngles;
		//RectTransformUtility.ScreenPointToLocalPointInRectangle(panelRectTransform, data.position, data.pressEventCamera, out originalLocalPointerPosition);
		originalLocalPointerPosition = data.position;
		//RectTransformUtility.ScreenPointToLocalPointInRectangle(panelRectTransform, data.position + Vector2.down * panelRectTransform.sizeDelta.y * 0.5f, data.pressEventCamera, out originalLocalPointerPosition);
		//Center = new Vector2(panelRectTransform.sizeDelta.x * 0.5f, 0f);
		Center = data.pressEventCamera.WorldToScreenPoint(panelRectTransform.position);
		Debug.Log(originalLocalPointerPosition);
	}

	public void OnDrag(PointerEventData data)
	{
		if (panelRectTransform == null)
			return;

		Vector2 localPointerPosition;
		//RectTransformUtility.ScreenPointToLocalPointInRectangle(panelRectTransform, data.position, data.pressEventCamera, out localPointerPosition);
		localPointerPosition = data.position;
		//Vector3 offsetToOriginal = localPointerPosition - originalLocalPointerPosition;
		Vector2 startVec = originalLocalPointerPosition - Center;
		Vector2 endVec = localPointerPosition - Center;
        
		float deltaAngle = Vector2.Angle(startVec, endVec);
		if (endVec.x > 0)
		{
			deltaAngle = 360-deltaAngle;
		}
		//Debug.Log(originalLocalPointerPosition + "        "+ localPointerPosition + "       "+ Center);
		//Debug.Log(startVec+"        "+ endVec + "       "+deltaAngle);

		//Vector2 sizeDelta = originalSizeDelta + new Vector2(offsetToOriginal.x, -offsetToOriginal.y);
		Vector3 newAngle = originalAngleDelta;

		newAngle.z += deltaAngle;


		panelRectTransform.eulerAngles = newAngle;
	}
}