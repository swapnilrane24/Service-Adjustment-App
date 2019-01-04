using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SearchPanel : MonoBehaviour, IPanel
{
    public InputField caseNumberInput;
    public SelectPanel selectPanel;

    public void ProcessInfo()
    {
        AWSManager.Instance.GetList(caseNumberInput.text, () =>
        {
            string info = "";

            info += "CASE NUMBER " + UIManager.Instance.activeCase.caseID;
            info += "\n" + UIManager.Instance.activeCase.name;
            info += "\n" + UIManager.Instance.activeCase.locationNotes;
            info += "\n" + UIManager.Instance.activeCase.date;
            info += "\n" + UIManager.Instance.activeCase.photoNotes;

            selectPanel.caseInfo.text = info;

            selectPanel.gameObject.SetActive(true);
        });
    }
    
}
