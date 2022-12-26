//поле - двумерный массив размера N на M
//портал - объект, пренадлежит полю
//стены - объекты, прендалежат полю, по краям поля сделать стены
//герой - объект, НЕ пренадлежит полю, рисуется поверх полю

//1) генерация поля и стен на нём
//2) генерация портала в поле (перандомировать поле если мы оказались заперты)
//3) передвижение героя по полю
//4) вход героя в портал и переход к шагу 1

using ConsoleAppHeroAdventure;

static string Print(string[] mas, string[] mas1) 
{
    for (int i = 0; i < 4; i++)
    {
        Console.WriteLine($"   {mas[i]}: {mas1[i]}");
    }
    return " ";
}

Random random = new Random();

int currentLevel = 1;
int rows, cols;
Cell[,] field;

int CountMoney = 0;
int CountLiveMoney1 = 0;
int CountLiveMoney2 = 0;

int CountWonF = 0;
int CountWonS = 0;

int IFirstP, JFirstP;

bool heroInAdventure;

int currentWallPercent = (int)Constants.WallPercent;
int ISecondP, JSecondP;

string[] Management = {"Вверх", "Вправо", "Вниз", "Влево" };
string[] ManagementF = { "W", "D", "S", "A" };
string[] ManagementS = { "↑", "→", "↓", "<-" };
Console.WriteLine("Игра про двух путешественнкиво которые ищут выход и по пути собирают деньги.");
Console.WriteLine("Вы должны помочь им ");
Console.WriteLine();

Console.WriteLine("Управление за 1 игрока");
Print(Management, ManagementF);

Console.WriteLine("Управление за 2 игрока");
Print(Management, ManagementS);
Console.WriteLine();
Console.WriteLine("Нажмите на G для того чтобы помочь им");

