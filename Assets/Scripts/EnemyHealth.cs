using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float health;

    void Update()
    {
        if (health <= 0) Destroy(gameObject);
    }

    public void DecreaseHealth(float h) => health -= h;
}
