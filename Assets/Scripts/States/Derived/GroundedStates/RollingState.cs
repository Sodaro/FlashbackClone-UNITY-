using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingState : GroundedState
{

	private int rollParam = Animator.StringToHash("isRolling");
	public RollingState(Player character, StateMachine stateMachine) : base(character, stateMachine)
    {
    }

	public override void Enter()
	{
		base.Enter();
		character.SetAnimationBool(rollParam, true);
		//character.transform.localScale = new Vector3(0.5f, 0.5f, 1);
		character.Roll();
	}

	public override void Exit()
	{
		base.Exit();
		character.SetAnimationBool(rollParam, false);
		//character.transform.localScale = new Vector3(1, 1, 1);
	}

	public override void HandleInput()
	{
		base.HandleInput();
	}

	public override void LogicUpdate()
	{
		base.LogicUpdate();
		if (character.IsGrounded)
		{
			if (character.CanMove)
			{
				if (character.RollQueued)
					character.Roll();
				else if (character.CanMoveInDirection(Utilities.Direction.UP))
				{
					if (action)
						stateMachine.ChangeState(character.running);
					else
						stateMachine.ChangeState(character.idle);
				}
				else
				{
					character.QueueRoll();
				}
			}
		}
		else
		{
			stateMachine.ChangeState(character.falling);
		}

	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
	}
}
