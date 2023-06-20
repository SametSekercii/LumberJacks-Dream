using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Trees : MonoBehaviour
{
    [SerializeField] protected int TreeHealth;
    [SerializeField] protected int TreeTier;
    public abstract void TakeDamage();
    public abstract int GetHealth();

}
