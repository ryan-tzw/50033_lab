using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Experimental
{
    public class PlayerMovement : MonoBehaviour
    {
        public float moveSpeed = 5.0f;

        private SpriteRenderer _spriteRenderer;
        private PlayerInput _playerInput;
        private InputActionMap _combatActions;
        private InputAction _moveAction;

        private Rigidbody _rb;
        
        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _playerInput = GetComponent<PlayerInput>();
            _combatActions = _playerInput.actions.FindActionMap("Combat");
            _moveAction = _combatActions.FindAction("Move");
        }

        private void Update()
        {
            var move = _moveAction.ReadValue<Vector2>();
            if (move.x != 0)
            {
                _spriteRenderer.flipX = move.x < 0;
            }
        }

        private void FixedUpdate()
        {
            var moveDir = _moveAction.ReadValue<Vector2>();
            _rb.linearVelocity = Vector3.ClampMagnitude(new Vector3(moveDir.x, 0, moveDir.y), 1f) * moveSpeed;
        }
    }
}
