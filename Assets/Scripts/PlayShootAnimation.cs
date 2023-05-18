using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayShootAnimation : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] string trigger;
    [SerializeField] TowerShoot tower;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        tower = GetComponentInParent<TowerShoot>();
        tower.OnShoot += StartAnimation;
    }

    public void StartAnimation() => anim.SetTrigger(trigger);
}
