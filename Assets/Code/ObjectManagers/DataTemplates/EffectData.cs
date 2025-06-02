using System;
[Serializable]
public struct StatusEffect 
{
    public StatusEffectType statusEffectType;
    public float force;
    public int ticks;
}


public enum StatusEffectType
{
    damage,
    heal,
    damageOT, //dmg over time
    stan
}