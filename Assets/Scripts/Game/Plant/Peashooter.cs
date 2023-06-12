using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peashooter : Plant
{
    public Transform[] shootPosArray;

    private TimerTask m_Shoot_TimerTask;

    public override void Init(PlantConfig cardConfig)
    {
        base.Init(cardConfig);

        m_Shoot_TimerTask = TimerMgr.Ins.Register(m_CardConfig.attackTI, onComplete: () =>
        {
            Shoot();
        }, loopCount: -1);
    }

    public virtual void Shoot()
    {
        foreach (Transform shootPos in shootPosArray)
        {
            GameObject bulletGo = PoolMgr.Ins.peaBulletPool.Get();
            bulletGo.transform.position = shootPos.position;
            bulletGo.transform.rotation = shootPos.rotation;
            bulletGo.GetComponent<PeaBullet>().Init(this);
        }
    }

    private void OnDestroy()
    {
        m_Shoot_TimerTask?.Dispose();
        m_Shoot_TimerTask = null;
    }
}
