using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Allow climb during fall and jump
public class ClimbingState : State
{
	protected int climbParam = Animator.StringToHash("isClimbing");
	public ClimbingState(Player character, StateMachine stateMachine) : base(character, stateMachine)
	{
	}
	public override void Enter()
	{
		base.Enter();
		character.SetAnimationBool(climbParam, true);
		character.Climb();
	}

	public override void Exit()
	{
		base.Exit();
		character.ResetMoveParams();
		character.SetAnimationBool(climbParam, false);
	}
	public override void LogicUpdate()
	{
		base.LogicUpdate();
		if (character.CanMove)
			stateMachine.ChangeState(character.idle);
	}
}
