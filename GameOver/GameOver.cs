using Godot;
using System;

public partial class GameOver : Node2D
{
	public void _on_retry_pressed()
	{
		GetTree().ChangeSceneToFile("res://Main/main.tscn");
	}
	
	public void _on_quit_pressed()
	{
		GetTree().Quit();
	}
}
