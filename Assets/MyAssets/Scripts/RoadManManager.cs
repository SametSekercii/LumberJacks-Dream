using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoadManManager : UnitySingleton<RoadManManager>
{
    private GameObject RoadManPanel;
    [SerializeField] private List<Transform> WayPoints = new List<Transform>();
    [SerializeField] private TMP_Text CoinValueText;
    [SerializeField] private TMP_Text GemValueText;
    [SerializeField] private TMP_Text SellCoin_CoinText;
    [SerializeField] private TMP_Text SellCoin_MoneyText;
    [SerializeField] private TMP_Text SellGem_GemText;
    [SerializeField] private TMP_Text SellGem_MoneyText;
    [SerializeField] private TMP_Text TradeGem_GemText;
    [SerializeField] private TMP_Text TradeGem_CoinText;
    [SerializeField] private GameObject SellCoinGreenButton;
    [SerializeField] private GameObject SellCoinRedButton;
    [SerializeField] private GameObject SellGemGreenButton;
    [SerializeField] private GameObject SellGemRedButton;
    [SerializeField] private GameObject TradeGemGreenButton;
    [SerializeField] private GameObject TradeGemRedButton;
    [SerializeField] private GameObject missionButton;
    private Animator roadManAnim;
    private string[] goods = { "SellCoin", "SellGem", "TradeGem" };
    private int SellCoinValue = 1;
    private int SellGemValue = 1;
    private int TradeGemValue = 1;
    private float CoinPrice = 500;
    private float GemPrice = 3500;
    private int GemCost = 10;
    private Transform RoadMan;
    private int WayPointAmount;
    private int TargetIndex;
    Vector3 RoadMandir;
    Vector3 TargetPosition;
    Quaternion lookRotation;


    private void Start()
    {
        roadManAnim = transform.GetChild(1).GetChild(0).GetComponent<Animator>();
        RoadManPanel = transform.GetChild(0).GetChild(0).gameObject;
        RoadMan = transform.GetChild(1);
        WayPointAmount = WayPoints.Count;
        TargetIndex = 0;
        StartCoroutine("Patrol");
    }
    private void OnEnable()
    {
        ButtonManager.RoadManMenuEvent += RoadManMenu;
        ButtonManager.RoadManTrades += Trades;
    }

    private void OnDisable()
    {
        ButtonManager.RoadManMenuEvent -= RoadManMenu;
        ButtonManager.RoadManTrades -= Trades;
    }
    private void RoadManMenu(string sign)
    {
        if (sign == "Open")
        {
            RoadManPanel.gameObject.SetActive(true);
            missionButton.gameObject.SetActive(false);
            roadManAnim.SetBool("isTrading", true);
            StopAllCoroutines();

        }
        if (sign == "Close")
        {
            RoadManPanel.gameObject.SetActive(false);
            if (GameManager.factory == buildState.Solded) missionButton.gameObject.SetActive(true);
            roadManAnim.SetBool("isTrading", false);
            StartCoroutine("Patrol");

        }
        UpdateValues();
        for (int i = 0; i < goods.Length; i++)
        {
            ChangeStatus(goods[i]);
        }
    }

    private void Trades(string material, string sign)
    {
        if (material == "SellCoin")
        {
            if (sign == "+")
            {
                SellCoinValue++;
                ChangeStatus(material);
            }
            if (sign == "-" && SellCoinValue > 1)
            {
                SellCoinValue--;
                ChangeStatus(material);
            }
            if (sign == "Green")
            {
                PlayerController.Instance.RoadManTrades(material, SellCoinValue);
                ChangeStatus(material);
                UpdateValues();
            }
        }
        if (material == "SellGem")
        {
            if (sign == "+")
            {
                SellGemValue++;
                ChangeStatus(material);
            }
            if (sign == "-" && SellGemValue > 1)
            {
                SellGemValue--;
                ChangeStatus(material);
            }
            if (sign == "Green")
            {
                PlayerController.Instance.RoadManTrades(material, SellGemValue);
                ChangeStatus(material);
                UpdateValues();
            }
        }
        if (material == "TradeGem")
        {
            if (sign == "+")
            {
                TradeGemValue++;
                ChangeStatus(material);
            }
            if (sign == "-" && TradeGemValue > 1)
            {
                TradeGemValue--;
                ChangeStatus(material);
            }
            if (sign == "Green")
            {
                PlayerController.Instance.RoadManTrades(material, TradeGemValue);
                ChangeStatus(material);
                UpdateValues();
            }
        }
    }


    IEnumerator Patrol()
    {
        while (true)
        {
            TargetPosition = WayPoints[TargetIndex].position;
            RoadMandir = (TargetPosition - RoadMan.position).normalized;
            lookRotation = Quaternion.LookRotation(RoadMandir);

            while (Vector3.Distance(RoadMan.position, TargetPosition) > 0.1f)
            {
                RoadMan.rotation = Quaternion.Lerp(RoadMan.rotation, lookRotation, Time.deltaTime * 3f);
                RoadMan.position += RoadMandir * 2f * Time.deltaTime;
                yield return null;
            }
            TargetIndex++;
            if (TargetIndex == WayPointAmount) TargetIndex = 0;

            yield return null;
        }
    }

    private void UpdateValues()
    {
        CoinValueText.text = PlayerController.Instance.getCurrentCoin().ToString();
        GemValueText.text = PlayerController.Instance.getCurrentGem().ToString();
    }
    private void ChangeStatus(string material)
    {
        if (material == "SellCoin")
        {
            int CurrentCoin = PlayerController.Instance.getCurrentCoin();
            SellCoin_CoinText.text = SellCoinValue.ToString();
            SellCoin_MoneyText.text = Mathf.RoundToInt(CoinPrice * SellCoinValue).ToString();

            if (CurrentCoin >= SellCoinValue)
            {
                SellCoinGreenButton.SetActive(true);
                SellCoinRedButton.SetActive(false);
            }
            else
            {
                SellCoinGreenButton.SetActive(false);
                SellCoinRedButton.SetActive(true);
            }
        }
        if (material == "SellGem")
        {
            int CurrentGem = PlayerController.Instance.getCurrentGem();
            SellGem_GemText.text = SellGemValue.ToString();
            SellGem_MoneyText.text = Mathf.RoundToInt(GemPrice * SellGemValue).ToString();

            if (CurrentGem >= SellGemValue)
            {
                SellGemGreenButton.SetActive(true);
                SellGemRedButton.SetActive(false);
            }
            else
            {
                SellGemGreenButton.SetActive(false);
                SellGemRedButton.SetActive(true);
            }
        }
        if (material == "TradeGem")
        {
            int CurrentCoin = PlayerController.Instance.getCurrentCoin();
            TradeGem_GemText.text = TradeGemValue.ToString();
            TradeGem_CoinText.text = (TradeGemValue * GemCost).ToString();

            if (CurrentCoin >= TradeGemValue * GemCost)
            {
                TradeGemGreenButton.SetActive(true);
                TradeGemRedButton.SetActive(false);
            }
            else
            {
                TradeGemGreenButton.SetActive(false);
                TradeGemRedButton.SetActive(true);
            }
        }

    }

    public float getCoinPrice() => CoinPrice;
    public float getGemPrice() => GemPrice;
    public int getGemCost() => GemCost;




}
