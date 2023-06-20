using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;
using UnityEngine.UI;
public class FactoryMissionManager : UnitySingleton<FactoryMissionManager>
{

    [SerializeField] private GameObject missionPanel;
    [SerializeField] private TMP_Text chairAmountText;
    [SerializeField] private TMP_Text tableAmountText;
    [SerializeField] private TMP_Text sofaAmountText;
    [SerializeField] private TMP_Text wardrobeAmountText;
    [SerializeField] private TMP_Text earningAmountText;
    [SerializeField] private TMP_Text missionDurationText;
    [SerializeField] private Text ChangeText;
    [SerializeField] private Text changeCostText;
    [SerializeField] private GameObject sellButon;
    [SerializeField] private GameObject sellButonBackground;
    [SerializeField] private GameObject image1;
    [SerializeField] private GameObject image2;
    [SerializeField] private GameObject changeButon;
    [SerializeField] private GameObject[] completeLines; // Chair-Table-Sofa-Wardrobe
    private int missionRank;
    private float missionEarning;
    private int changeCost = 20;
    private int missionDuration = 600;
    private float chairSellPrice = 600f;
    private float tableSellPrice = 2150f;
    private float wardrobeSellPrice = 3750f;
    private float sofaSellPrice = 7200f;
    private int[] MissionCosts = new int[4]; // Chair-Table-Sofa-Wardrobe

    private void SetDatas()
    {
        GameManager.gameData.missionCosts[0] = MissionCosts[0];
        GameManager.gameData.missionCosts[1] = MissionCosts[1];
        GameManager.gameData.missionCosts[2] = MissionCosts[2];
        GameManager.gameData.missionCosts[3] = MissionCosts[3];
        GameManager.gameData.factoryMissionEarning = missionEarning;
        GameManager.gameData.missionDuration = missionDuration;
        GameManager.gameData.missionRank = missionRank;

    }
    private void LoadDatas()
    {
        MissionCosts[0] = GameManager.gameData.missionCosts[0];
        MissionCosts[1] = GameManager.gameData.missionCosts[1];
        MissionCosts[2] = GameManager.gameData.missionCosts[2];
        MissionCosts[3] = GameManager.gameData.missionCosts[3];
        missionEarning = GameManager.gameData.factoryMissionEarning;
        missionDuration = GameManager.gameData.missionDuration;
        missionRank = GameManager.gameData.missionRank;
    }
    private void Awake()
    {
        GameManager.setGameData += SetDatas;
        GameManager.loadGameData += LoadDatas;
    }

    private void Start()
    {
        changeMission("saved");
        updateTextValues();
        updateCompleteness();
        updateChangability();

    }
    private void OnEnable()
    {
        LoadDatas();
        changeMission("saved");
        ButtonManager.FactoryMissionMenuEvent += missionMenu;
        ButtonManager.ChangeFactoryMissionMenuEvent += changeMission;
    }
    private void OnDisable()
    {
        ButtonManager.FactoryMissionMenuEvent -= missionMenu;
        ButtonManager.ChangeFactoryMissionMenuEvent -= changeMission;
    }
    private void missionMenu(string sign)
    {
        if (sign == "Open") missionPanel.gameObject.SetActive(true);
        if (sign == "Close") missionPanel.gameObject.SetActive(false);
        updateTextValues();
        updateCompleteness();
        updateChangability();
        int rand = UnityEngine.Random.Range(1, 3);
        if (rand == 1)
        {
            image1.SetActive(true);
            image2.SetActive(false);
        }
        if (rand == 2)
        {
            image2.SetActive(true);
            image1.SetActive(false);
        }


    }
    IEnumerator getMission(string option)
    {
        if (option != "saved")
        {
            giveNewMission();
            missionDuration = 600;
        }
        updateTextValues();
        updateCompleteness();
        updateChangability();

        for (int i = missionDuration; i >= 0; i--)
        {
            missionDurationText.text = string.Format("{0:00}:{1:00}", i / 60, i % 60);
            missionDuration = i;
            yield return new WaitForSeconds(1f);
        }
        if (!FactoryManager.Instance.getTruckState()) changeMission("free");
    }
    public void changeMission(string option)
    {
        // 0 noMatter
        if (option == "saved")
        {
            StartCoroutine("getMission", "saved");
        }
        if (option == "free")
        {
            StopAllCoroutines();
            StartCoroutine("getMission", "0");
        }
        if (option == "gem")
        {
            PlayerController.Instance.SpendGem(changeCost);
            StopAllCoroutines();
            StartCoroutine("getMission", "0");
        }
        updateTextValues();
        updateCompleteness();
        updateChangability();
    }
    public void updateChangability()
    {
        if (PlayerController.Instance.getCurrentGem() >= changeCost && FactoryManager.Instance.getTruckState() == false)
        {
            changeButon.GetComponent<Button>().interactable = true;
            changeButon.GetComponent<Image>().fillAmount = 1;
            ChangeText.color = Color.white;
            changeCostText.color = Color.white;
        }
        else
        {
            changeButon.GetComponent<Button>().interactable = false;
            changeButon.GetComponent<Image>().fillAmount = 0;
            ChangeText.color = Color.red;
            changeCostText.color = Color.red;
        }
    }

