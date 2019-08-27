using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

public class ItemDetails : MonoBehaviour {
    public Text Name;
    public Image Spend;
    public Image Icon;
    public Text Description;
    public Transform Group;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void DisPlay(ItemData ChoosenItem) {
        SpriteAtlas iconAtlas = Resources.Load<SpriteAtlas>("ItemIcon");
        //修改图标
        string iconnum = ChoosenItem.Icon.ToString().Substring(1);
        Icon.sprite = iconAtlas.GetSprite(iconnum);
        Name.text = ChoosenItem.name;
        Description.text = ChoosenItem.Description;
        Group.DeleteAllChild();
        for (int i = 0; i < ChoosenItem.Spend; i++) {
            GameObject cell = Prefabs.Load("Prefabs/Bag/SpendDetails", Group);
        }
    }
}
