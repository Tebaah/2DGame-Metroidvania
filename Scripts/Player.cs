using Godot;
using System;
// using System.Numerics;

public partial class Player : CharacterBody2D
{
    // variables de movimeinto (fisica)
    [Export] public float speed;
    private const float JumpForce = 300;
    private const float DashForce = 25f;

    // variable de fisica
    private float _gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    //  variables de estado
    private bool _canDash = true;

    // variable global
    private Global _global;

    // variable de armas y ataque
    private WeaponsSelection _weapons;
    private Sword _sword;

    // metodos
    public override void _Ready()
    {
        _global = (Global)GetNode("/root/Global");
        _weapons = GetNode<WeaponsSelection>("Weapons");
        _sword = new Sword();

    }

    public override void _Process(double delta)
    {
        ChangeWeapons();
        Dead();
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;

        float directionHorizontal = Input.GetAxis("left", "right");

        velocity = Gravity(delta, velocity);

        // comportamiento del jugador fisicas
        Idle();
        velocity = Move(velocity, directionHorizontal);
        velocity = Jump(velocity);
        velocity = Dash(velocity, directionHorizontal);

        Velocity = velocity;
        MoveAndSlide();

        // comportamiento del jugador otros TODO ver si es necesario desde aca 
        Attack();
    }

    private Vector2 Gravity(double delta, Vector2 velocity)
    {
        if (!IsOnFloor())
        {
            velocity.Y += _gravity * (float)delta;
        }

        return velocity;
    }

    public void Idle()
    {

    }

    private Vector2 Move(Vector2 velocity, float direction)
    {
        // float directionHorizontal = Input.GetAxis("left", "right");
        velocity.X = direction * speed;

        return velocity;
    }

    private Vector2 Jump(Vector2 velocity)
    {
        if(Input.IsActionJustPressed("jump") && IsOnFloor())
        {
            velocity.Y = -JumpForce;
        }

        return velocity;
    }

    private Vector2 Dash(Vector2 velocity, float direction)
    {
        if(Input.IsActionJustPressed("dash") && _canDash == true)
        {
            velocity.X = direction * speed * DashForce;
            CanDash();
        }

        return velocity;
    }

    public void Attack()
    {
        if(Input.IsActionJustPressed("attack"))
        {
            // _sword.Attack();
        }
    }

    private async void CanDash()
    {
        _canDash = false;
        await ToSignal(GetTree().CreateTimer(1), "timeout");
        _canDash = true;
    }

    private void Dead()
    {
        if(_global.lifePlayer <= 0)
        {
            GD.Print("Player dead");
        }
    }

    private void ChangeWeapons()
    {
        if(Input.IsActionJustPressed("changeweapons"))
        {
            _weapons.ChangeWeapons();
        }
        
    }

}