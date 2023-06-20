using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TradeCompany : UnitySingleton<TradeCompany>
{
    private GameObject TradePanel;
    [SerializeField] private TMP_Text MoneyValueText;
    [SerializeField] private TMP_Text GemValueText;
    [SerializeField] private TMP_Text WoodValueText;
    [SerializeField] private TMP_Text StoneValueText;
    [SerializeField] private TMP_Text IronValueText;
    [SerializeField] private TMP_Text ClothValueText;
    [SerializeField] private TMP_Text TradeStoneCostText;
    [SerializeField] private TMP_Text TradeClothCostText;
    [SerializeField] private TMP_Text TradeIronCostWoodText;
    [SerializeField] private TMP_Text TradeIronCostStoneText;
    [SerializeField] private TMP_Text TradeStoneValueText;
    [SerializeField] private TMP_Text TradeIronValueText;
    [SerializeField] private TMP_Text TradeClothValueText;
    [SerializeField] private GameObject StoneGreenButton;
    [SerializeField] private GameObject StoneRedButton;
    [SerializeField] private GameObject IronGreenButton;
    [SerializeField] private GameObject IronRedButton;
    [SerializeField] private GameObject ClothGreenButton;
    [SerializeField] private GameObject ClothRedButton;
    [SerializeField] private GameObject missionButton;
    private string[] Goods = { "Stone", "Iron", "Cloth" };
    private int TradeStoneCost = 2;
    private int[] TradeIronCost = { 3, 2 };
    private float TradeClothCost = 500;
    private int TradeStoneValue = 1;
    private int TradeIronValue = 1;
    private int TradeClothValue = 1;
    private void OnEnable()
    {
        for (int i = 0; i < Goods.Length; i++)
        {
            UpdateInventory(Goods[i]);
            ChangeStatus(Goods[i]);
        }
        ButtonManager.TradeMenuEvent += TradeMenu;
        ButtonManager.TradesEvent += Trades;
    }
    private void OnDisable()
    {
        ButtonManager.TradeMenuEvent += TradeMenu;
        ButtonManager.TradesEvent -= Trades;
    }
    private void Start()
    {
        TradePanel = transform.GetChild(0).GetChild(0).gameObject;
    }
    private void TradeMenu(string sign)
    {
        if (sign == "Open")
        {
            TradePanel.SetActive(true);
            missionButton.gameObject.SetActive(false);

        }
        if (sign == "Close")
        {
            TradePanel.SetActive(false);
            if (GameManager.factory == buildState.Solded) missionButton.gameObject.SetActive(true);

        }

        for (int i = 0; i < Goods.Length; i++)
        {
            UpdateInventory(Goods[i]);
            ChangeStatus(Goods[i]);
        }
    }
    private void Trades(string material, string sign)
    {
        if (material == "Stone")
        {
            if (sign == "+")
            {
                TradeStoneValue++;
                ChangeStatus(material);
            }
            if (sign == "-" && TradeStoneValue > 1)
            {
                TradeStoneValue--;
                ChangeStatus(material);
            }
            if (sign == "Green")
            {
                PlayerController.Instance.Trades(material, TradeStoneValue);
                UpdateInventory(material);
                ChangeStatus(material);
            }
        }
        if (material == "Iron")
        {
            if (sign == "+")
            {
                TradeIronValue++;
                ChangeStatus(material);
            }
            if (sign == "-" && TradeIronValue > 1)
            {
                TradeIronValue--;
                ChangeStatus(material);
            }
            if (sign == "Green")
            {
                PlayerController.Instance.Trades(material, TradeIronValue);
                UpdateInventory(material);
                ChangeStatus(material);
            }

        }
        if (material == "Cloth")
        {
            if (sign == "+")
            {
                TradeClothValue++;
                ChangeStatus(material);
            }
            if (sign == "-" && TradeClothValue > 1)
            {
                TradeClothValue--;
                ChangeStatus(material);
            }
            if (sign == "Green")
            {
                PlayerController.Instance.Trades(material, TradeClothValue);
                UpdateInventory(material);
                ChangeStatus(material);
            }

        }

    }

    private void UpdateInventory(string material)
    {
        if (material == "Stone")
        {
            StoneValueText.text = PlayerController.Instance.getCurrentStone().ToString();
            WoodValueText.text = PlayerWoodStorageManager.Instance.getCurrentWood().ToString();
        }
        if (material == "Iron")
        {
            StoneValueText.text = PlayerController.Instance.getCurrentStone().ToString();
            WoodValueText.text = PlayerWoodStorageManager.Instance.getCurrentWood().ToString();
            IronValueText.text = PlayerController.Instance.getCurrentIron().ToString();
        }
        if (material == "Cloth")
        {
            MoneyValueText.text = Mathf.RoundToInt(PlayerController.Instance.getCurrentMoney()).ToString();
            ClothValueText.text = PlayerController.Instance.getCurrentCloth().ToString();
        }
        GemValueText.text = PlayerController.Instance.getCurrentGem().ToString();


    }
    private void ChangeStatus(string material)
    {
        int PlayerCurrentWood = PlayerWoodStorageManager.Instance.getCurrentWood();
        TradeStoneValueText.text = TradeStoneValue.ToString();
        TradeStoneCostText.text = (TradeStoneValue * TradeStoneCost).ToString();
        if ((TradeStoneValue * TradeStoneCost) <= PlayerCurrentWood)
        {
            StoneGreenButton.SetActive(true);
            StoneRedButton.SetActive(false);
        }
        else
        {
            StoneGreenButton.SetActive(false);
            StoneRedButton.SetActive(true);
        }
        int PlayerCurrentStone = PlayerController.Instance.getCurrentStone();
        TradeIronValueText.text = TradeIronValue.ToString();
        TradeIronCostWoodText.text = (TradeIronCost[0] * TradeIronValue).ToString();
        TradeIronCostStoneText.text = (TradeIronCost[1] * TradeIronValue).ToString();
        if ((TradeIronValue * TradeIronCost[0]) <= PlayerCurrentWood && PlayerCurrentStone >= (TradeIronValue * TradeIronCost[1]))
        {
            IronGreenButton.SetActive(true);
            IronRedButton.SetActive(false);
        }
        else
        {
            IronGreenButton.SetActive(false);
            IronRedButton.SetActive(true);
        }
        float PlayerCurrentMoney = PlayerController.Instance.getCurrentMoney();
        TradeClothValueText.text = TradeClothValue.ToString();
        TradeClothCostText.text = (TradeClothValue * TradeClothCost).ToString();
        if ((TradeClothValue * TradeClothCost) <= PlayerCurrentMoney)
        {
            ClothGreenButton.SetActive(true);
            ClothRedButton.SetActive(false);
        }
        else
        {
            ClothGreenButton.SetActive(false);
            ClothRedButton.SetActive(true);
        }
    }

    public int getStoneCost()
    {
        return TradeStoneCost;
    }
    public int[] getIronCost()
    {
        return TradeIronCost;
    }
    public float getClothCost()
    {
        return TradeClothCost;
    }
}
