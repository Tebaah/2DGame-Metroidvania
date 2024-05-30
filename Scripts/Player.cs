using Godot;
using System;

public partial class Player : CharacterBody2D
{
    // variables de movimeinto (fisica)
    [Export] public float speed;
    private const float JumpForce = 300;
    private const float DashForce = 2.5f;

    // variable de fisica
    private float _gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    //  variables de estado
    private bool _dashing = false;
    private bool _canDash = true;
    private bool _isJumping = false;

    //  variables de animacion
    private AnimatedSprite2D _animationController;
    private AnimationPlayer _animationWeapon;

    public override void _Ready()
    {
        _animationController = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _animationWeapon = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    public override void _Process(double delta)
    {
        Dash();
        Animation();
        Jump();
        Attack();
    }

    public override void _PhysicsProcess(double delta)
    {
        Movement(delta);

        // LookAt(GetGlobalMousePosition());
    }

    public void Movement(double delta)
    {
        Vector2 velocity = Velocity;

        if(!IsOnFloor())
        {
            velocity.Y += _gravity * (float)delta;
        }

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

        if(_isJumping == true && IsOnFloor())
        {
            velocity.Y = -JumpForce;
            _isJumping = false;
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

    public void Jump()
    {
        if(Input.IsActionJustPressed("jump"))
        {
            _isJumping = true;
        }
    }

    public void Attack()
    {
        if(Input.IsActionJustPressed("attack"))
        {
            if(GetGlobalMousePosition().X < Position.X)
            {
                _animationWeapon.Play("attackleft");
            }  
            else
            {
                _animationWeapon.Play("attack");
            }
        }
    }
    public void Animation()
    {
        if(_dashing == true)
        {
            _animationController.Play("dash");
            if(GetGlobalMousePosition().X < Position.X)
            {   
                _animationController.FlipH = true;
            }
            else
            {
                _animationController.FlipH = false;
            }
        }
        else
        {
            _animationController.Play("idle");
            if(GetGlobalMousePosition().X < Position.X)
            {   
                _animationController.FlipH = true;
            }
            else
            {
                _animationController.FlipH = false;
            }
        }
    }
}
