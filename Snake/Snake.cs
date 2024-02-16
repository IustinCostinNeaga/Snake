using Godot;
using System;
using System.Numerics;
using Vector2 = Godot.Vector2;

public partial class Snake : CharacterBody2D
{
	[Export] public int Speed { get; set; } = 10;
	[Signal] public delegate void gameOverEventHandler();
	
	public bool stillPlaying = true;
	private Vector2 velocity = Vector2.Zero;
	
	public void move(double delta)
	{
		
		
			if (Input.IsActionJustPressed("tasto_su"))
			{
				velocity.Y = -1;
				velocity.X = 0;
			}
			else if (Input.IsActionJustPressed("tasto_giu"))
			{
				velocity.Y = 1;
				velocity.X = 0;
			}
			else if (Input.IsActionJustPressed("tasto_sinistra"))
			{
				velocity.X = -1;
				velocity.Y = 0;
			}
			else if (Input.IsActionJustPressed("tasto_destra"))
			{
				velocity.X = 1;
				velocity.Y = 0;
			}
			
		if (velocity.Length() > 0 && stillPlaying)
		{
			velocity = velocity.Normalized() * Speed;
			var collider = MoveAndCollide(velocity);
			if (collider != null)
			{
				var collision = ((Node)collider.GetCollider()).Name;
				GD.Print(collision.ToString());
				if (collision == "SnakeBody" || collision.ToString().Contains("Muro") || ((Node)collider.GetCollider() is CharacterBody2D))
				{
					GD.Print("Emmiting signal");
					EmitSignal(SignalName.gameOver);
				}
			}
		}
	}
}
