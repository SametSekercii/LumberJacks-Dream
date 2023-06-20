using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreesTriggers : MonoBehaviour
{
    public static event Action<Transform, GameObject, int> onTreeTrigger;
    private Transform TriggerParent;
    private Animator playerAnim;
    private Transform lumberjack;
    private OakTree tree;
    float CuttingCast = 1.276f;


    void Awake()
    {
        playerAnim = FindObjectOfType<PlayerController>().transform.GetChild(0).GetComponent<Animator>();
        lumberjack = FindObjectOfType<PlayerController>().transform.GetComponent<Transform>();
        TriggerParent = transform.parent;
        tree = TriggerParent.GetComponent<OakTree>();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Lumberjack") && other.GetComponent<Rigidbody>().velocity == Vector3.zero && tree.GetHealth() > 0)
        {


            if ((CuttingCast += Time.deltaTime) >= 1.000f)
            {
                string SpawnPointTag = TriggerParent.parent.tag;
                int x = getSpawnPointIndex(SpawnPointTag);
                onTreeTrigger?.Invoke(TriggerParent, other.gameObject, x);
                playerAnim.SetBool("isRunning", false);
                playerAnim.SetBool("isHitting", true);
                StopAllCoroutines();
                StartCoroutine("lookAtTree", lumberjack);
                CuttingCast = 0;

            }


        }
    }
    private void OnTriggerExit(Collider other)
    {
        StopAllCoroutines();
        TreeManager.Instance.stopTakingDamage();
        playerAnim.SetBool("isHitting", false);
    }

    IEnumerator lookAtTree(Transform lumberjack)
    {

        Vector3 dir = (lumberjack.transform.position - transform.parent.transform.position).normalized;
        Quaternion lookDirection = Quaternion.LookRotation(-dir, Vector3.up);
        while (true)
        {
            lumberjack.rotation = Quaternion.RotateTowards(lumberjack.rotation, lookDirection, 300 * Time.deltaTime);
            yield return null;
        }
    }
    private int getSpawnPointIndex(string tag)
    {
        int x = -1;
        switch (tag)
        {
            case "SpawnPoint1":
                x = 0;
                break;
            case "SpawnPoint2":
                x = 1;
                break;
            case "SpawnPoint3":
                x = 2;
                break;
            case "SpawnPoint4":
                x = 3;
                break;
            case "SpawnPoint5":
                x = 4;
                break;
            case "SpawnPoint6":
                x = 5;
                break;
            case "SpawnPoint7":
                x = 6;
                break;
            case "SpawnPoint8":
                x = 7;
                break;
            case "SpawnPoint9":
                x = 8;
                break;
            case "SpawnPoint10":
                x = 9;
                break;
            case "SpawnPoint11":
                x = 10;
                break;
            case "SpawnPoint12":
                x = 11;
                break;
        }

        return x;


    }

}
