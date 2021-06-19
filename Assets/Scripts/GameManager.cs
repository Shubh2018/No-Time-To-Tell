using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    bool isDoorOpen;
    bool hasReachedDoor;
    bool timeUp;
    bool isDead;
    [SerializeField]
    float totalTime;

    public bool IsDoorOpen
    {
        set
        {
            isDoorOpen = value;
        }

        get
        {
            return isDoorOpen;
        }
    }

    public bool HasReachedDoor
    {
        set
        {
            hasReachedDoor = value;
        }

        get
        {
            return hasReachedDoor;
        }
    }

    public bool TimeUp
    {
        set
        {
            timeUp = value;
        }

        get
        {
            return timeUp;
        }
    }

    public float TotalTime
    {
        get
        {
            return totalTime;
        }

        set
        {
            totalTime = value;
        }
    }

    public bool IsDead
    {
        get
        {
            return isDead;
        }

        set
        {
            isDead = value;
        }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;

        hasReachedDoor = false;
    }

    private void Update()
    {
        if (isDead || (isDoorOpen && hasReachedDoor))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
