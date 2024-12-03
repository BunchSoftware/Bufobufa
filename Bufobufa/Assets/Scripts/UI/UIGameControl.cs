using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameControl : MonoBehaviour
{
    [SerializeField] private Fade fade;
    [SerializeField] private SaveManager saveManager;

    public void Start()
    {
        fade.FadeWhite();
    }

    public void ApllicationQuit()
    {
        Application.Quit();
    }

    public void LoadLevel(int buildIndex)
    {
        fade.currentIndexScene = buildIndex;
        fade.FadeBlack();
    }
}