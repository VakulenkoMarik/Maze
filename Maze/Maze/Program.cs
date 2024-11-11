using System.Reflection.Metadata;

class Maze
{
    static Random random = new Random();

    // Supplement
    static int moves = 10;
    static string gameTitle = "";

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
    static bool endGame = false;

    // Jetpack
    static int jetpackX = 0, jetpackY = 0;
    static int jetpacksCount = 0;
    static bool jetpackElevated = false;

    static void Main(string[] args)
    {
        GoToMainMenu();
    }

    static void GoToMainMenu()
    {
        DrawMenuText();
        MainMenuLogic(MainMenuInput());
    }

    static char MainMenuInput()
    {
        string? inp = Console.ReadLine();

        if (string.IsNullOrEmpty(inp))
        {
            return MainMenuInput();
        }

        return inp[0];
    }

    static void MainMenuLogic(char symbol)
    {
        if (symbol == 'a')
        {
            CreateNewGame();
            StartNewGame();
        }
        else if (symbol == 'b')
        {
            SettingsMenuLogic();
            GoToMainMenu();
        }
        else if (symbol == 'c')
        {
            return;
        }
        else
        {
            GoToMainMenu();
        }
    }

    static void ColoredText(string text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(text);
    }

    static void SettingsMenuLogic()
    {
        gameTitle = "";

        if (!TrySetHeight(5, 50) || !TrySetWidth(5, 100) || !TrySetBlockFrequency(0, 90))
        {
            return;
        }

        gameTitle = "Success!";
    }

    static bool TrySetHeight(int min, int max)
    {
        string? input = ParameterInput("Enter height: ");
        
        if (!TryParseAndValidate(input, min, max, out int newHeight))
        {
            gameTitle = "Height error";
            return false;
        }

        height = newHeight;

        return true;
    }

    static bool TrySetWidth(int min, int max)
    {
        string? input = ParameterInput("Enter width: ");

        if (!TryParseAndValidate(input, min, max, out int newWidth))
        {
            gameTitle = "Width error";
            return false;
        }

        width = newWidth;

        return true;
    }

    static bool TrySetBlockFrequency(int min, int max)
    {
        string? input = ParameterInput("Enter wall frequency: ");

        if (!TryParseAndValidate(input, min, max, out int newBlockFreq))
        {
            gameTitle = "Frequency error";
            return false;
        }

        blockFreq = newBlockFreq;

        return true;
    }

    static string? ParameterInput(string text)
    {
        Console.Clear();
        Console.WriteLine(text);

        return Console.ReadLine();
    }

    static bool TryParseAndValidate(string? input, int min, int max, out int result)
    {
        result = 0;

        if (!int.TryParse(input, out result) || result < min || result > max)
        {
            return false;
        }

        return true;
    }

    static void DrawMenuText()
    {
        Console.Clear();

        Console.WriteLine(gameTitle);

        ColoredText("-\u0E4F MAZE \u0E4F- \n", ConsoleColor.Green);
        ColoredText("- Play (a) \n- Settings (b) \n- Exit (c)", ConsoleColor.Yellow);
    }

    static void CreateNewGame()
    {
        gameTitle = "";

        endGame = false;
        jetpackElevated = false;

        moves = 10;
    }

    static void StartNewGame()
    {
        Generate();
        Draw();

        while (!IsEndGame())
        {
            GetInput();
            Logic();
            Draw();
        }
        
        GoToMainMenu();
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

        ColoredText("Moves: " + moves, ConsoleColor.Blue);

        Console.ForegroundColor = ConsoleColor.White;

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
            endGame = true;
        }
    }

    static bool IsEndGame()
    {
        if (IsWin())
        {
            gameTitle = "WIN";
            endGame = true;
        }

        if (IsFail())
        {
            gameTitle = "FAIL";
            endGame = true;
        }
        
        return endGame;
    }

    static bool IsFail()
    {
        if (moves <= 0 && !endGame)
        {
            return true;
        }

        return false;
    }

    static bool IsWin()
    {
        if ((dogX, dogY) == (finishX, finishY))
        {
            return true;
        }

        return false;
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
    }
}