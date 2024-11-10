using System;

class Game
{
    const int Width = 10;
    const int Height = 12;
    const int BlockFreq = 28;
    const int MaxTurns = 20;

    char[,] field = new char[Height, Width];
    const char Dog = '@';
    const char Jetpack = 'J';
    int dogX, dogY;
    int dx, dy;
    int finishX, finishY;
    bool reachedFinish;
    bool hasJetpack = false;
    int turnsLeft = MaxTurns;

    static void Main()
    {
        var game = new Game();
        game.Run();
    }

    void GenerateField()
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

        int jetpackX = random.Next(Width);
        int jetpackY = random.Next(Height);
        field[jetpackY, jetpackX] = Jetpack;
    }

    void Draw()
    {
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                DrawSymbol(i, j);
            }
            Console.WriteLine();
        }
        Console.WriteLine($"Number of move: {turnsLeft}");
        if (hasJetpack) Console.WriteLine("You have a jetpack!");
    }

    void DrawSymbol(int i, int j)
    {
        Console.Write(i == dogY && j == dogX ? Dog : field[i, j]);
    }

    void PlaceDog()
    {
        var random = new Random();
        dogX = random.Next(Width);
        dogY = random.Next(Height);
    }

    void Generate()
    {
        GenerateField();
        PlaceDog();
    }

    void GetInput()
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

    bool IsEndGame() => reachedFinish || turnsLeft <= 0;

    bool IsWalkable(int x, int y) => field[y, x] != '#';

    bool CanGoTo(int newX, int newY)
    {
        bool withinBounds = newX >= 0 && newY >= 0 && newX < Width && newY < Height;
        return withinBounds && (IsWalkable(newX, newY) || hasJetpack);
    }

    void TryGoTo(int newX, int newY)
    {
        if (CanGoTo(newX, newY))
        {
            if (field[newY, newX] == '#')
            {
                hasJetpack = false;
            }
            GoTo(newX, newY);
        }
    }

    void GoTo(int newX, int newY)
    {
        dogX = newX;
        dogY = newY;

        if (field[dogY, dogX] == Jetpack)
        {
            hasJetpack = true;
            field[dogY, dogX] = '.';
            Console.WriteLine("You picked up a jetpack!");
        }
    }

    void CheckFinish()
    {
        if (dogX == finishX && dogY == finishY)
        {
            reachedFinish = true;
        }
    }

    void UpdateGameLogic()
    {
        int newDogX = dogX + dx;
        int newDogY = dogY + dy;
        TryGoTo(newDogX, newDogY);
        CheckFinish();
    }

    void Run()
    {
        Generate();
        while (!IsEndGame())
        {
            Draw();
            GetInput();

            if (dx != 0 || dy != 0)
            {
                UpdateGameLogic();
                turnsLeft--;
            }
        }
        if (reachedFinish)
        {
            Console.WriteLine("УИИИИИИИИ");
        }
        else
        {
            Console.WriteLine("Game over");
        }
    }
}
