using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{

    public int playerGem;
    public float playerMoney;
    public int playerWood;
    public int playerCoin;
    public int playerStone;
    public int playerIron;
    public int playerCloth;
    public int playerChair;
    public int playerTable;
    public int playerSofa;
    public int playerWardrobe;
    public int axeLevel;
    public int stackLevel;
    public int maxCarryableWood;
    public int moveSpeedLevel;
    public int playTime;
    //mission process save
    public float factoryMissionEarning;
    public int[] missionCosts;
    public int missionDuration;
    public int missionRank;
    //
    public int totalCuttedTree;
    public float totalEarnedMoney;
    public int totalCollectedWood;
    public int totalCollectedGem;
    public int totalProducedChair;
    public int totalProducedTable;
    public int totalProducedWardrobe;
    public int totalProducedSofa;
    //factory process save
    public int chairProductionValue;
    public int tableProductionValue;
    public int sofaProductionValue;
    public int wardrobeProductionValue;
    public float chairProductionTimer;
    public float tableProductionTimer;
    public float sofaProductionTimer;
    public float wardrobeProductionTimer;
    public GoodsStates ChairState, TableState, WardrobeState, SofaState;
    public buildState lumberjackHouse;
    public buildState trade;
    public buildState factory;
    public buildState miner1;
    public buildState miner2;
    public buildState miner3;
    public buildState miner4;
    public GameData()
    {
        playerGem = 50000;
        playerMoney = 5000000;
        playerWood = 50000;
        playerCoin = 50000;
        playerStone = 50000;
        playerIron = 50000;
        playerCloth = 50000;
        playerChair = 0;
        playerTable = 0;
        playerSofa = 0;
        playerWardrobe = 0;
        axeLevel = 0;
        stackLevel = 0;
        maxCarryableWood = 10;
        moveSpeedLevel = 0;
        playTime = 0;
        //mission process save
        missionCosts = new int[4] { 4, -1, -1, -1 };
        factoryMissionEarning = 7800;
        missionDuration = 600;
        missionRank = 1;
        //
        totalCuttedTree = 0;
        totalEarnedMoney = 0;
        totalCollectedWood = 0;
        totalCollectedGem = 0;
        totalProducedChair = 0;
        totalProducedTable = 0;
        totalProducedWardrobe = 0;
        totalProducedSofa = 0;
        //factory process save
        chairProductionValue = 0;
        tableProductionValue = 0;
        sofaProductionValue = 0;
        wardrobeProductionValue = 0;
        chairProductionTimer = 0;
        tableProductionTimer = 0;
        sofaProductionTimer = 0;
        wardrobeProductionTimer = 0;
        ChairState = GoodsStates.notProducing;
        TableState = GoodsStates.notProducing;
        WardrobeState = GoodsStates.notProducing;
        SofaState = GoodsStates.notProducing;
        //Builds
        lumberjackHouse = buildState.onSale;
        factory = buildState.onSale;
        trade = buildState.onSale;
        miner1 = buildState.onSale;
        miner2 = buildState.onSale;
        miner3 = buildState.onSale;
        miner4 = buildState.onSale;
        //
    }


}
