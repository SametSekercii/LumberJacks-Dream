using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum GoodsStates { Producing, notProducing }
public class FactoryManager : UnitySingleton<FactoryManager>
{
    [SerializeField] private GameObject missionButton;
    [SerializeField] private GameObject missionManager;
    [SerializeField] private GameObject FactoryPanel;
    [SerializeField] private TMP_Text WoodValueText;
    [SerializeField] private TMP_Text StoneValueText;
    [SerializeField] private TMP_Text IronValueText;
    [SerializeField] private TMP_Text ClothValueText;
    [SerializeField] private Image ChairCooldownBar;
    [SerializeField] private TMP_Text ChairCooldownText;
    [SerializeField] private GameObject ProductChairButton;
    [SerializeField] private Image TableCooldownBar;
    [SerializeField] private TMP_Text TableCooldownText;
    [SerializeField] private GameObject ProductTableButton;
    [SerializeField] private Image SofaCooldownBar;
    [SerializeField] private TMP_Text SofaCooldownText;
    [SerializeField] private GameObject ProductSofaButton;
    [SerializeField] private Image WardrobeCooldownBar;
    [SerializeField] private TMP_Text WardrobeCooldownText;
    [SerializeField] private GameObject ProductWardrobeButton;
    [SerializeField] private TMP_Text productNumberText;
    [SerializeField] private TMP_Text ChairCostText;
    [SerializeField] private TMP_Text TableWoodCostText;
    [SerializeField] private TMP_Text TableIronCostText;
    [SerializeField] private TMP_Text WardrobeWoodText;
    [SerializeField] private TMP_Text WardrobeStoneCostText;
    [SerializeField] private TMP_Text WardrobeIronCostText;
    [SerializeField] private TMP_Text SofaWoodCostText;
    [SerializeField] private TMP_Text SofaClothCostText;
    [SerializeField] private TMP_Text SofaIronCostText;
    [SerializeField] private TMP_Text TotalChairText;
    [SerializeField] private TMP_Text TotalTableText;
    [SerializeField] private TMP_Text TotalWardrobeText;
    [SerializeField] private TMP_Text TotalSofaText;
    [SerializeField] private GameObject Truck;
    [SerializeField] private Button TruckButton;
    [SerializeField] private Transform[] TruckWayPoints;
    private Vector3 TruckParkDir;
    Quaternion TruckParklookRotation;
    private Vector3[] TruckWayPointsPositions;
    private Transform TruckParkPosition;
    private Transform TruckPosition;
    GoodsStates ChairState = GoodsStates.notProducing;
    GoodsStates TableState = GoodsStates.notProducing;
    GoodsStates WardrobeState = GoodsStates.notProducing;
    GoodsStates SofaState = GoodsStates.notProducing;



    // if producing is interruptted i used this variables on load
    private int chairProductionValue;
    private int tableProductionValue;
    private int sofaProductionValue;
    private int wardrobeProductionValue;
    private float chairProductionTimer;
    private float tableProductionTimer;
    private float sofaProductionTimer;
    private float wardrobeProductionTimer;

    //
    private bool isTruckOnRoad = false;

    private int ChairCost = 4; //Wood
    private int[] TableCost = { 6, 1 }; // Wood-Iron
    private int[] WardrobeCost = { 5, 2, 2 }; //Wood-Stone-Iron
    private int[] SofaCost = { 2, 6, 2 }; // Wood-Cloth-Iron
    private int productNumber = 1;
    private float chairCooldownTime = 4f;
    private float TableCooldownTime = 8f;
    private float SofaCooldownTime = 24f;
    private float WardrobeCooldownTime = 15f;
    private float SellTime = 60f;
    private int[] ProductsOnArray = new int[4]; // Chair-Table-Sofa-Wardrobe

