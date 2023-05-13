using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
public class HealthManager : MonoBehaviour
{
    public static float health = -1;
    [SerializeField] float healthInspector;

    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] GameObject gameOverPanel;  
    bool invoked;
    public static event Action onDeath;
    private void Awake()
    {
        gameOverPanel.SetActive(false);
        invoked = false;
    }

    void Start()
    {
    }

    void Update()
    {
        if (health <= 0)
        {
            if (invoked) return;
            gameOverPanel.SetActive(true);
            onDeath?.Invoke();
            invoked = true;
        }
        healthInspector = health;

        healthText.text = "Health: " + health;
    }
}
