using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.UI;

public class ShopCtrl : MonoBehaviour {
    public Transform contentGrid;
    private int Index=1;
    private ItemData SelecteItem;
    private List<ItemData> ShopItem;
    public DesView desview;
    private List<ItemData> SaveItem=new List<ItemData>();
    private bool[] isBuy;
    private List<ItemData> SellItem;
    private Vector3 startPos;
    public CharactorCtrl MoneyView;
	// Use this for initialization
	void Start () {
        startPos = contentGrid.parent.GetComponent<RectTransform>().position;
    }
    void OnEnable()
    {
        contentGrid.DeleteAllChild();

        ShopItem = new List<ItemData>();
        if (GameCtrl.firstbuy)
        {
            Init();
            GameCtrl.firstbuy = false;
        }
        else
        {
            string data = PlayerPrefs.GetString("SellItem");
            ShopItem = JsonMapper.ToObject<List<ItemData>>(data);
        }

        isBuy = new bool[ShopItem.Count];
        for (int i = 0; i < ShopItem.Count; i++)
        {
            GameObject go = Prefabs.Load("Prefabs/SHop/SellCell", contentGrid);
            go.GetComponent<SellCellView>().DisPlay(ShopItem[i]);
            isBuy[i] = false;
        }
        SelecteItem = ShopItem[0];

        SellItem = ShopItem;

        desview.DisPlay(SelecteItem);
    }

    void Init() {
        ShopItem = ReadAllItemJson.readallItemJson();
    }
	
	// Update is called once per frame
	void Update () {
        Move();
        Buy();
    }

    void Move() {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Index += 1;
            if (Index >ShopItem.Count) {
                Index = ShopItem.Count;
                return;
            }
            contentGrid.GetComponent<RectTransform>().position = new Vector2(
                contentGrid.GetComponent<RectTransform>().position.x,
                contentGrid.GetComponent<RectTransform>().position.y + 81.5f
                );
            SelecteItem = ShopItem[Index - 1];
            desview.DisPlay(SelecteItem);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            Index -= 1;
            if (Index <= 0)
            {
                Index = 1;
                return;
            }
            contentGrid.GetComponent<RectTransform>().position = new Vector2(
                contentGrid.GetComponent<RectTransform>().position.x,
                contentGrid.GetComponent<RectTransform>().position.y - 81.5f
                );
            SelecteItem = ShopItem[Index - 1];
            desview.DisPlay(SelecteItem);
        }
    }

    void Buy() {
        if (Input.GetKeyDown(KeyCode.C)) {
            if (SelecteItem.Money > GameCtrl.moneynum) {
                return;
            }

            int index = ShopItem.IndexOf(SelecteItem);
            if (isBuy[index]) {
                return;
            }
            isBuy[index] = true;
            SaveItem.Add(SelecteItem);
            //SellItem.Remove(SelecteItem);
            Debug.Log("你购买了"+SelecteItem.name);
            MoneyView.BuySth(SelecteItem.Money);
            //contentGrid.GetChild(index).Find("Panel").gameObject.SetActive(true);
            contentGrid.GetChild(index).Find("Num").GetComponent<Text>().text = "已拥有";
        }
    }

    void Save() {
            if (SaveItem == null) {
                return;
            }
            for (int i = 0; i < SaveItem.Count; i++) {
                SellItem.Remove(SaveItem[i]);
            }
            string buydata = JsonMapper.ToJson(SaveItem);
            Debug.Log(buydata);
            PlayerPrefs.SetString("BuyenItem", buydata);

            string selldata = JsonMapper.ToJson(SellItem);
            Debug.Log(selldata);
            PlayerPrefs.SetString("SellItem", selldata);     
    }

    void OnDisable()
    {
        Save();
    }

    void p() {
        if(Input.GetKeyDown(KeyCode.Keypad0)){
            PlayerPrefs.DeleteKey("BuyenItem");

            PlayerPrefs.DeleteKey("SellItem");
        }
    }
}
