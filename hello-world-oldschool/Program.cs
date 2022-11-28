using System.Collections.Generic;

namespace hello_world_oldschool;
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to my excellent app!");
        DumboOctopus();
        Console.WriteLine("Please input your username: ");
        string? userName;

        userName = Console.ReadLine();
        Console.WriteLine("Welcome " + userName + "!");
        Console.WriteLine();

        bool mainMenu = true;
        while (mainMenu)
        {
            Console.WriteLine("Please select one of the following");
            Console.WriteLine("1. Chessboard");
            Console.WriteLine("2. Dumbo Octopus");
            Console.WriteLine("E. Exit");
            Console.Write("----> ");
            string? choice = Console.ReadLine().ToUpper();
            switch (choice)
            {
                case "1":
                    Console.WriteLine("1 selected");
                    ChessBoard();
                    Console.WriteLine();
                    break;
                case "2":
                    Console.WriteLine("2 selected");
                    DumboOctopus();
                    Console.WriteLine();
                    break;
                case "E":
                    Console.WriteLine("E selected");
                    Console.WriteLine();
                    mainMenu = false;
                    break;
                default:
                    Console.WriteLine("Please type either 1 or E and press enter");
                    Console.WriteLine();
                    break;

            }
        }
        Console.WriteLine("Thank you for using Menu System 2.0. Please come again");
    }

    static void DumboOctopus()
    {
        Console.WriteLine("Welcome to Dumbo Octopus");
        String input = File.ReadAllText(@"data.txt");
        int[,] octopus = new int[10, 10];
        int j = 0;

        foreach (var row in input.Split('\n'))
        {
            var intArray = row.Select(c => c - '0').ToArray();
            for (int i = 0; i < intArray.Count(); i++)
            {
                octopus[j, i] = intArray[i];
            }
            j++;
        }
        /*
        int[,] octopus = new int[5, 5]{
                                {1, 1, 1, 1, 1},
                                {1, 9, 9, 9, 1},
                                {1, 9, 9, 9, 1},
                                {1, 9, 9, 9, 1},
                                {1, 1, 1, 1, 1}
                            };
*/
        printBoard(octopus);
        /*
         * First, the energy level of each octopus increases by 1.
        Then, any octopus with an energy level greater than 9 flashes. This increases the energy level of all adjacent octopuses by 1, including octopuses that are diagonally adjacent. If this causes an octopus to have an energy level greater than 9, it also flashes. This process continues as long as new octopuses keep having their energy level increased beyond 9. (An octopus can only flash at most once per step.)
        Finally, any octopus that flashed during this step has its energy level set to 0, as it used all of its energy to flash.
        */
        bool getInput = true;
        int value = 0;
        while (getInput)
        {
            Console.Write("Please indicate how many steps to run the simulation for ---> ");
            bool correctInput = int.TryParse(Console.ReadLine(), out value);
            if (!correctInput)
            {
                Console.WriteLine("Please enter a valid number.");
            }
            else
            {
                getInput = false;
            }

        }
        Console.WriteLine("You entered " + value + " steps.");
        Console.WriteLine("===================================");
        bool keepScanning = true;
        int totalFlashes = 0;
        int flashedThisRound = 0;
        int flashedPreviousRound = 0;
        Console.WriteLine();
        //Console.WriteLine("Step number: 0");
        // octopus = increaseEnergy(octopus);
        var flashed = detectFlashed(octopus);
        var stepFlashed = detectFlashed(octopus);
        //var flashed;
        for (int step = 0; step < value; step++)
        {
            keepScanning = true;
            stepFlashed.Clear();
            //Console.WriteLine();
            //Console.WriteLine("Step number: " + (step + 1) + " total flashes: " + totalFlashes);
            octopus = increaseEnergy(octopus);
            flashed = detectFlashed(octopus);
            stepFlashed = stepFlashed.Concat(flashed).ToList();
            octopus = resetFlashed(octopus, flashed);
            if (flashed.Count() == 0)
            {
                //System.Console.WriteLine("No flashes this step");
                continue;
            }
            //flashedThisRound = flashed.Count();

            while (keepScanning)
            {

                //flashedPreviousRound = flashedThisRound;
                //totalFlashes += flashedPreviousRound;
                //octopus = resetFlashed(octopus, flashed);
                flashedThisRound = 0;
                foreach (var coord in flashed)
                {
                    var adjacent = findAdjacent(octopus, coord);
                    foreach (var adjacentCoord in adjacent)
                    {
                        octopus[adjacentCoord.Item1, adjacentCoord.Item2] += 1;
                    }
                }
                flashed = detectFlashed(octopus);
                stepFlashed = stepFlashed.Concat(flashed).ToList();
                octopus = resetFlashed(octopus, flashed);
                flashedPreviousRound = flashedThisRound;
                flashedThisRound = flashed.Count();
                //totalFlashes += flashedThisRound;
                //System.Console.WriteLine("Round: flashedThisRound: {0} flashedPreviousRound: {1} totalFlashes: {2}", flashedThisRound, flashedPreviousRound, totalFlashes);
                //printBoard(octopus, stepFlashed);
                //Console.ReadLine();
                if (flashedThisRound == flashedPreviousRound)
                {
                    //System.Console.WriteLine("No more flashes!");
                    keepScanning = false;
                }

            }
            octopus = resetFlashed(octopus, stepFlashed);
            //printBoard(octopus, stepFlashed);
            //totalFlashes += flashedThisRound;
            totalFlashes += stepFlashed.Count();
            //System.Console.WriteLine("Flashed this round: " + flashedThisRound);
            //System.Console.WriteLine("Total flashes: " + totalFlashes);
            //Console.ReadLine();
        }
        printBoard(octopus, stepFlashed);
        System.Console.WriteLine("Total flashes: " + totalFlashes);
        /*
        foreach (var tuple in flashed)
        {
            Console.WriteLine("{0} - {1}", tuple.Item1, tuple.Item2);
        }
        */
        Console.WriteLine();
    }

    static int[,] resetFlashed(int[,] arr, List<Tuple<int, int>> flashedThisStep)
    {
        int rows = arr.GetLength(0);
        int cols = arr.GetLength(1);

        int[,] newOctopus = new int[rows, cols];

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                Tuple<int, int> current = new Tuple<int, int>(row, col);
                if (flashedThisStep.Contains(current))
                {
                    //Console.WriteLine("Holy crap, this one flashed!");
                    newOctopus[row, col] = 0;
                }
                else
                {
                    newOctopus[row, col] = arr[row, col];
                }
            }
        }
        return newOctopus;
    }

    static int[,] increaseEnergy(int[,] arr)
    {
        int rows = arr.GetLength(0);
        int cols = arr.GetLength(1);

        int[,] newOctopus = new int[rows, cols];

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                newOctopus[row, col] = arr[row, col] + 1;
            }

        }
        //printBoard(newOctopus);
        return newOctopus;
    }

    static List<Tuple<int, int>> detectFlashed(int[,] arr)
    {
        int rows = arr.GetLength(0);
        int cols = arr.GetLength(1);

        int[,] newOctopus = new int[rows, cols];
        var flashedList = new List<Tuple<int, int>>();

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                if (arr[row, col] > 9)
                {
                    // Flash!
                    flashedList.Add(new Tuple<int, int>(row, col));
                }
            }
        }
        //System.Console.WriteLine("Flashed count: " + flashedList.Count);
        return flashedList;

    }

    static List<Tuple<int, int>> findAdjacent(int[,] arr, Tuple<int, int> coord)
    {
        int rows = arr.GetLength(0);
        int cols = arr.GetLength(1);

        int row = coord.Item1;
        int col = coord.Item2;

        var adjacentList = new List<Tuple<int, int>>();
        if (row > 0)
            adjacentList.Add(new Tuple<int, int>(row - 1, col));
        if (col > 0)
            adjacentList.Add(new Tuple<int, int>(row, col - 1));
        if (row > 0 && col > 0)
            adjacentList.Add(new Tuple<int, int>(row - 1, col - 1));
        if (row > 0 && col < cols - 1)
            adjacentList.Add(new Tuple<int, int>(row - 1, col + 1));
        if (row < rows - 1 && col > 0)
            adjacentList.Add(new Tuple<int, int>(row + 1, col - 1));
        if (row < rows - 1)
            adjacentList.Add(new Tuple<int, int>(row + 1, col));
        if (col < cols - 1)
            adjacentList.Add(new Tuple<int, int>(row, col + 1));
        if (row < rows - 1 && col < cols - 1)
            adjacentList.Add(new Tuple<int, int>(row + 1, col + 1));
        //System.Console.WriteLine("Length: " + adjacentList.Count);
        return adjacentList;
    }
    static void printBoard(int[,] arr)
    {
        int rows = arr.GetLength(0);
        int cols = arr.GetLength(1);
        //Console.WriteLine("rows: " + rows);
        //Console.WriteLine("cols: " + cols);
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                Console.Write(arr[row, col] + " ");
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }
    static void printBoard(int[,] arr, List<Tuple<int, int>> flashedThisStep)
    {
        int rows = arr.GetLength(0);
        int cols = arr.GetLength(1);
        //Console.WriteLine("rows: " + rows);
        //Console.WriteLine("cols: " + cols);

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                Tuple<int, int> current = new Tuple<int, int>(row, col);
                if (flashedThisStep.Contains(current))
                {
                    //Console.WriteLine("Holy crap, this one flashed!");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(arr[row, col] + " ");
                    Console.ResetColor();
                }
                else
                {
                    Console.Write(arr[row, col] + " ");
                }

            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }


    static void ChessBoard()
    {
        int[] myNumbers = { 14, 22, 7, 19 };
        Console.WriteLine("Welcome to the Chess Board");
        Console.Write("How large board? ----> ");
        string? input = Console.ReadLine();
        bool isNumber = int.TryParse(input, out int value);
        Console.WriteLine(value);

        static void printBoard(string[] arr, int size)
        {
            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    Console.Write(arr[(row * size) + col] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        // Skapa ett shackbräde som value x value stort
        // Skapa en endimensionell array med value^2 antal element
        string[] board = new string[value * value];

        printBoard(board, value);
        Console.WriteLine("value: " + value);
        for (int row = 0; row < value; row++)
        {
            for (int col = 0; col < value; col++)
            {
                int i = (row * value) + col;
                if (value % 2 == 0)
                {
                    if (row % 2 == 0)
                    {
                        // jämn rad
                        if (i % 2 == 0)
                        {
                            // Jämn kolumn
                            board[i] = "◼︎";
                            // board[row][col] = board[row][col] + 1;
                        }
                        else
                        {
                            // Udda kolumn
                            board[i] = "◻︎";
                        }
                    }

                    else
                    {
                        // udda rad
                        if (i % 2 == 0)
                        {
                            // JÄmnt tal
                            board[i] = "◻︎";
                        }
                        else
                        {
                            // Udda tal
                            board[i] = "◼︎";
                        }
                    }
                }

                else
                {
                    if (i % 2 == 0)
                    {
                        // JÄmnt tal
                        board[i] = "◼";
                    }
                    else
                    {
                        // Udda tal
                        board[i] = "◻︎";
                    }
                }

                // board[(row * value) + col] = "◼︎";
                //Console.Write((row * value) + col + " ");
            }
            //Console.WriteLine();
        }

        /*
        int oldQuotient = 0;
        for (int i = 0; i < board.Length; i++)
        {
            //Console.Write("Enter " + i + " element: ");
            // Sätt varannan ruta till ◼︎ och varannan till ◻︎
            int quotient = i / value;
            if (quotient % 2 == 0)
            {
                //j ämn rad
                if (i % 2 == 0)
                {
                    // JÄmnt tal
                    board[i] = "◼︎";
                }
                else
                {
                    // Udda tal
                    board[i] = "◻︎";
                }
            } else
            {
                // udda rad
                if (i % 2 == 0)
                {
                    // JÄmnt tal
                    board[i] = "◻︎";
                }
                else
                {
                    // Udda tal
                    board[i] = "◼︎";
                }
            }


        }
        */

        printBoard(board, value);

    }
}

