using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangingState : State
{
	public HangingState(Player character, StateMachine stateMachine) : base(character, stateMachine)
	{

	}

	public override void Enter()
	{
		base.Enter();
		character.StopMoving();
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void HandleInput()
	{
		base.HandleInput();
		//jump = Input.GetKey(Utilities.JumpKey);
	}

	public override void LogicUpdate()
	{
		base.LogicUpdate();
		if (!action)
		{
			stateMachine.ChangeState(character.falling);
		}
		if (jump)
		{
			stateMachine.ChangeState(character.climbing);
		}
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
	}
}
