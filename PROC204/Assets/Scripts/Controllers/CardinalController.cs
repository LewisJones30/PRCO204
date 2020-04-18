﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CardinalController : Controller
{
    Mover mover;
    Fighter fighter;
    Health health;
    CombatTarget combatTarget;
    Vector2 moveInput;

    bool jumpReset = true;
    bool rollReset = true;
    bool parryReset = true;
    bool isOneController;

    void Awake()
    {
        mover = GetComponent<Mover>();
        fighter = GetComponent<Fighter>();
        health = GetComponent<Health>();
        combatTarget = GetComponent<CombatTarget>();
    }    

    private void Update()
    {
        if (Gamepad.all.Count < 1) return;

        if (Gamepad.all.Count == 1) isOneController = true;
        else isOneController = false;

        var gamepad = Gamepad.all[0];
        if (gamepad == null) return;

        Dead(gamepad);

        if (isFrozen) return;

        Move(gamepad);
        Jump(gamepad);
        MeleeAttack(gamepad);
        ForwardRoll(gamepad);
        Dash(gamepad);
        Parry(gamepad);

        mover.Move(moveInput, 1f);
    }

    private void Dead(Gamepad gamepad)
    {
        if (!health.IsDead) return;

        if (gamepad.buttonSouth.IsPressed())
        {
            SceneManager.LoadScene("Level 1");
        }
        else if (gamepad.startButton.IsPressed())
        {
            SceneManager.LoadScene("Main Menu");
        }
    }

    private void Parry(Gamepad gamepad)
    {
        bool isParry;
        if (isOneController) isParry = gamepad.leftShoulder.IsPressed();
        else isParry = gamepad.leftShoulder.IsPressed() || gamepad.buttonEast.IsPressed();

        if (isParry && parryReset)
        {
            parryReset = false;
            combatTarget.Parry();
        }

        if (!isParry) parryReset = true;
    }

    private void Dash(Gamepad gamepad)
    {
        if (gamepad.leftTrigger.IsPressed())
        {
            mover.IsDashing = true;
        }
        else
        {
            mover.IsDashing = false;
        }
    }

    private void ForwardRoll(Gamepad gamepad)
    {
        bool isRolling;
        if (isOneController) isRolling = gamepad.leftStickButton.IsPressed();
        else isRolling = gamepad.buttonNorth.IsPressed() || gamepad.leftStickButton.IsPressed();

        if (isRolling && rollReset) //B on Xbox, Circle on PS4
        {
            rollReset = false;
            mover.ForwardRoll();
        }

        if (!isRolling)
        {
            rollReset = true;
        }
    }

    private void MeleeAttack(Gamepad gamepad)
    {
        bool isAttack;
        if (isOneController) isAttack = gamepad.rightTrigger.IsPressed();
        else isAttack = gamepad.buttonWest.IsPressed() || gamepad.rightTrigger.IsPressed();

        if (isAttack) fighter.Attack();
    }


    private void Jump(Gamepad gamepad)
    {
        bool isJump;

        if (isOneController) isJump = gamepad.rightShoulder.IsPressed();
        else isJump = gamepad.buttonSouth.IsPressed() || gamepad.rightShoulder.IsPressed();

        if (isJump && jumpReset) //A on Xbox, X on PS4
        {
            jumpReset = false;
            mover.Jump();
        }

        if (!isJump)
        {
            jumpReset = true;
        }
    }

    private void Move(Gamepad gamepad)
    {
        if (gamepad.leftStick.IsPressed()) //Only moves if player 1 is using the left stick
        {
            moveInput = gamepad.leftStick.ReadValue();
        }
        else
        {
            moveInput = Vector3.zero;
        }
    }
}
