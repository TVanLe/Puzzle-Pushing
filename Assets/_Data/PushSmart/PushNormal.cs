using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushNormal : MonoBehaviour
{
    [SerializeField] protected Vector3 direction;
    [SerializeField] protected Vector3 newPos;
    [SerializeField] protected Vector3 oldPos;
    [SerializeField] protected float speedPush;
    private Transform obstacle;
    private Transform push;
    private Transform sitdown;
    private Player player;

    private void Awake()
    {
        obstacle = transform.Find("obstacle");
        push = transform.Find("push");
        sitdown = transform.Find("sitdown");
        player = GameObject.FindObjectOfType<Player>();
        LoadDirection();
        LoadNewOldPos();
    }

    private void Start()
    {
        LoadWallIsBlocked();
    }

    protected virtual void LoadNewOldPos()
    {
        oldPos = push.transform.position;
        newPos = oldPos + direction;
    }

    protected virtual void LoadWallIsBlocked()
    {
        TilemapManager.wallIsBlocked[newPos] = 1;
        TilemapManager.wallIsBlocked[oldPos] = 1;
    }
    protected virtual void LoadDirection()
    {
        if (push.localRotation.eulerAngles.z == 0)
        {
            direction = new Vector3(1, 0, 0);
        }
        else if (push.localRotation.eulerAngles.z == 90)
        {
            direction = new Vector3(0, 1, 0);
        }
        else if (push.localRotation.eulerAngles.z == 180)
        {
            direction = new Vector3(-1, 0, 0);
        }
        else if (push.localRotation.eulerAngles.z == 270)
        {
            direction = new Vector3(0, -1, 0);
        }

        newPos = push.position + direction;
    }

    private void Update()
    {
        HandlePush();
    }

    protected virtual bool IsSitDown()
    {
        return player.GetNextLocation() == sitdown.position || BoxManager.allBoxLocation.ContainsKey(sitdown.position);
    }
    
    protected virtual void HandlePush()
    {
        if (IsSitDown())
        {
            Debug.Log("Sit Down");
            MoveUp();
        }
        else 
        {
            MoveDown();
        }
    }

    void MoveUp()
    {
        if (push.position == oldPos)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.push);
        }
        push.transform.position = Vector3.MoveTowards(push.transform.position, newPos, (speedPush) * Time.deltaTime);
        TilemapManager.wallIsBlocked.Remove(oldPos);
    }

    void MoveDown()
    {
        if (push.position == newPos)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.push);
        }
        CheckToMoveDown(oldPos);
        push.transform.position = Vector3.MoveTowards(push.transform.position, oldPos, (speedPush + 8) * Time.deltaTime);
        TilemapManager.wallIsBlocked[oldPos] = 1;
    }

    void CheckToMoveDown(Vector3 posCheck)
    {
        if (BoxManager.allBoxLocation.ContainsKey(posCheck))
        {
            Vector3 newPosBox = posCheck - direction;

            Transform boxTransform = BoxManager.allBoxLocation[posCheck];
            boxTransform.position = newPosBox;
            BoxManager.allBoxLocation[newPosBox] = boxTransform;
            BoxManager.allBoxLocation.Remove(posCheck);
        }

        if (player.GetNextLocation() == posCheck)
        {
            Vector3 target = posCheck - direction;
            player.TranslatePlayerforPush(target);
        }
    }
}
