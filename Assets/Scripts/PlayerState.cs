using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Player player;
    protected Rigidbody2D rb;

    protected float yInput;
    protected float xInput;
    private string animBoolName;

    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        player.anim.SetBool(animBoolName, true);
        rb = player.rb;
    }

    public virtual void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        // If there's any input, update the lastDirection in the Player script.
        if (Mathf.Abs(xInput) > 0.1f || Mathf.Abs(yInput) > 0.1f)
        {
            player.lastDirection = new Vector2(xInput, yInput).normalized;
        }
        else
        {
            // When idle, feed the animator the last nonzero direction instead of (0,0)
            xInput = player.lastDirection.x;
            yInput = player.lastDirection.y;
        }

        // Update animator parameters using the possibly modified xInput/yInput values.
        player.anim.SetFloat("moveX", xInput);
        player.anim.SetFloat("moveY", yInput);

        bool isMoving = Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.1f ||
                        Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0.1f;
        player.anim.SetBool("Move", isMoving);

        // Update yVelocity for jumping/falling animations.
        player.anim.SetFloat("yVelocity", rb.linearVelocity.y);
    }

    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);
    }
}
