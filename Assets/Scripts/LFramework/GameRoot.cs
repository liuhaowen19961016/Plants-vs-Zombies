using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    public void Awake()
    {
        UIMgr.Ins.Init(new Vector2(1920, 1080), true);
        UIMgr.Ins.Show("Prefabs/UI/UI_Win_Game");
        GameMgr.Ins.Init();
    }
}
