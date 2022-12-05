using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "Units/Inventory")]
public class BattleUnitInventorySO : ScriptableObject
{
    public List<BattleUnitSO> BattleUnitObjects;
    private int _totalFightCount;

    public void IncreaseFightCount()
    {
        _totalFightCount++;
        
        if (_totalFightCount % 5 == 0) 
            UnlockNewHero();
    }

    private void UnlockNewHero()
    {
        for (int i = 0; i < BattleUnitObjects.Count; i++)
        {
            if (BattleUnitObjects[i].IsLocked)
            {
                BattleUnitObjects[i].IsLocked = false;
                break;
            }
        }
    }
}