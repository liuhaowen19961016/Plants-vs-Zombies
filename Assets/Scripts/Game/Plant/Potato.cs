using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class Potato : Plant
{
    private TimerTask m_Boom_TimerTask;

    private bool isBoom;

    public override void Init(PlantConfig cardConfig)
    {
        base.Init(cardConfig);

        isBoom = false;
    }

    private void Boom(List<GameObject> other)
    {
        isBoom = true;
        ani.SetTrigger("Boom");
        m_Boom_TimerTask = TimerMgr.Ins.Register(AnimatorUtils.GetAnimationClipLength(ani, "PotatoBoom"), onComplete: () =>
        {
            foreach (GameObject enemy in other)
            {
                enemy.GetComponent<Enemy>().GetHurt(m_CardConfig.damage);
            }
            Die();
        });
    }

    private void OnDestroy()
    {
        m_Boom_TimerTask?.Dispose();
        m_Boom_TimerTask = null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (m_IsOnGround && !isBoom && other.tag == "Enemy")
        {
            Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, 0.5f);
            List<GameObject> enemyList = new List<GameObject>();
            foreach (var collider in collider2Ds)
            {
                if (collider.tag == "Enemy" && Mathf.Abs(transform.position.y - collider.transform.position.y) <= 0.5f)
                {
                    enemyList.Add(collider.gameObject);
                }
            }
            Boom(enemyList);
        }
    }
}
