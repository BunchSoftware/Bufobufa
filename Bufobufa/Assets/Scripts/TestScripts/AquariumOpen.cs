using Cinemachine;
using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AquariumOpen : MonoBehaviour
{
    public bool InTrigger = false;
    private bool AquariumIsOpen = false;
    private bool AquariumAnim = false;
    private bool ClickedMouse = false;

    private GameObject Player;
    private GameObject Vcam;
    private GameObject AquariumSprite;
    private GameObject Temperature;

    [Header("���������� ���� ������ ���� ������ ��� �������� �����(�����, ������ � ��� ��������)")]
    public Vector3 CoordPlayer = new();
    public Quaternion RotatePlayer = new();
    public float TimeAnimationPlayer = 1f;

    public Vector3 CoordVcam = new();
    public Quaternion RotateVcam = new();
    public float TimeAnimationVcam = 1f;

    public Vector3 CoordAquarium = new();
    public Quaternion RotateAquarium = new();
    public float TimeAnimationAquarium = 1f;

    private Vector3 currentPos = new();


    private void Start()
    {
        Vcam = GameObject.FindGameObjectWithTag("Vcam");
        Player = GameObject.FindGameObjectWithTag("Player");
        AquariumSprite = transform.Find("AquariumSprite").gameObject;
        Temperature = AquariumSprite.transform.Find("Termometr").gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            InTrigger = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            InTrigger = false;
        }
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var infoHit))
            {
                if (infoHit.collider.gameObject == gameObject)
                {
                    ClickedMouse = true;
                }
                else
                {
                    ClickedMouse = false;
                }
            }
        }


        if (!AquariumAnim && InTrigger && !AquariumIsOpen && ClickedMouse)
        {
            AquariumIsOpen = true;
            AquariumAnim = true;
            ClickedMouse = false;
            Vcam.GetComponent<CinemachineVirtualCamera>().Follow = null;
            Vcam.GetComponent<MoveAnimation>().startCoords = CoordVcam;
            Vcam.GetComponent<MoveAnimation>().needPosition = true;
            Vcam.GetComponent<MoveAnimation>().startRotate = RotateVcam;
            Vcam.GetComponent<MoveAnimation>().needRotate = true;
            Vcam.GetComponent<MoveAnimation>().TimeAnimation = TimeAnimationVcam;
            Vcam.GetComponent<MoveAnimation>().StartMove();


            currentPos = Player.transform.position;
            Player.GetComponent<PlayerMouseMove>().MovePlayer(CoordPlayer);
            Player.GetComponent<PlayerMouseMove>().StopPlayerMove();

            AquariumSprite.GetComponent<MoveAnimation>().startCoords = CoordAquarium;
            AquariumSprite.GetComponent<MoveAnimation>().needPosition = true;
            AquariumSprite.GetComponent<MoveAnimation>().startRotate = RotateAquarium;
            AquariumSprite.GetComponent<MoveAnimation>().needRotate = true;
            AquariumSprite.GetComponent<MoveAnimation>().TimeAnimation = TimeAnimationAquarium;
            AquariumSprite.GetComponent<MoveAnimation>().StartMove();


            StartCoroutine(WaitAnimAquarium(Vcam.GetComponent<MoveAnimation>().TimeAnimation));
            AquariumSprite.GetComponent<Aquarium>().AquariumOpen = true;
            Temperature.GetComponent<Temperature>().AquariumOpen = true;
            GetComponent<BoxCollider>().enabled = false;
        }
        else if (!AquariumAnim && AquariumIsOpen && Input.GetMouseButtonDown(1))
        {
            AquariumIsOpen = false;
            AquariumAnim = true;
            Vcam.GetComponent<MoveAnimation>().EndMove();
            StartCoroutine(WaitAnimAquarium(Vcam.GetComponent<MoveAnimation>().TimeAnimation));
            StartCoroutine(WaitAnimCamera(Vcam.GetComponent<MoveAnimation>().TimeAnimation));

            Player.GetComponent<PlayerMouseMove>().MovePlayer(currentPos);
            Player.GetComponent<PlayerMouseMove>().ReturnPlayerMove();

            AquariumSprite.GetComponent<MoveAnimation>().EndMove();

            
            AquariumSprite.GetComponent<Aquarium>().AquariumOpen = false;
            Temperature.GetComponent<Temperature>().AquariumOpen = false;
            GetComponent<BoxCollider>().enabled = true;
        }
    }
    IEnumerator WaitAnimAquarium(float f)
    {
        yield return new WaitForSeconds(f);
        AquariumAnim = false;
    }
    IEnumerator WaitAnimCamera(float f)
    {
        yield return new WaitForSeconds(f);
        Vcam.GetComponent<CinemachineVirtualCamera>().Follow = Player.transform;
    }
}
