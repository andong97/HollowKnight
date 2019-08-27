using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameCtrl  {
    public static bool isGamePause=false;
    public static bool isOpenBag = false;
    public static int moneynum=200000;
    public static int scenenum;
    public static Vector3 position;
    public static bool firstbuy = true;
    public static void Victor(GameObject t) {
        t.SetActive(true);
    }
}
