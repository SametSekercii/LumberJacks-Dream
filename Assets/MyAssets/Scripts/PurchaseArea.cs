using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PurchaseArea : MonoBehaviour
{
    [SerializeField] private Image PurchaseBar;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] public float price;
    [SerializeField] private GameObject PurchasableObject;
    private Transform Player;
    private Transform TeleportPoint;
    private float startingprice;
    private bool isPurchasing = false;
    void Start()
    {
        startingprice = price;
        PurchaseBar = transform.parent.GetChild(1).GetComponent<Image>();
        priceText = transform.parent.GetChild(2).GetComponent<TMP_Text>();
        TeleportPoint = transform.parent.GetChild(3).transform;
        priceText.text = string.Format("{0:#,###0}", price / 1000, price % 1000) + "K";
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Lumberjack") && isPurchasing == false)
        {
            Player = other.transform;
            isPurchasing = true;
            StartCoroutine(PurchaseStarter());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Lumberjack") && isPurchasing == true)
        {
            isPurchasing = false;
            StopAllCoroutines();
        }
    }
    IEnumerator PurchaseStarter()
    {
        while (price > 0)
        {
            if (PlayerController.Instance.getCurrentMoney() >= (startingprice / 300))
            {
                if ((startingprice / 300) >= PlayerController.Instance.getCurrentMoney())
                {
                    PlayerController.Instance.SpendMoney(PlayerController.Instance.getCurrentMoney());
                    price -= PlayerController.Instance.getCurrentMoney();
                }
                if (PlayerController.Instance.getCurrentMoney() > (startingprice / 300))
                {
                    if ((startingprice / 300) >= price)
                    {
                        PlayerController.Instance.SpendMoney(price);

                        price -= price;
                    }
                    else
                    {
                        PurchaseBar.fillAmount = 1 - (price / startingprice);
                        PlayerController.Instance.SpendMoney((startingprice / 300));
                        price -= (startingprice / 300);
                    }
                }
            }
            priceText.text = string.Format("{0:#,###0}", price / 1000, price % 1000) + "K"; //Mathf.RoundToInt(price).ToString();
            yield return null;
        }
        switch (transform.parent.tag)
        {
            case "lumberjackhouse":
                GameManager.lumberjackHouse = buildState.Solded;

                break;
            case "trade":
                GameManager.trade = buildState.Solded;

                break;
            case "factory":
                GameManager.factory = buildState.Solded;

                break;
            case "miner1":
                GameManager.miner1 = buildState.Solded;

                break;
            case "miner2":
                GameManager.miner2 = buildState.Solded;

                break;
            case "miner3":
                GameManager.miner3 = buildState.Solded;

                break;
            case "miner4":
                GameManager.miner4 = buildState.Solded;

                break;
        }
        transform.parent.gameObject.SetActive(false);
        PurchasableObject.SetActive(true);
        Player.position = TeleportPoint.position;
        yield return null;
    }
}
