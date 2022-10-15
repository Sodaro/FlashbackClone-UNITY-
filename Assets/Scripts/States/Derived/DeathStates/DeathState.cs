using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathState : State
{
	private int deathParam = Animator.StringToHash("isDead"); 
	public DeathState(Player character, StateMachine stateMachine) : base(character, stateMachine)
	{

	}

	public override void Enter()
	{
		base.Enter();
		character.SetAnimationBool(deathParam, true);
	}

	public override void Exit()
	{
		base.Exit();
		character.SetAnimationBool(deathParam, false);
	}

	public override void HandleInput()
	{
		base.HandleInput();
		if (Input.anyKeyDown)
		{
			CheckpointManager.Instance.LoadGameFromCheckpoint();
			character.Respawn();
		}
	}

	public override void LogicUpdate()
	{
		base.LogicUpdate();
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
	}
}
