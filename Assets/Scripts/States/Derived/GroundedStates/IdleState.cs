using UnityEngine;

public class IdleState : GroundedState
{
    //private bool jump;
    //private bool crouch;
    private int idleParam = Animator.StringToHash("isIdle");
    public IdleState(Player character, StateMachine stateMachine) : base(character, stateMachine)
    {
    }
    public override void Enter()
    {
        base.Enter();
        //speed = character.movementSpeed;
        //rotationSpeed = character.RotationSpeed;
        //crouch = false;
        //jump = false;
        character.SetAnimationBool(idleParam, true);
        character.RemoveQueuedJump();
    }
	public override void Exit()
	{
		base.Exit();
        character.SetAnimationBool(idleParam, false);
    }
	public override void HandleInput()
    {
        base.HandleInput();
        //action = Input.GetKeyDown(Utilities.ActionKey);
        //crouch = Input.GetButtonDown("Fire3");
        //jump = Input.GetButtonDown("Jump");
        
        //jump = Input.GetKeyDown(KeyCode.X);
        
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        Utilities.Direction dir = direction == 1 ? Utilities.Direction.RIGHT : Utilities.Direction.LEFT;
  //      if (action && stateMachine.PreviousState == character.runningJump)
		//{
  //          stateMachine.ChangeState(character.running);
		//}
        if (downKey && character.CanMove)
        {
            if (action && character.CanClimbDown()) //climb down to ledge
            {
                stateMachine.ChangeState(character.climbingDown);
                //return;
            }
            else
			{
                stateMachine.ChangeState(character.crouching);
            }
        }
        else if (character.JumpQueued && character.CanMove)
        {
            dir = character.FacingDirection == 1 ? Utilities.Direction.RIGHT : Utilities.Direction.LEFT;
            if (action)
			{
                if (character.CanMoveInDirection(dir))
                    stateMachine.ChangeState(character.forwardJump);
                else
                    character.RemoveQueuedJump();
            }
            else
                stateMachine.ChangeState(character.standingJump);
            //return;
        }
        else if (direction != 0)
		{
            if (character.CanMove)
			{
                if (character.FacingDirection == direction && character.CanMoveInDirection(dir))
                {
                    if (action)
                        stateMachine.ChangeState(character.running);
                    else
                        stateMachine.ChangeState(character.walking);
                }
                else
				{
                    character.FaceDirection(direction);
				}
            }
		}
        else if (action)
		{
            character.AttemptInteraction();
		}
        //if (crouch)
        //{
        //    stateMachine.ChangeState(character.crouching);
        //}
        //if (jump)
        //{
        //    stateMachine.ChangeState(character.jumping);
        //}
    }

}
