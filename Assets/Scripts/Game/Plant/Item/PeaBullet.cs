using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeaBullet : Bullet
{
    public override void Init(Plant plant)
    {
        base.Init(plant);

        m_TimerTask_Destroy = TimerMgr.Ins.Register(5, onComplete: () =>
        {
            PoolMgr.Ins.peaBulletPool.Put(gameObject);
        });
    }

    public override void Attack(Collider2D other)
    {
        base.Attack(other);

        other.GetComponent<Enemy>().GetHurt(plant.m_CardConfig.damage);
        PoolMgr.Ins.peaBulletPool.Put(gameObject);
    }
}
