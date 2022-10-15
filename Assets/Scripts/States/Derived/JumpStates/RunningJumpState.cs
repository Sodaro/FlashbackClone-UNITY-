using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningJumpState : JumpingState
{
	public RunningJumpState(Player character, StateMachine stateMachine) : base(character, stateMachine)
	{
	}
	public override void Enter()
	{
		base.Enter();
		character.Jump(Player.JumpType.RUNNING);
	}
	public override void HandleInput()
	{
		base.HandleInput();
	}
	public override void LogicUpdate()
	{
		if (character.CanClimbLedge() && character.CanMove)
		{
			//	Debug.Log(action);
			if (jump)
			{
				stateMachine.ChangeState(character.climbing);
				return;
			}
			else if (action)
			{
				stateMachine.ChangeState(character.hanging);
				return;
			}
		}
		base.LogicUpdate();

	}
}
