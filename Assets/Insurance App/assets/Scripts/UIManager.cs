using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


}
