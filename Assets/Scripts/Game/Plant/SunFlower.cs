using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunFlower : Plant
{
    public Animator ani;

    private TimerTask m_CreateSun_TimerTask;
    private TimerTask m_CreateSun_TimerTask2;

    public override void Init(PlantConfig cardConfig)
    {
        base.Init(cardConfig);

        m_CreateSun_TimerTask = TimerMgr.Ins.Register(m_CardConfig.attackTI, onComplete: () =>
        {
            ani.SetTrigger("CreateSun");
            float length = AnimatorUtils.GetAnimationClipLength(ani, "SunFlowerLight");
            m_CreateSun_TimerTask2 = TimerMgr.Ins.Register(length, onComplete: () =>
            {
                CreateSun();
            });
        }, loopCount: -1);
    }

    private void CreateSun()
    {
        GameMgr.Ins.SpawnFlowerSun(gameObject);
    }

    private void OnDestroy()
    {
        m_CreateSun_TimerTask?.Dispose();
        m_CreateSun_TimerTask2?.Dispose();
    }
}
