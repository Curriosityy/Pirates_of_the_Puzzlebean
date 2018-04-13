using System.Collections.Generic;

[System.Serializable]
public enum Rarity
{
    common, uncommon, rare, epic
}

[System.Serializable]
public class Items
{
    public List<Item> temp;

    public Item ItemFactory(Item item)
    {
        switch (item.id)
        {
            case 1:
                return new PurpleApple(item);

            case 2:
                return new MagicCannonball(item);

            case 3:
                return new GoldChest(item);

            case 4:
                return new MagicShield(item);

            case 5:
                return new MagicSail(item);

            case 6:
                return new Kebab(item);

            case 7:
                return new HotDog(item);

            case 8:
                return new SuperHull(item);

            case 9:
                return new SuperKebab(item);

            case 10:
                return new Purse(item);

            case 11:
                return new Monsoon(item);

            case 12:
                return new Telescope(item);

            case 13:
                return new Nurse(item);

            case 14:
                return new GoldenApple(item);

            case 15:
                return new DonerKebab(item);

            case 16:
                return new Oar(item);
        }
        return null;
    }
}

[System.Serializable]
public class Item
{
    public int id;
    public string description;
    public Rarity rarity;
    public string name;
    public string spritePath;
    public int tick = -1;

    public Item()
    {
    }

    public Item(Item item)
    {
        id = item.id;
        description = item.description;
        rarity = item.rarity;
        name = item.name;
        spritePath = item.spritePath;
    }

    public virtual void DoOnPickUp()
    {
    }

    public virtual void DoOnBattleStart()
    {
    }

    public virtual void DoOnEveryMove()
    {
    }

    public virtual void DoOnBattleEnd()
    {
    }
}

public class PurpleApple : Item
{
    public PurpleApple(Item item) : base(item)
    {
    }

    public override void DoOnBattleEnd()
    {
        Player.Instance.HitPoint += 8;
    }
}

public class MagicCannonball : Item
{
    public MagicCannonball(Item item) : base(item)
    {
    }

    public override void DoOnEveryMove()
    {
        Monster.Instance.CurrHp -= 1;
    }
}

public class GoldChest : Item
{
    public GoldChest(Item item) : base(item)
    {
    }

    public override void DoOnPickUp()
    {
        Player.Instance.gold += 600;
    }
}

public class MagicShield : Item
{
    public MagicShield(Item item) : base(item)
    {
    }

    public override void DoOnEveryMove()
    {
        Player.Instance.CurrShield += 2;
    }
}

public class MagicSail : Item
{
    public MagicSail(Item item) : base(item)
    {
    }

    public override void DoOnBattleStart()
    {
        Player.Instance.currShipEnergy += 1;
    }
}

public class Kebab : Item
{
    public Kebab(Item item) : base(item)
    {
    }

    public override void DoOnPickUp()
    {
        Player.Instance.HitPoint += 30;
    }
}

public class HotDog : Item
{
    public HotDog(Item item) : base(item)
    {
    }

    public override void DoOnBattleStart()
    {
        Player.Instance.currBuff += 1;
    }
}

public class SuperHull : Item
{
    public SuperHull(Item item) : base(item)
    {
    }

    public override void DoOnBattleStart()
    {
        Player.Instance.CurrShield += 10;
    }
}

public class SuperKebab : Item
{
    public SuperKebab(Item item) : base(item)
    {
    }

    public override void DoOnPickUp()
    {
        Player.Instance.MaxHitPoint = 15;
    }
}

public class Purse : Item
{
    public Purse(Item item) : base(item)
    {
    }

    public override void DoOnBattleEnd()
    {
        Player.Instance.gold += 80;
    }
}

public class Monsoon : Item
{
    public Monsoon(Item item) : base(item)
    {
        tick = 0;
    }

    public override void DoOnEveryMove()
    {
        tick += 1;
        if (tick >= 6)
        {
            Player.Instance.currShipEnergy += 1;
            tick = 0;
        }
    }
}

public class Telescope : Item
{
    public Telescope(Item item) : base(item)
    {
        tick = 0;
    }

    public override void DoOnEveryMove()
    {
        tick += 1;
        if (tick >= 8)
        {
            Player.Instance.currBuff += 1;
            tick = 0;
        }
    }
}

public class Nurse : Item
{
    public Nurse(Item item) : base(item)
    {
        tick = 0;
    }

    public override void DoOnEveryMove()
    {
        tick += 1;
        if (tick >= 8)
        {
            Player.Instance.HitPoint += 6;
            tick = 0;
        }
    }
}

public class GoldenApple : Item
{
    public GoldenApple(Item item) : base(item)
    {
    }

    public override void DoOnBattleEnd()
    {
        Player.Instance.HitPoint += 20;
    }
}

public class DonerKebab : Item
{
    public DonerKebab(Item item) : base(item)
    {
    }

    public override void DoOnPickUp()
    {
        Player.Instance.MaxHitPoint = 50;
    }
}

public class Oar : Item
{
    public Oar(Item item) : base(item)
    {
    }

    public override void DoOnPickUp()
    {
        Player.Instance.ShipEnergy += 1;
    }
}