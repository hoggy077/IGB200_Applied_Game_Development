using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PauseMenu : MonoBehaviour
{
    public GameObject Dimmer;
    public GameObject Main;
    public GameObject Controls;
    public GameObject Confirm;
    private bool isActive;
    private bool isOnlyMenu => true;
    private void Start() {
        CloseMenu();
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) && isOnlyMenu) {
            if (isActive) {
                CloseMenu();
            }else{
                OpenMenu();
            }
            
        }
    }
    private void OpenMenu() {
        Time.timeScale = 0;
        isActive = true;
        Dimmer.SetActive(true);
        OpenMain();
    }
    private void CloseMenu() {
        Time.timeScale = 1;
        isActive = false;
        ToggleMenu(false);
    }
    public void OpenMain() {
        Main.SetActive(true);
        Confirm.SetActive(false);
        Controls.SetActive(false);
    }
    public void OpenControls() {
        Controls.SetActive(true);
        Main.SetActive(false);
        Confirm.SetActive(false);
    }
    public void OpenConfirm() {
        Confirm.SetActive(true);
        Main.SetActive(false);
        Controls.SetActive(false);
    }
    private void ToggleMenu(bool state) {
        for (int i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).transform.gameObject.SetActive(state);
        }
    }
    public void MAINMENU() {
        MainMenuScript.MAINMENU();
    }
}
