using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class PlayerWoodStorageManager : UnitySingleton<PlayerWoodStorageManager>
{
    [SerializeField] private List<Transform> Woods = new List<Transform>();
    private int CurrentWoodOnCabin = 0;
    private int PlayerWoodStorage;
    [SerializeField] private Transform SpendWoodText;
    [SerializeField] private Transform EarnWoodText;
    [SerializeField] private Transform WoodIcon;

    private void SetDatas()
    {
        GameManager.gameData.playerWood = PlayerWoodStorage;
    }
    private void LoadDatas()
    {
        PlayerWoodStorage = GameManager.gameData.playerWood;
    }
    private void Awake()
    {
        GameManager.setGameData += SetDatas;
        GameManager.loadGameData += LoadDatas;
    }
    private void OnEnable()
    {
        PlayerController.onWoodStorageTrigger += TransferWoodFromPlayer;
    }
    private void OnDisable()
    {
        PlayerController.onWoodStorageTrigger += TransferWoodFromPlayer;
    }
    public void Work()
    {
        StartCoroutine(GainWood(CurrentWoodOnCabin));
    }
    IEnumerator GainWood(int _CurrentWoodOnCabin)
    {
        int WoodAmount = _CurrentWoodOnCabin;
        while (WoodAmount > 0)
        {
            Woods[WoodAmount - 1].parent = FindObjectOfType<ObjectPooler>().transform;
            Woods[WoodAmount - 1].gameObject.SetActive(false);
            Woods.RemoveAt(WoodAmount - 1);
            PlayerWoodStorage++;
            WoodAmount--;
            CurrentWoodOnCabin--;
            var ChangeIcon = ObjectPooler.Instance.getWoodIconAnimFromPool();
            var EarnText = ObjectPooler.Instance.getEarnWoodTextFromPool();
            if (ChangeIcon != null && EarnText != null)
            {
                ChangeIcon.transform.position = WoodIcon.transform.position;
                ChangeIcon.transform.rotation = WoodIcon.transform.rotation;
                EarnText.transform.position = EarnWoodText.position;
                EarnText.transform.rotation = EarnWoodText.rotation;
                ChangeIcon.SetActive(true);
                EarnText.SetActive(true);
                EarnText.GetComponent<TMP_Text>().text = "+" + 1;
            }
            yield return new WaitForSeconds(0.2f);

        }
        yield return null;

    }
    private void TransferWoodFromPlayer(Transform Wood)
    {
        Woods.Add(Wood);
        Wood.parent = transform.GetChild(0);
        CurrentWoodOnCabin++;
        AudioManager.Instance.PlaySFX("CollectWood");
    }
    public int getCurrentWood()
    {
        return PlayerWoodStorage;
    }
    public void SpendWoodOnTrader(string material, int value)
    {
        int StoneCost;
        if (material == "Stone")
        {
            StoneCost = TradeCompany.Instance.getStoneCost();
            useWood(value * StoneCost);
        }
        if (material == "Iron")
        {
            int[] IronCost = TradeCompany.Instance.getIronCost();
            useWood(value * IronCost[0]);
        }
    }
    public void useWood(int x)
    {
        PlayerWoodStorage -= x;

        var ChangeIcon = ObjectPooler.Instance.getWoodIconAnimFromPool();
        var SpendText = ObjectPooler.Instance.getSpendWoodTextFromPool();
        if (ChangeIcon != null && SpendText != null)
        {
            ChangeIcon.transform.position = WoodIcon.transform.position;
            ChangeIcon.transform.rotation = WoodIcon.transform.rotation;
            SpendText.transform.position = SpendWoodText.position;
            SpendText.transform.rotation = SpendWoodText.rotation;
            ChangeIcon.SetActive(true);
            SpendText.SetActive(true);
            SpendText.GetComponent<TMP_Text>().text = "-" + x.ToString();
        }

    }





}
