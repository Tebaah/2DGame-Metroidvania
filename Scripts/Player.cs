using Godot;
using System;
// using System.Numerics;

public partial class Player : CharacterBody2D
{
    // variables de movimeinto (fisica)
    [Export] public float speed;
    private const float JumpForce = 450;
    private const float DashForce = 2.5f;
    private int _jumpCounter = 0;

    // variable de fisica
    private float _gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    //  variables de estado
    private bool _canDash = false;
    private bool _dashing = false;

    // variable global
    private Global _global;

    // variable de armas y ataque
    private Weapons weapons;

    private string _typeSword;

    // metodos
    public override void _Ready()
    {
        _global = (Global)GetNode("/root/Global");
        weapons = GetNode<Weapons>("Crosshair/Weapons");

    }

    public override void _Process(double delta)
    {
        ChangeWeapons();
        LookAtMouse();
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

        Velocity = velocity;
        MoveAndSlide();

        // comportamiento del jugador otros TODO ver si es necesario desde aca 
        Attack();
    }

    private Vector2 Gravity(double delta, Vector2 velocity)
    {
        // si no esta en el suelo aplicara gravedad
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
        // modificara velocity en x en relacion a la direccion y la velocidad
        if(direction != 0)
        {
            if(Input.IsActionJustPressed("dash"))
            {
                _dashing = true;
                _canDash = true;
                CanDash();
            }
            if(_dashing == true && _canDash == true)
            {
                velocity.X = direction * speed * DashForce;
            }
            else
            {
                velocity.X = direction * speed;
            }
        }
        else
        {
            velocity.X = 0;
        }

        return velocity;
    }

    private Vector2 Jump(Vector2 velocity)
    {
        // si se presiona la tecla jump y esta en el suelo ejecutara el salto modificando velocity en y
        if(Input.IsActionJustPressed("jump") && IsOnFloor())
        {
            velocity.Y = -JumpForce;
            _jumpCounter = 1;
        }
        // si se presiona la tecla jump y no esta en el suelo y el contador de salto es 1 ejecutara el salto modificando velocity en y
        else if(Input.IsActionJustPressed("jump") && !IsOnFloor() && _jumpCounter == 1)
        {
            velocity.Y = -JumpForce;
            _jumpCounter = 0;
        }

        return velocity;
    }

    private Vector2 Dash(Vector2 velocity, float direction)
    {
        // si se presiona la tecla dash y puede hacer dash lo ejecutara modificando velocity en x
        if(Input.IsActionJustPressed("dash") && _canDash == true)
        {
            velocity.X = direction * speed * DashForce;
            CanDash();
        }

        return velocity;
    }

    public void Attack()
    {

        if(Input.IsActionJustPressed("physical attack"))
        {
            weapons.PhysicalAttack();
        }

        if(Input.IsActionJustPressed("ranged attack"))
        {
            weapons.RangedAttack();
        }
    }

    private async void CanDash()
    {
        // cambia el estado de dash despues de 1 segundo
        await ToSignal(GetTree().CreateTimer(.4), "timeout");
        _dashing = false;

        await ToSignal(GetTree().CreateTimer(1), "timeout");
        _canDash = false;
    }

    private void Dead()
    {
        if(_global.health <= 0)
        {
            GD.Print("Player dead");
        }
    }

    private void ChangeWeapons()
    {
        // ejecuta el cambio de armas en la clase weaponsSelection
        if(Input.IsActionJustPressed("changeweapons"))
        {
        }        
    }
    
    private void LookAtMouse()
    {  
        // determina la posicion del mouse y rota el personaje
        if (GetGlobalMousePosition().X > GlobalPosition.X)
        {
            Scale = new Vector2(1, 1);
            Rotation = 0;
        }
        else if(GetGlobalMousePosition().X < GlobalPosition.X)
        {
            Scale = new Vector2(-1, 1);
            Rotation = 0;
        }
    }
}