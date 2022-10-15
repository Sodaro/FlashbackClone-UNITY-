using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingState : State
{
    private int fallParam = Animator.StringToHash("isFalling");
    private int tilesFallen = 0;
    public FallingState(Player character, StateMachine stateMachine) : base(character, stateMachine)
    {
    }
    public override void Enter()
    {
        base.Enter();
        character.SetAnimationBool(fallParam, true);
        tilesFallen = 0;
        action = false;
        //Debug.Log("falling");
    }
	public override void Exit()
	{
		base.Exit();
        character.SetAnimationBool(fallParam, false);
        character.RemoveQueuedJump();
        tilesFallen = 0;
    }
	public override void HandleInput()
	{
		base.HandleInput();
    }
	public override void LogicUpdate()
    {
        base.LogicUpdate();
        Debug.Log("asdasd");
        if (tilesFallen <= 3 && (stateMachine.PreviousState is HangingState))
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
		}
        if (character.CanMove)
		{
            if (!character.IsGrounded)
			{
                character.Fall();
				tilesFallen++;
			}    
            else
            {
                //Debug.Log(tilesFallen);
                if (tilesFallen > 4)
                    stateMachine.ChangeState(character.death);
                else
				{
                    stateMachine.ChangeState(character.idle);
                    tilesFallen = 0; //safety
                }
                    
            }
        }
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        //grounded = character.CheckCollisionOverlap(character.transform.position);
    }
}
