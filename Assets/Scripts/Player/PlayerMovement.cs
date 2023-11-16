using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private CharacterController _characterController;
    private Vector3 _moveDirection;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    void Start()
    {
        
        _characterController = GetComponent<CharacterController>();
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
        
        _characterController.Move(_moveDirection * (moveSpeed * Time.deltaTime));
        
    }

    public Vector3 ReturnPlayerDirection()
    {

        return this._moveDirection;

    }

}
