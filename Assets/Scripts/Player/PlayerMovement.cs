using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D _rigidbody;
    private Vector3 _moveDirection;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        _moveDirection = new Vector3(horizontalInput, verticalInput, 0f).normalized;

        if (_moveDirection != Vector3.zero)
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
        _rigidbody.velocity = new Vector2(_moveDirection.x * moveSpeed, _moveDirection.y * moveSpeed);
    }

    public Vector3 ReturnPlayerDirection()
    {
        return this._moveDirection;
    }
}