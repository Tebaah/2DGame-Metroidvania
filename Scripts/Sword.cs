using Godot;
using System;

public partial class Sword : Area2D
{
    private AnimationPlayer _animationController;

    public override void _Ready()
    {
        _animationController = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    public override void _Process(double delta)
    {
        ;
        if(Input.IsActionJustPressed("attack"))
        {
            Attack();
        }
    }
    
    public void Attack()
    {
        _animationController.Play("attack");
    }
}
