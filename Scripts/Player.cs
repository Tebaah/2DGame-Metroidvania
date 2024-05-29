using Godot;
using System;

public partial class Player : CharacterBody2D
{
    // variables de movimeinto (fisica)
    [Export] public float speed;
    private const float JumpForce = 50;
    private const float DashForce = 2.5f;

    //  variables de estado
    private bool _dashing = false;
    private bool _canDash = true;

    //  variables de animacion
    private AnimatedSprite2D _animationController;

    public override void _Ready()
    {
        _animationController = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }

    public override void _Process(double delta)
    {
        Dash();
        Animation();
    }

    public override void _PhysicsProcess(double delta)
    {
        Movement(delta);
    }

    public void Movement(double delta)
    {
        Vector2 velocity = Velocity;

        float directionHorizontal = Input.GetAxis("left", "right");
        if(directionHorizontal != 0)
        {
            if(_dashing == true)
            {
                velocity.X = directionHorizontal * speed * DashForce;
            }
            else
            {
                velocity.X = directionHorizontal * speed;
            }
        }
        else
        {
            velocity.X = 0;
        }

        Velocity = velocity;
        MoveAndSlide();

    }
    public async void Dash()
    {
        if(Input.IsActionJustPressed("dash") && _canDash == true)
        {
            _dashing = true;
            _canDash = false;

            await ToSignal(GetTree().CreateTimer(0.5f), "timeout");
            _dashing = false;

            await ToSignal(GetTree().CreateTimer(2), "timeout");
            _canDash = true;
        }
    }
    public void Animation()
    {
        if(_dashing == true)
        {
            _animationController.Play("dash");
            _animationController.FlipH = Velocity.X < 0;
        }
        else
        {
            _animationController.Play("idle");
            _animationController.FlipH = Velocity.X < 0;
        }
    }
}
