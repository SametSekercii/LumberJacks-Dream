using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public enum buildState { Solded, onSale }
public class GameManager : UnitySingleton<GameManager>
{

    public static GameData gameData;
    public static event Action setGameData;
    public static event Action loadGameData;
    private int playTime;
    private int TotalCuttedTree;
    private int TotalCollectedGem = 0;
    private float TotalEarnedMoney = 0;
    private int TotalCollectedWood = 0;
    private int TotalProducedChair, TotalProducedTable, TotalProducedWardrobe, TotalProducedSofa = 0;
    public static buildState lumberjackHouse;
    public static buildState trade;
    public static buildState factory;
    public static buildState miner1;
    public static buildState miner2;
    public static buildState miner3;
    public static buildState miner4;
    [SerializeField] private GameObject[] builds;
    [SerializeField] private TMP_Text TotalCuttedTreeTreeText;
    [SerializeField] private TMP_Text TotalCollectedGemText;
    [SerializeField] private TMP_Text TotalEarnedMoneyText;
    [SerializeField] private TMP_Text TotalCollectedWoodText;
    [SerializeField] private TMP_Text TotalProducedChairText;
    [SerializeField] private TMP_Text TotalProducedTableText;
    [SerializeField] private TMP_Text TotalProducedWardrobeText;
    [SerializeField] private TMP_Text TotalProducedSofaText;
    [SerializeField] private TMP_Text CurretMoneyText;
    [SerializeField] private TMP_Text CurrentGemText;
    [SerializeField] private TMP_Text CurrentWoodText;
    [SerializeField] private TMP_Text CurrentCoinText;
    [SerializeField] private TMP_Text playCountText;

    private void SetDatas()
    {
        gameData.totalCuttedTree = TotalCuttedTree;
        gameData.totalCollectedGem = TotalCollectedGem;
        gameData.totalEarnedMoney = TotalEarnedMoney;
        gameData.totalCollectedWood = TotalCollectedWood;
        gameData.totalProducedChair = TotalProducedChair;
        gameData.totalProducedTable = TotalProducedTable;
        gameData.totalProducedWardrobe = TotalProducedWardrobe;
        gameData.totalProducedSofa = TotalProducedSofa;
        gameData.lumberjackHouse = lumberjackHouse;
        gameData.trade = trade;
        gameData.factory = factory;
        gameData.miner1 = miner1;
        gameData.miner2 = miner2;
        gameData.miner3 = miner3;
        gameData.miner4 = miner4;
        gameData.playTime = playTime;
    }
    private void LoadDatas()
    {
        TotalCuttedTree = gameData.totalCuttedTree;
        TotalCollectedGem = gameData.totalCollectedGem;
        TotalEarnedMoney = gameData.totalEarnedMoney;
        TotalCollectedWood = gameData.totalCollectedWood;
        TotalProducedChair = gameData.totalProducedChair;
        TotalProducedTable = gameData.totalProducedTable;
        TotalProducedWardrobe = gameData.totalProducedWardrobe;
        TotalProducedSofa = gameData.totalProducedSofa;
        lumberjackHouse = gameData.lumberjackHouse;
        trade = gameData.trade;
        factory = gameData.factory;
        miner1 = gameData.miner1;
        miner2 = gameData.miner2;
        miner3 = gameData.miner3;
        miner4 = gameData.miner4;
        playTime = gameData.playTime;
    }
    void Start()
    {
        loadGameData += LoadDatas;
        setGameData += SetDatas;
        gameData = new GameData();
        //gameData = SaveSystem.Load(gameData);
        setBuildsStates();
        StartCoroutine("CountPlayTime");
        AudioManager.Instance.PlayMusic("PlaytimeAmbiance");
    }


