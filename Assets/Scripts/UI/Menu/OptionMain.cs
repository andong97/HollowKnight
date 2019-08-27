using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OptionMain : MonoBehaviour
{
    private int ItemNum;
    private Transform[] ItemChoose = new Transform[5];
    private Transform ChoosenItem;
    private Transform Item;
    private bool isInput;

     void OnEnable()
    {
        ItemNum = 0;
        if (!ItemChoose[0]) {
            return;
        }
        ChoosenItem = ItemChoose[0];
        ChoosenItem.Find("Arror").gameObject.SetActive(true);
    }

    void Start()
    {
        Item = transform.Find("Item");
        isInput = true;
        ItemNum = 0;
        //ItemChoose = new Transform [5];
        for (int i = 0; i < Item.childCount; i++)
        {
            //Transform t = transform.GetChild(i);
            ItemChoose[i] = Item.GetChild(i);
        }
        ItemChoose[4] = transform.Find("Return");
        ChoosenItem = ItemChoose[0];
        ChoosenItem.Find("Arror").gameObject.SetActive(true);
    }
    void Update()
    {
        ChooseButton();
    }
    void ChooseButton()
    {
        if (!isInput)
        {
            return;
        }
        //向下
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ItemNum += 1;
            if (ItemNum > 4)
            {
                ItemNum = 0;
            }
            ChoosenItem.Find("Arror").gameObject.SetActive(false);
            ChoosenItem = ItemChoose[ItemNum];
            ChoosenItem.Find("Arror").gameObject.SetActive(true);
        }
        //向上
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ItemNum -= 1;
            if (ItemNum < 0)
            {
                ItemNum = 4;
            }
            ChoosenItem.Find("Arror").gameObject.SetActive(false);
            ChoosenItem = ItemChoose[ItemNum];
            ChoosenItem.Find("Arror").gameObject.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(ToweenFinish());
        }
    }

     void OnDisable()
    {
        ChoosenItem.Find("Arror").gameObject.SetActive(false);
    }

    IEnumerator ToweenFinish()
    {
        isInput = false;
        Tween myTween = ChoosenItem.Find("Check").DOScaleX(34, 0.5f);
        yield return myTween.WaitForCompletion();
        ChoosenItem.Find("Check").DOScaleX(0, 0);
        isInput = true;
        if (ItemNum == 4)
        {
            Tween closeTween = transform.GetComponent<SpriteRenderer>().DOFade(0, 0.5f);
            yield return closeTween.WaitForCompletion();
            transform.gameObject.SetActive(false);
            GameObject.Find("Canvas").transform.Find("Start").gameObject.SetActive(true);
        }
    }

}
