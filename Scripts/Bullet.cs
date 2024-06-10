using Godot;
using System;

public partial class Bullet : Area2D
{

    // movimiento de la bala
    private Vector2 _direction;
    private int _speed = 5;
    public override void _Ready()
    {
        _direction = Position.DirectionTo(GetGlobalMousePosition());
    }

    public override void _PhysicsProcess(double delta)
    {
        Position += _direction * _speed;
        Destroy();
    }

    public async void Destroy()
    {
        await ToSignal(GetTree().CreateTimer(3), "timeout");
        QueueFree();
    }



}
