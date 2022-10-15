using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningState : GroundedState
{
    private int runParam = Animator.StringToHash("isRunning");
    public RunningState(Player character, StateMachine stateMachine) : base(character, stateMachine)
    {
    }
    public override void Exit()
    {
        base.Exit();
        character.SetAnimationBool(runParam, false);
        lastDirection = direction;
    }
    public override void Enter()
    {
        base.Enter();
        speed = character.runSpeed;
        character.SetAnimationBool(runParam, true);
    }
    public override void HandleInput()
    {
        base.HandleInput();
        //crouch = Input.GetButtonDown("Fire3");
        //jump = Input.GetButtonDown("Jump");
        //crouch = Input.GetKeyDown(KeyCode.Z);
        //jump = Input.GetKeyDown(KeyCode.X);

    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        Utilities.Direction dir = lastDirection == 1 ? Utilities.Direction.RIGHT : Utilities.Direction.LEFT;
        Utilities.Direction characterFaceDir = character.FacingDirection == 1 ? Utilities.Direction.RIGHT : Utilities.Direction.LEFT;
        //Debug.Log(lastDirection);
        if (character.CanMove)
        {

            if (dir != characterFaceDir && dir != 0) // do turn around instead
            {
                stateMachine.ChangeState(character.idle);
                return;
            }
            if (downKey)
                character.QueueRoll();
            if (character.RollQueued)
			{
                stateMachine.ChangeState(character.rolling);
			}
            else if (character.CanMoveInDirection(characterFaceDir))
            {
                if (character.JumpQueued && character.CanMoveInDirection(characterFaceDir))
                {
                    stateMachine.ChangeState(character.runningJump);
                }
                else if (character.RunQueued || action)
				{
                    character.Move(lastDirection, speed);
                }
                else if (!character.RunQueued && !action)
                {
                    stateMachine.ChangeState(character.idle);
                }

            }
            else
			{
                stateMachine.ChangeState(character.idle);
            }


        }
        else
		{
            if (downKey)
            {
                character.QueueRoll();
            }
        }



        //if (horizontalInput == 0)
        //    stateMachine.ChangeState(character.idle);
        //if (crouch)
        //{
        //    stateMachine.ChangeState(character.crouching);
        //}
        //else if (jump && horizontalInput == 0)
        //{
        //    stateMachine.ChangeState(character.jumping);
        //}
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (action)
		{
            character.Move(lastDirection, speed);
        }


        //else if (lastDirection != 0)
        //    character.Move(lastDirection, speed);
    }
}
