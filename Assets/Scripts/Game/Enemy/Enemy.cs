using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected EnemyConfig m_EnemyConfig;

    protected Animator ani;
    protected BoxCollider2D collder2D;

    protected TimerTask m_AttackTimerTask;
    protected TimerTask m_SlowDownTimerTask;
    protected int m_CurHp;
    protected bool m_InAttack;
    protected bool m_IsDie;
    protected float curSpeed;

    public void Awake()
    {
        collder2D = GetComponent<BoxCollider2D>();
        ani = GetComponent<Animator>();
    }

    public virtual void Init(EnemyConfig enemyConfig)
    {
        m_EnemyConfig = enemyConfig;
        m_CurHp = enemyConfig.totalHp;
        curSpeed = enemyConfig.moveSpeed;
        m_InAttack = false;
    }

    public void Update()
    {
        if (m_IsDie || m_InAttack)
        {
            return;
        }
        Move();
    }

    public virtual void Move()
    {

    }

    public virtual void GetHurt(int damage)
    {
        m_CurHp -= damage;
        if (m_CurHp <= 0)
        {
            Die();
        }
    }

    public void SlowDown(float speed)
    {
        curSpeed = speed;
        m_SlowDownTimerTask?.Dispose();
        m_SlowDownTimerTask = TimerMgr.Ins.Register(2, onComplete: () =>
        {
            curSpeed = m_EnemyConfig.moveSpeed;
        });
    }

    public void BeBurn()
    {

    }

    public virtual void Die()
    {
        m_IsDie = true;
        collder2D.enabled = false;
        ani.SetTrigger("Die");
        TimerMgr.Ins.Register(AnimatorUtils.GetAnimationClipLength(ani, "Die"), onComplete: () =>
        {
            Destroy(gameObject);
        });
    }

    public virtual void Attack(GameObject other)
    {
        m_InAttack = true;
        m_AttackTimerTask = TimerMgr.Ins.Register(m_EnemyConfig.attackTI, onComplete: () =>
        {
            if (other != null)
            {
                other.GetComponent<Plant>()?.GetHurt(m_EnemyConfig.damage);
            }
        }, loopCount: -1);
    }

    public virtual void AttackFinish()
    {
        m_InAttack = false;
        m_AttackTimerTask?.Dispose();
        m_AttackTimerTask = null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Plant" && Mathf.Abs(transform.position.y - other.transform.position.y) <= 0.5f)
        {
            Attack(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Plant")
        {
            AttackFinish();
        }
    }

    private void OnDestroy()
    {
        m_AttackTimerTask?.Dispose();
        m_AttackTimerTask = null;
        m_SlowDownTimerTask?.Dispose();
        m_SlowDownTimerTask = null;
    }
}
