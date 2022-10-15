using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardJumpState : JumpingState
{
	public ForwardJumpState(Player character, StateMachine stateMachine) : base(character, stateMachine)
	{
	}
	public override void Enter()
	{
		base.Enter();
		character.Jump(Player.JumpType.FORWARD);
	}
	//public override void LogicUpdate()
	//{
	//	base.LogicUpdate();
	//	if (!character.IsJumping)
	//		stateMachine.ChangeState(character.idle);
	//}
}
