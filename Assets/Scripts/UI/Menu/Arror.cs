using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Arror : MonoBehaviour {
    private int ItemNum;
    private Transform[] ItemChoose=new Transform[5];
    private Transform ChoosenItem;
    private bool isInput;

    void OnEnable()
    {
        ItemNum = 0;
        if (!ItemChoose[0])
        {
            return;
        }
        ChoosenItem = ItemChoose[0];
        ChoosenItem.Find("Arror").gameObject.SetActive(true);
    }

    void Start () {
        isInput = true;
        ItemNum = 0;
       //ItemChoose = new Transform[5];
        for (int i=0; i < transform.childCount; i++) {
            //Transform t = transform.GetChild(i);
            ItemChoose[i] = transform.GetChild(i);
        }
        ChoosenItem = ItemChoose[0];
        ChoosenItem.Find("Arror").gameObject.SetActive(true);
    }
    void Update()
    {
        ChooseButton();
    }
    void ChooseButton() {
        if (!isInput)
        {
            return;
        }
        //向下
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            ItemNum += 1;
            if (ItemNum == 5) {
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
        //确定
        if (Input.GetKeyDown(KeyCode.Space)) {
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
        Tween myTween= ChoosenItem.Find("Check").DOScaleX(34, 0.5f);
        yield return myTween.WaitForCompletion();
        ChoosenItem.Find("Check").DOScaleX(0,0);
        isInput = true;
        if (ItemNum == 1){
            Tween closeTween= transform.parent.parent.GetComponent<SpriteRenderer>().DOFade(0, 0.5f);
            yield return closeTween.WaitForCompletion();
            transform.parent.parent.gameObject.SetActive(false);
            GameObject.Find("Canvas").transform.Find("OptionMain").gameObject.SetActive(true);
        }
        if (ItemNum == 0) {
            SceneManager.LoadScene(1);
        }
    }
}
