using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Area : Godot.Area2D
{
    [Export] public PackedScene FruitScene { get; set; }
    [Export] public PackedScene SnakeBodyScene { get; set; }
    private int _amountOfApples = 0;
    private readonly List<Fruit> _appleArray = new();
    public Vector2 ScreenSize;
    public Label punteggio;
    int score = 0;
    private Snake snake;
    private Path2D path2D;
    private List<PathFollow2D> pathFollowList = new();
    private List<snake_body> SnakeBodies = new();

    public override void _Ready()
    {
        snake = GetNode<Snake>("Polygon2D/Snake");
        
        path2D = new Path2D();
        path2D.Curve = new Curve2D();
        AddChild(path2D);

        ScreenSize = GetViewportRect().Size;
        snake.Position = new Vector2(
            x: ScreenSize.X / 2,
            y: ScreenSize.Y / 2
        );
        GD.Print(snake.Position);

        punteggio = GetNode<Label>("Punteggio");
        punteggio.Text = "Punteggio: " + score.ToString();
        CreateNewRandomFruit();
    }

    public override void _Process(double delta)
    {
        if (path2D.Curve.PointCount == 0 || path2D.Curve.GetPointPosition(path2D.Curve.PointCount - 1) != snake.Position)
        {
            path2D.Curve.AddPoint(snake.Position);
        }
        snake.move(delta);
        SnakeBodyMove();
        foreach (var pathFollow2D in pathFollowList)
        {
            pathFollow2D.Progress += 500 * (float)delta;   
        }
    }

    void SnakeBodyMove()
    {
        for (int i = 0; i < SnakeBodies.Count; i++)
        {
                SnakeBodies[i].move(pathFollowList[i].Position);
        }
    }
    void CreateNewRandomFruit()
    {
        Fruit fruit = FruitScene.Instantiate<Fruit>();
        fruit.Position = new Vector2(
            x: (float)GD.RandRange(25, GetViewportRect().Size.X - 25),
            y: (float)GD.RandRange(25, GetViewportRect().Size.Y - 25)
        );
        fruit.BodyEntered += OnBodyEntered;
        fruit.ContactMonitor = true;
        fruit.MaxContactsReported = 1;
        AddChild(fruit);
        _amountOfApples++;
        _appleArray.Add(fruit);
    }

    void AddSnakeBody()
    {
        snake_body snakeBody = SnakeBodyScene.Instantiate<snake_body>();
        SnakeBodies.Add(snakeBody);
        pathFollowList.Add(new PathFollow2D());
        if (pathFollowList.Count <= 1)
        {
            pathFollowList[0].Progress = pathFollowList[0].ProgressRatio - 100f;
        }
        else
        {
            pathFollowList[^2].Progress -= - 100f;
        }
        snakeBody.Position = pathFollowList[^1].Position;
        path2D.AddChild(pathFollowList.Last());
        AddChild(snakeBody);
    }
    

    void RemoveFruit()
    {
        if (_appleArray.Count > 0)
        {
            RemoveChild(_appleArray[0]);
            _appleArray.RemoveAt(0);
            _amountOfApples--;
            score++;
            punteggio.Text = "Punteggio: " + score.ToString();
        }
    }

    public void OnBodyEntered(Node body)
    {
        GD.Print("Hello!");
        if (body.Name.Equals("Snake"))
        {
            CallDeferred(nameof(RemoveFruit));
            CallDeferred(nameof(CreateNewRandomFruit));
            CallDeferred(nameof(AddSnakeBody));
        }
    }

    public void _on_snake_game_over()
    {
        snake.stillPlaying= false;
        GD.Print("Hit");
        GetTree().ChangeSceneToFile("res://GameOver/GameOver.tscn");
    }
}