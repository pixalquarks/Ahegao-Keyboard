
using System;
using UnityEngine;

public class ButtonFunction : MonoBehaviour
{
    private void OnMouseDown()
    {
        SaveLoadAudio.OpenFileExplorer();
    }

    // public void OnBtnPress()
    // {
    //     Debug.Log("Opening File explorer");
    //     SaveLoadAudio.OpenFileExplorer();
    // }

    public void CreateNewSchema()
    {
        SaveLoadAudio.BuildingNewSchema = true;
    }

    public void SaveSchema()
    {
        SaveLoadAudio.BuildingNewSchema = false;
    }
}
