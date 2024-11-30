using Cinemachine;
using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableOpen : MonoBehaviour
{
    private GameObject Player;
    private GameObject MixTable;


    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        MixTable = transform.Find("MixTable").gameObject;
    }
    private void Update()
    {
        if (GetComponent<OpenObject>().ObjectIsOpen && !MixTable.GetComponent<ThingsInTableMix>().MixTableOn)
        {
            MixTable.GetComponent<ThingsInTableMix>().MixTableOn = true;
        }
        else if (!GetComponent<OpenObject>().ObjectAnim && GetComponent<OpenObject>().ObjectIsOpen && Input.GetMouseButtonDown(1))
        {
            if (MixTable.GetComponent<ThingsInTableMix>().currentPrinterObject != null)
            {
                Player.GetComponent<PlayerInfo>().currentPickObject = MixTable.GetComponent<ThingsInTableMix>().currentPrinterObject;
                MixTable.GetComponent<ThingsInTableMix>().currentPrinterObject = null;
                Player.GetComponent<PlayerInfo>().PlayerPickSometing = true;
            }
        }
    }
}
