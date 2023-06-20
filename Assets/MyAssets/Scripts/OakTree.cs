using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OakTree : Trees
{


    public bool isFalling;
    int damage;


    private void Start()
    {
        Rigidbody rb = transform.gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = true;
        rb.mass = 1500;

        setTreeHealthValue();
        setTreeTierValue();
        isFalling = false;
    }
    public override void TakeDamage()
    {
        int axeLevel = PlayerController.Instance.getAxeLevel();
        switch (axeLevel)
        {
            case 0:
                damage = 50;
                break;
            case 1:
                damage = 75;
                break;
            case 2:
                damage = 150;
                break;
            case 3:
                damage = 250;
                break;
            case 4:
                damage = 450;
                break;
            case 5:
                damage = 600;
                break;
        }

        TreeHealth -= damage;
    }
    public override int GetHealth()
    {
        return TreeHealth;
    }
    void setTreeTierValue()
    {
        if (transform.gameObject.CompareTag("Tier1Tree")) TreeTier = 1;
        if (transform.gameObject.CompareTag("Tier2Tree")) TreeTier = 2;
        if (transform.gameObject.CompareTag("Tier3Tree")) TreeTier = 3;
        if (transform.gameObject.CompareTag("Tier4Tree")) TreeTier = 4;
        if (transform.gameObject.CompareTag("Tier5Tree")) TreeTier = 5;
        if (transform.gameObject.CompareTag("Tier6Tree")) TreeTier = 6;

    }
    public void setTreeHealthValue()
    {
        if (transform.gameObject.CompareTag("Tier1Tree")) TreeHealth = 100;
        if (transform.gameObject.CompareTag("Tier2Tree")) TreeHealth = 200;
        if (transform.gameObject.CompareTag("Tier3Tree")) TreeHealth = 300;
        if (transform.gameObject.CompareTag("Tier4Tree")) TreeHealth = 400;
        if (transform.gameObject.CompareTag("Tier5Tree")) TreeHealth = 500;
        if (transform.gameObject.CompareTag("Tier6Tree")) TreeHealth = 600;

    }
}
