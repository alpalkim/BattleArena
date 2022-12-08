using UnityEngine;

public abstract class BattleUnitSO : ScriptableObject
{
    [SerializeField] protected int AttackPower;
    [SerializeField] protected int InitialHP;
    [SerializeField] protected Sprite UnitSprite;

    // Get Attributes
    public int GetAttackPower() => AttackPower;
    public int GetHP() => InitialHP;
    public Sprite GetUnitSprite() => UnitSprite;
}