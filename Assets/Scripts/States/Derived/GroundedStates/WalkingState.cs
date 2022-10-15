using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingState : GroundedState
{
    private int walkParam = Animator.StringToHash("isWalking");
    public WalkingState(Player character, StateMachine stateMachine) : base(character, stateMachine)
    {
    }
    public override void Exit()
    {
        base.Exit();
        character.SetAnimationBool(walkParam, false);
    }
    public override void Enter()
    {
        base.Enter();
        speed = character.walkSpeed;
        character.SetAnimationBool(walkParam, true);
    }
    public override void HandleInput()
    {
        base.HandleInput();
       // direction = Input.GetAxisRaw("Horizontal");
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        //if (character.JumpQueued && character.CanMove)
        //{
        //    stateMachine.ChangeState(character.forwardJump);
        //}
        Utilities.Direction dir = direction == 1 ? Utilities.Direction.RIGHT : Utilities.Direction.LEFT;        
        
        if (character.CanMove)
		{
            if (direction != lastDirection && direction != 0)
			{
                stateMachine.ChangeState(character.idle);
                return;
            }
            if (direction != 0)
            {
                lastDirection = direction;
            }

            if (character.CanMoveInDirection(dir))
            {
                if (action)
                    stateMachine.ChangeState(character.running);
            }
            else
                stateMachine.ChangeState(character.idle);
            if (direction == 0)
                stateMachine.ChangeState(character.idle);

        }


    }
	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
        // Debug.Log(direction);

        if (direction != 0)
		{
            character.Move(direction, speed);
            lastDirection = direction;
        }
            
    }
}
