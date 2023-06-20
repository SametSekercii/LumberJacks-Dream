using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManager : UnitySingleton<ButtonManager>
{
    public static event Action<string> UpgradesEvent;
    public static event Action<string, string> TradesEvent;
    public static event Action<string> UpgradeMenuEvent;
    public static event Action<string> TradeMenuEvent;
    public static event Action<string> FactoryMenuEvent;
    public static event Action<string> FactoryMissionMenuEvent;
    public static event Action<string> ChangeFactoryMissionMenuEvent;
    public static event Action<string> UpdateFactoryProductAmount;
    public static event Action<string, int, bool> ProduceGoodsEvent;
    public static event Action<string, string> RoadManTrades;
    public static event Action<string> RoadManMenuEvent;
    public static event Action SellGoods;
    public GameObject PausePanel;
    public GameObject SettingsPanel;
    public GameObject MainMenuPanel;
    public GameObject StaticticPanel;
    public GameObject InfoPanel;
    public GameObject CreditPanel;
    private bool isGamePaused = false;
    public void Upgrades(string skill)
    {
        if (skill == "Axe")
        {
            if (PlayerController.Instance.getAxeLevel() < 5) UpgradesEvent?.Invoke(skill);
        }
        if (skill == "StackLimit")
        {
            if (PlayerController.Instance.getStackLevel() < 5) UpgradesEvent?.Invoke(skill);
        }
        if (skill == "MoveSpeed")
        {
            if (PlayerMovement.Instance.GetMoveSpeedLevel() < 5) UpgradesEvent?.Invoke(skill);
        }
    }
    public void ProduceGoods(string goods)
    {
        int productNumber = FactoryManager.Instance.getProductNumber();
        ProduceGoodsEvent?.Invoke(goods, productNumber, false);

    }
    public void TradeStone(string sign)
    {
        TradesEvent?.Invoke("Stone", sign);
    }
    public void TradeIron(string sign)
    {
        TradesEvent?.Invoke("Iron", sign);
    }
    public void TradeCloth(string sign)
    {
        TradesEvent?.Invoke("Cloth", sign);
    }
    public void SellCoin(string sign)
    {
        RoadManTrades?.Invoke("SellCoin", sign);
    }
    public void SellGem(string sign)
    {
        RoadManTrades?.Invoke("SellGem", sign);
    }
    public void TradeGem(string sign)
    {
        RoadManTrades?.Invoke("TradeGem", sign);
    }

    public void UpgradeMenu(string sign)
    {
        UpgradeMenuEvent?.Invoke(sign);
    }
    public void TradeMenu(string sign)
    {
        TradeMenuEvent?.Invoke(sign);
    }
    public void FactoryMenu(string sign)
    {
        FactoryMenuEvent?.Invoke(sign);
    }
    public void FactoryMissionMenu(string sign)
    {
        FactoryMissionMenuEvent?.Invoke(sign);
    }
    public void RoadManMenu(string sign)
    {
        RoadManMenuEvent?.Invoke(sign);
    }
    public void ChangeProductNumber(string sign)
    {
        UpdateFactoryProductAmount?.Invoke(sign);
    }
    public void ChangeFactoryMission(string sign)
    {
        ChangeFactoryMissionMenuEvent?.Invoke(sign);
    }
    public void SendTruck()
    {
        SellGoods?.Invoke();
    }
    public void PauseExitButton(string Panel)
    {
        if (Panel == "Pause")
        {
            if (!isGamePaused)
            {
                Time.timeScale = 0f;
                PausePanel.SetActive(true);
                isGamePaused = true;
                return;
            }
            if (isGamePaused)
            {
                PausePanel.SetActive(false);
                isGamePaused = false;
                Time.timeScale = 1f;
                return;
            }
        }
        if (Panel == "Statistic")
        {
            StaticticPanel.SetActive(false);
        }
        if (Panel == "Settings")
        {
            SettingsPanel.SetActive(false);
            isGamePaused = false;
            Time.timeScale = 1f;
        }
        if (Panel == "Info")
        {
            InfoPanel.SetActive(false);
        }
        if (Panel == "Credit")
        {
            CreditPanel.SetActive(false);
        }
    }
    public void SettingsButton()
    {
        PausePanel.SetActive(false);
        SettingsPanel.SetActive(true);
    }
    public void TurnBackMainMenu()
    {
        Time.timeScale = 0f;
        AudioManager.Instance.musicSource.Pause();
        SceneManager.LoadScene(1);


    }
    public void StartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(2);
        AudioManager.Instance.musicSource.Play();

    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void StatisticButton()
    {
        GameManager.Instance.UpdateStatistic();
        StaticticPanel.SetActive(true);
    }
    public void InfoButton()
    {
        InfoPanel.SetActive(true);
    }


}
