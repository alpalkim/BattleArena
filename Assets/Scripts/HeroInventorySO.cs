using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "Units/Inventory")]
public class HeroInventorySO : ScriptableObject
{
    public List<HeroUnitSO> BattleUnitObjects;
    private int _totalFightCount = 0;
    private int _nextUnlockableHeroIndex = 3;   // Since we are already giving 3 heroes as unlocked initially, index of the next hero to be unlocked is 3.

    public void IncreaseFightCount()
    {
        _totalFightCount++;

        if (_totalFightCount % GlobalSettings.RequiredFightCountToUnlockNewHero == 0)
            UnlockNewHero();
    }

    private void UnlockNewHero()
    {
        if (BattleUnitObjects[_nextUnlockableHeroIndex] == null) return;
        
        BattleUnitObjects[_nextUnlockableHeroIndex].UnlockUnit();
        _nextUnlockableHeroIndex++;
    }
}