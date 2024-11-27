using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draft : MonoBehaviour
{
    private bool InText = false;
    private bool WhileAnimGo = false;
    private float OrigXSizeColliser;
    [SerializeField] DialogManager Dialog; //�������
    public string CodeWord = ""; //�������
    [SerializeField] GameObject OffStrela; //�������
    [SerializeField] GameObject OnStrela; //�������
    private void Start()
    {
        Dialog = GameObject.Find("DialogManager").GetComponent<DialogManager>(); //�������
        OffStrela = GameObject.Find("Strelki").transform.Find("Strelka (2)").gameObject; //�������
        OnStrela = GameObject.Find("Strelki").transform.Find("Strelka (3)").gameObject; //�������

    }
    private void OnMouseDown()
    {
        if (!WhileAnimGo && !transform.parent.GetComponent<OrgansBoardOpen>().OpenModel)
        {
            transform.parent.GetComponent<OrgansBoardOpen>().OpenModel = true;

            Dialog.RunConditionSkip(CodeWord); //�������
            OffStrela.SetActive(false); //�������
            OnStrela.SetActive(true); //�������

            OrigXSizeColliser = GetComponent<BoxCollider>().size.x;
            GetComponent<BoxCollider>().size = new Vector3(1f, GetComponent<BoxCollider>().size.y, GetComponent<BoxCollider>().size.z);
            GetComponent<MoveAnimation>().StartMove();
            InText = true;
            WhileAnimGo = true;
            StartCoroutine(WaitAnimGo(GetComponent<MoveAnimation>().TimeAnimation));
        }
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && InText && !WhileAnimGo && transform.parent.GetComponent<OrgansBoardOpen>().OpenModel)
        {
            transform.parent.GetComponent<OrgansBoardOpen>().OpenModel = false;
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