    public void SetDatas()
    {
        GameManager.gameData.chairProductionTimer = chairProductionTimer;
        GameManager.gameData.tableProductionTimer = tableProductionTimer;
        GameManager.gameData.sofaProductionTimer = sofaProductionTimer;
        GameManager.gameData.wardrobeProductionTimer = wardrobeProductionTimer;
        GameManager.gameData.playerChair = ProductsOnArray[0];
        GameManager.gameData.playerTable = ProductsOnArray[1];
        GameManager.gameData.playerSofa = ProductsOnArray[2];
        GameManager.gameData.playerWardrobe = ProductsOnArray[3];
        GameManager.gameData.chairProductionValue = chairProductionValue;
        GameManager.gameData.tableProductionValue = tableProductionValue;
        GameManager.gameData.sofaProductionValue = sofaProductionValue;
        GameManager.gameData.wardrobeProductionValue = wardrobeProductionValue;
        GameManager.gameData.ChairState = ChairState;
        GameManager.gameData.TableState = TableState;
        GameManager.gameData.WardrobeState = WardrobeState;
        GameManager.gameData.SofaState = SofaState;

    }
    public void LoadDatas()
    {
        chairProductionTimer = GameManager.gameData.chairProductionTimer;
        tableProductionTimer = GameManager.gameData.tableProductionTimer;
        sofaProductionTimer = GameManager.gameData.sofaProductionTimer;
        wardrobeProductionTimer = GameManager.gameData.wardrobeProductionTimer;
        ProductsOnArray[0] = GameManager.gameData.playerChair;
        ProductsOnArray[1] = GameManager.gameData.playerTable;
        ProductsOnArray[2] = GameManager.gameData.playerSofa;
        ProductsOnArray[3] = GameManager.gameData.playerWardrobe;
        chairProductionValue = GameManager.gameData.chairProductionValue;
        tableProductionValue = GameManager.gameData.tableProductionValue;
        sofaProductionValue = GameManager.gameData.sofaProductionValue;
        wardrobeProductionValue = GameManager.gameData.wardrobeProductionValue;
        ChairState = GameManager.gameData.ChairState;
        TableState = GameManager.gameData.TableState;
        WardrobeState = GameManager.gameData.WardrobeState;
        SofaState = GameManager.gameData.SofaState;

    }


    private void OnEnable()
    {
        missionManager.SetActive(true);
        missionButton.SetActive(true);

        ButtonManager.SellGoods += SellGoods;
        ButtonManager.FactoryMenuEvent += FactoryMenu;
        ButtonManager.ProduceGoodsEvent += startCoolDown;
        ButtonManager.UpdateFactoryProductAmount += UpdateProductNumber;
    }
    private void OnDisable()
    {
        ButtonManager.SellGoods -= SellGoods;
        ButtonManager.FactoryMenuEvent -= FactoryMenu;
        ButtonManager.ProduceGoodsEvent -= startCoolDown;
        ButtonManager.UpdateFactoryProductAmount -= UpdateProductNumber;
    }

