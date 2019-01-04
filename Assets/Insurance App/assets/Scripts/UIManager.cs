using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;

    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("UIManager is null");
            }

            return _instance;
        }
    }

    public Case activeCase;
    public ClientInfoPanel clientInfoPanel;
    public GameObject borderPanel;
    public Button submitBtn;

    private void Awake()
    {
        _instance = this;
    }

    public void CreateNewCase()
    {
        activeCase = new Case();

        //generate case ID
        int caseID = Random.Range(0, 1000);
        activeCase.caseID = "" + caseID;
        clientInfoPanel.gameObject.SetActive(true);
        clientInfoPanel.caseNumber.text = "CASE NUMBER " + activeCase.caseID;
        borderPanel.SetActive(true);
    }

    public void SubmitButton()
    {
        submitBtn.interactable = false;
        Case awsCase = new Case();
        awsCase = activeCase;

        string filePath = Application.persistentDataPath + "/case#" + awsCase.caseID + ".dat";

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(filePath);

        bf.Serialize(file, awsCase);
        file.Close();

        Debug.Log("Application DataPath: " + Application.persistentDataPath);

        //Send to AWS
        AWSManager.Instance.PostObject(filePath, awsCase.caseID);

    }

}
