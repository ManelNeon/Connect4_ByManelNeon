using Godot;
using System;

public partial class RowController : Area2D
{
	bool isInside;
	[Export] int rowNumber;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (isInside)
		{
			if (GameManager.Instance.isPlaying && Input.IsActionPressed("right_click"))
			{
				GameManager.Instance.AddingBall(rowNumber);
			}
		}
	}
	
	public override void _EnterTree()
	{
		MouseEntered += OnMouseEntered;
		MouseExited += OnMouseExit;
	}
	
	void OnMouseEntered()
	{
		isInside = true;
	}

	void OnMouseExit()
	{
		isInside = false;
	}
}
