using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ship")]
public class Character : ScriptableObject
{
    //public string name;
    public int startHitPoint;

    public int shipEnergy;
    public Sprite sprite;
}