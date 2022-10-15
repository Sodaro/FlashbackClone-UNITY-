using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GroundedState : State
{
    protected float speed;
    protected float rotationSpeed;

    
    //protected bool action = false;
    //protected bool jump = false;
    protected float direction = 0;

    protected float lastDirection = 1;
    //bool upHeld = false;

    //protected float horizontalInput;
    public GroundedState(Player character, StateMachine stateMachine) : base(character, stateMachine)
    {
    }
    public override void Enter()
    {
        base.Enter();
        horizontalInput = 0.0f;
        if (stateMachine.PreviousState == character.falling)
            character.StartCoroutine(character.MovementShortDisable());
  //      if (character.JumpQueued)
		//{
  //          stateMachine.ChangeState(character.jumping);
		//}
    }

    public override void Exit()
    {
        base.Exit();
        //character.ResetMoveParams();
    }

    public override void HandleInput()
    {
        base.HandleInput();
        direction = horizontalInput;

        //jump = Input.GetKey(KeyCode.X);
    }
	public override void LogicUpdate()
	{
		base.LogicUpdate();
        if (character.IsGrounded)
        {
            if (!downKey)
			{
                if (jump) //if jumpkey pressed or a jump is queued
                    character.QueueJump();
                
            }
            if (character.CanMove)
			{
                if (direction != 0)
                    lastDirection = direction;
            }

            //if (direction == -lastDirection)
            //{
            //	stateMachine.ChangeState(character.idle);
            //}

        }
        else //not grounded
        {
            stateMachine.ChangeState(character.falling);
            return;
        }

    }

	public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

	}

}