    private bool updateAffordability()
    {
        int counter = 0;
        int[] productsOnFactory = new int[MissionCosts.Length];
        productsOnFactory = FactoryManager.Instance.getProductsArray();

        for (int i = 0; i < productsOnFactory.Length; i++)
        {
            if (productsOnFactory[i] >= MissionCosts[i] && MissionCosts[i] != -1)
            {

                counter++;
            }
        }

        if (counter == missionRank) return true;
        else return false;
    }

    public void updateCompleteness()
    {
        if (!FactoryManager.Instance.getTruckState())
        {
            int[] productsOnFactory = new int[MissionCosts.Length];
            productsOnFactory = FactoryManager.Instance.getProductsArray();
            for (int i = 0; i < MissionCosts.Length; i++)
            {
                if (productsOnFactory[i] >= MissionCosts[i] && MissionCosts[i] != -1)
                {
                    completeLines[i].SetActive(true);
                }
                else completeLines[i].SetActive(false);
            }
            if (updateAffordability())
            {
                sellButon.GetComponent<Button>().interactable = true;
                sellButon.GetComponent<Image>().fillAmount = 1;
            }
            else
            {
                sellButon.GetComponent<Button>().interactable = false;
                sellButon.GetComponent<Image>().fillAmount = 0;
            }
        }
    }

    private void updateTextValues()
    {
        chairAmountText.text = " CHAIR X" + MissionCosts[0].ToString();
        tableAmountText.text = "TABLE X" + MissionCosts[1].ToString();
        sofaAmountText.text = "SOFA X" + MissionCosts[2].ToString();
        wardrobeAmountText.text = "WARDROBE X" + MissionCosts[3].ToString();
        earningAmountText.text = missionEarning.ToString();
        changeCostText.text = changeCost.ToString();

        for (int i = 0; i < MissionCosts.Length; i++)
        {
            if (MissionCosts[i] == -1)
            {
                switch (i)
                {
                    case 0:
                        chairAmountText.text = "NO ORDER";
                        break;
                    case 1:
                        tableAmountText.text = "NO ORDER";
                        break;
                    case 2:
                        sofaAmountText.text = "NO ORDER";
                        break;
                    case 3:
                        wardrobeAmountText.text = "NO ORDER";
                        break;
                }
            }
        }
    }
    private void CalculateEarning()
    {
        float earning = 0;

        for (int i = 0; i < 4; i++)
        {
            if (MissionCosts[i] != -1)
            {
                switch (i)
                {
                    case 0:
                        earning += MissionCosts[0] * chairSellPrice;

                        break;
                    case 1:
                        earning += MissionCosts[1] * tableSellPrice;

                        break;
                    case 2:
                        earning += MissionCosts[2] * sofaSellPrice;

                        break;
                    case 3:
                        earning += MissionCosts[3] * wardrobeSellPrice;

                        break;
                }
            }
        }
        int bonus = missionRank * UnityEngine.Random.Range(2000, 6000);
        missionEarning = earning + bonus;
    }
    private void giveNewMission()
    {
        int rand = UnityEngine.Random.Range(1, 101);

        int[] checkArray = new int[4] { 9, 9, 9, 9 };
        int rand2 = 9;


        if (rand > 0 && 0 >= rand)
        {
            missionRank = 4;

            for (int i = 0; i < MissionCosts.Length; i++)
            {
                MissionCosts[i] = UnityEngine.Random.Range(1, 11);
            }

        }
        if (rand > 10 && 30 >= rand)
        {
            missionRank = 3;

            for (int i = 0; i < 3; i++)
            {
                while (checkArray.Contains(rand2))
                {
                    rand2 = UnityEngine.Random.Range(0, 4);
                }
                checkArray[i] = rand2;
                MissionCosts[rand2] = UnityEngine.Random.Range(1, 11);
            }

            for (int i = 0; i < checkArray.Length; i++)
            {
                if (!checkArray.Contains(i))
                {
                    MissionCosts[i] = -1;
                }
            }
        }
        if (rand > 30 && 60 >= rand)
        {
            missionRank = 2;

            for (int i = 0; i < 2; i++)
            {
                while (checkArray.Contains(rand2))
                {
                    rand2 = UnityEngine.Random.Range(0, 4);
                }
                checkArray[i] = rand2;
                MissionCosts[rand2] = UnityEngine.Random.Range(1, 11);
            }

            for (int i = 0; i < checkArray.Length; i++)
            {
                if (!checkArray.Contains(i))
                {
                    MissionCosts[i] = -1;
                }
            }
        }
        if (rand > 60 && 100 >= rand)
        {
            missionRank = 1;

            rand2 = UnityEngine.Random.Range(0, 4);
            checkArray[rand2] = rand2;
            MissionCosts[rand2] = UnityEngine.Random.Range(1, 11);

            for (int i = 0; i < checkArray.Length; i++)
            {
                if (!checkArray.Contains(i))
                {
                    MissionCosts[i] = -1;
                }
            }
        }
        CalculateEarning();
    }
    public int[] getMissionCosts() => MissionCosts;
    public float getMissionEarning() => missionEarning;

}
