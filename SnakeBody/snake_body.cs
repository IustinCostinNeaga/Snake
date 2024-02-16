using Godot;
using System;

public partial class snake_body : CharacterBody2D
{
	[Export] private int lenght;

	public void move(Vector2 pos)
	{
		Position = pos;
		
	}
}
