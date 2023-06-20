using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMovement : UnitySingleton<PlayerMovement>
{
    public static event Action<string> UpgradesEvent;
    private int MoveSpeedLevel = 0;
    private Animator playerAnim;
    public FixedJoystick joystick;
    Rigidbody rb;

    private void SetDatas()
    {
        GameManager.gameData.moveSpeedLevel = MoveSpeedLevel;
    }
    private void LoadDatas()
    {
        MoveSpeedLevel = GameManager.gameData.moveSpeedLevel;
    }


    private void Awake()
    {
        GameManager.setGameData += SetDatas;
        GameManager.loadGameData += LoadDatas;
        playerAnim = FindObjectOfType<PlayerController>().transform.GetChild(0).GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody>();
    }
    void Update()
    {

        if (Input.touchCount > 0)
        {
            rb.velocity = new Vector3(-joystick.Horizontal * (MoveSpeedLevel * 1 + 10), 0, -joystick.Vertical * (MoveSpeedLevel * 1 + 10));
            transform.LookAt(transform.position + rb.velocity);
        }
        else rb.velocity = Vector3.zero;


        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            playerAnim.SetBool("isRunning", true);
            playerAnim.SetBool("isHitting", false);
        }
        else
        {
            playerAnim.SetBool("isRunning", false);
        }
    }
    public void IncreaseMoveSpeed(string skill)
    {
        MoveSpeedLevel++;
        UpgradesEvent?.Invoke(skill);
        AudioManager.Instance.PlaySFX("UpgradeMS");
    }
    public int GetMoveSpeedLevel()
    {
        return MoveSpeedLevel;
    }

}

