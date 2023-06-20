using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeHouse : MonoBehaviour
{
    private GameObject UpgradePanel;
    [SerializeField] private TMP_Text AxeUpgradeCostText;
    [SerializeField] private TMP_Text AxeUpgradeStoneCostText;
    [SerializeField] private Transform AxeUpgradeButton;
    [SerializeField] private TMP_Text StackUpgradeCostText;
    [SerializeField] private TMP_Text StackUpgradeIronCostText;
    [SerializeField] private Transform StackUpgradeButton;
    [SerializeField] private TMP_Text MoveSpeedUpgradeCostText;
    [SerializeField] private TMP_Text MoveSpeedUpgradeGemCostText;
    [SerializeField] private Transform MoveSpeedUpgradeButton;
    [SerializeField] private TMP_Text TotalMoneyText;
    [SerializeField] private TMP_Text AxeUpgradeText;
    [SerializeField] private TMP_Text StackLimitUpgradeText;
    [SerializeField] private TMP_Text MoveSpeedUpgradeText;
    [SerializeField] private GameObject missionButton;
    private string[] skills = { "Axe", "StackLimit", "MoveSpeed" };
    private float AxeUpgradeCost;
    private float StackUpgradeCost;
    private float MoveSpeedUpgradeCost;
    private float AxeUpgradeStoneCost;
    private float StackUpgradeIronCost;
    private float MoveSpeedUpgradeGemCost;

    private void OnEnable()
    {

        StartCoroutine("PlaceCirclesOnEnable");
        PlayerController.UpgradesEvent += UpdateCircles;
        PlayerMovement.UpgradesEvent += UpdateCircles;
        PlayerController.UpgradesEvent += UpdateUpgradeCosts;
        PlayerMovement.UpgradesEvent += UpdateUpgradeCosts;
        ButtonManager.UpgradeMenuEvent += UpgradeMenu;
    }
    private void OnDisable()
    {
        PlayerController.UpgradesEvent -= UpdateCircles;
        PlayerMovement.UpgradesEvent -= UpdateCircles;
        PlayerController.UpgradesEvent -= UpdateUpgradeCosts;
        PlayerMovement.UpgradesEvent -= UpdateUpgradeCosts;
        ButtonManager.UpgradeMenuEvent -= UpgradeMenu;
    }
    void Start()
    {
        UpgradePanel = transform.GetChild(0).GetChild(0).gameObject;
    }
    private void UpgradeMenu(string sign)
    {
        if (sign == "Open")
        {
            UpgradePanel.SetActive(true);
            missionButton.gameObject.SetActive(false);

        }
        if (sign == "Close")
        {
            UpgradePanel.SetActive(false);
            if (GameManager.factory == buildState.Solded) missionButton.gameObject.SetActive(true);


        }

        for (int i = 0; i < skills.Length; i++)
        {
            UpdateUpgradeCosts(skills[i]);
        }
        CheckUpgradability();
    }

    private void UpdateUpgradeCosts(string skill)
    {
        if (skill == "Axe")
        {
            AxeUpgradeCost = PlayerController.Instance.getAxeLevel() * 12000 + 15000;
            AxeUpgradeStoneCost = PlayerController.Instance.getAxeLevel() * 30 + 30;
            AxeUpgradeCostText.text = Mathf.RoundToInt(AxeUpgradeCost).ToString();
            AxeUpgradeStoneCostText.text = AxeUpgradeStoneCost.ToString();
        }
        if (skill == "StackLimit")
        {
            StackUpgradeCost = PlayerController.Instance.getStackLevel() * 12000 + 15000;
            StackUpgradeIronCost = PlayerController.Instance.getStackLevel() * 10 + 10;
            StackUpgradeCostText.text = Mathf.RoundToInt(StackUpgradeCost).ToString();
            StackUpgradeIronCostText.text = StackUpgradeIronCost.ToString();
        }
        if (skill == "MoveSpeed")
        {
            MoveSpeedUpgradeCost = PlayerMovement.Instance.GetMoveSpeedLevel() * 12000 + 15000;
            MoveSpeedUpgradeGemCost = PlayerMovement.Instance.GetMoveSpeedLevel() * 5 + 10;
            MoveSpeedUpgradeCostText.text = Mathf.RoundToInt(MoveSpeedUpgradeCost).ToString();
            MoveSpeedUpgradeGemCostText.text = MoveSpeedUpgradeGemCost.ToString();
        }
        CheckUpgradability();
        TotalMoneyText.text = Mathf.RoundToInt(PlayerController.Instance.getCurrentMoney()).ToString();
    }
    private void UpdateCircles(string skill)
    {
        if (skill == "Axe")
        {
            int AxeLevel = PlayerController.Instance.getAxeLevel();
            AxeUpgradeButton.GetChild(AxeLevel).gameObject.SetActive(true);
        }
        if (skill == "StackLimit")
        {
            int StackLevel = PlayerController.Instance.getStackLevel();
            StackUpgradeButton.GetChild(StackLevel).gameObject.SetActive(true);
        }
        if (skill == "MoveSpeed")
        {
            int MoveSpeedLevel = PlayerMovement.Instance.GetMoveSpeedLevel();
            MoveSpeedUpgradeButton.GetChild(MoveSpeedLevel).gameObject.SetActive(true);
        }
    }
    private void CheckUpgradability()
    {
        int AxeLevel = PlayerController.Instance.getAxeLevel();
        int StackLevel = PlayerController.Instance.getStackLevel();
        int MoveSpeedLevel = PlayerMovement.Instance.GetMoveSpeedLevel();
        float CurrentMoney = PlayerController.Instance.getCurrentMoney();
        int CurrentGem = PlayerController.Instance.getCurrentGem();
        int CurrentIron = PlayerController.Instance.getCurrentIron();
        int CurrentStone = PlayerController.Instance.getCurrentStone();
        if (PlayerController.Instance.getAxeLevel() == 5)
        {
            AxeUpgradeCostText.text = "-";
            AxeUpgradeStoneCostText.text = "-";
            AxeUpgradeText.color = Color.green;
            AxeUpgradeText.text = "MAX LEVEL";

        }
        else
        {
            if (CurrentMoney >= AxeUpgradeCost && CurrentStone >= AxeUpgradeStoneCost)
            {
                AxeUpgradeText.color = Color.green;
            }

            else
            {
                AxeUpgradeText.color = Color.red;
            }
        }
        if (PlayerController.Instance.getStackLevel() == 5)
        {
            StackUpgradeCostText.text = "-";
            StackUpgradeIronCostText.text = "-";
            StackLimitUpgradeText.color = Color.green;
            StackLimitUpgradeText.text = "MAX LEVEL";
        }
        else
        {
            if (CurrentMoney >= StackUpgradeCost && CurrentIron >= StackUpgradeIronCost)
            {
                StackLimitUpgradeText.color = Color.green;
            }
            else
            {
                StackLimitUpgradeText.color = Color.red;
            }
        }
        if (PlayerMovement.Instance.GetMoveSpeedLevel() == 5)
        {
            MoveSpeedUpgradeCostText.text = "-";
            MoveSpeedUpgradeGemCostText.text = "-";
            MoveSpeedUpgradeText.color = Color.green;
            MoveSpeedUpgradeText.text = "MAX LEVEL";
        }
        else
        {
            if (CurrentMoney >= MoveSpeedUpgradeCost && CurrentGem >= MoveSpeedUpgradeGemCost)
            {
                MoveSpeedUpgradeText.color = Color.green;
            }
            else
            {
                MoveSpeedUpgradeText.color = Color.red;
            }
        }
    }

    IEnumerator PlaceCirclesOnEnable()
    {
        int AxeLevel = PlayerController.Instance.getAxeLevel();
        int StackLevel = PlayerController.Instance.getStackLevel();
        int MoveSpeedLevel = PlayerMovement.Instance.GetMoveSpeedLevel();
        for (int i = 1; i <= AxeLevel; i++)
        {
            AxeUpgradeButton.GetChild(i).gameObject.SetActive(true);

        }
        for (int i = 1; i <= StackLevel; i++)
        {
            StackUpgradeButton.GetChild(i).gameObject.SetActive(true);

        }
        for (int i = 1; i <= MoveSpeedLevel; i++)
        {
            MoveSpeedUpgradeButton.GetChild(i).gameObject.SetActive(true);
        }
        yield return null;
    }





}
