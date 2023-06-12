using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu]
public class GameConfig : ScriptableObject
{
    public static GameConfig Get()
    {
        return Resources.Load<GameConfig>("GameConfig");
    }

    public int moneyPerSun;
    public int initMoney;
    public List<PlantConfig> plantConfigs;
    public List<EnemyConfig> enemyConfigs;
    public float[] enemySpawnPosY;
}

[Serializable]
public class PlantConfig
{
    public Sprite canUse;
    public Sprite cantUse;
    public float cooldownTime;
    public GameObject prefab;
    public int price;

    public int hp;
    public int damage;
    public float attackTI;
    public int[] param1_int;
}

[Serializable]
public class EnemyConfig
{
    public GameObject prefab;
    public int totalHp;
    public int[] stageHp;
    public float attackTI;
    public int damage;
    public float moveSpeed;
}
