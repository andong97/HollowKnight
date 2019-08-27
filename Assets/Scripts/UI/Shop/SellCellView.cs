using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

public class SellCellView : MonoBehaviour {
    public Image Icon;
    public Text MoneyText;
    // Use this for initialization
    void Start()
    {

    }

    public void DisPlay(ItemData shopItem) {
        SpriteAtlas iconAtlas = Resources.Load<SpriteAtlas>("ItemIcon");
        string iconnum = shopItem.Icon.ToString().Substring(1);
        Icon.sprite = iconAtlas.GetSprite(iconnum);
        MoneyText.text = shopItem.Money.ToString();
    }

}
