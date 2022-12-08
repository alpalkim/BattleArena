using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "Units/Inventory")]
public class HeroInventorySO : ScriptableObject
{
    public List<HeroUnitSO> BattleUnitObjects;
    private int _totalFightCount = 0;

    public void IncreaseFightCount()
    {
        _totalFightCount++;

        if (_totalFightCount % GlobalSettings.RequiredFightCountToUnlockNewHero == 0)
            UnlockNewHero();
    }

    private void UnlockNewHero()
    {
        for (int i = 0; i < BattleUnitObjects.Count; i++)   // To unlock next hero that is not unlocked yet.
        {
            if (BattleUnitObjects[i].IsUnitLocked())
            {
                BattleUnitObjects[i].UnlockUnit();
                break;
            }
        }
    }
}