using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBullet : Bullet
{
    public float slowDownSpeed;

    public override void Init(Plant plant)
    {
        base.Init(plant);

        m_TimerTask_Destroy = TimerMgr.Ins.Register(5, onComplete: () =>
        {
            PoolMgr.Ins.iceBulletPool.Put(gameObject);
        });
    }

    public override void Attack(Collider2D other)
    {
        base.Attack(other);

        other.GetComponent<Enemy>().SlowDown(slowDownSpeed);
        other.GetComponent<Enemy>().GetHurt(plant.m_CardConfig.damage);
        PoolMgr.Ins.iceBulletPool.Put(gameObject);
    }
}
