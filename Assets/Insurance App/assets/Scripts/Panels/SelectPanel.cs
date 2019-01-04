using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectPanel : MonoBehaviour, IPanel
{
    public Text caseInfo;

    public void ProcessInfo()
    {
        OverviewPanel.Instance.SetOverviewPanel();
    }
}
