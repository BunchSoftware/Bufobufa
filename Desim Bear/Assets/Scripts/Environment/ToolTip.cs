using Game.Environment.LMixTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTip : MonoBehaviour
{
    [SerializeField] private IngradientSpawner ingradientSpawner; // ������ �� ������ IngradientSpawner
    [SerializeField] private string toolTipText; // ����� ���������

    private void OnMouseEnter()
    {
        // �������� ���������� ������������ �� IngradientSpawner
        int count = ingradientSpawner.GetIngradient().countIngradient;

        // ��������� ����� ��������� � ����������� ������������
        string fullToolTipText = $"{toolTipText}: {count}";

        // ���������� ���������
        ToolTipManager._instance.ToolTipOn(fullToolTipText);
    }

    private void OnMouseExit()
    {
        // �������� ���������
        ToolTipManager._instance.ToolTipOff();
    }
}
