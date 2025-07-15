using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyInputController : MonoBehaviour, IInputSystem
{
    private InputActions _inputActions;
    private bool _moveForward = false;
    private bool _moveBack = false;
    private bool _jump = false;
    private bool _seatDown = false;
    private bool _block = false;
    private bool _attack = false;
    private bool _superAttack1 = false;
    private bool _superAttack2 = false;

    private void Awake()
    {
        _inputActions = new InputActions();
    }

    private void OnEnable()
    {
        _inputActions.enemy.Enable();

        _inputActions.enemy.MoveForward.performed += OnMoveForward;
        _inputActions.enemy.MoveForward.canceled += OnMoveForwardÑanceled;
        _inputActions.enemy.MoveBack.performed += OnMoveBack;
        _inputActions.enemy.MoveBack.canceled += OnMoveBackÑanceled;
        _inputActions.enemy.Jump.performed += OnJump;
        _inputActions.enemy.Jump.canceled += OnJumpÑanceled;
        _inputActions.enemy.SeatDown.performed += OnSeatDown;
        _inputActions.enemy.SeatDown.canceled += OnSeatDownÑanceled;

        _inputActions.enemy.Block.performed += OnBlock;
        _inputActions.enemy.Block.canceled += OnBlockÑanceled;
        _inputActions.enemy.Attack.performed += OnAttack;
        _inputActions.enemy.Attack.canceled += OnAttackÑanceled;
        _inputActions.enemy.SuperAttack1.performed += OnSuperAttack1;
        _inputActions.enemy.SuperAttack1.canceled += OnSuperAttack1Ñanceled;
        _inputActions.enemy.SuperAttack2.performed += OnSuperAttack2;
        _inputActions.enemy.SuperAttack2.canceled += OnSuperAttack2Ñanceled;
    }

    private void OnDisable()
    {
        _inputActions.enemy.MoveForward.performed -= OnMoveForward;
        _inputActions.enemy.MoveForward.canceled -= OnMoveForwardÑanceled;
        _inputActions.enemy.MoveBack.performed -= OnMoveBack;
        _inputActions.enemy.MoveBack.canceled -= OnMoveBackÑanceled;
        _inputActions.enemy.Jump.performed -= OnJump;
        _inputActions.enemy.Jump.canceled -= OnJumpÑanceled;
        _inputActions.enemy.SeatDown.performed -= OnSeatDown;
        _inputActions.enemy.SeatDown.canceled -= OnSeatDownÑanceled;

        _inputActions.enemy.Block.performed -= OnBlock;
        _inputActions.enemy.Block.canceled -= OnBlockÑanceled;
        _inputActions.enemy.Attack.performed -= OnAttack;
        _inputActions.enemy.Attack.canceled -= OnAttackÑanceled;
        _inputActions.enemy.SuperAttack1.performed -= OnSuperAttack1;
        _inputActions.enemy.SuperAttack1.canceled -= OnSuperAttack1Ñanceled;
        _inputActions.enemy.SuperAttack2.performed -= OnSuperAttack2;
        _inputActions.enemy.SuperAttack2.canceled -= OnSuperAttack2Ñanceled;


        _inputActions.enemy.Disable();
    }

    private void OnMoveForward(InputAction.CallbackContext context) { _moveForward = true; }
    private void OnMoveForwardÑanceled(InputAction.CallbackContext context) { _moveForward = false; }
    private void OnMoveBack(InputAction.CallbackContext context) { _moveBack = true; }
    private void OnMoveBackÑanceled(InputAction.CallbackContext context) { _moveBack = false; }
    private void OnSeatDown(InputAction.CallbackContext context) { _seatDown = true; }
    private void OnSeatDownÑanceled(InputAction.CallbackContext context) { _seatDown = false; }
    private void OnJump(InputAction.CallbackContext context) { _jump = true; }
    private void OnJumpÑanceled(InputAction.CallbackContext context) { _jump = false; }
    private void OnBlock(InputAction.CallbackContext context) { _block = true; }
    private void OnBlockÑanceled(InputAction.CallbackContext context) { _block = false; }
    private void OnAttack(InputAction.CallbackContext context) { _attack = true; }
    private void OnAttackÑanceled(InputAction.CallbackContext context) { _attack = false; }
    private void OnSuperAttack1(InputAction.CallbackContext context) { _superAttack1 = true; }
    private void OnSuperAttack1Ñanceled(InputAction.CallbackContext context) { _superAttack1 = false; }
    private void OnSuperAttack2(InputAction.CallbackContext context) { _superAttack2 = true; }
    private void OnSuperAttack2Ñanceled(InputAction.CallbackContext context) { _superAttack2 = false; }

    public InputFlags GetInputValues()
    {
        return new InputFlags
        {
            MoveForward = _moveForward,
            MoveBack = _moveBack,
            Jump = _jump,
            SeatDown = _seatDown,
            Block = _block,
            Attack = _attack,
            SuperAttack1 = _superAttack1,
            SuperAttack2 = _superAttack2,
        };
    }
}
