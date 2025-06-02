using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class NavStatsManager : StatsManager
{
    NavManager navManager;

    private void Awake()
        {
            navManager = GetComponent<NavManager>();
        }

    private void Start()
    {
        if(navManager != null) navManager.agent.speed = moveSpeed;
    }
}