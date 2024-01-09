using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private float _initialMoveSpeed;
    private bool _stopMovement;
    private Rigidbody2D _rb;
    private Vector3 _moveDirection;
    private Animator _animator;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _initialMoveSpeed = moveSpeed;
    }

    void Update()
    {
        if (!GameController.Instance.canPlayTheGame)
        {
            _stopMovement = true;
            return;
        }

        _stopMovement = false;

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
        if (_stopMovement)
        {
            _rb.velocity = Vector2.zero;
            _animator.SetBool("isWalking", false);
            return;
        }

        _rb.velocity = new Vector2(_moveDirection.x * moveSpeed, _moveDirection.y * moveSpeed);
    }

    public Vector3 ReturnPlayerDirection()
    {
        return this._moveDirection;
    }

    public void EnableDashSpeed(float speed)
    {
        moveSpeed = speed;
    }

    public void DisableDashSpeed()
    {
        moveSpeed = _initialMoveSpeed;
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