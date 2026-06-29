using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D _rb;
    private Vector2 _moveInput;
    private PlayerCollisionHandler _collisionHandler;
    private Vector2 _lastValidPosition;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collisionHandler = GetComponent<PlayerCollisionHandler>();
        _lastValidPosition = _rb.position;
    }

    void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }

    void FixedUpdate()
    {
        Vector2 newVelocity = _moveInput.normalized * moveSpeed;
        Vector2 newPosition = _rb.position + newVelocity * Time.fixedDeltaTime;

        // Prüfe, ob die neue Position begehbar ist
        if (_collisionHandler.CanMoveTo(new Vector3(newPosition.x, newPosition.y, 0)))
        {
            _rb.linearVelocity = newVelocity;
            _lastValidPosition = _rb.position;
        }
        else
        {
            // Stoppe Bewegung wenn Kollision erkannt wird
            _rb.linearVelocity = Vector2.zero;
            // Setze auf letzte gültige Position zurück
            _rb.position = _lastValidPosition;
        }
    }
}