    void Awake()
    {
        GameManager.setGameData += SetDatas;
        GameManager.loadGameData += LoadDatas;
        ProductsOnArray = new int[] { 0, 0, 0, 0 };
        TruckWayPointsPositions = new Vector3[TruckWayPoints.Length];
        for (int i = 0; i < TruckWayPoints.Length; i++)
        {
            TruckWayPointsPositions[i] = TruckWayPoints[i].position;
        }
        FactoryPanel = transform.GetChild(0).GetChild(0).gameObject;
        TruckPosition = transform.GetChild(3);
        TruckParkPosition = transform.GetChild(4);
        TruckParkDir = (TruckParkPosition.position - TruckPosition.position).normalized;
        Quaternion TruckParklookRotation = Quaternion.LookRotation(TruckParkDir);
    }
    private void Start()
    {
        isTruckOnRoad = false;

        if (ChairState == GoodsStates.Producing)
        {
            startCoolDown("Chair", chairProductionValue, true);
        }
        if (TableState == GoodsStates.Producing)
        {
            startCoolDown("Table", tableProductionValue, true);
        }
        if (WardrobeState == GoodsStates.Producing)
        {
            startCoolDown("Wardrobe", wardrobeProductionValue, true);
        }
        if (SofaState == GoodsStates.Producing)
        {
            startCoolDown("Sofa", sofaProductionValue, true);
        }
        UpdateGoodsCost();
        UpdateMaterialsValues();
        UpdateInventoryInfos();
    }
    private void FactoryMenu(string sign)
    {
        if (sign == "Open")
        {
            FactoryPanel.gameObject.SetActive(true);
            missionButton.gameObject.SetActive(false);
        }
        if (sign == "Close")
        {
            FactoryPanel.gameObject.SetActive(false);
            missionButton.gameObject.SetActive(true);
        }

        UpdateGoodsCost();
        UpdateMaterialsValues();
        UpdateInventoryInfos();
        productNumberText.text = productNumber.ToString();
    }
    IEnumerator intoCoolDown(string goods, int _productNumber, bool isSaved)
    {
        bool producabilty = CheckProducablity(goods, _productNumber);
        if (isSaved) producabilty = true;
        if (producabilty)
        {
            if (!isSaved) SpendMaterials(goods, _productNumber);
            AudioManager.Instance.PlaySFX("Produce");
            float timer;
            float TotalTime;
            if (goods == "Chair")
            {
                ChairState = GoodsStates.Producing;
                chairProductionValue = _productNumber;
                ProductChairButton.gameObject.SetActive(false);

                if (isSaved)
                {
                    timer = chairProductionTimer;
                    TotalTime = chairCooldownTime * (float)chairProductionValue;
                }

                else
                {
                    timer = chairCooldownTime * (float)_productNumber;
                    TotalTime = chairCooldownTime * (float)_productNumber;
                }
                while (timer >= 0.0f)
                {

                    ChairCooldownText.text = Mathf.RoundToInt(timer).ToString();
                    ChairCooldownText.gameObject.SetActive(true);
                    ChairCooldownBar.fillAmount = timer / TotalTime;
                    yield return new WaitForSeconds(0.05f);
                    timer -= 0.05f;
                    chairProductionTimer = timer;
                }
                if (timer <= 0)
                {
                    chairProductionTimer = 0;
                    ChairCooldownBar.fillAmount = 1.0f;
                    ProductChairButton.gameObject.SetActive(true);
                }
                ChairState = GoodsStates.notProducing;
                ProductsOnArray[0] += _productNumber;

            }
            if (goods == "Table")
            {
                TableState = GoodsStates.Producing;
                tableProductionValue = _productNumber;
                ProductTableButton.gameObject.SetActive(false);
                if (isSaved)
                {
                    timer = tableProductionTimer;
                    TotalTime = TableCooldownTime * (float)tableProductionValue;
                }

                else
                {
                    timer = TableCooldownTime * (float)_productNumber;
                    TotalTime = TableCooldownTime * (float)_productNumber;
                }
                while (timer >= 0.0f)
                {
                    TableCooldownText.text = Mathf.RoundToInt(timer).ToString();
                    TableCooldownText.gameObject.SetActive(true);
                    TableCooldownBar.fillAmount = timer / TotalTime;
                    yield return new WaitForSeconds(0.05f);
                    timer -= 0.05f;
                    tableProductionTimer = timer;
                }
                if (timer < 0)
                {
                    tableProductionTimer = 0;
                    TableCooldownBar.fillAmount = 1.0f;
                    ProductTableButton.gameObject.SetActive(true);
                }
                ProductsOnArray[1] += _productNumber;
                TableState = GoodsStates.notProducing;
            }
            if (goods == "Sofa")
            {
                SofaState = GoodsStates.Producing;
                sofaProductionValue = _productNumber;
                ProductSofaButton.gameObject.SetActive(false);
                if (isSaved)
                {
                    timer = sofaProductionTimer;
                    TotalTime = SofaCooldownTime * (float)sofaProductionValue;
                }

                else
                {
                    timer = SofaCooldownTime * (float)_productNumber;
                    TotalTime = SofaCooldownTime * (float)_productNumber;
                }
                while (timer >= 0.0f)
                {
                    SofaCooldownText.text = Mathf.RoundToInt(timer).ToString();
                    SofaCooldownText.gameObject.SetActive(true);
                    SofaCooldownBar.fillAmount = timer / TotalTime;
                    yield return new WaitForSeconds(0.05f);
                    timer -= 0.05f;
                    sofaProductionTimer = timer;
                }
                if (timer < 0)
                {
                    sofaProductionTimer = 0;
                    SofaCooldownBar.fillAmount = 1.0f;
                    ProductSofaButton.gameObject.SetActive(true);
                }
                ProductsOnArray[2] += _productNumber;
                SofaState = GoodsStates.notProducing;
            }
            if (goods == "Wardrobe")
            {
                WardrobeState = GoodsStates.Producing;
                wardrobeProductionValue = _productNumber;
                ProductWardrobeButton.gameObject.SetActive(false);
                if (isSaved)
                {
                    timer = wardrobeProductionTimer;
                    TotalTime = WardrobeCooldownTime * (float)wardrobeProductionValue;
                }

                else
                {
                    timer = WardrobeCooldownTime * (float)_productNumber;
                    TotalTime = WardrobeCooldownTime * (float)_productNumber;
                }
                while (timer >= 0.0f)
                {
                    WardrobeCooldownText.text = Mathf.RoundToInt(timer).ToString();
                    WardrobeCooldownText.gameObject.SetActive(true);
                    WardrobeCooldownBar.fillAmount = timer / TotalTime;
                    yield return new WaitForSeconds(0.05f);
                    timer -= 0.05f;
                    wardrobeProductionTimer = timer;
                }
                if (timer < 0)
                {
                    wardrobeProductionTimer = 0;
                    WardrobeCooldownBar.fillAmount = 1.0f;
                    ProductWardrobeButton.gameObject.SetActive(true);
                }
                ProductsOnArray[3] += _productNumber;
                WardrobeState = GoodsStates.notProducing;
            }

            UpdateGoodsCost();
            UpdateInventoryInfos();
            FactoryMissionManager.Instance.updateCompleteness();
            GameManager.Instance.IncreaseProducedGoods(goods, _productNumber);
            yield return null;
        }
        else yield return null;
    }
    IEnumerator SendTruck()
    {
        isTruckOnRoad = true;
        FactoryMissionManager.Instance.updateChangability();
        int[] missionCost = FactoryMissionManager.Instance.getMissionCosts();
        float missionEarning = FactoryMissionManager.Instance.getMissionEarning();

        TruckButton.interactable = false;
        Image TruckImage = TruckButton.GetComponent<Image>();
        TruckImage.fillAmount = 0.0f;
        float timer = 0f;

        AudioManager.Instance.PlaySFX("StartTruckEngine");
        StartCoroutine("TruckMovement");
        while (timer < SellTime)
        {
            TruckImage.fillAmount = timer / SellTime;
            timer += 0.02f;
            yield return new WaitForSeconds(0.02f);
        }
        StopCoroutine("TruckMovement");
        Truck.transform.position = TruckParkPosition.position;
        Truck.transform.rotation = TruckParkPosition.rotation;
        AudioManager.Instance.PlaySFX2("TruckArrive");
        while (Vector3.Distance(Truck.transform.position, TruckPosition.position) > 0.1f)
        {

            Truck.transform.position += TruckParkDir * -5f * Time.deltaTime;
            Truck.transform.rotation = Quaternion.Lerp(Truck.transform.rotation, TruckParklookRotation, Time.deltaTime * 10f);
            yield return null;
        }
        for (int i = 0; i < missionCost.Length; i++)
        {
            if (missionCost[i] != -1) ProductsOnArray[i] -= missionCost[i];
        }
        UpdateInventoryInfos();
        PlayerController.Instance.earnMoney(missionEarning);
        GameManager.Instance.IncreaseEarnedMoney(missionEarning);
        isTruckOnRoad = false;
        FactoryMissionManager.Instance.changeMission("free");




        yield return null;
    }
    IEnumerator TruckMovement()
    {
        Vector3 Truckdir;
        Vector3 TargetPosition;
        Quaternion lookRotation;
        for (int i = 0; i < TruckWayPointsPositions.Length; i++)
        {
            TargetPosition = TruckWayPointsPositions[i];
            Truckdir = (TargetPosition - Truck.transform.position).normalized;
            lookRotation = Quaternion.LookRotation(Truckdir);
            while (Vector3.Distance(Truck.transform.position, TruckWayPointsPositions[i]) > 0.5f)
            {
                Truck.transform.rotation = Quaternion.Lerp(Truck.transform.rotation, lookRotation, Time.deltaTime * 5f);
                Truck.transform.position += Truckdir * 5f * Time.deltaTime;
                yield return null;
            }
        }
        yield return null;
    }