    void Update()
    {
        setGameData?.Invoke();
        SaveSystem.Save(gameData);
        UpdateUIMaterials();
    }
    private void UpdateUIMaterials()
    {
        CurretMoneyText.text = Mathf.RoundToInt(PlayerController.Instance.getCurrentMoney()).ToString();
        CurrentGemText.text = PlayerController.Instance.getCurrentGem().ToString();
        CurrentWoodText.text = PlayerWoodStorageManager.Instance.getCurrentWood().ToString();
        CurrentCoinText.text = PlayerController.Instance.getCurrentCoin().ToString();
    }

    public void UpdateStatistic()
    {
        TotalCuttedTreeTreeText.text = TotalCuttedTree.ToString();
        TotalCollectedGemText.text = TotalCollectedGem.ToString();
        TotalEarnedMoneyText.text = Mathf.RoundToInt(TotalEarnedMoney).ToString();
        TotalCollectedWoodText.text = TotalCollectedWood.ToString();
        TotalProducedChairText.text = TotalProducedChair.ToString();
        TotalProducedTableText.text = TotalProducedTable.ToString();
        TotalProducedWardrobeText.text = TotalProducedWardrobe.ToString();
        TotalProducedSofaText.text = TotalProducedSofa.ToString();
    }
    public void IncreaseCuttedTree(int x) => TotalCuttedTree += x;
    public void IncreaceCollecedGem(int x) => TotalCollectedGem += x;
    public void IncreaseEarnedMoney(float x) => TotalEarnedMoney += x;
    public void InceraseCollectedWood(int x) => TotalCollectedWood += x;
    public void IncreaseProducedGoods(string goods, int productNumber)
    {
        if (goods == "Chair")
        {
            TotalProducedChair += productNumber;
        }
        if (goods == "Table")
        {
            TotalProducedTable += productNumber;
        }
        if (goods == "Wardrobe")
        {
            TotalProducedWardrobe += productNumber;
        }
        if (goods == "Sofa")
        {
            TotalProducedSofa += productNumber;
        }

    }
    private void setBuildsStates()
    {
        loadGameData?.Invoke();

        if (lumberjackHouse == buildState.Solded)
        {
            builds[0].SetActive(false);
            builds[1].SetActive(true);
        }
        else
        {
            builds[0].SetActive(true);
            builds[1].SetActive(false);

        }
        if (factory == buildState.Solded)
        {
            builds[2].SetActive(false);
            builds[3].SetActive(true);
        }
        else
        {
            builds[2].SetActive(true);
            builds[3].SetActive(false);

        }
        if (trade == buildState.Solded)
        {
            builds[4].SetActive(false);
            builds[5].SetActive(true);
        }
        else
        {
            builds[4].SetActive(true);
            builds[5].SetActive(false);

        }

        if (miner1 == buildState.Solded)
        {
            builds[6].SetActive(false);
            builds[7].SetActive(true);
        }
        else
        {
            builds[6].SetActive(true);
            builds[7].SetActive(false);

        }
        if (miner2 == buildState.Solded)
        {
            builds[8].SetActive(false);
            builds[9].SetActive(true);
        }
        else
        {
            builds[8].SetActive(true);
            builds[9].SetActive(false);
        }
        if (miner3 == buildState.Solded)
        {
            builds[10].SetActive(false);
            builds[11].SetActive(true);
        }
        else
        {
            builds[10].SetActive(true);
            builds[11].SetActive(false);

        }
        if (miner4 == buildState.Solded)
        {
            builds[12].SetActive(false);
            builds[13].SetActive(true);
        }
        else
        {
            builds[12].SetActive(true);
            builds[13].SetActive(false);

        }
        loadGameData?.Invoke();
    }

    IEnumerator CountPlayTime()
    {
        playCountText.text = string.Format("{0:00}h:{1:00}m", playTime / 60, playTime % 60);
        while (true)
        {
            yield return new WaitForSeconds(60);
            playTime++;
            playCountText.text = string.Format("{0:00}h:{1:00}m", playTime / 60, playTime % 60);

        }

    }

}
