using DG.Tweening;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class PlayerController : UnitySingleton<PlayerController>
{

    [SerializeField] public Transform WoodsTargetLocation;
    [SerializeField] public Transform MoneyTargetLocation;
    [SerializeField] private static List<Transform> CollectedWoods = new List<Transform>();
    [SerializeField] private List<Transform> CollectedMoney = new List<Transform>();
    [SerializeField] private List<Transform> WoodsPlaces = new List<Transform>();
    [SerializeField] private Transform StorageCabin;
    [SerializeField] private Transform CoinAnimIcon;
    [SerializeField] private Transform CoinAnimText;
    [SerializeField] private Transform GemEarnAnimText;
    [SerializeField] private Transform GemSpendAnimText;
    [SerializeField] private Transform GemEarnIconAnim;
    [SerializeField] private Transform stackUIText;
    private Transform mainCameraTransform;
    public static event Action<Transform> onMarketTrigger;
    public static event Action<Transform> onWoodStorageTrigger;
    public static event Action<string> UpgradesEvent;


    private float transferrate = 0;
    private int MaxCarryableWood = 10;

    private int CurrentWood = 0;
    private float YAxis = 0;
    private int Counter = 0;
    private static int CurrentGem;
    private static int CurrentCoin;
    private static float CurrentMoney;
    private static int CurrentStone;
    private static int CurrentCloth;
    private static int CurrentIron;
    private static int AxeLevel;
    private static int StackLevel;
    private void SetDatas()
    {
        GameManager.gameData.playerGem = CurrentGem;
        GameManager.gameData.playerCoin = CurrentCoin;
        GameManager.gameData.playerMoney = CurrentMoney;
        GameManager.gameData.playerStone = CurrentStone;
        GameManager.gameData.playerCloth = CurrentCloth;
        GameManager.gameData.playerIron = CurrentIron;
        GameManager.gameData.axeLevel = AxeLevel;
        GameManager.gameData.stackLevel = StackLevel;
        GameManager.gameData.maxCarryableWood = MaxCarryableWood;
    }
    private void LoadDatas()
    {
        CurrentGem = GameManager.gameData.playerGem;
        CurrentCoin = GameManager.gameData.playerCoin;
        CurrentMoney = GameManager.gameData.playerMoney;
        CurrentStone = GameManager.gameData.playerStone;
        CurrentCloth = GameManager.gameData.playerCloth;
        CurrentIron = GameManager.gameData.playerIron;
        AxeLevel = GameManager.gameData.axeLevel;
        StackLevel = GameManager.gameData.stackLevel;
        MaxCarryableWood = GameManager.gameData.maxCarryableWood;
    }
    private void Awake()
    {
        GameManager.setGameData += SetDatas;
        GameManager.loadGameData += LoadDatas;
    }
    private void Start()
    {
        mainCameraTransform = Camera.main.transform;
    }
    private void OnEnable()
    {
        ButtonManager.UpgradesEvent += IncreaseSkills;
        CollectableObjects.CollectWood += PickupWood;
    }
    private void OnDisable()
    {
        ButtonManager.UpgradesEvent -= IncreaseSkills;
        CollectableObjects.CollectWood -= PickupWood;
    }
    private void FixedUpdate()
    {
        UpdateStackText();
        if (CollectedWoods.Count > 0)
        {
            for (int i = 0; i < CollectedWoods.Count; i++)
            {
                if (CollectedWoods.ElementAt(i) != null && Vector3.Distance(CollectedWoods.ElementAt(i).position, WoodsTargetLocation.position + new Vector3(0, i * 0.37f, 0)) > 0.01f)
                {
                    CollectedWoods.ElementAt(i).position = Vector3.Lerp(CollectedWoods.ElementAt(i).position, WoodsTargetLocation.position + new Vector3(0, i * 0.37f, 0), Time.deltaTime * 4.2f);
                    CollectedWoods.ElementAt(i).rotation = Quaternion.Lerp(CollectedWoods.ElementAt(i).rotation, WoodsTargetLocation.rotation, Time.deltaTime * 5f);
                }
            }
        }
        if (CollectedMoney.Count > 0)
        {
            for (int i = CollectedMoney.Count - 1; i >= 0; i--)
            {
                if (CollectedMoney.ElementAt(i) != null && Vector3.Distance(CollectedMoney.ElementAt(i).position, MoneyTargetLocation.position) > 0.3f)
                {
                    CollectedMoney.ElementAt(i).position = Vector3.Lerp(CollectedMoney.ElementAt(i).position, MoneyTargetLocation.position, Time.fixedDeltaTime * 3.0f);
                    CollectedMoney.ElementAt(i).rotation = Quaternion.Lerp(CollectedMoney.ElementAt(i).rotation, MoneyTargetLocation.rotation, Time.deltaTime * 5f);
                }
                else if (CollectedMoney.ElementAt(i) != null)
                {
                    CollectedMoney.ElementAt(i).gameObject.SetActive(false);
                    CollectedMoney.ElementAt(i).localScale = new Vector3(0, 0, 0);
                    CollectedMoney.RemoveAt(i);
                }
            }
        }
        Debug.DrawRay(transform.GetChild(0).GetChild(0).position, transform.forward, Color.green);

        if (Physics.Raycast(transform.GetChild(0).GetChild(0).position, transform.forward, out var hit, 0.5f))
        {

            if (hit.collider.CompareTag("MarketTrigger") && CurrentWood > 0)
            {

                if (MarketManager.Instance.GetCurrentWoodOnMarket() < MarketManager.Instance.GetMarketCapacity())
                {

                    for (int i = CollectedWoods.Count - 1; i >= 0; i--)
                    {
                        if (MarketManager.Instance.GetCurrentWoodOnMarket() >= MarketManager.Instance.GetMarketCapacity())
                        {
                            break;
                        }
                        CollectedWoods[i].rotation = WoodsPlaces.ElementAt(Counter).rotation;
                        CollectedWoods[i].DOJump(new Vector3(WoodsPlaces.ElementAt(Counter).position.x, WoodsPlaces.ElementAt(Counter).position.y + YAxis, WoodsPlaces.ElementAt(Counter).position.z),
                        4f, 1, 0.2f).SetDelay(transferrate).SetEase(Ease.Flash);
                        onMarketTrigger?.Invoke(CollectedWoods.ElementAt(CurrentWood - 1));
                        CollectedWoods.RemoveAt(CurrentWood - 1);
                        CurrentWood--;
                        transferrate += 0.05f;
                        if (Counter >= 11)
                        {
                            Counter = 0;
                            YAxis += 0.5f;
                        }
                        else
                        {
                            Counter++;
                        }
                    }
                    transferrate = 0f;
                }

                if (MarketManager.Instance.getWorkingState() == false)
                {
                    hit.transform.parent.GetComponent<MarketManager>().Work();
                }
            }
            if (hit.collider.CompareTag("StorageTrigger") && CurrentWood > 0)
            {
                for (int i = CollectedWoods.Count - 1; i >= 0; i--)
                {
                    CollectedWoods[i].transform.DOJump(StorageCabin.position, 2f, 1, 0.2f).SetDelay(transferrate).SetEase(Ease.Flash);
                    onWoodStorageTrigger?.Invoke(CollectedWoods.ElementAt(CurrentWood - 1));
                    CollectedWoods.RemoveAt(CurrentWood - 1);
                    CurrentWood--;
                    transferrate += 0.05f;
                }
                transferrate = 0f;
                hit.transform.parent.GetComponent<PlayerWoodStorageManager>().Work();
            }
        }
    }

    private void UpdateStackText()
    {
        /* Vector3 lookDirection = (stackUIText.position - mainCameraTransform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
        stackUIText.rotation = lookRotation; */

        stackUIText.GetChild(0).GetComponent<TMP_Text>().text = CollectedWoods.Count + "/" + MaxCarryableWood;

    }
    void PickupWood(Transform Wood)
    {
        CollectedWoods.Add(Wood);
        CurrentWood++;
    }
    public void addMoney(Transform Money)
    {
        CollectedMoney.Add(Money);
    }

    private void IncreaseSkills(string skill)
    {
        if (skill == "Axe")
        {
            if (CurrentMoney >= AxeLevel * 12000 + 15000 && CurrentStone >= AxeLevel * 30 + 30)
            {
                SpendMoney(AxeLevel * 12000 + 15000);
                CurrentStone -= AxeLevel * 30 + 30;
                AxeLevel++;
                UpgradesEvent?.Invoke(skill);
                AudioManager.Instance.PlaySFX("UpgradeAxe");
            }
        }
        if (skill == "StackLimit")
        {
            if (CurrentMoney >= StackLevel * 12000 + 15000 && CurrentIron >= StackLevel * 10 + 10)
            {
                SpendMoney(StackLevel * 12000 + 15000);
                CurrentIron -= StackLevel * 10 + 10;
                StackLevel++;
                MaxCarryableWood += StackLevel * 10;
                UpgradesEvent?.Invoke(skill);
                AudioManager.Instance.PlaySFX("UpgradeStack");
            }
        }
        if (skill == "MoveSpeed")
        {
            int MoveSpeedLevel = PlayerMovement.Instance.GetMoveSpeedLevel();
            if (CurrentMoney >= MoveSpeedLevel * 12000 + 15000 && CurrentGem >= MoveSpeedLevel * 5 + 10 && skill == "MoveSpeed")
            {
                SpendMoney(MoveSpeedLevel * 12000 + 15000);
                SpendGem(MoveSpeedLevel * 5 + 10);
                PlayerMovement.Instance.IncreaseMoveSpeed("MoveSpeed");
            }
        }
    }
    public void Trades(string material, int value)
    {
        if (material == "Stone")
        {
            PlayerWoodStorageManager.Instance.SpendWoodOnTrader(material, value);
            CurrentStone += value;
            AudioManager.Instance.PlaySFX("TradeStone");

        }
        if (material == "Iron")
        {
            int[] IronCost = TradeCompany.Instance.getIronCost();
            CurrentStone -= value * IronCost[1];
            PlayerWoodStorageManager.Instance.SpendWoodOnTrader(material, value);
            CurrentIron += value;
            AudioManager.Instance.PlaySFX("TradeIron");
        }
        if (material == "Cloth")
        {
            float ClothCost = TradeCompany.Instance.getClothCost();
            SpendMoney(ClothCost * value);
            CurrentCloth += value;
        }
    }
    public void RoadManTrades(string material, int value)
    {
        if (material == "SellCoin")
        {
            float CoinPrice = RoadManManager.Instance.getCoinPrice();
            spendCoin(value);
            earnMoney(value * CoinPrice);
        }
        if (material == "SellGem")
        {
            float GemPrice = RoadManManager.Instance.getGemPrice();
            SpendGem(value);
            earnMoney(value * GemPrice);
        }
        if (material == "TradeGem")
        {
            int GemCost = RoadManManager.Instance.getGemCost();
            EarnGem(value);
            spendCoin(value * GemCost);
        }
    }
    public void useCloth(int x) => CurrentCloth -= x;
    public void useStone(int x) => CurrentStone -= x;
    public void useIron(int x) => CurrentIron -= x;
    public int getMaxCarryableWood() => MaxCarryableWood;
    public int getCurrentWood() => CurrentWood;
    public int getCurrentStone() => CurrentStone;
    public int getCurrentCoin() => CurrentCoin;
    public int getAxeLevel() => AxeLevel;
    public float getCurrentMoney() => CurrentMoney;
    public int getCurrentCloth() => CurrentCloth;
    public int getCurrentGem() => CurrentGem;
    public int getCurrentIron() => CurrentIron;
    public int getStackLevel() => StackLevel;
    public void earnMoney(float x)
    {
        var Text = ObjectPooler.Instance.getEarnTextFromPool();

        if (Text != null)
        {
            Text.GetComponent<TMP_Text>().text = "+" + Mathf.RoundToInt(x).ToString();
            Text.SetActive(true);
        }
        CurrentMoney += x;
        GameManager.Instance.IncreaseEarnedMoney(x);
        AudioManager.Instance.PlaySFX("EarnMoney");
    }
    public void SpendMoney(float x)
    {
        var Text = ObjectPooler.Instance.getSpendTextFromPool();
        if (Text != null)
        {
            Text.GetComponent<TMP_Text>().text = "-" + Mathf.RoundToInt(x).ToString();
            Text.SetActive(true);
        }
        CurrentMoney -= x;
        AudioManager.Instance.PlaySFX("SpendMoney");
    }

    public void earnCoin(int x)
    {
        var CoinText = ObjectPooler.Instance.getCoinAnimTextFromPool();
        var CoinIcon = ObjectPooler.Instance.getCoinIconAnimFromPool();
        if (CoinIcon != null && CoinText != null)
        {
            CoinText.transform.position = CoinAnimText.position;
            CoinText.transform.rotation = CoinAnimText.rotation;
            CoinIcon.transform.position = CoinAnimIcon.position;
            CoinIcon.transform.rotation = CoinAnimIcon.rotation;

            CoinText.GetComponent<TMP_Text>().text = "+" + x.ToString();
            CoinText.GetComponent<TMP_Text>().color = Color.green;
            CoinIcon.SetActive(true);
            CoinText.SetActive(true);
        }
        CurrentCoin += x;
    }
    public void spendCoin(int x)
    {
        var CoinText = ObjectPooler.Instance.getCoinAnimTextFromPool();
        var CoinIcon = ObjectPooler.Instance.getCoinIconAnimFromPool();

        if (CoinIcon != null && CoinText != null)
        {
            CoinText.transform.position = CoinAnimText.position;
            CoinText.transform.rotation = CoinAnimText.rotation;
            CoinIcon.transform.position = CoinAnimIcon.position;
            CoinIcon.transform.rotation = CoinAnimIcon.rotation;

            CoinText.GetComponent<TMP_Text>().text = "-" + x.ToString();
            CoinText.GetComponent<TMP_Text>().color = Color.red;
            CoinIcon.SetActive(true);
            CoinText.SetActive(true);
        }
        CurrentCoin -= x;
    }

    public void EarnGem(int x)
    {
        var GemEarnText = ObjectPooler.Instance.getGemEarnAnimTextFromPool();
        var GemEarnIcon = ObjectPooler.Instance.getGemIconAnimFromPool();
        if (GemEarnText != null && GemEarnIcon != null)
        {
            GemEarnText.transform.position = GemEarnAnimText.position;
            GemEarnText.transform.rotation = GemEarnAnimText.rotation;
            GemEarnIcon.transform.position = GemEarnIconAnim.position;
            GemEarnIcon.transform.rotation = GemEarnIconAnim.rotation;

            GemEarnText.GetComponent<TMP_Text>().text = "+" + x.ToString();
            GemEarnText.SetActive(true);
            GemEarnIcon.SetActive(true);
        }
        CurrentGem += x;
        AudioManager.Instance.PlaySFX2("EarnGem");
    }
    public void SpendGem(int x)
    {
        var GemSpendText = ObjectPooler.Instance.getGemSpendAnimTextFromPool();
        if (GemSpendText != null)
        {
            GemSpendText.transform.position = GemEarnAnimText.position;
            GemSpendText.transform.rotation = GemEarnAnimText.rotation;
            GemSpendText.GetComponent<TMP_Text>().text = "-" + x.ToString();
            GemSpendText.SetActive(true);
        }
        CurrentGem -= x;
    }
    public void decreaseCounter()
    {
        Counter--;
        if (Counter < 0)
        {
            Counter = 11;
            YAxis -= 0.5f;
        }
    }
}
