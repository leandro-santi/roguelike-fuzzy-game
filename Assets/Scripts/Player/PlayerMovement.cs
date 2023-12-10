using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D _rb;
    private Vector3 _moveDirection;
    private Animator _animator;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        _moveDirection = new Vector3(horizontalInput, verticalInput, 0f).normalized;

        if (_moveDirection != Vector3.zero)
        {
            _animator.SetBool("isWalking", true);

            FlipCharacter(horizontalInput);
        }

        else
        {
            _animator.SetBool("isWalking", false);
        }
    }

    void FixedUpdate()
    {
        _rb.velocity = new Vector2(_moveDirection.x * moveSpeed, _moveDirection.y * moveSpeed);
    }

    public Vector3 ReturnPlayerDirection()
    {
        return this._moveDirection;
    }

    private void FlipCharacter(float horizontalInput)
    {
        if (horizontalInput < 0)
        {
            var localScale = transform.localScale;
            localScale = new Vector3(-Mathf.Abs(localScale.x), localScale.y,
                localScale.z);
            transform.localScale = localScale;
        }
        else if (horizontalInput > 0)
        {
            var localScale = transform.localScale;
            localScale = new Vector3(Mathf.Abs(localScale.x), localScale.y,
                localScale.z);
            transform.localScale = localScale;
        }
    }
}