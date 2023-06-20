using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractEdger : MonoBehaviour
{
    private Button Btn;
    private void Start()
    {
        Btn = transform.parent.GetChild(0).GetComponent<Button>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Lumberjack"))
        {
            Btn.interactable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Lumberjack"))
        {
            Btn.interactable = false;
        }

    }
}
