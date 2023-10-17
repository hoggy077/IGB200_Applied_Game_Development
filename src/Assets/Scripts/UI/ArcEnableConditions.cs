using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ArcEnableConditions : MonoBehaviour
{
    private bool m_enabled = false;
    private Button thisBtn;
    public GameObject DirectionPanel;

    private void Start()
    {
        thisBtn = GetComponent<Button>();
    }

    void Update()
    {
        if(!m_enabled && UnlockManager.Instance.Registry.IsUnlocked("ArcLeft") && UnlockManager.Instance.Registry.IsUnlocked("ArcRight"))
        {
            thisBtn.interactable = true;
            m_enabled = true;
        }
        else if(m_enabled == false)
            thisBtn.interactable = false;
        //else if(first_run)
        //{
        //    thisBtn.interactable = false;
        //    ToggleOptions();
        //    first_run = false;
        //}
    }

    public void ToggleOptions()
    {
        DirectionPanel.SetActive(!DirectionPanel.activeInHierarchy);
    }
}
