using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

/*Credits :
 * John Leonard French for
 * https://gamedevbeginner.com/the-right-way-to-lerp-in-unity-with-examples/#right_way_to_use_lerp SmoothStep Function
*/
public class Player : MonoBehaviour
{
    public enum JumpType { STANDING, FORWARD, RUNNING };
    #region Variables
    [SerializeField]
    Tilemap highlightTilemap;
    [SerializeField]
    TileBase highlightTile;
    [SerializeField]
    TileBase ledgeHighLight;

    Animator animator;
    SpriteRenderer spriteRenderer;
    //[SerializeField] GameObject spriteObject;
    BoxCollider2D boxCollider;
    public float walkSpeed = 1f;
    public float runSpeed = 2f;

    [SerializeField] Tilemap collisionTilemap;
    [SerializeField] Tilemap ledgeTilemap;
    [SerializeField] Tilemap deathTilemap;
    //[SerializeField] Grid grid;
    [SerializeField] GridLayout gridLayout;


    Vector3Int grabbableledgePosition;

    #region States
    public StateMachine movementSM;
    public IdleState idle;


    public RunningJumpState runningJump;
    public StandingJumpState standingJump;
    public ForwardJumpState forwardJump;


    public WalkingState walking;
    public RunningState running;

    public FallingState falling;

    public ClimbingState climbing;
    public ClimbingDownState climbingDown;
    public HangingState hanging;

    public CrouchingState crouching;
    public RollingState rolling;

    public DeathState death;


    #endregion
    bool rollQueued = false;
    bool jumpQueued = false;
    bool runQueued = false;
    bool canMove = true;
    bool grounded = true;
    bool isJumping = false;
    float facingDirection = 1;


    #region AnimatorParams
    private int jumpParam = Animator.StringToHash("isJumping");
    #endregion
    #endregion