if (Console.ReadKey(false).Key == ConsoleKey.G)
{
    while (true)
    {
        rows = random.Next((int)Constants.MinRows, (int)Constants.MaxRows + 1);
        cols = random.Next((int)Constants.MinCols, (int)Constants.MaxCols + 1);

        field = new Cell[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                field[i, j] = Cell.Empty;
            }
        }

        for (int i = 0; i < rows; i++)
        {
            field[i, 0] = Cell.Bound;
            field[i, cols - 1] = Cell.Bound;
        }

        for (int j = 0; j < cols; j++)
        {
            field[0, j] = Cell.Bound;
            field[rows - 1, j] = Cell.Bound;
        }

        IFirstP = (int)Constants.StartIHero;
        JFirstP = (int)Constants.StartJHero;

        ISecondP = random.Next(3, rows - 2);
        JSecondP = random.Next(3, cols - 2);

        int iPortal, jPortal;
        do
        {
            iPortal = random.Next(1, rows - 1);
            jPortal = random.Next(1, cols - 1);
        } while (iPortal == IFirstP && jPortal == JFirstP && iPortal == ISecondP && jPortal == JSecondP);

        field[iPortal, jPortal] = Cell.Portal;

        int countWalls = (int)((rows - 2) * (cols - 2) * currentWallPercent / 100.0);
        for (int i = 0; i < countWalls; i++)
        {
            int iWall, jWall;
            do
            {
                iWall = random.Next(1, rows - 1);
                jWall = random.Next(1, cols - 1);
            } while (iWall == IFirstP && jWall == JFirstP
                     || field[iWall, jWall] == Cell.Portal
                     || field[iWall, jWall] == Cell.Wall);

            field[iWall, jWall] = Cell.Wall;
        }

        int MOneyI, MOneyJ;

        do
        {
            MOneyI = random.Next(1, rows - 1);
            MOneyJ = random.Next(1, cols - 1);
        } while (MOneyI == IFirstP && MOneyI == JFirstP && MOneyI == ISecondP && MOneyI == JSecondP);

        int MOneyI2, MOneyJ2;

        do
        {
            MOneyI2 = random.Next(1, rows - 1);
            MOneyJ2 = random.Next(1, cols - 1);
        } while (MOneyI2 == IFirstP && MOneyI2 == JFirstP && MOneyI2 == ISecondP && MOneyI2 == JSecondP);

        heroInAdventure = true;
        while (heroInAdventure)
        {
            Console.Clear();

            Console.ResetColor();

            Console.WriteLine($"Current Level = {currentLevel}");
            Console.WriteLine($"Number of coins = {CountMoney}");
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (i == IFirstP && j == JFirstP && CountWonF == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write((char)Constants.FerstP);
                    } else if (i == MOneyI2 && j == MOneyJ2 && CountLiveMoney2 == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write((char)Cell.Money);
                    } else if (i == MOneyI && j == MOneyJ && CountLiveMoney1 == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write((char)Cell.Money);
                    }
                     else if (i == ISecondP && j == JSecondP && CountWonS == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write((char)Constants.SecondP);
                    }
                    else
                    {
                        switch (field[i, j])
                        {
                            case Cell.Empty:
                                Console.ForegroundColor = ConsoleColor.Gray;
                                break;
                            case Cell.Wall:
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                break;
                            case Cell.Portal:
                                Console.ForegroundColor = ConsoleColor.Blue;
                                break;
                            case Cell.Bound:
                                Console.ForegroundColor = ConsoleColor.Green;
                                break;

                        }
                        Console.Write((char)field[i, j]);
                    }
                }

                Console.WriteLine();
            }

            ConsoleKey key = Console.ReadKey(false).Key;
            switch (key)
            {
                case ConsoleKey.A:
                    if (field[IFirstP, JFirstP - 1] == Cell.Empty || field[IFirstP, JFirstP - 1] == Cell.Portal)
                    {
                        JFirstP--;
                    }
                    break;

                case ConsoleKey.W:
                    if (field[IFirstP - 1, JFirstP] == Cell.Empty || field[IFirstP - 1, JFirstP] == Cell.Portal)
                    {
                        IFirstP--;
                    }
                    break;

                case ConsoleKey.D:
                    if (field[IFirstP, JFirstP + 1] == Cell.Empty || field[IFirstP, JFirstP + 1] == Cell.Portal)
                    {
                        JFirstP++;
                    }
                    break;

                case ConsoleKey.S:
                    if (field[IFirstP + 1, JFirstP] == Cell.Empty || field[IFirstP + 1, JFirstP] == Cell.Portal)
                    {
                        IFirstP++;
                    }
                    break;

                case ConsoleKey.R:
                    heroInAdventure = false;
                    break;
            }

            switch (key)
            {
                case ConsoleKey.LeftArrow:
                    if (field[ISecondP, JSecondP - 1] == Cell.Empty || field[ISecondP, JSecondP - 1] == Cell.Portal)
                    {
                        JSecondP--;
                    }

                    break;

                case ConsoleKey.UpArrow:
                    if (field[ISecondP - 1, JSecondP] == Cell.Empty || field[ISecondP - 1, JSecondP] == Cell.Portal)
                    {
                        ISecondP--;
                    }
                    break;

                case ConsoleKey.RightArrow:
                    if (field[ISecondP, JSecondP + 1] == Cell.Empty || field[ISecondP, JSecondP + 1] == Cell.Portal)
                    {
                        JSecondP++;
                    }
                    break;

                case ConsoleKey.DownArrow:
                    if (field[ISecondP + 1, JSecondP] == Cell.Empty || field[ISecondP + 1, JSecondP] == Cell.Portal)
                    {
                        ISecondP++;
                    }

                    break;
            }

            if (MOneyI == IFirstP && MOneyJ == JFirstP || MOneyI == ISecondP && MOneyJ == JSecondP ) 
            {
                CountMoney++;
                CountLiveMoney1++;

            }

            if (MOneyI2 == IFirstP && MOneyJ2 == JFirstP || MOneyI2 == ISecondP && MOneyJ2 == JSecondP) 
            {
                CountMoney++;
                CountLiveMoney2++;
            }

            if (field[IFirstP, JFirstP] == Cell.Portal)
            {
                CountWonF = 1;
            }
            if (field[ISecondP, JSecondP] == Cell.Portal)
            {
                CountWonS = 1;
            }
            if (CountWonS + CountWonF == 2)
            {
                currentLevel++;
                currentWallPercent += 2;
                heroInAdventure = false;
                CountWonS = 0;
                CountWonF = 0;
                CountLiveMoney1 = 0;
                CountLiveMoney2 = 0;
            }
        }
    }
}