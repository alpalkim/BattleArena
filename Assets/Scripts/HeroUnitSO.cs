using UnityEngine;

[CreateAssetMenu(fileName = "New Hero", menuName = "Units/Hero")]
public class HeroUnitSO : BattleUnitSO
{
    [SerializeField] private int Level;
    [SerializeField] private int ExperiencePoint;
    [SerializeField] private bool IsLocked;
    public bool IsSelected;
    
    // Get Attributes
    public int GetLevel() => Level;
    public int GetExperiencePoint() => ExperiencePoint;
    public bool IsUnitLocked() => IsLocked;
    
    
    // Set Attributes
    public void UnlockUnit() => IsLocked = false;
    public void IncreaseAttackPower(int increaseAmount) => AttackPower += increaseAmount;
    public void IncreaseHP(int increaseAmount) => InitialHP += increaseAmount;
    public void IncreaseLevel() => Level++;
    public void IncreaseExperiencePoint() => ExperiencePoint++;
}