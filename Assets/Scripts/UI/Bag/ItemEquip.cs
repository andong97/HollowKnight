using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class ItemEquip : MonoBehaviour {
    public Transform Gird;
    public Transform SpendGroup;
    private Transform[] SpendIcon;
    private int spendnum;
	void Start () {
        SpendIcon = new Transform[5];
        for (int i = 0; i < transform.Find("SpendGroup").childCount; i++) {
            SpendIcon[i] = transform.Find("SpendGroup").GetChild(i);
        }
	}
	
	// Update is called once per frame
    public  void DisPlay(List<ItemData> EquipItemList) {
        if (SpendIcon==null) {
            SpendIcon = new Transform[5];
            for (int i = 0; i < transform.Find("SpendGroup").childCount; i++)
            {
                SpendIcon[i] = transform.Find("SpendGroup").GetChild(i);
            }
        }
        Gird.DeleteAllChild();
        for (int i = 0; i < 5; i++)
        {
            SpendIcon[i].gameObject.GetComponent<Image>().color = Color.white;
        }

        spendnum = 0;
        for (int i = 0; i < EquipItemList.Count; i++) {
            GameObject go = Prefabs.Load("Prefabs/Bag/ItemIcon", Gird);
            go.GetComponent<ItemIcon>().Display(EquipItemList[i]);
            spendnum += EquipItemList[i].Spend;
        }
        for (int i = 0; i < spendnum; i++) {
            SpendIcon[i].gameObject.GetComponent<Image>().color = Color.red;
        }
    }
}