    private void SellGoods()
    {
        StartCoroutine("SendTruck");
    }

    private void startCoolDown(string good, int _productNumber, bool isSaved)
    {
        StartCoroutine(intoCoolDown(good, _productNumber, isSaved));
    }
    private void UpdateProductNumber(string x)
    {
        if (x == "+")
        {
            productNumber++;
        }
        if (x == "-")
        {
            if (productNumber > 0) productNumber--;
        }
        productNumberText.text = productNumber.ToString();
        UpdateGoodsCost();
    }
    private void UpdateGoodsCost()
    {
        if (ChairState == GoodsStates.notProducing)
        {
            ChairCostText.text = (productNumber * ChairCost).ToString();
            ChairCooldownText.text = Mathf.RoundToInt((productNumber * chairCooldownTime)).ToString();

        }
        else
        {
            ChairCostText.text = (chairProductionValue * ChairCost).ToString();
        }
        if (TableState == GoodsStates.notProducing)
        {
            TableWoodCostText.text = (productNumber * TableCost[0]).ToString();
            TableIronCostText.text = (productNumber * TableCost[1]).ToString();
            TableCooldownText.text = Mathf.RoundToInt((productNumber * TableCooldownTime)).ToString();
        }
        else
        {
            TableWoodCostText.text = (tableProductionValue * TableCost[0]).ToString();
            TableIronCostText.text = (tableProductionValue * TableCost[1]).ToString();
        }
        if (WardrobeState == GoodsStates.notProducing)
        {
            WardrobeWoodText.text = (productNumber * WardrobeCost[0]).ToString();
            WardrobeStoneCostText.text = (productNumber * WardrobeCost[1]).ToString();
            WardrobeIronCostText.text = (productNumber * WardrobeCost[2]).ToString();
            WardrobeCooldownText.text = Mathf.RoundToInt((productNumber * WardrobeCooldownTime)).ToString();
        }
        else
        {
            WardrobeWoodText.text = (wardrobeProductionValue * WardrobeCost[0]).ToString();
            WardrobeStoneCostText.text = (wardrobeProductionValue * WardrobeCost[1]).ToString();
            WardrobeIronCostText.text = (wardrobeProductionValue * WardrobeCost[2]).ToString();
        }
        if (SofaState == GoodsStates.notProducing)
        {
            SofaWoodCostText.text = (productNumber * SofaCost[0]).ToString();
            SofaClothCostText.text = (productNumber * SofaCost[1]).ToString();
            SofaIronCostText.text = (productNumber * SofaCost[2]).ToString();
            SofaCooldownText.text = Mathf.RoundToInt((productNumber * SofaCooldownTime)).ToString();
        }
        else
        {
            SofaWoodCostText.text = (sofaProductionValue * SofaCost[0]).ToString();
            SofaClothCostText.text = (sofaProductionValue * SofaCost[1]).ToString();
            SofaIronCostText.text = (sofaProductionValue * SofaCost[2]).ToString();
        }
    }
    private void UpdateMaterialsValues()
    {
        WoodValueText.text = PlayerWoodStorageManager.Instance.getCurrentWood().ToString();
        StoneValueText.text = PlayerController.Instance.getCurrentStone().ToString();
        IronValueText.text = PlayerController.Instance.getCurrentIron().ToString();
        ClothValueText.text = PlayerController.Instance.getCurrentCloth().ToString();
    }
    private void UpdateInventoryInfos()
    {
        // Chair-Table-Sofa-Wardrobe
        TotalChairText.text = ProductsOnArray[0].ToString() + "X";
        TotalTableText.text = ProductsOnArray[1].ToString() + "X";
        TotalSofaText.text = ProductsOnArray[2].ToString() + "X";
        TotalWardrobeText.text = ProductsOnArray[3].ToString() + "X";

    }

