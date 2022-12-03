using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public BattlePhase currentPhase;

    public GameObject bossPrefab;
    public GameObject heroPrefab;


    public Transform bossTransform;
    public Transform hero1Transform;
    public Transform hero2Transform;
    public Transform hero3Transform;

    private BattleUnit _bossUnit;
    private BattleUnit _hero1Unit;
    private BattleUnit _hero2Unit;
    private BattleUnit _hero3Unit;

    public void Start()
    {
        currentPhase = BattlePhase.PLAYER_TURN;
        Init();
    }

    private void Init()
    {
        SetBattleUnit(_bossUnit, bossPrefab, bossTransform);
        SetBattleUnit(_hero1Unit, heroPrefab, hero1Transform);
        SetBattleUnit(_hero2Unit, heroPrefab, hero2Transform);
        SetBattleUnit(_hero3Unit, heroPrefab, hero3Transform);
    }

    private void SetBattleUnit(BattleUnit battleUnit, GameObject prefabToBeInstatiated, Transform transformToBeSet)
    {
        battleUnit = Instantiate(prefabToBeInstatiated, transformToBeSet).GetComponent<BattleUnit>();
    }
}

public enum BattlePhase
{
    PLAYER_TURN,
    BOSS_TURN,
    WIN,
    LOSE
}