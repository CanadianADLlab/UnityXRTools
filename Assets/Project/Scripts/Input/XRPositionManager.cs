﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Singleton class that manages positions of controllers and cameras
/// </summary>
public class XRPositionManager : MonoBehaviour
{
    public GameObject LeftHand;
    public GameObject RightHand;
    public GameObject PlaySpace;
    public GameObject Camera;


    #region Singleton
    public static XRPositionManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("XRPosition manager already exist destroying the position manager attacthed to : " + transform.name);
            Destroy(this);
        }
    }

    #endregion
}