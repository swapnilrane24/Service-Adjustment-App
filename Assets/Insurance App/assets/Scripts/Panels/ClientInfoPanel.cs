using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClientInfoPanel : MonoBehaviour, IPanel
{
    
    public Text caseNumber;
    public InputField firstNameInput, lastNameInput;
    public LocationPanel locationPanel;

    public void ProcessInfo()
    {
        if (string.IsNullOrEmpty(firstNameInput.text) || string.IsNullOrEmpty(lastNameInput.text))
        {
            Debug.Log("First name or Last name is empty");
        }
        else
        {
            UIManager.Instance.activeCase.name = firstNameInput.text + " " + lastNameInput.text;
            locationPanel.caseNumber.text = "CASE NUMBER " + UIManager.Instance.activeCase.caseID;
            locationPanel.gameObject.SetActive(true);
        }
    }
}
