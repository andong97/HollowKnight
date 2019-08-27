using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharactorCtrl : MonoBehaviour {
    public Text moneynum;
    public Transform[] hpgroup;
    public int hpnum=4;
    public Transform[] soulgroup;
    public int soulnum=2;
	// Use this for initialization
	void Start () {
        moneynum.text = GameCtrl.moneynum.ToString();
    }
    public void GetHp() {
        hpnum += 1;
        if (hpnum > 4) {
            hpnum = 4;
            return;
        }
        CostSoul();
        hpgroup[hpnum].gameObject.SetActive(true);
    }
    public void GetSoul() {
        soulnum += 1;
        if (soulnum > 2) {
            soulnum = 2;
            return;
        }
        soulgroup[soulnum].gameObject.SetActive(true);
    }
    public void GetMoney(int num) {
        moneynum.text = (GameCtrl.moneynum + num).ToString();
        GameCtrl.moneynum += num;
    }
    public void CostSoul() {
        soulgroup[soulnum].gameObject.SetActive(false);
        soulnum -= 1;
    }
    public void Damage() {
        if (hpnum < 0) {
            return;
        }
        hpgroup[hpnum].gameObject.SetActive(false);
        hpnum -= 1;
    }
    public void BuySth(int num) {
        GameCtrl.moneynum -= num;
        moneynum.text = GameCtrl.moneynum.ToString();
    }
}
