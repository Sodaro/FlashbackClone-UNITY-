using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * credits: raywenderlich.com
 * 
 */

public class JumpingState : State
{
    //private bool grounded;
    private int jumpParam = Animator.StringToHash("isJumping");
    
    //private int landParam = Animator.StringToHash("Land");
    //bool upHeld = false;
    public JumpingState(Player character, StateMachine stateMachine) : base(character, stateMachine)
    {
    }
    public override void Enter()
    {
        base.Enter();
        
        character.SetAnimationBool(jumpParam, true);
    }
	public override void Exit()
	{
		base.Exit();
        character.SetAnimationBool(jumpParam, false);
    }
	public override void HandleInput()
	{
		base.HandleInput();
        //jump = Input.GetKey(KeyCode.UpArrow);
	}
	public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!character.IsJumping && !character.IsGrounded)
        {
            //SoundManager.Instance.PlaySound(SoundManager.Instance.landing);
            stateMachine.ChangeState(character.falling);
        }
        else if (character.IsGrounded && !character.IsJumping)
		{
            Debug.Log("PREVIOUS STATE: " + stateMachine.PreviousState);
            if (action && stateMachine.PreviousState == character.running)
			{
                stateMachine.ChangeState(character.running);
			}
            else
            {
                stateMachine.ChangeState(character.idle);
            }
            
		}
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        //grounded = character.CheckCollisionOverlap(character.transform.position);
    }

    //private void Jump()

}
