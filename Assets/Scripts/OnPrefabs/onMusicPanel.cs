using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class onMusicPanel : MonoBehaviour,IDropHandler
{
	public void OnDrop(PointerEventData data)
	{
		var originalObj = data.pointerDrag;
		this.GetComponent<RawImage>().texture = originalObj.GetComponent<RawImage>().texture;
		if (this.transform.childCount > 1)
		{
			Destroy(this.transform.GetChild(2).gameObject);
			Destroy(this.transform.GetChild(1).gameObject);
		}
		this.transform.GetChild(0).GetComponent<TMP_Text>().text = originalObj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text;
        if (GameManager.Instance.currentObjectValidationCheck())
        {
			//GameManager.Instance.currentObjectInWindow.data.thisAudio = originalObj.GetComponent<AudioSource>().clip;
			GameManager.Instance.EventsManager.data.thisAudio = originalObj.GetComponent<AudioSource>().clip;
        }
	}
}