    private bool CheckProducablity(string goods, int _productNumber)
    {
        int x = _productNumber;
        int Wood = PlayerWoodStorageManager.Instance.getCurrentWood();
        int Stone = PlayerController.Instance.getCurrentStone();
        int Iron = PlayerController.Instance.getCurrentIron();
        int Cloth = PlayerController.Instance.getCurrentCloth();
        if (goods == "Chair")
        {
            if (Wood >= x * ChairCost) return true;
            else return false;
        }
        else if (goods == "Table")
        {
            if (Wood >= x * TableCost[0] && Iron >= x * TableCost[1]) return true;
            else return false;
        }
        else if (goods == "Wardrobe")
        {
            if (Wood >= x * WardrobeCost[0] && Stone >= x * WardrobeCost[1] && Iron >= x * WardrobeCost[2]) return true;
            else return false;
        }
        else if (goods == "Sofa")
        {
            if (Wood >= x * SofaCost[0] && Cloth >= x * SofaCost[1] && Iron >= x * SofaCost[2]) return true;
            else return false;
        }
        else return false;
    }
    private void SpendMaterials(string goods, int _productNumber)
    {
        if (goods == "Chair")
        {
            PlayerWoodStorageManager.Instance.useWood(_productNumber * ChairCost);

        }
        else if (goods == "Table")
        {
            PlayerWoodStorageManager.Instance.useWood(_productNumber * TableCost[0]);
            PlayerController.Instance.useIron(_productNumber * TableCost[1]);
        }
        else if (goods == "Wardrobe")
        {
            PlayerWoodStorageManager.Instance.useWood(_productNumber * WardrobeCost[0]);
            PlayerController.Instance.useStone(_productNumber * WardrobeCost[1]);
            PlayerController.Instance.useIron(_productNumber * WardrobeCost[2]);
        }
        else if (goods == "Sofa")
        {
            PlayerWoodStorageManager.Instance.useWood(_productNumber * SofaCost[0]);
            PlayerController.Instance.useCloth(_productNumber * SofaCost[1]);
            PlayerController.Instance.useIron(_productNumber * SofaCost[2]);
        }
        UpdateMaterialsValues();
    }

    public int getProductNumber() => productNumber;

    public int[] getProductsArray()
    {
        return ProductsOnArray;
    }
    public bool getTruckState() => isTruckOnRoad;




}
