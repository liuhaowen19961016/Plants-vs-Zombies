using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeaShooterPro : Peashooter
{
    public override void Shoot()
    {
        foreach (Transform shootPos in shootPosArray)
        {
            TimerMgr.Ins.Register(0.1f, onComplete: () =>
            {
                GameObject bulletGo = PoolMgr.Ins.peaBulletPool.Get();
                bulletGo.transform.position = shootPos.position;
                bulletGo.GetComponent<PeaBullet>().Init(this);
            }, loopCount: 3);
        }
    }
}
