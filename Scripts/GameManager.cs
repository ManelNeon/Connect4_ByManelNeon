using Godot;
using System;
using System.Threading.Tasks;

public partial class GameManager : Node
{
	public static GameManager Instance;
	
	public bool isPlaying;
	
	public bool isRedTurn;
	
	[Export] AudioStreamPlayer2D ballPut;
	
	[Export] AudioStreamPlayer2D playerChange;
	
	[Export] AudioStreamPlayer2D winSound;
	
	[Export] Panel bluePlaying;
	
	[Export] Panel redPlaying;
	
	[Export] TextEdit editableText;
	
	[Export] PackedScene blueBall;
	
	[Export] PackedScene redBall;
	
	[Export] Node2D[] spawnColumn0;
	
	[Export] Node2D[] spawnColumn1;
	
	[Export] Node2D[] spawnColumn2;
	
	[Export] Node2D[] spawnColumn3;
	
	[Export] Node2D[] spawnColumn4;
	
	[Export] Node2D[] spawnColumn5;
	
	[Export] Node2D[] spawnColumn6;
	
	//first comes the columns and then the rows, 2 for blue, 1 for red, 0 for empty
	public int[,] digitalBoard = new int[7,6];
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (Instance != null)
		{
			QueueFree();
			return;
		}
		
		Instance = this;
		
