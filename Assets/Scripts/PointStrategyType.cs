using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public interface IPointStrategyType
{
    void DoWhenClicked();

    void DoOnEnd();
}

public class NormalPoint : IPointStrategyType
{
    void IPointStrategyType.DoOnEnd()
    {
        int loot = Random.Range(10, 60);
        BattleControler.goldLoot = loot;
        Player.Instance.gold += loot;
    }

    void IPointStrategyType.DoWhenClicked()
    {
        Enemy[] enemies = Resources.LoadAll<Enemy>("ScriptableObject/NormalEnemy/");
        Monster monster = Resources.Load<Monster>("Pref/Monster");
        Monster.Instantiate(monster);
        Monster.Instance.Initialize(enemies[Random.Range(0, enemies.Length)]);
        SceneManager.LoadScene(2);
    }
}

public class ElitePoint : IPointStrategyType
{
    void IPointStrategyType.DoOnEnd()
    {
        int loot = Random.Range(60, 160);
        BattleControler.goldLoot = loot;
        Player.Instance.gold += loot;
    }

    void IPointStrategyType.DoWhenClicked()
    {
        Enemy[] enemies = Resources.LoadAll<Enemy>("ScriptableObject/EliteEnemy/");
        Monster monster = Resources.Load<Monster>("Pref/Monster");
        Monster.Instantiate(monster);
        Monster.Instance.Initialize(enemies[Random.Range(0, enemies.Length)]);
        SceneManager.LoadScene(2);
    }
}

public class ShopPoint : IPointStrategyType
{
    void IPointStrategyType.DoOnEnd()
    {
        throw new System.NotImplementedException();
    }

    void IPointStrategyType.DoWhenClicked()
    {
        Debug.Log("Sklep");
    }
}

public class RestPoint : IPointStrategyType
{
    void IPointStrategyType.DoOnEnd()
    {
        throw new System.NotImplementedException();
    }

    void IPointStrategyType.DoWhenClicked()
    {
        Player player = Player.Instance;
        player.HitPoint += player.MaxHitPoint / 3;
    }
}

public class BossPoint : IPointStrategyType
{
    void IPointStrategyType.DoOnEnd()
    {
        throw new System.NotImplementedException();
    }

    void IPointStrategyType.DoWhenClicked()
    {
        Debug.Log("Boss");
    }
}