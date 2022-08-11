using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ModeButton : MonoBehaviour
{
    private SelectModePanelManager selectModePanelManager;

    [SerializeField] private TextMeshProUGUI modeText;
    private string modeName;

    private void Start()
    {
        selectModePanelManager = GameObject.FindObjectOfType<SelectModePanelManager>();
    }

    public void SetModeName(string modeName)
    {
        this.modeName = modeName;
        modeText.SetText(modeName);
    }

    public void StartMode()
    {
        selectModePanelManager.SelectAndStartMode(modeName);
    }
}