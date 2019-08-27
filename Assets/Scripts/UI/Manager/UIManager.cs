using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {
    public Transform ShopUI;
    public Transform ItemUI;
    public Transform MenuUI;
    private bool isopenUI;
    private Transform tmpUI;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape) && !isopenUI) {
            OpenMenu();
            return;
        }
        if (Input.GetKeyDown(KeyCode.Escape) && isopenUI)
        {
            CloseMenu();
            return;
        }
        if (Input.GetKeyDown(KeyCode.B) && !isopenUI) {
            OpenBag();
            return;
        }
        if (Input.GetKeyDown(KeyCode.B) && isopenUI) {
            CloseBag();
        }
        if (Input.GetKeyDown(KeyCode.P) && !isopenUI) {
            OpenShop();
            return;
        }
        if (Input.GetKeyDown(KeyCode.P) && isopenUI) {
            CloseShop();
            return;
        }
    }

    void OpenMenu() {
            MenuUI.gameObject.SetActive(true);
            isopenUI = true;
        
    }
    void CloseMenu() {
            MenuUI.gameObject.SetActive(false);
            isopenUI = false;    
    }
    void OpenBag() {

    }
    void CloseBag() {

    }
    void OpenShop() {
        ShopUI.gameObject.SetActive(true);
        isopenUI = true;
    }
    void CloseShop() {
        ShopUI.gameObject.SetActive(false);
        isopenUI = false;
    }
}
