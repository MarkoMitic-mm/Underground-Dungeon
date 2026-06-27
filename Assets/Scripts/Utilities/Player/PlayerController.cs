using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D _rb;
    private Vector2 _moveInput;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }

    void FixedUpdate()
    {
        _rb.linearVelocity = _moveInput.normalized * moveSpeed;
    }
}
