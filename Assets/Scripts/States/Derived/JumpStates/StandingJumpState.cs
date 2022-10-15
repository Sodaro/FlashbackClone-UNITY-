using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandingJumpState : JumpingState
{
	Vector3 jumpStartPos = Vector3.zero;
	public StandingJumpState(Player character, StateMachine stateMachine) : base(character, stateMachine)
	{
	}

	public override void Enter()
	{
		base.Enter();
		//if (character.CanClimbLedge())
		//Debug.Log("DO LEDGE GRAB");
		jumpStartPos = character.transform.position;
		character.Jump(Player.JumpType.STANDING);
	}
	public override void LogicUpdate()
	{
		if (character.CanClimbLedge() && character.CanMove)
		{
			if (action)
			{
				stateMachine.ChangeState(character.hanging);
				return;
			}
				
			else if (jump)
			{
				stateMachine.ChangeState(character.climbing);
				return;
			}
				
		}
		base.LogicUpdate();
	}
}
