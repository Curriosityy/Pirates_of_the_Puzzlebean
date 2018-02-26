using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "Enemy")]
public class Enemy : ScriptableObject
{
    public int[] dmgRange;
    public string enemyName;
    public int deff;
    public int startHP;
    public Sprite enemyLook;
    public int attackBuff;
    public int defBuff;
    public int attackDebuff;
    public int defDebuff;

    public int DoAtack()
    {
        return Random.Range(dmgRange[0], dmgRange[1]);
    }

    public int DoDefense()
    {
        return deff;
    }

    public int Buff(int typeOfBuff)
    {
        if (typeOfBuff == 0)
        {
            return defBuff;
        }
        if (typeOfBuff == 1)
        {
            return attackBuff;
        }
        return -1;
    }

    public int Debuff(int typeOfDebuff)
    {
        if (typeOfDebuff == 0)
        {
            return defDebuff;
        }
        if (typeOfDebuff == 1)
        {
            return attackDebuff;
        }
        return -1;
    }
}