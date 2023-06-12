using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolMgr : MonoSingleton<PoolMgr>
{
    public GameObjectPool sunPool;
    public GameObjectPool peaBulletPool;
    public GameObjectPool iceBulletPool;

    public void Init()
    {
        GameObject poolRoot = new GameObject("PoolRoot");
        sunPool = ObjectPoolMgr.Ins.GetOrCreateGameObjectPool(Resources.Load<GameObject>("Prefabs/Sun"), 3, poolRoot.transform);
        peaBulletPool = ObjectPoolMgr.Ins.GetOrCreateGameObjectPool(Resources.Load<GameObject>("Prefabs/PeaBullet"), 20, poolRoot.transform);
        iceBulletPool = ObjectPoolMgr.Ins.GetOrCreateGameObjectPool(Resources.Load<GameObject>("Prefabs/IceBullet"), 20, poolRoot.transform);
    }
}
