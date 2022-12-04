using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "Units/Inventory")]
public class BattleUnitInventorySO : ScriptableObject
{
    public List<BattleUnitSO> BattleUnitObjects;
}