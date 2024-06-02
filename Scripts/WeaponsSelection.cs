using Godot;
using System;

public partial class WeaponsSelection : Node2D
{
    // variables de armas
    [Export] public PackedScene[] weapons;
    private int _currentWeapon = 0;
    private Marker2D _spawnWeapon;
    
    public override void _Ready()
    {
        _spawnWeapon = GetNode<Marker2D>("Marker2D");
    }

    public void ChangeWeapons()
    {
        if(GetChildCount() > 1)
        {
            GetChild(1).QueueFree();
        }

        Area2D newWeapon = (Area2D)weapons[_currentWeapon].Instantiate();
        newWeapon.Position = _spawnWeapon.Position;
        AddChild(newWeapon);
        
        if(_currentWeapon == 0)
        {
            _currentWeapon = 1;
        }
        else
        {
            _currentWeapon = 0;
        }
    }
}
