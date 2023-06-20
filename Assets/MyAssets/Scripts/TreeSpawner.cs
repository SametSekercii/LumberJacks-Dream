using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum _SpawnPointStates { Alive, Cutted }
public class TreeSpawner : UnitySingleton<TreeSpawner>
{
    private int SpawnSpeed;
    [SerializeField]
    private Transform[] SpawnPoints;
    private _SpawnPointStates[] SpawnPointStates = new _SpawnPointStates[12];
    float ControlDelay = 0;
    int counter = 0;
    void Start()
    {
        for (int i = 0; i < SpawnPointStates.Length; i++)
        {
            SpawnPointStates[i] = _SpawnPointStates.Cutted;
        }
        SpawnSpeed = 9;

    }
    void Update()
    {
        CreateTree(SpawnSpeed);
    }

    void CreateTree(int SpawnSpeed)
    {
        int temp = SpawnSpeed;

        if (counter >= 11) SpawnSpeed = temp;
        else SpawnSpeed = 1;

        if ((ControlDelay += Time.deltaTime) > SpawnSpeed)
        {
            if (counter < 11) counter++;
            for (int i = 0; i < SpawnPointStates.Length; i++)
            {
                if (SpawnPointStates[i] == _SpawnPointStates.Cutted)
                {
                    int Rand = Random.Range(0, 100);
                    var Tree = ObjectPooler.Instance.getTreeFromPool(Rand);
                    if (Tree != null)
                    {
                        Tree.transform.position = SpawnPoints[i].position;
                        Tree.transform.rotation = SpawnPoints[i].rotation;
                        Tree.transform.parent = SpawnPoints[i].transform;
                        Tree.SetActive(true);
                        SpawnPointStates[i] = _SpawnPointStates.Alive;

                    }
                    break;
                }
            }
            ControlDelay = 0;
        }
    }

    public void ChangePointState(int index)
    {
        SpawnPointStates[index] = _SpawnPointStates.Cutted;

    }
    public Transform GetSpawnPoint(int index)
    {
        return SpawnPoints[index];
    }
}
