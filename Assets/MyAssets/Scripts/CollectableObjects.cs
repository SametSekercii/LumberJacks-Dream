using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableObjects : MonoBehaviour
{
    public static event Action<Transform> CollectWood;

    private GameObject CollectedWood;

    private void OnEnable()
    {
        StartCoroutine(LimitExistence());
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Lumberjack") && transform.CompareTag("Wood"))
        {
            if (PlayerController.Instance.getCurrentWood() < PlayerController.Instance.getMaxCarryableWood())
            {
                CollectedWood = ObjectPooler.Instance.getCollectedWoodFromPool();

                if (CollectedWood != null)
                {
                    CollectedWood.transform.parent = FindObjectOfType<PlayerController>().transform.GetChild(1);
                    int rand1 = UnityEngine.Random.Range(-3, 4);
                    int rand2 = UnityEngine.Random.Range(2, 5);
                    int rand3 = UnityEngine.Random.Range(-3, 4);
                    CollectedWood.transform.position = transform.position + new Vector3(rand1, rand2, rand3);
                    CollectedWood.transform.rotation = transform.rotation;
                    CollectedWood.SetActive(true);
                    AudioManager.Instance.PlaySFX("CollectWood");
                }
                CollectWood?.Invoke(CollectedWood.transform);
                transform.gameObject.SetActive(false);
                GameManager.Instance.InceraseCollectedWood(1);
            }
        }

        if (other.gameObject.CompareTag("Lumberjack") && transform.CompareTag("Gem"))
        {
            transform.gameObject.SetActive(false);
            GameManager.Instance.IncreaceCollecedGem(1);
            PlayerController.Instance.EarnGem(1);
        }
    }


    IEnumerator LimitExistence()
    {
        yield return new WaitForSeconds(30);
        transform.gameObject.SetActive(false);
    }
}
