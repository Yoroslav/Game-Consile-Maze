using System;

class Game
{
    private const int Width = 10;
    private const int Height = 12;
    private const int BlockFreq = 28;

    private char[,] field = new char[Height, Width];
    private const char Dog = '@';
    private int dogX, dogY;
    private int dx, dy;
    private int finishX, finishY;
    private bool reachedFinish;

    static void Main()
    {
        var game = new Game();
        game.Run();
    }

    private void GenerateField()
    {
        var random = new Random();
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                field[i, j] = random.Next(100) < BlockFreq ? '#' : '.';
            }
        }

        finishX = random.Next(Width);
        finishY = random.Next(Height);
        field[finishY, finishX] = 'F';
    }

    private void Draw()
    {
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                DrawSymbol(i, j);
            }
            Console.WriteLine();
        }
    }

    private void DrawSymbol(int i, int j)
    {
        Console.Write(i == dogY && j == dogX ? Dog : field[i, j]);
    }

    private void PlaceDog()
    {
        var random = new Random();
        dogX = random.Next(Width);
        dogY = random.Next(Height);
    }

    private void Generate()
    {
        GenerateField();
        PlaceDog();
    }

    private void GetInput()
    {
        dx = dy = 0;
        Console.Write("(w/a/s/d): ");
        char input = Console.ReadKey().KeyChar;
        Console.WriteLine();

        switch (input)
        {
            case 'w': dy = -1; break;
            case 's': dy = 1; break;
            case 'a': dx = -1; break;
            case 'd': dx = 1; break;
        }
    }

    private bool IsEndGame() => reachedFinish;

    private bool IsWalkable(int x, int y) => field[y, x] != '#';

    private bool CanGoTo(int newX, int newY)
    {
        bool withinBounds = newX >= 0 && newY >= 0 && newX < Width && newY < Height;
        return withinBounds && IsWalkable(newX, newY);
    }

    private void TryGoTo(int newX, int newY)
    {
        if (CanGoTo(newX, newY))
        {
            GoTo(newX, newY);
        }
    }

    private void GoTo(int newX, int newY)
    {
        dogX = newX;
        dogY = newY;
    }

    private void CheckFinish()
    {
        if (dogX == finishX && dogY == finishY)
        {
            reachedFinish = true;
        }
    }

    private void UpdateGameLogic()
    {
        int newDogX = dogX + dx;
        int newDogY = dogY + dy;
        TryGoTo(newDogX, newDogY);
        CheckFinish();
    }

    public void Run()
    {
        Generate();
        while (!IsEndGame())
        {
            Draw();
            GetInput();
            UpdateGameLogic();
        }
        Console.WriteLine("УИИИИИИИИ");
    }
}
