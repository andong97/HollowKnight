using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class SceneUIMain : MonoBehaviour {
    private Transform[] Text;
    private bool isInput;
    private int ItemNum;
    private Transform ChoosenItem;

	void Start () {
    }

    private void OnEnable()
    {
        ItemNum = 0;
        isInput = true;
        GameCtrl.isGamePause = false;
        Text = new Transform[3];
        Transform tmp = transform.Find("Panel").Find("Text");
        for (int i = 0; i < tmp.childCount; i++)
        {
            Text[i] = tmp.GetChild(i);
        }
        ChoosenItem = Text[0];
        ChoosenItem.Find("ArrorScene").gameObject.SetActive(true);

        GameCtrl.isGamePause = true;
        transform.Find("Panel").Find("Text").DOScale(1, 0.5f);
    }
    // Update is called once per frame
    void Update () {
        if (!isInput) {
            return;
        }
        ChooseItem();
    }

     void OnDisable()
    {
        GameCtrl.isGamePause = false;
    }

    void ChooseItem() {
        if (!GameCtrl.isGamePause)
        {
            return;
        }
        if (GameCtrl.isGamePause) {
            if (Input.GetKeyDown(KeyCode.DownArrow)) {
                ItemNum += 1;
                if (ItemNum > 2)
                {
                    ItemNum = 0;
                }
                ChoosenItem.Find("ArrorScene").gameObject.SetActive(false);
                ChoosenItem = Text[ItemNum];
                ChoosenItem.Find("ArrorScene").gameObject.SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ItemNum -= 1;
            if (ItemNum < 0)
            {
                ItemNum = 2;
            }
            ChoosenItem.Find("ArrorScene").gameObject.SetActive(false);
            ChoosenItem = Text[ItemNum];
            ChoosenItem.Find("ArrorScene").gameObject.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (ItemNum == 0)
            {               
                this.gameObject.SetActive(false);
                GameCtrl.isGamePause = false;
            }
            if (ItemNum == 1) {
                Tween myTween = ChoosenItem.Find("Check").DOScaleX(24, 0.5f);
            }
            if (ItemNum == 2)
            {
                StartCoroutine(ToweenFinish());
            }
        }
    }

    IEnumerator ToweenFinish()
    {
        isInput = false;
        Tween myTween = ChoosenItem.Find("Check").DOScaleX(24, 0.5f);
        yield return myTween.WaitForCompletion();
        ChoosenItem.Find("Check").DOScaleX(0, 0);
        isInput = true;
        SceneManager.LoadScene(0);
    }

    public void returngame() {
        SceneManager.LoadScene(0);
    }
}
