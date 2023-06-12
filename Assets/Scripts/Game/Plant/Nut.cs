using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nut : Plant
{
    public override void Init(PlantConfig cardConfig)
    {
        base.Init(cardConfig);
    }

    public override void GetHurt(int damage)
    {
        base.GetHurt(damage);

        int curHurtState = -1;
        for (int i = 0; i < m_CardConfig.param1_int.Length; i++)
        {
            if (m_CurHp <= m_CardConfig.param1_int[i])
            {
                curHurtState = i+1;
            }
        }
        if (curHurtState != -1)
        {
            ani.SetInteger("hurtState", curHurtState);
        }
    }
}
