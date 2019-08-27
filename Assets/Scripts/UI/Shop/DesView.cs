using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

public class DesView : MonoBehaviour {
    public Text Name;
    public Text Des;
    public Transform Group;
	// Use this for initialization
	void Start () {
		
	}
    public void DisPlay(ItemData SelectItem) {
        Group.DeleteAllChild();
        Name.text = SelectItem.name;
        Des.text = SelectItem.Description;
        for (int i = 0; i < SelectItem.Spend; i++)
        {
            GameObject cell = Prefabs.Load("Prefabs/Bag/SpendDetails", Group);
        }
    }
}
