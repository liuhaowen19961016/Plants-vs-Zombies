using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : PoolObject
{
    [HideInInspector]
    public Plant plant;
    public float speed;

    protected TimerTask m_TimerTask_Destroy;

    public virtual void Init(Plant plant)
    {
        this.plant = plant;
    }

    public virtual void Update()
    {
        transform.Translate(transform.right * Time.deltaTime * speed);
    }

    public override void Reset()
    {
        m_TimerTask_Destroy?.Dispose();
        m_TimerTask_Destroy = null;
    }

    public virtual void Attack(Collider2D other)
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            Attack(other);
        }
    }
}
