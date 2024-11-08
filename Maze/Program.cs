class Maze
{
    static Random random = new Random();

    // Supplement
    static int moves = 10;

    // Symbols
    static char wall = '#';
    static char dog = '@';
    static char jetpack = 'M';
    static char finish = 'Д';

    // Field
    static int width = 10, height = 12;
    static int blockFreq = 28;
    static char[][] field = [];

    // Dog
    static int dogX = 0, dogY = 0;

    // Input
    static int dX = 0, dY = 0;

    // Finish
    static int finishX = 0, finishY = 0;
    static bool reachedFinish = false;

    // Jetpack
    static int jetpackX = 0, jetpackY = 0;
    static int jetpacksCount = 0;
    static bool jetpackElevated = false;

    static void Main(string[] args)
    {
        Generate();
        Draw();

        while (!IsEndGame())
        {
            GetInput();
            Logic();
            Draw();
        }
        
        Console.Write("The end");
    }

    static void GenerateField()
    {
        field = new char[height][];
        
        for (int i = 0; i < height; i++)
        {
            field[i] = new char[width];

            for (int j = 0; j < width; j++)
            {
                int randNum = random.Next(0, 100);
                char symbol;

                if (randNum < blockFreq)
                {
                    symbol = wall;
                }
                else
                {
                    symbol = '.';
                }

                field[i][j] = symbol;
            }
        }

        finishX = random.Next(0, width - 1);
        finishY = random.Next(0, height - 1);

        field[finishY][finishX] = finish;
    }

    static void Draw()
    {
        Console.Clear();
        Console.WriteLine("Moves: " + moves);

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                char symbol;

                if (i == dogY && j == dogX)
                {
                    symbol = dog;
                }
                else if (i == jetpackY && j == jetpackX && !jetpackElevated)
                {
                    symbol = jetpack;
                }
                else
                {
                    symbol = field[i][j];
                }

                Console.Write(symbol);
            }

            Console.WriteLine();
        }
    }

    static void PlaceDog()
    {
        dogX = random.Next(0, width - 1);
        dogY = random.Next(0, height - 1);
    }

    static void PlaceJetpack()
    {
        jetpackX = random.Next(0, width - 1);
        jetpackY = random.Next(0, height - 1);

        if ((jetpackX, jetpackY) == (finishX, finishY))
        {
            PlaceJetpack();
        }
        else
        {
            field[jetpackY][jetpackX] = jetpack;
        }
    }

    static void Generate()
    {
        GenerateField();

        PlaceJetpack();
        PlaceDog();
    }

    static void GetInput()
    {
        (dX, dY) = (0, 0);

        string? inp = Console.ReadLine();

        if (string.IsNullOrEmpty(inp))
        {
            return;
        }

        char firstSymbol = inp[0];

        if (firstSymbol == 'W' || firstSymbol == 'w')
        {
            dY = -1;
        }
        else if (firstSymbol == 'S' || firstSymbol == 's')
        {
            dY = 1;
        }
        else if (firstSymbol == 'A' || firstSymbol == 'a')
        {
            dX = -1;
        }
        else if (firstSymbol == 'D' || firstSymbol == 'd')
        {
            dX = 1;
        }
        else if (firstSymbol == 'E') // Exit
        {
            reachedFinish = true;
        }
    }

    static bool IsEndGame()
    {
        if (moves <= 0)
        {
            return true;
        }

        return reachedFinish;
    }

    static bool IsWalkable(int x, int y)
    {
        if (field[y][x] == wall)
        {
            return false;
        }

        return true;
    }

    static bool CanUseJetpack()
    {
        if (jetpacksCount > 0)
        {
            return true;
        }

        return false;
    }

    static void UseJetpack()
    {
        jetpacksCount -= 1;
    }

    static bool CanGoTo(int newX, int newY)
    {
        if (newX < 0 || newY < 0 || newX >= width || newY >= height)
        {
            return false;
        }

        if (!IsWalkable(newX, newY))
        {
            if (CanUseJetpack())
            {
                UseJetpack();
                
                return true;
            }

            return false;
        }

        return true;
    }

    static void GoTo(int newX, int newY)
    {
        moves--;

        dogX = newX;
        dogY = newY;
    }

    static void TryGoTo(int newX, int newY)
    {
        if (CanGoTo(newX, newY))
        {
            GoTo(newX, newY);
        }
    }

    static void CheckFinish()
    {
        if (dogX == finishX && dogY == finishY)
        {
            reachedFinish = true;
        }
    }

    static void CheckJetpackSelection()
    {
        if (dogX == jetpackX && dogY == jetpackY)
        {
            jetpackElevated = true;
            jetpacksCount += 1;

            field[jetpackY][jetpackX] = '.';
        }
    }

    static void Logic()
    {
        TryGoTo(dogX + dX, dogY + dY);

        CheckJetpackSelection();
        CheckFinish();
    }
}