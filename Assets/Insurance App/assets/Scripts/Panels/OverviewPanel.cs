using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class OverviewPanel : MonoBehaviour, IPanel
{
    private static OverviewPanel _instance;

    public static OverviewPanel Instance
    {
        get { return _instance; }
    }


    public Text caseNumber, nameText, dateText, locationNotesText, photoNotesText;
    public RawImage rawImage;

    void Awake()
    {
        _instance = this;
    }

    public void SetOverviewPanel()
    {
        caseNumber.text = "CASE NUMBER " + UIManager.Instance.activeCase.caseID;
        nameText.text = UIManager.Instance.activeCase.name;
        dateText.text = "" + DateTime.Today;
        locationNotesText.text = "LOCATION NOTES:\n" + UIManager.Instance.activeCase.locationNotes;
        photoNotesText.text = "PHOTO NOTES:\n" + UIManager.Instance.activeCase.photoNotes;
        //Rebuild photo and display it
        //conver bytes to png
        //convert png to texture2D
        Texture2D reconstructedImg = new Texture2D(1, 1);
        reconstructedImg.LoadImage(UIManager.Instance.activeCase.photoTaken);

        rawImage.texture = reconstructedImg as Texture;
    }

    public void ProcessInfo()
    {
        
    }

}
