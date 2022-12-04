using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Hero", menuName = "Units/Hero")]
public class BattleUnitSO : ScriptableObject
{
    private string Name;
    public int AttackPower;
    public int InitialHP;
    public int Level;
    public int ExperiencePoint;
    public Sprite UnitSprite;

    private void Awake()
    {
        Name = name;
    }
}