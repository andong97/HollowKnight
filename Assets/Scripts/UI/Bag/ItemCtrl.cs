using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class ItemCtrl : MonoBehaviour {
    private List<ItemData> CharacterItem;
    private List<ItemData> CharacterEquipItem;
    private ItemData ChoosenItem;
    public  Transform Gird;
    public ItemDetails ItemDetailsView;
    public ItemIcon AllItemView;
    public ItemEquip ItemEquipView;
    public Transform Choosen;
    public Transform AllItem;
    private bool isEquip;

    int row = 0;//行数，最多为3
    int col = 0;//列数，最多为8

    void Start () {
        // CharacterEquipItem = new List<ItemData>();

        // string Equipdata = PlayerPrefs.GetString("CharacterItem");
        // CharacterEquipItem = JsonMapper.ToObject<List<ItemData>>(Equipdata);

        //// CharacterItem = ReadAllItemJson.readallItemJson();

        // string Buydata = PlayerPrefs.GetString("BuyenItem");

        // CharacterItem = JsonMapper.ToObject<List<ItemData>>(Buydata);

        // for (int i = 0; i < 24; i++) {
        //     GameObject go = Prefabs.Load("Prefabs/Bag/ItemIcon", Gird);
        //     go.GetComponent<ItemIcon>().Display(CharacterItem[i]);
        // }
        // ChoosenItem = CharacterItem[0];

        // Debug.Log(ChoosenItem);

        // ItemDetailsView.DisPlay(ChoosenItem);
    }

    private void OnEnable()
    {
        CharacterEquipItem = new List<ItemData>();
        AllItem.DeleteAllChild();

        string Equipdata = PlayerPrefs.GetString("CharacterItem");
        CharacterEquipItem = JsonMapper.ToObject<List<ItemData>>(Equipdata);

        // CharacterItem = ReadAllItemJson.readallItemJson();

        string Buydata = PlayerPrefs.GetString("BuyenItem");

        CharacterItem = JsonMapper.ToObject<List<ItemData>>(Buydata);

        for (int i = 0; i < CharacterItem.Count; i++)
        {
            GameObject go = Prefabs.Load("Prefabs/Bag/ItemIcon", Gird);
            go.GetComponent<ItemIcon>().Display(CharacterItem[i]);
        }
        ChoosenItem = CharacterItem[0];

        Debug.Log(ChoosenItem);

        ItemDetailsView.DisPlay(ChoosenItem);
    }
    void Update () {
        SwitchGroup();
        if (isEquip)
        {
            EquipItemMove();
            if (Input.GetKeyDown(KeyCode.Z)) {
                UnEquipItem();
            }
        }
        else if(!isEquip) {
            ChoosenMove();
            if (Input.GetKeyDown(KeyCode.Z)){
                EquipItem();
            }
        }
        if (Input.GetKeyDown(KeyCode.Q)) {
            SaveData();
        }
    }

    void ChoosenMove() {
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            col += 1;
            if (col == 8 && row != 2)
            {
                Choosen.GetComponent<RectTransform>().position = new Vector3(
                Choosen.GetComponent<RectTransform>().position.x - 146 * (col-1),
                Choosen.GetComponent<RectTransform>().position.y - 143,
                0
                );
                col = 0;
                row += 1;
            }
            else if (col ==  8 && row == 2)
            {
                Choosen.GetComponent<RectTransform>().position = new Vector3(
                Choosen.GetComponent<RectTransform>().position.x - 146 * (col-1),
                Choosen.GetComponent<RectTransform>().position.y + 143 * row,
                0
                );
                col = 0;
                row = 0;
            }
            else
            {
                Choosen.GetComponent<RectTransform>().position = new Vector3(
                    Choosen.GetComponent<RectTransform>().position.x + 146,
                    Choosen.GetComponent<RectTransform>().position.y,
                    0
                    );
            }
            ChoosenItem = CharacterItem[row * 8 + col];
            ItemDetailsView.DisPlay(ChoosenItem);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            col -= 1;
            if (col == -1 && row != 0)
            {
                Choosen.GetComponent<RectTransform>().position = new Vector3(
                Choosen.GetComponent<RectTransform>().position.x + 146 * 7,
                Choosen.GetComponent<RectTransform>().position.y + 143,
                0
                );
                col = 7;
                row -= 1;
            }
            else if (col == -1 && row == 0)
            {
                Choosen.GetComponent<RectTransform>().position = new Vector3(
                Choosen.GetComponent<RectTransform>().position.x + 146 * 7,
                Choosen.GetComponent<RectTransform>().position.y - 143 * 2,
                0
                );
                col = 7;
                row = 2;
            }
            else
            {
                Choosen.GetComponent<RectTransform>().position = new Vector3(
                    Choosen.GetComponent<RectTransform>().position.x - 146,
                    Choosen.GetComponent<RectTransform>().position.y,
                    0
                    );
            }
            ChoosenItem = CharacterItem[row * 8 + col];
            ItemDetailsView.DisPlay(ChoosenItem);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            row += 1;
            if (row == 3)
            {
                Choosen.GetComponent<RectTransform>().position = new Vector3(
                               Choosen.GetComponent<RectTransform>().position.x,
                               Choosen.GetComponent<RectTransform>().position.y + 143 * 2,
                               0
                               );
                row = 0;
            }
            else {
                Choosen.GetComponent<RectTransform>().position = new Vector3(
                    Choosen.GetComponent<RectTransform>().position.x,
                    Choosen.GetComponent<RectTransform>().position.y - 143,
                    0
                    );
            }
            ChoosenItem = CharacterItem[row * 8 + col];
            ItemDetailsView.DisPlay(ChoosenItem);
        }
        else if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            row -= 1;
            if (row == -1)
            {
                Choosen.GetComponent<RectTransform>().position = new Vector3(
                        Choosen.GetComponent<RectTransform>().position.x,
                        Choosen.GetComponent<RectTransform>().position.y - 143 * 2,
                         0
                );
                row = 2;
            }
            else
            {
                Choosen.GetComponent<RectTransform>().position = new Vector3(
                   Choosen.GetComponent<RectTransform>().position.x,
                   Choosen.GetComponent<RectTransform>().position.y + 143,
                   0
                   );
            }
            ChoosenItem = CharacterItem[row * 8 + col];
            ItemDetailsView.DisPlay(ChoosenItem);
        }
    }


    void EquipItemMove() {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            if (row == 0)
            {
                Choosen.GetComponent<RectTransform>().position = new Vector3(
                   Choosen.GetComponent<RectTransform>().position.x,
                   Choosen.GetComponent<RectTransform>().position.y,
                   0
                   );
            }
            else
            {
                Choosen.GetComponent<RectTransform>().position = new Vector3(
                       Choosen.GetComponent<RectTransform>().position.x - 150,
                       Choosen.GetComponent<RectTransform>().position.y,
                       0
                       );
                row -= 1;
            }
            ChoosenItem = CharacterEquipItem[row];
            ItemDetailsView.DisPlay(ChoosenItem);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow)){
            if (row == CharacterEquipItem.Count - 1)
            {
                Choosen.GetComponent<RectTransform>().position = new Vector3(
                   Choosen.GetComponent<RectTransform>().position.x,
                   Choosen.GetComponent<RectTransform>().position.y,
                   0
                   );
            }
            else
            {
                Choosen.GetComponent<RectTransform>().position = new Vector3(
                       Choosen.GetComponent<RectTransform>().position.x + 150,
                       Choosen.GetComponent<RectTransform>().position.y,
                       0
                       );
                row += 1;
            }
            ChoosenItem = CharacterEquipItem[row];
            ItemDetailsView.DisPlay(ChoosenItem);
        }
    }


    void SwitchGroup() {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            if (CharacterEquipItem.Count == 0)
            {
                Debug.Log("当前未装备任何护符！");
                return;
            }
            isEquip = !isEquip;
            if (isEquip)
            {
                row = 0;
                col = 0;
                Choosen.GetComponent<RectTransform>().position = new Vector2(286, 786);
                ChoosenItem = CharacterEquipItem[0];
                ItemDetailsView.DisPlay(ChoosenItem);
            }
            else if (!isEquip) {
                row = 0;
                col = 0;
                Choosen.GetComponent<RectTransform>().position = new Vector2(256, 477);
                ChoosenItem = CharacterItem[0];
                ItemDetailsView.DisPlay(ChoosenItem);
            }
        }
        
    }


    void EquipItem() {
        int spendnum = 0;
        for (int i = 0; i < CharacterEquipItem.Count; i++)
        {
            spendnum += CharacterEquipItem[i].Spend;
        }
        if (spendnum + ChoosenItem.Spend> 5) {
            Debug.Log("你已经不能再装备更多的护符了");
            return;
        }
        if (Choosen == null) {
            Debug.Log("未选中护符");
            return;
        }
        if (CharacterEquipItem.Contains(ChoosenItem))
        {
            Debug.Log("你已经装备了这个护符");
            return;
        }
        CharacterEquipItem.Add(ChoosenItem);
        ItemEquipView.DisPlay(CharacterEquipItem);
    }

    void UnEquipItem() {    
        int index=CharacterEquipItem.IndexOf(ChoosenItem);
        CharacterEquipItem.Remove(ChoosenItem);

        if (CharacterEquipItem.Count == 0)
        {
            isEquip = false;
            row = 0;
            col = 0;
            Choosen.GetComponent<RectTransform>().position = new Vector2(256, 477);
            ChoosenItem = CharacterItem[0];
            ItemDetailsView.DisPlay(ChoosenItem);
        }
        else {
            ChoosenItem = CharacterEquipItem[index];
        }

        ItemDetailsView.DisPlay(ChoosenItem);
        ItemEquipView.DisPlay(CharacterEquipItem);
    }

    private void OnDisable()
    {
        SaveData();
    }
    void SaveData() {
        string data = JsonMapper.ToJson(CharacterEquipItem);
        PlayerPrefs.SetString("CharacterItem", data);
    }
}
