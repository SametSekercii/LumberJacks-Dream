using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinersScripts : MonoBehaviour
{

    private float workSpeed = 6f;
    private string _tag;


    private void OnEnable()
    {
        _tag = gameObject.tag;
        switch (_tag)
        {
            case "miner1.1":
                workSpeed = 19f;
                break;
            case "miner2.1":
                workSpeed = 15;
                break;
            case "miner3.1":
                workSpeed = 11;
                break;
            case "miner4.1":
                workSpeed = 7;
                break;

        }
        StartCoroutine("makeCoin");
    }


    IEnumerator makeCoin()
    {
        while (true)
        {
            yield return new WaitForSeconds(workSpeed);
            PlayerController.Instance.earnCoin(1);

        }
    }
}
