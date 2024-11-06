class Maze
{
    static Random random = new Random();

    // field
    static int width = 10;
    static int height = 12;
    static char[][] field = [];
    static int blockFreq = 28;

    // dog
    static char dog = '@';
    static int dogX = 0, dogY = 0;

    // input
    static int dX = 0, dY = 0;

    // finish
    static int finishX = 0, finishY = 0;
    static bool reachedFinish = false;

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
                    symbol = '#';
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

        field[finishY][finishX] = 'Д';
    }

    static void Draw()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                char symbol;

                if (i == dogY && j == dogX)
                {
                    symbol = dog;
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

    static void Generate()
    {
        GenerateField();
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

        if (firstSymbol == 'S' || firstSymbol == 's')
        {
            dY = 1;
        }

        if (firstSymbol == 'A' || firstSymbol == 'a')
        {
            dX = -1;
        }

        if (firstSymbol == 'D' || firstSymbol == 'd')
        {
            dX = 1;
        }
    }

    static bool IsEndGame()
    {
        return reachedFinish;
    }

    static bool IsWalkable(int x, int y)
    {
        if (field[y][x] == '#')
        {
            return false;
        }

        return true;
    }

    static bool CanGoTo(int newX, int newY)
    {
        if (newX < 0 || newY < 0 || newX >= width || newY >= height)
        {
            return false;
        }

        if (!IsWalkable(newX, newY))
        {
            return false;
        }

        return true;
    }

    static void GoTo(int newX, int newY)
    {
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

    static void Logic()
    {
        TryGoTo(dogX + dX, dogY + dY);

        CheckFinish();
    }

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
        
        Console.Write("УІІІІІ");
    }
}