using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class ItemIcon : MonoBehaviour {
    public Image Icon;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public  void Display(ItemData Item) {
        SpriteAtlas iconAtlas = Resources.Load<SpriteAtlas>("ItemIcon");
        string iconnum = Item.Icon.ToString().Substring(1);
        Icon.sprite = iconAtlas.GetSprite(iconnum);
        Icon.SetNativeSize();
    }
}