		isPlaying = true;
	}
	
	public void AddingBall(int column)
	{	
		for (int i = 0; i < 6; i++)
		{
			if (digitalBoard[column, i] == 0)
			{
				if (isRedTurn)
				{
					digitalBoard[column,i] = 1;
				}
				else 
				{
					digitalBoard[column,i] = 2;
				}
				DelayMethod();
				PuttingBall(column, i);
				CheckingVictory(column, i, digitalBoard[column,i]);
				break;
			}
		}
	}
	
	void ChangingPlayer()
	{
		playerChange.Play();
		isRedTurn = !isRedTurn;
		if (isRedTurn)
		{
			redPlaying.Visible = true;
			bluePlaying.Visible = false;
		}
		else 
		{
			bluePlaying.Visible = true;
			redPlaying.Visible = false;
		}
	}
	
	void WonPlayer()
	{
		winSound.Play();
		isPlaying = false;
		editableText.Text = "WINNER IS";
	}
	
	void PuttingBall(int column, int row)
	{
		ballPut.Play();
		Node ballInstance;
		if (!isRedTurn)
		{
			ballInstance = blueBall.Instantiate();
		}
		else 
		{
			ballInstance = redBall.Instantiate();
		}
		AddChild(ballInstance);
		switch(column)
		{
			case 0: 
				ballInstance.GetNode<Node2D>("BlueBall").MoveLocalY(spawnColumn0[row].GlobalPosition.Y, false);
				ballInstance.GetNode<Node2D>("BlueBall").MoveLocalX(spawnColumn0[row].GlobalPosition.X, false);
				return;
			case 1: 
				ballInstance.GetNode<Node2D>("BlueBall").MoveLocalY(spawnColumn1[row].GlobalPosition.Y, false);
				ballInstance.GetNode<Node2D>("BlueBall").MoveLocalX(spawnColumn1[row].GlobalPosition.X, false);
				return;
			case 2: 
				ballInstance.GetNode<Node2D>("BlueBall").MoveLocalY(spawnColumn2[row].GlobalPosition.Y, false);
				ballInstance.GetNode<Node2D>("BlueBall").MoveLocalX(spawnColumn2[row].GlobalPosition.X, false);
				return;
			case 3: 
				ballInstance.GetNode<Node2D>("BlueBall").MoveLocalY(spawnColumn3[row].GlobalPosition.Y, false);
				ballInstance.GetNode<Node2D>("BlueBall").MoveLocalX(spawnColumn3[row].GlobalPosition.X, false);
				return;
			case 4: 
				ballInstance.GetNode<Node2D>("BlueBall").MoveLocalY(spawnColumn4[row].GlobalPosition.Y, false);
				ballInstance.GetNode<Node2D>("BlueBall").MoveLocalX(spawnColumn4[row].GlobalPosition.X, false);
				return;
			case 5:
				ballInstance.GetNode<Node2D>("BlueBall").MoveLocalY(spawnColumn5[row].GlobalPosition.Y, false);
				ballInstance.GetNode<Node2D>("BlueBall").MoveLocalX(spawnColumn5[row].GlobalPosition.X, false);
				return;
			case 6:
				ballInstance.GetNode<Node2D>("BlueBall").MoveLocalY(spawnColumn6[row].GlobalPosition.Y, false);
				ballInstance.GetNode<Node2D>("BlueBall").MoveLocalX(spawnColumn6[row].GlobalPosition.X, false);
				return;
			default: return;
		}
	}
	
	private async void DelayMethod()
	{
		isPlaying = false;
		await Task.Delay(TimeSpan.FromMilliseconds(2000));
		if (editableText.Text != "WINNER IS")
		{
			ChangingPlayer();
			isPlaying = true;
		}
	}
	
	void CheckingVictory(int column, int row, int ballCode)
	{
		int count = 1;
		
		//Verifying if it matches to the left
		for (int i = column - 1; i >= 0; i--)
		{
			if (digitalBoard[i,row] == ballCode)
			{
				count++;
				if (count == 4)
				{
					GD.Print("WON");
					WonPlayer();
					return;
				}
			}
			else 
			{
				break;
			}
		}
		//Verifying if it matches to the right
		for (int i = column + 1; i <= 6; i++)
		{
			if (digitalBoard[i,row] == ballCode)
			{
				count++;
				if (count == 4)
				{
					GD.Print("WON");
					WonPlayer();
					return;
				}
			}
			else 
			{
				break;
			}
		}
		
		count = 0;
		//Verifying if it matches downwards
		for (int i = row; i >= 0; i--)
		{
			if (digitalBoard[column,i] == ballCode)
			{
				count++;
				if (count == 4)
				{
					GD.Print("WON");
					WonPlayer();
					return;
				}
			}
			else 
			{
				count = 0;
				break;
			}
		}
		
		count = 1;
		//Verifying if it matches sideways to the right
		for (int i = 1; i < 4; i++)
		{
			if (column + i > 6)
			{
				break;
			}
			if (row + i > 5)
			{
				break;
			}
			if (digitalBoard[column + i, row + i] == ballCode)
			{
				count++;
				if (count == 4)
				{
					GD.Print("WON");
					WonPlayer();
					return;
				}
			}
			else 
			{
				break;
			}
		}
		for (int i = 1; i < 4; i++)
		{
			if (column - i < 0)
			{
				break;
			}
			if (row - i < 0)
			{
				break;
			}
			if (digitalBoard[column - i, row - i] == ballCode)
			{
				count++;
				if (count == 4)
				{
					GD.Print("WON");
					WonPlayer();
					return;
				}
			}
			else 
			{
				break;
			}
		}
		
		count = 1;
		//Verifying if it matches sideways to the left
		for (int i = 1; i < 4; i++)
		{
			if (column + i > 6)
			{
				break;
			}
			if (row - i < 0)
			{
				break;
			}
			if (digitalBoard[column + i, row - i] == ballCode)
			{
				count++;
				if (count == 4)
				{
					GD.Print("WON");
					WonPlayer();
					return;
				}
			}
			else 
			{
				break;
			}
		}
		for (int i = 1; i < 4; i++)
		{
			if (column - i < 0)
			{
				break;
			}
			if (row + i > 5)
			{
				break;
			}
			if (digitalBoard[column - i, row + i] == ballCode)
			{
				count++;
				if (count == 4)
				{
					GD.Print("WON");
					WonPlayer();
					return;
				}
			}
			else 
			{
				break;
			}
		}
	}
}
