using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Item_Card : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private PlantConfig cardConfig;

    public Image img_canUse;
    public Image img_cantUse;
    public Button btn_use;

    private bool m_InCooldown;

    public void InitView(PlantConfig cardConfig)
    {
        this.cardConfig = cardConfig;
        img_canUse.sprite = cardConfig.canUse;
        img_cantUse.sprite = cardConfig.cantUse;
        RefreshBtnState();
    }

    public void RefreshBtnState()
    {
        if (GameMgr.Ins.GetMoney() >= cardConfig.price)
        {
            img_canUse.gameObject.SetActive(true);
        }
        else
        {
            img_canUse.gameObject.SetActive(false);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (m_InCooldown)
        {
            return;
        }
        GameMgr.Ins.m_ItemGo = Instantiate(cardConfig.prefab);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (GameMgr.Ins.m_ItemGo != null)
        {
            ItemFollowMouse(eventData);

            Vector2 worldPos = CTUtils.Screen2World(eventData.position);
            Collider2D[] colliders = Physics2D.OverlapCircleAll(worldPos, 1, LayerMask.GetMask("GroundGrid"));
            float minDis = float.MaxValue;
            GameObject targetGridGround = null;
            foreach (var collider in colliders)
            {
                float tempDis = Vector2.Distance(worldPos, collider.transform.position);
                if (tempDis < minDis)
                {
                    minDis = tempDis;
                    targetGridGround = collider.gameObject;
                }
            }
            if (targetGridGround != null && targetGridGround.transform.childCount == 0)
            {
                GameMgr.Ins.UpdateGroundGrid(targetGridGround.GetComponent<GroundGrid>());
            }
            else
            {
                GameMgr.Ins.UpdateGroundGrid(null);
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (GameMgr.Ins.m_ItemGo != null)
        {
            Vector2 worldPos = CTUtils.Screen2World(eventData.position);
            Collider2D[] colliders = Physics2D.OverlapCircleAll(worldPos, 1, LayerMask.GetMask("GroundGrid"));
            float minDis = float.MaxValue;
            GameObject targetGridGround = null;
            foreach (var collider in colliders)
            {
                float tempDis = Vector2.Distance(worldPos, collider.transform.position);
                if (tempDis < minDis)
                {
                    minDis = tempDis;
                    targetGridGround = collider.gameObject;
                }
            }
            if (targetGridGround != null && targetGridGround.transform.childCount == 0)
            {
                GameMgr.Ins.m_ItemGo.transform.SetParent(targetGridGround.transform, false);
                GameMgr.Ins.m_ItemGo.transform.localPosition = Vector2.zero;
                GameMgr.Ins.m_ItemGo.GetComponent<Plant>().Init(cardConfig);
                m_InCooldown = true;
                TimerMgr.Ins.Register(cardConfig.cooldownTime, onComplete: () =>
                {
                    m_InCooldown = false;
                }, onUpdate: (leftTime) =>
                {
                    img_canUse.fillAmount = (cardConfig.cooldownTime - leftTime) / cardConfig.cooldownTime;
                });
                GameMgr.Ins.GetMoney(-cardConfig.price);
            }
            else
            {
                Destroy(GameMgr.Ins.m_ItemGo);
            }
            GameMgr.Ins.m_ItemGo = null;
            GameMgr.Ins.UpdateGroundGrid(null);
        }
    }

    private void ItemFollowMouse(PointerEventData eventData)
    {
        GameMgr.Ins.m_ItemGo.transform.position = (Vector2)CTUtils.Screen2World(eventData.position);
    }
}
