using Godot;
using System;

public partial class Weapons : Node2D
{
    // armas y ataque
    [Export] public PackedScene[] weapons;
    private Marker2D _spawnWeapon;

    // TODO configrar el contador de manera global
    private int _bulletCounter = 5;

    public override void _Ready()
    {
        _spawnWeapon = GetNode<Marker2D>("Marker2D");
    }

    public void PhysicalAttack()
    {
        // if(GetChildCount() > 1)
        // {
        //     GetChild(1).QueueFree();
        // }
        // instancia el arma seleccionada
        Area2D newWeapon = (Area2D)weapons[0].Instantiate();
        newWeapon.Position = _spawnWeapon.Position;  
        AddChild(newWeapon);

    }

    public void RangedAttack()
    {
        if(_bulletCounter > 0)
        {
            // instancia las balas 
            Area2D newWeapon = (Area2D)weapons[1].Instantiate();
            newWeapon.GlobalPosition = _spawnWeapon.GlobalPosition;
            FindParent("root").AddChild(newWeapon);

        }

        _bulletCounter--;

        GD.Print($"Counter: {_bulletCounter}");

        if(_bulletCounter == 0)
        {
            ActivateBullets();
        }

    }

    public async void ActivateBullets()
    {
        await ToSignal(GetTree().CreateTimer(3), "timeout");
        _bulletCounter = 5;
    }
}