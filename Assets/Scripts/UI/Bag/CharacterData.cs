using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum skillType {
    
}

public class CharacterData{
    public int ATK;
    public List<skillType> Skill;
    public List<ItemData> AllItem;
    public List<ItemData> EquipItem;
    public int Soul;
    public int Hp;
    public bool isDeaded;
    public int SpendALL;
}
