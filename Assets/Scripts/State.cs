using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    public Sprite spriteOfMove;
    public int moveQuantity;

    public State(Sprite xSprite, int xMoveQuantity)
    {
        spriteOfMove = xSprite;
        moveQuantity = xMoveQuantity;
    }

    public abstract void DoAction();
}

public class AttackState : State
{
    public AttackState(Sprite xSprite, int xMoveQuantity) : base(xSprite, xMoveQuantity)
    {
    }

    override
    public void DoAction()
    {
        moveQuantity += Monster.Instance.CurrBuff;
        int temp = Player.Instance.CurrShield;
        Player.Instance.CurrShield -= moveQuantity;
        moveQuantity -= temp;
        if (moveQuantity > 0)
        {
            Player.Instance.HitPoint -= moveQuantity;
        }
    }
}

public class BuffState : State
{
    public BuffState(Sprite xSprite, int xMoveQuantity) : base(xSprite, xMoveQuantity)
    {
    }

    override
    public void DoAction()
    {
        Monster.Instance.CurrBuff += moveQuantity;
    }
}

public class DebuffState : State
{
    public DebuffState(Sprite xSprite, int xMoveQuantity) : base(xSprite, xMoveQuantity)
    {
    }

    override
    public void DoAction()
    {
        Player.Instance.currBuff -= moveQuantity;
    }
}

public class HealState : State
{
    public HealState(Sprite xSprite, int xMoveQuantity) : base(xSprite, xMoveQuantity)
    {
    }

    override
    public void DoAction()
    {
        Monster.Instance.CurrHp += moveQuantity;
    }
}

public class ShieldState : State
{
    public ShieldState(Sprite xSprite, int xMoveQuantity) : base(xSprite, xMoveQuantity)
    {
    }

    override
    public void DoAction()
    {
        Monster.Instance.CurrShield += moveQuantity;
    }
}