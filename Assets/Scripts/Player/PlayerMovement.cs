using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D _rb;
    private Vector2 _moveDirection;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    void Start()
    {
        
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

    }

    void Update()
    {
        
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
        _moveDirection = new Vector2(horizontalInput, verticalInput).normalized;
        
        if (_moveDirection != Vector2.zero)
        {
            _animator.SetBool("isWalking", true);
            
            if (horizontalInput < 0)
            {
                _spriteRenderer.flipX = true;
            }
            else if (horizontalInput > 0)
            {
                _spriteRenderer.flipX = false;
            }
        }
        else
        {
            _animator.SetBool("isWalking", false);
        }
        
    }

    void FixedUpdate()
    {
        
        _rb.velocity = _moveDirection * moveSpeed;
        
    }
}
