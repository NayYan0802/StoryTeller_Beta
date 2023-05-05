using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePosUpdate : MonoBehaviour
{
    Vector3 mousPos;
    public Camera mouseCam;

    void Update()
    {
        //Debug.Log(Input.mousePosition);
        mousPos = mouseCam.ScreenToWorldPoint(Input.mousePosition);
        mousPos.z = 0;
        this.transform.position = mousPos;

        //RaycastHit hitInfo;
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //if (Physics.Raycast(ray, out hitInfo, 100))
        //{
        //    if (hitInfo.transform!=null)
        //    {
        //        this.transform.position = hitInfo.point;
        //    }
        //}
    }
}
