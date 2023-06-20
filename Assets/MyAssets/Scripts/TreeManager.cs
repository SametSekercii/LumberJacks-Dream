
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager : UnitySingleton<TreeManager>
{

    Animator playerAnim;
    private Vector3 forceDir;
    [SerializeField] private Transform hitEffectTransform;
    private Vector3 hitEffectPos;

    private void Start()
    {
        playerAnim = FindObjectOfType<PlayerController>().transform.GetChild(0).GetComponent<Animator>();

    }
    void OnEnable()
    {
        TreesTriggers.onTreeTrigger += _onTreeTrigger;
    }
    void OnDisable()
    {
        TreesTriggers.onTreeTrigger -= _onTreeTrigger;
    }
    IEnumerator ResetTheTree(GameObject tree, int SpawnPoint)
    {

        OakTree _tree;
        _tree = tree.GetComponent<OakTree>();
        string tag = _tree.tag;
        Rigidbody rb = tree.GetComponent<Rigidbody>();
        if (tag == "Tier3Tree" || tag == "Tier4Tree" || tag == "Tier5Tree" || tag == "Tier6Tree") AudioManager.Instance.PlaySFX2("TreeFalling");
        yield return new WaitForSeconds(4.5f);
        tree.gameObject.SetActive(false);
        GameManager.Instance.IncreaseCuttedTree(1);
        DropLoot(tree, SpawnPoint);

        rb.isKinematic = true;
        tree.gameObject.transform.parent = FindObjectOfType<ObjectPooler>().transform;
        TreeSpawner.Instance.ChangePointState(SpawnPoint);
        _tree.setTreeHealthValue();
        _tree.isFalling = false;

    }

    void DropLoot(GameObject tree, int SpawnPoint)
    {
        Rigidbody CollectableRb = new Rigidbody();

        switch (tree.tag)
        {
            case "Tier1Tree":
                for (int i = 0; i < 2; i++)
                {

                    var wood = ObjectPooler.Instance.getWoodFromPool();
                    if (wood != null)
                    {
                        wood.transform.position = TreeSpawner.Instance.GetSpawnPoint(SpawnPoint).position + new Vector3(0, 1, 0);
                        wood.transform.rotation = TreeSpawner.Instance.GetSpawnPoint(SpawnPoint).rotation;
                        CollectableRb = wood.GetComponent<Rigidbody>();
                        wood.SetActive(true);
                        CollectableRb.AddForce(Vector3.up, ForceMode.Impulse);

                    }
                }

                break;
            case "Tier2Tree":
                for (int i = 0; i < 3; i++)
                {

                    var wood = ObjectPooler.Instance.getWoodFromPool();
                    if (wood != null)
                    {
                        wood.transform.position = TreeSpawner.Instance.GetSpawnPoint(SpawnPoint).position + new Vector3(0, 1, 0);
                        wood.transform.rotation = TreeSpawner.Instance.GetSpawnPoint(SpawnPoint).rotation;
                        CollectableRb = wood.GetComponent<Rigidbody>();
                        wood.SetActive(true);
                        CollectableRb.AddForce(Vector3.up, ForceMode.Impulse);

                    }
                }


                break;
            case "Tier3Tree":
                for (int i = 0; i < 4; i++)
                {

                    var wood = ObjectPooler.Instance.getWoodFromPool();
                    if (wood != null)
                    {
                        wood.transform.position = TreeSpawner.Instance.GetSpawnPoint(SpawnPoint).position + new Vector3(0, 1, 0);
                        wood.transform.rotation = TreeSpawner.Instance.GetSpawnPoint(SpawnPoint).rotation;
                        CollectableRb = wood.GetComponent<Rigidbody>();
                        wood.SetActive(true);
                        CollectableRb.AddForce(Vector3.up, ForceMode.Impulse);

                    }

                }



                break;
            case "Tier4Tree":
                for (int i = 0; i < 5; i++)
                {

                    var wood = ObjectPooler.Instance.getWoodFromPool();
                    if (wood != null)
                    {
                        wood.transform.position = TreeSpawner.Instance.GetSpawnPoint(SpawnPoint).position + new Vector3(0, 1, 0);
                        wood.transform.rotation = TreeSpawner.Instance.GetSpawnPoint(SpawnPoint).rotation;
                        CollectableRb = wood.GetComponent<Rigidbody>();
                        wood.SetActive(true);
                        CollectableRb.AddForce(Vector3.up / 2, ForceMode.Impulse);

                    }
                }



                break;
            case "Tier5Tree":
                for (int i = 0; i < 6; i++)
                {

                    var wood = ObjectPooler.Instance.getWoodFromPool();
                    if (wood != null)
                    {
                        wood.transform.position = TreeSpawner.Instance.GetSpawnPoint(SpawnPoint).position + new Vector3(0, 1, 0);
                        wood.transform.rotation = TreeSpawner.Instance.GetSpawnPoint(SpawnPoint).rotation;
                        CollectableRb = wood.GetComponent<Rigidbody>();
                        wood.SetActive(true);
                        CollectableRb.AddForce(Vector3.up / 2, ForceMode.Impulse);
                    }
                }
                if (Random.Range(1, 101) <= 55)
                {
                    var gem = ObjectPooler.Instance.getGemFromPool();
                    if (gem != null)
                    {
                        gem.transform.position = TreeSpawner.Instance.GetSpawnPoint(SpawnPoint).position + new Vector3(0, 2, 0);
                        gem.transform.rotation = TreeSpawner.Instance.GetSpawnPoint(SpawnPoint).rotation;
                        CollectableRb = gem.GetComponent<Rigidbody>();
                        gem.SetActive(true);
                        CollectableRb.AddForce(Vector3.up, ForceMode.Impulse);

                    }

                }

                break;
            case "Tier6Tree":
                for (int i = 0; i < 7; i++)
                {

                    var wood = ObjectPooler.Instance.getWoodFromPool();
                    if (wood != null)
                    {
                        wood.transform.position = TreeSpawner.Instance.GetSpawnPoint(SpawnPoint).position + new Vector3(0, 1, 0);
                        wood.transform.rotation = TreeSpawner.Instance.GetSpawnPoint(SpawnPoint).rotation;
                        CollectableRb = wood.GetComponent<Rigidbody>();
                        wood.SetActive(true);
                        CollectableRb.AddForce(Vector3.up / 2, ForceMode.Impulse);
                    }
                }

                for (int i = 0; i < 3; i++)
                {
                    if (Random.Range(1, 101) < 50)
                    {
                        var gem = ObjectPooler.Instance.getGemFromPool();
                        if (gem != null)
                        {
                            gem.transform.position = TreeSpawner.Instance.GetSpawnPoint(SpawnPoint).position + new Vector3(0, 2, 0);
                            gem.transform.rotation = TreeSpawner.Instance.GetSpawnPoint(SpawnPoint).rotation;
                            CollectableRb = gem.GetComponent<Rigidbody>();
                            gem.SetActive(true);
                            CollectableRb.AddForce(Vector3.up, ForceMode.Impulse);

                        }
                    }

                }

                break;
        }

    }
    void _onTreeTrigger(Transform tree, GameObject Lumberjack, int SpawnPoint)
    {
        OakTree _tree;
        _tree = tree.GetComponent<OakTree>();
        if (_tree.GetHealth() > 0)
        {
            StartCoroutine(_TakeDamage(_tree, SpawnPoint));

        }



    }
    public void stopTakingDamage()
    {
        StopCoroutine("_TakeDamage");
    }

    IEnumerator _TakeDamage(OakTree _tree, int SpawnPoint)
    {


        yield return new WaitForSeconds(1f);
        playerAnim.SetBool("isHitting", false);
        var hitEffect = ObjectPooler.Instance.getHitEffectFromPool();

        if (hitEffect != null)
        {
            hitEffectPos = hitEffectTransform.position + new Vector3(UnityEngine.Random.Range(1, 1.4f), UnityEngine.Random.Range(1, 1.4f), UnityEngine.Random.Range(1, 1.4f));
            hitEffect.transform.position = hitEffectPos;
            hitEffect.SetActive(true);
        }


        _tree.TakeDamage();
        if (_tree.GetHealth() <= 0 && _tree.isFalling == false)
        {

            Rigidbody rb = _tree.gameObject.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            int rand = UnityEngine.Random.Range(1, 5);
            switch (rand)
            {
                case 1:
                    forceDir = Vector3.left;
                    break;
                case 2:
                    forceDir = Vector3.back;
                    break;
                case 3:
                    forceDir = Vector3.right;
                    break;
                case 4:
                    forceDir = Vector3.forward;
                    break;

            }
            rb.AddForce(forceDir * 150, ForceMode.Impulse);
            StartCoroutine(ResetTheTree(_tree.gameObject, SpawnPoint));
            _tree.isFalling = true;
        }





    }

}
