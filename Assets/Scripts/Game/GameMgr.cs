using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : MonoSingleton<GameMgr>
{
    public GameConfig config;

    public GameObject m_ItemGo;

    private int money;
    public int GetMoney()
    {
        return money;
    }

    private GroundGrid m_LastGroundGrid;
    public void UpdateGroundGrid(GroundGrid curGroundGrid)
    {
        if (curGroundGrid == m_LastGroundGrid)
        {
            return;
        }
        if (m_LastGroundGrid != null)
        {
            m_LastGroundGrid.UpdateSelectFrame(false);
        }
        if (curGroundGrid != null)
        {
            curGroundGrid.UpdateSelectFrame(true);
        }
        m_LastGroundGrid = curGroundGrid;
    }

    public void Init()
    {
        config = GameConfig.Get();

        PoolMgr.Ins.Init();
        SpawnGroundGrid();
        GetMoney(config.initMoney);

        TimerMgr.Ins.Register(2, onComplete: () =>
        {
            SpawnSkySun();
        }, loopCount: -1);
        InvokeRepeating("SpawnEnemy", 2, 1);
    }

    public void GetMoney(int cost)
    {
        money += cost;
        MsgSystem.Dispatch(MsgConst.UpdateMoney, money);
    }

    public void Start()
    {

    }

    public void SpawnSkySun()
    {
        GameObject sunGo = PoolMgr.Ins.sunPool.Get();
        float minX = -3.5f;
        float maxX = 2.5f;
        sunGo.transform.position = new Vector2(Random.Range(minX, maxX), 3.5f);
        sunGo.GetComponent<Sun>().Init(true, -2f);
    }

    public void SpawnFlowerSun(GameObject flower)
    {
        GameObject sunGo = PoolMgr.Ins.sunPool.Get();
        sunGo.transform.position = new Vector2(flower.transform.position.x + Random.Range(-0.5f, 0.5f), flower.transform.position.y);
        sunGo.GetComponent<Sun>().Init(false);
    }

    private void SpawnGroundGrid()
    {
        Transform root = GameObject.Find("GroundGrid").transform;

        for (int col = 0; col < 9; col++)
        {
            for (int row = 0; row < 5; row++)
            {
                GameObject gridGroundGo = Instantiate(Resources.Load<GameObject>("Prefabs/GroundGrid"));
                gridGroundGo.transform.SetParent(root, false);
                gridGroundGo.transform.localPosition = new Vector3(col * 0.82f, row * 1);
                gridGroundGo.name = $"{row}-{col}";
                gridGroundGo.tag = "GroundGrid";
            }
        }
    }

    private void SpawnEnemy()
    {
        int count = Random.Range(1, 10);
        for (int i = 0; i < count; i++)
        {
            EnemyConfig tempEnemyConfig = config.enemyConfigs[Random.Range(0, config.enemyConfigs.Count)];
            GameObject enemyGo = Instantiate(tempEnemyConfig.prefab);
            enemyGo.GetComponent<Enemy>().Init(tempEnemyConfig);
            enemyGo.transform.position = new Vector3(5, config.enemySpawnPosY[Random.Range(0, config.enemySpawnPosY.Length)]);
        }
    }
}
