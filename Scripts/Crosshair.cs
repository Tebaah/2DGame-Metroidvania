using Godot;
using System;

public partial class Crosshair : Node2D
{
 
    public override void _Process(double delta)
    {
        LookAt(GetGlobalMousePosition());
    }
    
}
