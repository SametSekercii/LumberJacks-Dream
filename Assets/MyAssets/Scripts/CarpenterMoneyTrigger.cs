using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class CarpenterMoneyTrigger : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Lumberjack") && MarketManager.Instance.getTotalMoneyOnMarket() > 0)
        {
            int counter = MarketManager.Instance.getCurrentMoneyObjectCounter();
            int TotalGain = MarketManager.Instance.getTotalMoneyOnMarket();

            for (int i = 0; i < counter; i++)
            {
                var Money = ObjectPooler.Instance.getMoneyFromPool();
                if (Money != null)
                {
                    Money.transform.parent = FindObjectOfType<PlayerController>().transform.GetChild(1);
                    int rand1 = UnityEngine.Random.Range(-3, 4);
                    int rand2 = UnityEngine.Random.Range(2, 5);
                    int rand3 = UnityEngine.Random.Range(-3, 4);
                    Money.transform.DOScale(new Vector3(0.55f, 0.55f, 0.55f), 1.2f).SetEase(Ease.OutElastic);
                    Money.transform.position = FindObjectOfType<PlayerController>().transform.position + new Vector3(rand1, rand2, rand3);
                    Money.transform.rotation = FindObjectOfType<PlayerController>().transform.rotation;
                    Money.SetActive(true);
                }
                PlayerController.Instance.addMoney(Money.transform);
            }
            MarketManager.Instance.collectMoney(counter, TotalGain);


        }
    }
}
