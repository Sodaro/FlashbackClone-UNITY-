using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchingState : GroundedState
{
    private int crouchParam = Animator.StringToHash("isCrouching");
    public CrouchingState(Player character, StateMachine stateMachine) : base(character, stateMachine)
    {
    }
    public override void Enter()
    {
        base.Enter();
        //character.transform.localScale = new Vector3(0.5f, 0.5f, 1);
        character.SetAnimationBool(crouchParam, true);
    }
    public override void Exit()
    {
        base.Exit();
        character.SetAnimationBool(crouchParam, false);
        //character.SetAnimationBool(character.crouchParam, false);
    }
    public override void HandleInput()
    {
        base.HandleInput();
        //crouchHeld = Input.GetButton("Fire3");
    }
    public override void LogicUpdate()
    {
        //base.LogicUpdate();
        //if (!(crouchHeld || belowCeiling))
        if(!downKey && character.CanMoveInDirection(Utilities.Direction.UP))
        {
            stateMachine.ChangeState(character.idle);
            character.transform.localScale = new Vector3(1, 1, 1);
        }
        if (direction != 0 || character.RollQueued)
		{
            if (character.CanMove)
			{
                if (character.FacingDirection != direction)
                    character.FaceDirection(direction);
                else
                    stateMachine.ChangeState(character.rolling);
            }

		}
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
