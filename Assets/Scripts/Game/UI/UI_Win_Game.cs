using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Win_Game : BaseUI
{
    public Text txt_money;
    public Transform c_card;

    private List<Item_Card> cardItems = new List<Item_Card>();

    public void Awake()
    {
        MsgSystem.AddListener<int>(MsgConst.UpdateMoney, OnUpdateMoney);
    }

    public void Start()
    {
        foreach (var cardConfig in GameMgr.Ins.config.plantConfigs)
        {
            GameObject cardGo = Instantiate(Resources.Load<GameObject>("Prefabs/UI/Item_Card"));
            cardGo.transform.SetParent(c_card.transform, false);
            cardGo.GetComponent<Item_Card>().InitView(cardConfig);
            cardItems.Add(cardGo.GetComponent<Item_Card>());
        }
    }

    private void OnUpdateMoney(int money)
    {
        txt_money.text = money.ToString();
        foreach (var carItem in cardItems)
        {
            carItem.RefreshBtnState();
        }
    }

    public void OnDestroy()
    {
        MsgSystem.RemoveListener<int>(MsgConst.UpdateMoney, OnUpdateMoney);
    }
}
