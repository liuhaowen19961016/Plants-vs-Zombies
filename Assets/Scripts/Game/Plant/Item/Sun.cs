using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : PoolObject
{
    private bool m_IsSkySun;
    private bool m_InCollectAni;

    private float m_TargetY;

    private TimerTask m_TimerTask_Destroy;

    public void Init(bool isSkySun, float targetY = 0)
    {
        m_InCollectAni = false;
        m_IsSkySun = isSkySun;
        if (isSkySun)
        {
            m_TargetY = targetY;
        }
        else
        {

        }
    }

    public void Update()
    {
        if (!m_InCollectAni)
        {
            if (m_IsSkySun)
            {
                HandleSkySun();
            }
            else
            {
                HandleFlowerSun();
            }
        }
        else
        {
            Transform targetTrans = GameObject.Find("txt_sun").transform;
            Vector2 targetPos = CTUtils.UIWorld2World(targetTrans.position);
            if (Vector2.Distance(transform.position, targetPos) > 0.5f)
            {
                transform.position = Vector2.MoveTowards(transform.position, targetPos, Time.deltaTime * 8);
                transform.localScale -= Vector3.one * Time.deltaTime * 1f;
            }
            else
            {
                PoolMgr.Ins.sunPool.Put(gameObject);
            }
        }
    }

    public override void Reset()
    {
        transform.localScale = Vector3.one;
        m_TimerTask_Destroy?.Dispose();
        m_TimerTask_Destroy = null;
    }

    private void HandleSkySun()
    {
        if (transform.position.y > m_TargetY)
        {
            Vector2 targetPos = new Vector2(transform.position.x, m_TargetY);
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * 2);
        }
        else
        {
            if (m_TimerTask_Destroy == null)
            {
                m_TimerTask_Destroy = TimerMgr.Ins.Register(3, onComplete: () =>
                {
                    if (!m_InCollectAni)
                    {
                        PoolMgr.Ins.sunPool.Put(gameObject);
                    }
                });
            }
        }
    }

    private void HandleFlowerSun()
    {
        if (m_TimerTask_Destroy == null)
        {
            m_TimerTask_Destroy = TimerMgr.Ins.Register(3, onComplete: () =>
            {
                if (!m_InCollectAni)
                {
                    PoolMgr.Ins.sunPool.Put(gameObject);
                }
            });
        }
    }

    public void OnMouseDown()
    {
        if (!m_InCollectAni)
        {
            GameMgr.Ins.GetMoney(GameMgr.Ins.config.moneyPerSun);
            m_InCollectAni = true;
        }
    }
}
