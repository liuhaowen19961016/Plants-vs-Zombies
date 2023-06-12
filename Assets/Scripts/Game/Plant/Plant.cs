using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    private BoxCollider2D[] colliders;
    protected Animator ani;

    [HideInInspector]
    public PlantConfig m_CardConfig;

    protected int m_CurHp;
    protected bool m_IsOnGround;

    public void Awake()
    {
        ani = GetComponent<Animator>();
        colliders = GetComponentsInChildren<BoxCollider2D>();
        foreach (var collider in colliders)
        {
            collider.enabled = false;
        }
        m_IsOnGround = false;
    }

    public virtual void Init(PlantConfig cardConfig)
    {
        m_CardConfig = cardConfig;
        m_CurHp = cardConfig.hp;
        foreach (var collider in colliders)
        {
            collider.enabled = true;
        }
        m_IsOnGround = true;
    }

    public virtual void GetHurt(int damage)
    {
        m_CurHp -= damage;
        if (m_CurHp <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }
}
