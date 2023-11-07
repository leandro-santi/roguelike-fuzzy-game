using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Attack")]
    public float attackForce = 10f;
    public float attackCooldown = 1f;
    
    private float _nextAttackTime;
    
    [Header("Dash")]
    public float dashDistance = 10f;
    public float dashDuration = 0.5f;
    public float dashCooldown = 3f;
    
    private float _nextDashTime;
    
    void Start()
    {
       
        
        
    }
    
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0) && Time.time > _nextAttackTime)
        {
            Attack();
            _nextAttackTime = Time.time + attackCooldown;
        }
        
        if (Input.GetMouseButtonDown(1) && Time.time > _nextDashTime)
        {
            Dash();
            _nextDashTime = Time.time + dashCooldown;
        }
        
    }

    private void Attack()
    {
        
        Debug.Log("Atacando");
        
    }

    private void Dash()
    {
        
        Debug.Log("Esquivando");
        
    }

}