    #region Properties
    public float FacingDirection => facingDirection;
    public bool CanMoveInDirection(Utilities.Direction direction)
	{
        RaycastHit2D hit;
        int layermask = 1;
        switch (direction)
		{
            case Utilities.Direction.DOWN:
                hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.25f), Vector2.down, 1f, layermask);
                break;
            case Utilities.Direction.UP:
                hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.25f), Vector2.up, 1f, layermask);
                break;
            case Utilities.Direction.RIGHT:
                hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.25f), Vector2.right, 1f, layermask);
                break;
            case Utilities.Direction.LEFT:
                hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.25f), Vector2.left, 1f, layermask);
                break;
            default:
                hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.25f), Vector2.down, 1f, layermask);
                break;
        }
        return !collisionTilemap.HasWallInDirection(transform.position, direction, highlightTile, highlightTilemap) && hit.collider == null;
    }
    
        
    public bool CanMove => canMove;
    public bool RollQueued => rollQueued;
    public bool JumpQueued => jumpQueued;
    public bool RunQueued => runQueued;
    public bool IsGrounded => grounded;
    public bool IsJumping => isJumping;
    public int crouchParam => Animator.StringToHash("Crouch");
	#endregion

	void Start()
    {
        transform.position = gridLayout.GetGridCenterPos(transform.position);
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        movementSM = new StateMachine();

        idle = new IdleState(this, movementSM);
        crouching = new CrouchingState(this, movementSM);
        rolling = new RollingState(this, movementSM);

        standingJump = new StandingJumpState(this, movementSM);
        runningJump = new RunningJumpState(this, movementSM);
        forwardJump = new ForwardJumpState(this, movementSM);

        falling = new FallingState(this, movementSM);
        walking = new WalkingState(this, movementSM);
        running = new RunningState(this, movementSM);

        hanging = new HangingState(this, movementSM);
        climbing = new ClimbingState(this, movementSM);
        climbingDown = new ClimbingDownState(this, movementSM);

        death = new DeathState(this, movementSM);

        boxCollider = GetComponent<BoxCollider2D>();

        movementSM.Initialize(idle);
        grounded = CheckGrounded(transform.position);
    }
    public void Respawn()
	{
        ResetMoveParams();
        movementSM.ChangeState(idle);
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
        //if (deathTilemap.HasLedgeTileInDirection(transform.position, Utilities.Direction.DOWN, ledgeHighLight, highlightTilemap))
        //{
        //    movementSM.ChangeState(death);
        //}
        movementSM.CurrentState.HandleInput();
        movementSM.CurrentState.LogicUpdate();
    }

	void FixedUpdate()
	{
        movementSM.CurrentState.PhysicsUpdate();
    }

    public void Roll()
	{
        if (!canMove)
		{
            rollQueued = true;
            return;
        }
        StopAllCoroutines();
        Utilities.Direction moveDir = facingDirection == 1 ? Utilities.Direction.RIGHT : Utilities.Direction.LEFT;
        if (!CanMoveInDirection(moveDir))
		{
            rollQueued = false;
            return;
		}
        rollQueued = false;
        canMove = false;
        Vector3 targetPosition = gridLayout.GetGridCenterPos(new Vector3(transform.position.x + facingDirection*2, transform.position.y, transform.position.z));
        
        Vector3 inbetweenpos = new Vector3(transform.position.x + facingDirection * 1, transform.position.y, transform.position.z);
        bool wallhit2 = collisionTilemap.HasWallInDirection(inbetweenpos, moveDir, highlightTile, highlightTilemap);
        bool wallHit = collisionTilemap.HasWallInDirection(transform.position, moveDir, highlightTile, highlightTilemap);
        if (!wallHit)
		{
            if (!wallhit2)
			{
                StartCoroutine(MoveOverTime(0.25f, targetPosition, true));
            }
            else
                StartCoroutine(MoveOverTime(0.25f, inbetweenpos, true));
        }
        else
            canMove = true;
    }
    
    public void Move(float direction, float speed)
	{
        if (!canMove)
		{
            if (speed == runSpeed)
                runQueued = true;
            return;
        }
        grounded = CheckGrounded(transform.position); //make sure we are actually grounded
        if (!grounded || jumpQueued)
            return;
        canMove = false;
        runQueued = false;

        Utilities.Direction moveDir = direction == 1 ? Utilities.Direction.RIGHT : Utilities.Direction.LEFT;
        Vector3 targetPosition = gridLayout.GetGridCenterPos(new Vector3(transform.position.x + facingDirection, transform.position.y, transform.position.z));

        bool wallHit = collisionTilemap.HasWallInDirection(transform.position, moveDir, highlightTile, highlightTilemap);
        if (!wallHit)
        {
            wallHit = collisionTilemap.HasHalfTileInDirection(transform.position, moveDir, highlightTile, highlightTilemap);
            if (!wallHit)
            {
                if (speed == walkSpeed)
                    StartCoroutine(MoveOverTime(0.5f, targetPosition, true));
                if (speed == runSpeed)
                    StartCoroutine(MoveOverTime(0.25f, targetPosition, true));
            }
            else
                canMove = true;

        }
        else
            canMove = true;
	}
    public void FaceDirection(float direction)
	{
        if (facingDirection != direction) //turn around and face new direction
        {
            facingDirection = direction;
            spriteRenderer.flipX = facingDirection == -1 ? true : false;
            //spriteObject.transform.eulerAngles = facingDirection == -1 ? new Vector3(0, 180, 0) : Vector3.zero;
            StartCoroutine(MovementShortDisable());
        }
    }
    public void Fall()
	{
        if (grounded || !canMove)
            return;
        //Vector3Int cellPosition = collisionTilemap.WorldToCell(new Vector3(transform.position.x, transform.position.y - Utilities.unitSize, transform.position.z));
        Vector3 targetPosition = gridLayout.GetGridCenterPos(new Vector3(transform.position.x, transform.position.y - Utilities.playerMovementOffset, transform.position.z));
        //Vector3 targetPosition = collisionTilemap.GetCellCenterWorld(cellPosition);
        StartCoroutine(MoveOverTime(0.05f, targetPosition, false));
	}
	#region Jumping
	public void Jump()
    {
        Vector3 startPos = gridLayout.GetGridCenterPos(transform.position);
        //Vector3 startPos = collisionTilemap.GetTileCenterPos(transform.position);
        Vector3 endPos = gridLayout.GetGridCenterPos(new Vector3(startPos.x, startPos.y + Utilities.unitSize, startPos.z));
        //SetAnimationBool(jumpParam, true);
        StartCoroutine(MoveOverTime(0.5f, endPos, true));
        //Vector3 endPos = collisionTilemap.GetTileCenterPos(new Vector3(startPos.x, startPos.y + Utilities.unitSize, startPos.z));
        
    }
    public void Jump(JumpType jumpType)
	{
        isJumping = true;
        canMove = false;
        switch (jumpType)
		{
            case JumpType.STANDING: Jump(); break;
            case JumpType.FORWARD: ForwardJump(); break;
            case JumpType.RUNNING: RunningJump(); break;
		}
	}
    public void ForwardJump()
    {
        StopAllCoroutines();;
        transform.position = gridLayout.GetGridCenterPos(transform.position);

        Vector3 pos = transform.position;
        List <Vector3> positions = new List<Vector3>();

        var dir = facingDirection == 1 ? Utilities.Direction.RIGHT : Utilities.Direction.LEFT;

        if (!CanMoveInDirection(dir))    
        {
            canMove = true;
            isJumping = false;
            return;
        }
        positions.Add(pos);
        jumpQueued = false;

        int i = 1;
        while (i < 3)
		{
            Vector3 position = new Vector3(pos.x + i * facingDirection, pos.y, pos.z);
            bool positionBlocked = collisionTilemap.HasWallInDirection(positions[i-1], dir, highlightTile, highlightTilemap) || collisionTilemap.HasHalfTileInDirection(positions[i-1], dir, highlightTile, highlightTilemap);
            if (!positionBlocked)
                positions.Add(position);
            else
                break;
            i++;
        }
        if (positions.Count > 1)
		{
            SetAnimationBool(jumpParam, true);
            StartCoroutine(MoveBetweenMultiplePoints(0.25f, positions, true));
        }
        else
		{
			canMove = true;
			isJumping = false;
		}
    }
    public void RunningJump()
	{
        StopAllCoroutines();
        Vector3 startPos = transform.position;
        //transform.position = collisionTilemap.GetTileCenterPos(transform.position);
        transform.position = gridLayout.GetGridCenterPos(transform.position);
        List<Vector3> positions = new List<Vector3>();

        Vector3 abovePos = new Vector3(startPos.x, startPos.y + 1f, startPos.z);

        Vector3 pos1 = new Vector3(startPos.x + 1*facingDirection, startPos.y + 1, startPos.z);
        Vector3 pos2 = new Vector3(pos1.x + 1*facingDirection, pos1.y, pos1.z);
        //Vector3 pos3 = new Vector3(pos2.x + 1 * facingDirection, pos2.y - 1, pos2.z);
        Vector3 pos3 = new Vector3(pos2.x + 1*facingDirection, pos2.y, pos2.z);
        Vector3 pos4 = new Vector3(pos3.x + 1*facingDirection, pos3.y -1, pos3.z);

        positions.Add(startPos);

        Utilities.Direction moveDir = facingDirection == 1 ? Utilities.Direction.RIGHT : Utilities.Direction.LEFT;

        if (collisionTilemap.HasWallInDirection(startPos, moveDir, highlightTile, highlightTilemap) || collisionTilemap.HasWallInDirection(abovePos, moveDir, highlightTile, highlightTilemap)) //immediate block above or front
		{
            canMove = true;
            isJumping = false;
            return;
        }
        SetAnimationBool(jumpParam, true);
        positions.Add(pos1);

        if (collisionTilemap.HasWallInDirection(pos1, moveDir, highlightTile, highlightTilemap)) //mid-air block
		{
            StartCoroutine(MoveBetweenMultiplePoints(0.25f, positions, true));
            return;
        }
            
        positions.Add(pos2);

        if (collisionTilemap.HasWallInDirection(pos2, moveDir, highlightTile, highlightTilemap   )) //mid-air block
        {
            StartCoroutine(MoveBetweenMultiplePoints(0.25f, positions, true));
            return;
        }

        positions.Add(pos3);
        Vector3 belowPos = new Vector3(pos3.x, pos3.y - 1f, startPos.z);

        if (collisionTilemap.HasWallInDirection(belowPos, moveDir, highlightTile, highlightTilemap)) //end block
		{
			StartCoroutine(MoveBetweenMultiplePoints(0.25f, positions, true));
			return;
		}
		positions.Add(pos4);

		StartCoroutine(MoveBetweenMultiplePoints(0.25f, positions, true));
    }
	#endregion
	IEnumerator MoveBetweenMultiplePoints(float durationPerPoint, List<Vector3> points, bool smoothStep)
	{
        float timer = 0;
        for (int i = 1; i < points.Count; i++)
        {
            while (timer < durationPerPoint)
            {
                if (smoothStep)
                    transform.position = Utilities.SmoothStep(points[i - 1], points[i], timer, durationPerPoint); //smooth step for small increments
                else
                    transform.position = Vector3.Lerp(points[i - 1], points[i], timer / durationPerPoint);

                timer += Time.deltaTime;
                yield return null;
            }
            timer = 0;
            transform.position = points[i];
        }
        grounded = CheckGrounded(transform.position);
        ResetMoveParams();
    }
	#region Climbing
    public bool CanClimbDown()
	{
        RaycastHit2D hit;
        int layermask = 1;
        hit = Physics2D.Raycast(transform.position, Vector2.down, 1f, layermask);

        if (!hit)
            return false;
        if (hit.collider.gameObject == ledgeTilemap.gameObject)
		{
            grabbableledgePosition = ledgeTilemap.WorldToCell(hit.point);
            return true;
        }
        return false;
    }
    public bool CanClimbLedge()
    {
        RaycastHit2D hit, hit2;
        int layermask = 1;
        hit = Physics2D.Raycast(transform.position, Vector2.up, 1f, layermask);
        hit2 = Physics2D.Raycast(new Vector2(transform.position.x - facingDirection, transform.position.y), Vector2.up, 1f, layermask);

        if (!hit)
            return false;
        if (hit.collider.gameObject == ledgeTilemap.gameObject && hit2.collider == null)
        {
            grabbableledgePosition = ledgeTilemap.WorldToCell(hit.point);
            return true;
        }
        return false;
    }

    public void Climb()
	{
        StopAllCoroutines();
        canMove = false;
        //transform.position = collisionTilemap.GetTileCenterPos(transform.position);
        transform.position = gridLayout.GetGridCenterPos(grabbableledgePosition);
        //Vector3 targetPosition = new Vector3(transform.position.x + facingDirection, transform.position.y + 1, transform.position.z);
        Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        StartCoroutine(MoveOverTime(1, targetPosition, true));
	}
    public void ClimbDown()
    {
        StopAllCoroutines();
        canMove = false;
        //transform.position = collisionTilemap.GetTileCenterPos(transform.position);
        transform.position = gridLayout.GetGridCenterPos(transform.position);
        Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
        StartCoroutine(MoveOverTime(1, targetPosition, true));
    }
    #endregion

    IEnumerator MoveOverTime(float duration, Vector3 targetPos, bool smoothStep)
	{
        float timer = 0;
        // Vector3 startPos = collisionTilemap.GetTileCenterPos(transform.position);
        Vector3 startPos = gridLayout.GetGridCenterPos(transform.position);
        canMove = false;
        while (timer < duration)
		{
            if (smoothStep)
                transform.position = Utilities.SmoothStep(startPos, targetPos, timer, duration); //smooth step for small increments
            else
                transform.position = Vector3.Lerp(startPos, targetPos, timer/duration);

            timer += Time.deltaTime;
            yield return null;
		}
        transform.position = targetPos;
        transform.position = gridLayout.GetGridCenterPos(transform.position);
        grounded = CheckGrounded(transform.position);
        //Debug.Log(transform.position);
        if (!grounded)
		{
            jumpQueued = false;
            isJumping = false;
            runQueued = false;
        }
            
        canMove = true;
        SetAnimationBool(jumpParam, false);
    }
    public void ResetMoveParams()
	{
        jumpQueued = false;
        isJumping = false;
        canMove = true;
        SetAnimationBool(jumpParam, false);
    }
    public IEnumerator MovementShortDisable()
    {
        canMove = false;
        yield return new WaitForSeconds(0.1f);
        canMove = true;
    }
    public void AttemptInteraction()
	{
        int layerMask = 1 << 8; //interaction layer == 8
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, boxCollider.size, 0, layerMask);
        if (colliders.Length > 0)
		{
            foreach (var collider in colliders)
			{
                //Debug.Log(collider.name);
                var colliderScript = collider.GetComponent<InteractibleBase>();
                if (colliderScript)
				{
                    colliderScript.Interact(gameObject);
				}
			}
		}
	}
    public void StopMoving()
	{
        StopAllCoroutines();
        transform.position = gridLayout.GetGridCenterPos(transform.position);
        canMove = true;
        grounded = CheckGrounded(transform.position);
	}
    public void SetAnimationBool(int param, bool value) { animator.SetBool(param, value); }
    public void TriggerAnimation(int param) { animator.SetTrigger(param); }
    public void QueueJump() { jumpQueued = true; }
    public void QueueRoll() { rollQueued = true; }
    public void RemoveQueuedJump() { jumpQueued = false; }
    bool CheckGrounded(Vector3 position) => collisionTilemap.HasTileInDirection(position, Utilities.Direction.DOWN, 0.5f, highlightTile, highlightTilemap) || ledgeTilemap.HasTileInDirection(position, Utilities.Direction.DOWN, 0.5f, highlightTile, highlightTilemap);
}
