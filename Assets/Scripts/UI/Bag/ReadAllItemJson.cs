using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

public static class ReadAllItemJson{
    public static List<ItemData> readallItemJson() {
        TextAsset ta = Resources.Load<TextAsset>("Json/ItemData");
        List<ItemData> AllItemData = JsonMapper.ToObject<List<ItemData>>(ta.text);
        return AllItemData;
    }
}
