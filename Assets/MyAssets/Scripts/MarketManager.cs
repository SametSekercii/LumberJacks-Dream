using DG.Tweening;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class MarketManager : UnitySingleton<MarketManager>
{

    [SerializeField] private List<Transform> Woods = new List<Transform>();
    [SerializeField] private List<Transform> MoneyLocations = new List<Transform>();
    [SerializeField] private List<Transform> CurrentMoneyObjects = new List<Transform>();
    [SerializeField] private Transform MoneyTarget;
    [SerializeField] private int MoneyCounter = 0;
    private int MarketCapacity = 35;
    [SerializeField] private int CurrentWoodOnMarket = 0;
    private float YAxis = 0;
    [SerializeField] private int CurrentMoneyObjectCounter;
    [SerializeField] private int TotalMoneyOnMarket = 0;
    private float WorkSpeed = 5f;
    private float WorkStartTime = 1f;
    private bool isWorking = false;
    private void OnEnable()
    {
        PlayerController.onMarketTrigger += TransferWoodFromPlayer;
    }
    private void OnDisable()
    {
        PlayerController.onMarketTrigger -= TransferWoodFromPlayer;
    }
    void TransferWoodFromPlayer(Transform wood)
    {
        wood.parent = this.transform.GetChild(1);
        Woods.Add(wood);
        CurrentWoodOnMarket++;
        AudioManager.Instance.PlaySFX("CollectWood");
    }
    public int GetMarketCapacity()
    {
        return MarketCapacity;
    }
    public int GetCurrentWoodOnMarket()
    {
        return CurrentWoodOnMarket;
    }

    public void Work()
    {
        if (isWorking == false)
        {
            InvokeRepeating("SpendWood", WorkStartTime, WorkSpeed);
            StartCoroutine(MakeMoney(WorkSpeed, CurrentWoodOnMarket));
            isWorking = true;
        }
    }

    private void SpendWood()
    {

        if (CurrentWoodOnMarket > 0)
        {

            Woods[CurrentWoodOnMarket - 1].parent = FindObjectOfType<ObjectPooler>().transform;
            Woods[CurrentWoodOnMarket - 1].gameObject.SetActive(false);
            Woods.RemoveAt(CurrentWoodOnMarket - 1);
            PlayerController.Instance.decreaseCounter();
            CurrentWoodOnMarket--;
        }
        else
        {
            StopAllCoroutines();
            CancelInvoke("SpendWood");
            isWorking = false;

        }
    }
    IEnumerator MakeMoney(float speed, int CurrentWood)
    {
        int _CurrentWood = CurrentWood;
        yield return new WaitForSeconds(WorkStartTime);

        if (TotalMoneyOnMarket < 48)
        {
            while (_CurrentWood != 0)
            {
                _CurrentWood = CurrentWood;     //*****//
                var Money = ObjectPooler.Instance.getMoneyFromPool();

                if (Money != null)
                {

                    Money.transform.rotation = MoneyLocations[MoneyCounter].rotation;
                    Money.transform.position = new Vector3(MoneyLocations[MoneyCounter].position.x, MoneyLocations[MoneyCounter].position.y + YAxis, MoneyLocations[MoneyCounter].position.z);
                    Money.transform.parent = transform.GetChild(2).transform;
                    CurrentMoneyObjects.Add(Money.transform);
                    CurrentMoneyObjectCounter++;
                    Money.SetActive(true);
                    Money.transform.DOScale(new Vector3(0.55f, 0.55f, 0.55f), 1.2f).SetEase(Ease.OutElastic);
                    if (MoneyCounter >= 11)
                    {
                        YAxis += 0.2f;
                        MoneyCounter = 0;
                    }
                    else
                    {
                        MoneyCounter++;
                    }
                }
                _CurrentWood--;
                TotalMoneyOnMarket++;
                yield return new WaitForSecondsRealtime(WorkSpeed);
            }

        }
        else
        {
            while (_CurrentWood != 0)
            {
                _CurrentWood = CurrentWood;
                _CurrentWood--;
                TotalMoneyOnMarket++;
                yield return new WaitForSecondsRealtime(WorkSpeed);
            }
        }
    }
    public bool getWorkingState()
    {
        return isWorking;
    }
    public void collectMoney(int counter, int TotalGain)
    {
        PlayerController.Instance.earnMoney(TotalGain * 100);
        TotalMoneyOnMarket -= TotalGain;
        for (int i = counter - 1; i >= 0; i--)
        {
            CurrentMoneyObjects.ElementAt(i).gameObject.SetActive(false);
            CurrentMoneyObjects.ElementAt(i).localScale = new Vector3(0, 0, 0);
            CurrentMoneyObjects.RemoveAt(i);
            CurrentMoneyObjectCounter--;
        }
        MoneyCounter = 0;



    }

    public int getCurrentMoneyObjectCounter() => CurrentMoneyObjectCounter;
    public int getTotalMoneyOnMarket() => TotalMoneyOnMarket;






}
