using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : UnitySingleton<ObjectPooler>
{

    private GameObject[,] Trees = new GameObject[6, 10];
    private List<GameObject> ChoppedWoods = new List<GameObject>();
    private List<GameObject> Gems = new List<GameObject>();
    private List<GameObject> CollectedWoods = new List<GameObject>();
    private List<GameObject> Moneys = new List<GameObject>();
    private List<GameObject> SpendMoneyAnimTexts = new List<GameObject>();
    private List<GameObject> EarnMoneyAnimTexts = new List<GameObject>();
    private List<GameObject> SpendWoodAnimTexts = new List<GameObject>();
    private List<GameObject> EarnWoodAnimTexts = new List<GameObject>();
    private List<GameObject> WoodChangeIconAnim = new List<GameObject>();
    private List<GameObject> CoinChangeIconAnim = new List<GameObject>();
    private List<GameObject> CoinAnimText = new List<GameObject>();
    private List<GameObject> GemEarnAnimText = new List<GameObject>();
    private List<GameObject> GemSpendAnimText = new List<GameObject>();
    private List<GameObject> GemEarnIconAnim = new List<GameObject>();
    private List<GameObject> hitEffects = new List<GameObject>();
    private int MoneyAmount = 100;
    private int CollectedWoodAmount = 50;
    private int TreeAmount = 10;
    private int WoodAmount = 100;
    private int GemsAmount = 50;
    [SerializeField] private GameObject[] TreePrefabs;
    [SerializeField] private GameObject GemPrefab;
    [SerializeField] private GameObject ChoppedWoodPrefab;
    [SerializeField] private GameObject CollectedWoodPrefab;
    [SerializeField] private GameObject MoneyPrefab;
    [SerializeField] private GameObject SpendMoneyAnimTextPreFab;
    [SerializeField] private GameObject EarnMoneyAnimTextPreFab;
    [SerializeField] private GameObject SpendWoodAnimTextPreFab;
    [SerializeField] private GameObject EarnWoodAnimTextPreFab;
    [SerializeField] private GameObject WoodChangeIconAnimPrefab;
    [SerializeField] private GameObject CoinChangeIconAnimPrefab;
    [SerializeField] private GameObject CoinAnimTextPreFab;
    [SerializeField] private GameObject GemEarnAnimTextPreFab;
    [SerializeField] private GameObject GemSpendAnimTextPreFab;
    [SerializeField] private GameObject GemChangeIconAnimPrefab;
    [SerializeField] private GameObject ChangeMoneyTextsParent;
    [SerializeField] private GameObject ChangeWoodTextsParent;
    [SerializeField] private GameObject ChangeCoinTextParent;
    [SerializeField] private GameObject ChangeGemTextParent;
    [SerializeField] private GameObject hitEffectPrefab;


    void Awake()
    {
        CreateTreePool();
        CreateWoodPool();
        CreateGemsPool();
        CreateCollectedWoodPool();
        CreateMoneyPool();
        CreateSpendTextPool();
        CreateEarnTextPool();
        CreateWoodIconAnimTextPool();
        CreateSpendWoodTextPool();
        CreateEarnWoodTextPool();
        CreateCoinAnimTextPool();
        CreateCoinIconAnimTextPool();
        CreateGemIconAnimTextPool();
        CreateGemSpendAnimTextPool();
        CreateGemEarnAnimTextPool();
        CreateHitEffect();
    }
    private void CreateHitEffect()
    {
        for (int i = 0; i < 40; i++)
        {
            var effect = Instantiate(hitEffectPrefab);
            effect.transform.SetParent(transform);
            effect.SetActive(false);
            hitEffects.Add(effect);
        }
    }
    private void CreateGemIconAnimTextPool()
    {
        for (int i = 0; i < 40; i++)
        {
            var Icon = Instantiate(GemChangeIconAnimPrefab);
            Icon.transform.SetParent(ChangeGemTextParent.transform);
            Icon.SetActive(false);
            GemEarnIconAnim.Add(Icon);
        }
    }
    private void CreateGemSpendAnimTextPool()
    {
        for (int i = 0; i < 40; i++)
        {
            var Text = Instantiate(GemSpendAnimTextPreFab);
            Text.transform.SetParent(ChangeGemTextParent.transform);
            Text.SetActive(false);
            GemSpendAnimText.Add(Text);
        }
    }
    private void CreateGemEarnAnimTextPool()
    {
        for (int i = 0; i < 40; i++)
        {
            var Text = Instantiate(GemEarnAnimTextPreFab);
            Text.transform.SetParent(ChangeGemTextParent.transform);
            Text.SetActive(false);
            GemEarnAnimText.Add(Text);
        }
    }
    private void CreateCoinAnimTextPool()
    {
        for (int i = 0; i < 40; i++)
        {
            var Text = Instantiate(CoinAnimTextPreFab);
            Text.transform.SetParent(ChangeCoinTextParent.transform);
            Text.SetActive(false);
            CoinAnimText.Add(Text);
        }
    }
    private void CreateCoinIconAnimTextPool()
    {
        for (int i = 0; i < 40; i++)
        {
            var Icon = Instantiate(CoinChangeIconAnimPrefab);
            Icon.transform.SetParent(ChangeCoinTextParent.transform);
            Icon.SetActive(false);
            CoinChangeIconAnim.Add(Icon);
        }
    }
    private void CreateWoodIconAnimTextPool()
    {
        for (int i = 0; i < 40; i++)
        {
            var Icon = Instantiate(WoodChangeIconAnimPrefab);
            Icon.transform.SetParent(ChangeWoodTextsParent.transform);
            Icon.SetActive(false);
            WoodChangeIconAnim.Add(Icon);
        }
    }
    private void CreateSpendWoodTextPool()
    {
        for (int i = 0; i < 40; i++)
        {
            var Text = Instantiate(SpendWoodAnimTextPreFab);
            Text.transform.SetParent(ChangeWoodTextsParent.transform);
            Text.SetActive(false);
            SpendWoodAnimTexts.Add(Text);
        }
    }
    private void CreateEarnWoodTextPool()
    {
        for (int i = 0; i < 40; i++)
        {
            var Text = Instantiate(EarnWoodAnimTextPreFab);
            Text.transform.SetParent(ChangeWoodTextsParent.transform);
            Text.SetActive(false);
            EarnWoodAnimTexts.Add(Text);
        }

    }
    private void CreateEarnTextPool()
    {
        for (int i = 0; i < 40; i++)
        {
            var Text = Instantiate(EarnMoneyAnimTextPreFab);
            Text.transform.SetParent(ChangeMoneyTextsParent.transform);
            Text.SetActive(false);
            EarnMoneyAnimTexts.Add(Text);
        }

    }
    private void CreateSpendTextPool()
    {
        for (int i = 0; i < 40; i++)
        {
            var Text = Instantiate(SpendMoneyAnimTextPreFab);
            Text.transform.SetParent(ChangeMoneyTextsParent.transform);
            Text.SetActive(false);
            SpendMoneyAnimTexts.Add(Text);
        }
    }
    private void CreateMoneyPool()
    {
        for (int i = 0; i < MoneyAmount; i++)
        {
            var Money = Instantiate(MoneyPrefab);
            Money.transform.parent = transform;
            Money.SetActive(false);
            Moneys.Add(Money);
        }

    }
    private void CreateCollectedWoodPool()
    {
        for (int i = 0; i < CollectedWoodAmount; i++)
        {
            var wood = Instantiate(CollectedWoodPrefab);
            wood.transform.parent = transform;
            wood.SetActive(false);
            CollectedWoods.Add(wood);
        }
    }

    private void CreateTreePool()
    {
        for (int x = 0; x < TreePrefabs.Length; x++)
        {
            for (int y = 0; y < TreeAmount; y++)
            {
                var tree = Instantiate(TreePrefabs[x]);
                tree.transform.parent = transform;
                tree.SetActive(false);
                Trees[x, y] = tree;
            }
        }
    }

    private void CreateWoodPool()
    {
        for (int x = 0; x < WoodAmount; x++)
        {
            var Wood = Instantiate(ChoppedWoodPrefab);
            Wood.transform.parent = transform;
            Wood.SetActive(false);
            ChoppedWoods.Add(Wood);

        }

    }
    private void CreateGemsPool()
    {
        for (int x = 0; x < GemsAmount; x++)
        {
            var Gem = Instantiate(GemPrefab);
            Gem.transform.parent = transform;
            Gem.SetActive(false);
            Gems.Add(Gem);
        }

    }
    public GameObject getGemIconAnimFromPool()
    {
        for (int i = 0; i < 40; i++)
        {
            if (!GemEarnIconAnim[i].activeSelf) return GemEarnIconAnim[i];
        }
        return null;
    }
    public GameObject getGemSpendAnimTextFromPool()
    {
        for (int i = 0; i < 40; i++)
        {
            if (!GemSpendAnimText[i].activeSelf) return GemSpendAnimText[i];
        }
        return null;
    }
    public GameObject getGemEarnAnimTextFromPool()
    {
        for (int i = 0; i < 40; i++)
        {
            if (!GemEarnAnimText[i].activeSelf) return GemEarnAnimText[i];
        }
        return null;
    }
    public GameObject getCoinAnimTextFromPool()
    {
        for (int i = 0; i < 40; i++)
        {
            if (!CoinAnimText[i].activeSelf) return CoinAnimText[i];
        }
        return null;
    }
    public GameObject getCoinIconAnimFromPool()
    {
        for (int i = 0; i < 40; i++)
        {
            if (!CoinChangeIconAnim[i].activeSelf) return CoinChangeIconAnim[i];
        }
        return null;
    }
    public GameObject getWoodIconAnimFromPool()
    {
        for (int i = 0; i < 40; i++)
        {
            if (!WoodChangeIconAnim[i].activeSelf) return WoodChangeIconAnim[i];
        }
        return null;
    }
    public GameObject getEarnWoodTextFromPool()
    {
        for (int i = 0; i < 40; i++)
        {
            if (!EarnWoodAnimTexts[i].activeSelf) return EarnWoodAnimTexts[i];
        }
        return null;
    }
    public GameObject getSpendWoodTextFromPool()
    {
        for (int i = 0; i < 40; i++)
        {
            if (!SpendWoodAnimTexts[i].activeSelf) return SpendWoodAnimTexts[i];
        }
        return null;
    }
    public GameObject getSpendTextFromPool()
    {
        for (int i = 0; i < 40; i++)
        {
            if (!SpendMoneyAnimTexts[i].activeSelf) return SpendMoneyAnimTexts[i];
        }
        return null;
    }
    public GameObject getEarnTextFromPool()
    {
        for (int i = 0; i < 40; i++)
        {
            if (!EarnMoneyAnimTexts[i].activeSelf) return EarnMoneyAnimTexts[i];
        }
        return null;
    }
    public GameObject getMoneyFromPool()
    {
        for (int i = 0; i < MoneyAmount; i++)
        {
            if (!Moneys[i].activeSelf) return Moneys[i];
        }
        return null;
    }
    public GameObject getCollectedWoodFromPool()
    {
        for (int i = 0; i < CollectedWoodAmount; i++)
        {
            if (!CollectedWoods[i].activeSelf) return CollectedWoods[i];
        }
        return null;
    }

    public GameObject getWoodFromPool()
    {
        for (int i = 0; i < WoodAmount; i++)
        {
            if (!ChoppedWoods[i].activeSelf) return ChoppedWoods[i];
        }
        return null;

    }
    public GameObject getGemFromPool()
    {
        for (int i = 0; i < GemsAmount; i++)
        {
            if (!Gems[i].activeSelf) return Gems[i];
        }
        return null;
    }
    public GameObject getHitEffectFromPool()
    {
        for (int i = 0; i < 10; i++)
        {
            if (!hitEffects[i].activeSelf) return hitEffects[i];
        }
        return null;
    }

    public GameObject getTreeFromPool(int x)
    {//5 15 30 49 72 100

        if (x > 0 && x <= 5)
        {
            for (int i = 0; i < TreeAmount; i++)
            {
                if (!Trees[5, i].activeSelf) return Trees[5, i];
            }
            return null;
        }
        if (x > 5 && x <= 15)
        {
            for (int i = 0; i < TreeAmount; i++)
            {
                if (!Trees[4, i].activeSelf) return Trees[4, i];
            }
            return null;
        }
        if (x > 15 && x <= 30)
        {
            for (int i = 0; i < TreeAmount; i++)
            {
                if (!Trees[3, i].activeSelf) return Trees[3, i];
            }
            return null;
        }
        if (x > 30 && x <= 49)
        {
            for (int i = 0; i < TreeAmount; i++)
            {
                if (!Trees[2, i].activeSelf) return Trees[2, i];
            }
            return null;
        }
        if (x > 49 && x <= 72)
        {
            for (int i = 0; i < TreeAmount; i++)
            {
                if (!Trees[1, i].activeSelf) return Trees[1, i];
            }
            return null;
        }
        if (x > 72 && x <= 100)
        {
            for (int i = 0; i < TreeAmount; i++)
            {
                if (!Trees[0, i].activeSelf) return Trees[0, i];
            }
            return null;
        }

        return null;


    }

}
