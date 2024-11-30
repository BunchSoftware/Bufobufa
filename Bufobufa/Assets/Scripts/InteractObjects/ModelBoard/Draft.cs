using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draft : MonoBehaviour
{
    private bool InText = false;
    private bool WhileAnimGo = false;
    private float OrigXSizeColliser;

    private void OnMouseDown()
    {
        if (!WhileAnimGo && transform.parent.GetComponent<OpenObject>().ObjectIsOpen && !transform.parent.GetComponent<OrgansBoardOpen>().ModelOpen)
        {
            transform.parent.GetComponent<OrgansBoardOpen>().ModelOpen = true;
            transform.parent.GetComponent<OpenObject>().MoreBoolQuit = false;

            OrigXSizeColliser = GetComponent<BoxCollider>().size.x;
            GetComponent<BoxCollider>().size = new Vector3(44.73f, GetComponent<BoxCollider>().size.y, GetComponent<BoxCollider>().size.z);
            GetComponent<MoveAnimation>().StartMove();
            InText = true;
            WhileAnimGo = true;
            StartCoroutine(WaitAnimGo(GetComponent<MoveAnimation>().TimeAnimation));
        }
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && InText && !WhileAnimGo && transform.parent.GetComponent<OpenObject>().ObjectIsOpen)
        {
            transform.parent.GetComponent<OrgansBoardOpen>().ModelOpen = false;
            transform.parent.GetComponent<OpenObject>().MoreBoolQuit = false;
            GetComponent<BoxCollider>().size = new Vector3(OrigXSizeColliser, GetComponent<BoxCollider>().size.y, GetComponent<BoxCollider>().size.z);
            GetComponent<MoveAnimation>().EndMove();
            InText = false;
            WhileAnimGo = true;
            StartCoroutine(WaitAnimGo(GetComponent<MoveAnimation>().TimeAnimation));
        }
    }
    IEnumerator WaitAnimGo(float t)
    {
        yield return new WaitForSeconds(t);
        WhileAnimGo = false;
    }
}
