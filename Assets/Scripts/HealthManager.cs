using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthManager : MonoBehaviour
{
    public static float health = -1;
    [SerializeField] float healthInitial;
    [SerializeField] float healthInspector;

    public static event Action onDeath;
    private void Awake()
    {
        if (health == -1) health = healthInitial;
    }

    void Start()
    {
    }

    void Update()
    {
        healthInspector = health;
        if (health <= 0) onDeath?.Invoke();
    }
}
