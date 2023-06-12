using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CommonEnemy : Enemy
{
    private bool isLostHead;

    public GameObject head;

    public override void Init(EnemyConfig enemyConfig)
    {
        base.Init(enemyConfig);

        head.gameObject.SetActive(false);
        SetState();
    }

    public override void Move()
    {
        base.Move();

        if (transform.position.x <= -4.5f)
        {
            Vector3 dir = GameObject.Find("RoomPos").transform.position - transform.position;
            transform.right = -dir;
        }
        transform.position += Time.deltaTime * curSpeed * -transform.right;
    }

    public override void GetHurt(int damage)
    {
        base.GetHurt(damage);

        bool lostHead = m_CurHp <= m_EnemyConfig.stageHp[0];
        if (!isLostHead && lostHead)
        {
            head.gameObject.SetActive(true);
            TimerMgr.Ins.Register(0.5f, onComplete: () =>
            {
                head.gameObject.SetActive(false);
            });
            isLostHead = true;
            SetState();
        }
    }

    public override void Attack(GameObject other)
    {
        base.Attack(other);

        SetState();
    }

    public override void AttackFinish()
    {
        base.AttackFinish();

        SetState();
    }

    public void SetState()
    {
        ani.SetBool("isMove", !m_InAttack);
        ani.SetBool("isHurt", isLostHead);
    }
}
