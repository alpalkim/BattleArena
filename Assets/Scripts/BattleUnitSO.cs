using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Hero", menuName = "Units/Hero")]
public class BattleUnitSO : ScriptableObject
{
    private string Name;
    [SerializeField] private int AttackPower;
    [SerializeField] private int InitialHP;
    [SerializeField] private int Level;
    [SerializeField] private int ExperiencePoint;
    [SerializeField] private Sprite UnitSprite;
    public bool IsSelected;
    [SerializeField] private bool IsLocked;

    private void Awake()
    {
        Name = name;    //Set scriptable object's name as attribute, can be changed to custom name if needed.. 
    }

    // Get Attributes
    public int GetAttackPower() => AttackPower;
    public int GetHP() => InitialHP;
    public int GetLevel() => Level;
    public int GetExperiencePoint() => ExperiencePoint;
    public Sprite GetUnitSprite() => UnitSprite;
    public bool IsUnitLocked() => IsLocked;
    
    
    // Set Attributes
    public void UnlockUnit() => IsLocked = false;
    public void IncreaseAttackPower(float increaseMultiplier) => AttackPower = (int) (AttackPower * increaseMultiplier);
    public void IncreaseHP(float increaseMultiplier) => InitialHP = (int) (InitialHP * increaseMultiplier);
    public void IncreaseLevel() => Level++;
    public void IncreaseExperiencePoint() => ExperiencePoint++;
}