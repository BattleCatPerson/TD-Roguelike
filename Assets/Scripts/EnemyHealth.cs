using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float health;
    public float Health => health;
    public EnemyElement element;

    void Update()
    {
        if (health <= 0) Destroy(gameObject);
    }

    public void DecreaseHealth(float h) => health -= h;
}
