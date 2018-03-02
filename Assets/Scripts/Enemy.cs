using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "Enemy")]
public class Enemy : ScriptableObject
{
    public string enemyName;
    public Sprite enemyLook;
    public int[] dmgRange;
    public int startHP;
    public int deff;
    public int buff;
    public int debuff;
    public int healQuant;

    [Tooltip("0-attack,1-shield,2-heal,3-buff,4-debuff")]
    public float[] moveProb;
}