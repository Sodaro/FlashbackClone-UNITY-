using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingDownState : State
{
	protected int climbParam = Animator.StringToHash("isClimbing");
	public ClimbingDownState(Player character, StateMachine stateMachine) : base(character, stateMachine)
	{

	}
	public override void Enter()
	{
		base.Enter();
		character.SetAnimationBool(climbParam, true);
		character.ClimbDown();
	}

	public override void Exit()
	{
		base.Exit();
		character.ResetMoveParams();
		character.SetAnimationBool(climbParam, false);
	}
	public override void HandleInput()
	{
		base.HandleInput();
	}
	public override void LogicUpdate()
	{
		base.LogicUpdate();
		if (character.CanMove)
		{
			if (action)
				stateMachine.ChangeState(character.hanging);
			else
				stateMachine.ChangeState(character.falling);
		}
	}
}
