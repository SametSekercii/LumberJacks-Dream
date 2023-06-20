using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform Player;
    Vector3 diffVector;


    void Start()
    {
        diffVector = Player.position - transform.position;

    }
    void LateUpdate()
    {


        transform.position = Player.position - diffVector;




    }



}
