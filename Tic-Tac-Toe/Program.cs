using System;
using System.Collections;

namespace Tic_Tac_Toe
{   
    /**
     *  Base program class for Tic-Tac-Toe game or m,n,k variations
     */
    class Program
    {
        private static Metrics _gameMetrics;
        private static Stack _gameHistory = new Stack();

        private static readonly string UNDO = "undo";

        static void SetDialogColor()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
        }

        static void SetPromptColor()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
        }

        static void SetErrorColor()
        {
            Console.ForegroundColor = ConsoleColor.Red;
        }

        static void SetSuccessColor()
        {
            Console.ForegroundColor = ConsoleColor.Green;
        }
                
        static void Main(string[] args)
        {
            StartGame();
        }

        static void StartGame()
        {
            SetDialogColor();
            Console.WriteLine("Would you like to play a traditional game of Tic-Tac-Toe, y/n?");
            ConsoleKeyInfo key = Console.ReadKey();
            
            _gameMetrics = key.Key == ConsoleKey.Y ? new Metrics() : GetMetrics();
            
            SetPromptColor();
            Console.WriteLine();
            Console.WriteLine("Please enter name for Player X below:");
            _gameMetrics.FirstPlayer = Console.ReadLine();
            Console.WriteLine("Please enter name for Player O below:");
            _gameMetrics.SecondPlayer = Console.ReadLine();
            
            SetDialogColor();
            Console.WriteLine("Starting 'Tic-Tac-Toe'... {0} will go first (X), then it's {1}'s turn (O).", _gameMetrics.FirstPlayer, _gameMetrics.SecondPlayer);
            Move[,] board = InitBoardData();
            
            // Push initial starting state record, m == numRows, assuming !_playerX for first state, 0 count
            _gameHistory.Push(new History(board, UpdateBoardDisplay(new BoardDisplay(_gameMetrics.GetM()), board), false, 0)); 
            RunGame();

            SetPromptColor();
            Console.WriteLine("Would you like to play again? y/n/ESC");
            ConsoleKeyInfo answer = Console.ReadKey();

            switch (answer.Key)
            {
                case ConsoleKey.Y:
                    Console.WriteLine();
                    StartGame();
                    break;
                case ConsoleKey.N:
                case ConsoleKey.Escape:
                default:
                    break;
            }
        }
        
        static void RunGame()
        {
            History previous = (History)_gameHistory.Peek();
            PrintBoardDisplay(previous.GetBoardDisplay());
            SetDialogColor();
            Console.WriteLine("{0} ({1}) it's your turn!", _gameMetrics.GetCurrentPlayer(previous.WasPlayerX()),
                previous.WasPlayerX() ? Move.O.ToString() : Move.X.ToString());
            SetPromptColor();
            Console.WriteLine("Please enter your move below (zero based) [row,column], Ex: 0,1");
            Console.WriteLine("Type 'undo' to undo the last move.");
            
            try
            {
                String entry = Console.ReadLine();
                if (UNDO.Equals(entry))
                {
                    if (_gameHistory.Count <= 1)
                    {
                        SetErrorColor();
                        Console.WriteLine("Cannot revert history back any further!");
                        RunGame();
                        return;
                    }

                    _gameHistory.Pop();
                    SetDialogColor();
                    Console.WriteLine("Successfully reverted the last move.");
                    RunGame();
                    return;
                }
                
                String[] entries = entry != null ? entry.Split(",") : new String[0];
                
                // must match format [row,col] exactly
                if (entries.Length != 2)
                {
                    throw new Exception();
                }

                // check for invalid selection, i.e. selected space is not empty
                int m = Int16.Parse(entries[0]), n = Int16.Parse(entries[1]);
                if (previous.GetBoard()[m,n] != Move.E)
                {
                    throw new InvalidOperationException();
                }

                MakeMove(m, n, previous);
                
                if (FinalState(m, n))
                {
                    Console.WriteLine("Thank you for playing!");
                    return;
                }
            }
            catch (InvalidOperationException ignored)
            {
                SetErrorColor();
                Console.WriteLine("Invalid selection, please select an empty space.");
            }
            catch (Exception ignored) // catch generic, Parse, IndexOutOfRangeException, InvalidDataExceptions
            {
                SetErrorColor();
                Console.WriteLine("Does not match format: row,column. Please try again.");
                Console.WriteLine("(Make sure your selection is within the bounds)");
            }
            
            RunGame();
        }
        
        /**
         *  Assumes data integrity of user input for EMPTY m/n coordinate selection on board
         *  Updates all relvant state tracking
         */
        private static void MakeMove(int m, int n, History previous)
        {
            Move[,] newBoard = (Move[,]) previous.GetBoard().Clone();
            newBoard[m,n] = previous.WasPlayerX() ? Move.O : Move.X;
            _gameHistory.Push(new History(newBoard, UpdateBoardDisplay(new BoardDisplay(_gameMetrics.GetM()), newBoard), !previous.WasPlayerX(), previous.GetMoveCount()+1));
        }

        /**
         *  Helper to verify if someone has won or there has been a draw.
         *  Increases performance to only check relevant rows/cols/diags associated with last selection.
         *  One can only win if the current player's last selection led to it, no need to retry redudant cells.
         */
        private static bool FinalState(int m, int n)
        {
            History previous = (History)_gameHistory.Peek();
            Move lastMove = previous.WasPlayerX() ? Move.X : Move.O;
            
            if (CheckCol(n, previous.GetBoard(), lastMove) || CheckRow(m, previous.GetBoard(), lastMove) ||
                CheckDiags(m, n, previous.GetBoard(), lastMove))
            {
                SetSuccessColor();
                Console.WriteLine("Congratulations {0}, you've won the game!", _gameMetrics.GetLastPlayer(previous.WasPlayerX()));
                PrintBoardDisplay(previous.GetBoardDisplay());
                return true;
            }
            
            // check draw
            if (previous.GetMoveCount() == _gameMetrics.GetMaxMoves())
            {
                SetSuccessColor();
                Console.WriteLine("The game has ended in a draw...");
                PrintBoardDisplay(previous.GetBoardDisplay());
                return true;
            }
            
            return false;
        }

        /**
         *  check winning 'k' line on selected 'n' column
         */
        private static bool CheckCol(int n, Move[,] board, Move move)
        {
            int colMatch = 0;
            for (int i = 0; i < _gameMetrics.GetM(); i++)
            {
                // bump if we have a match, reset if we break the chain
                colMatch = board[i,n] == move ? colMatch + 1 : 0;

                if (colMatch == _gameMetrics.GetK())
                {
                    return true;
                }
            }

            return false;
        }

        /**
         *  check winning 'k' line on selected 'm' row
         */
        private static bool CheckRow(int m, Move[,] board, Move move)
        {
            int rowMatch = 0;
            for (int i = 0; i < _gameMetrics.GetN(); i++)
            {
                // bump if we have a match, reset if we break the chain
                rowMatch = board[m,i] == move ? rowMatch + 1 : 0;

                if (rowMatch == _gameMetrics.GetK())
                {
                    return true;
                }
            }

            return false;
        }
        
        /**
         *  checks both directions for diagonal match, using relationship of sum and |diff| of coordinates
         *  future iterations of this could predetermine all matching states ahead of time (once game metrics have been provided)
         */
        private static bool CheckDiags(int m, int n, Move[,] board, Move move)
        {
            int sum = m + n;
            int diff = m - n;
            
            int diagSumMatch = 0;
            int diagDiffMatch = 0;
            
            for (int i = 0; i < _gameMetrics.GetM() ; i++)
            {
                for (int j = 0; j < _gameMetrics.GetN() ; j++)
                {
                    if (i + j == sum)
                    {
                        diagSumMatch = board[i, j] == move ? diagSumMatch + 1 : 0;
                    }
                    
                    if (i - j == diff)
                    {
                        diagDiffMatch = board[i, j] == move ? diagDiffMatch + 1 : 0;
                    }

                    if (diagSumMatch == _gameMetrics.GetK() || diagDiffMatch == _gameMetrics.GetK())
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /**
         *  Helper method to fetch starting metrics for their game preference if they don't want the generic 3,3,3 game
         */
        private static Metrics GetMetrics()
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine();
            Console.WriteLine("Please enter the metrics you'd like to play with.");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Example: 3,3,3 or 19,19,5. [numbers only]");
            
            try
            {
                String entry = Console.ReadLine();
                String[] entries = entry != null ? entry.Split(",") : new String[0];
                
                if (entries.Length != 3)
                {
                    throw new Exception();
                }
                
                return new Metrics(Int16.Parse(entries[0]), Int16.Parse(entries[1]), Int16.Parse(entries[2]));
            }
            catch (Exception ignored) // Catch generic for ThrowArgumentNullException and checked exception
            {
                SetErrorColor();
                Console.WriteLine("Does not match format: m,n,k. Please try again.");
                return GetMetrics();
            }
        }
        
        /**
         *  Helper for generating initial board data structure on selected game metrics 
         */
        private static Move[,] InitBoardData()
        {
            Move[,] board = new Move[_gameMetrics.GetM(), _gameMetrics.GetN()];
            for (int m = 0; m < _gameMetrics.GetM() ; m++)
            {
                for (int n = 0; n < _gameMetrics.GetN() ; n++)
                {
                    board[m,n] = Move.E;
                }
            }

            return board;
        }

        /**
         *  Helper for updating board display object when state has changed
         */
        private static BoardDisplay UpdateBoardDisplay(BoardDisplay boardDisplay, Move[,] board)
        {
            for (int m = 0; m < _gameMetrics.GetM(); m++)
            {
                string preRow = "", row = "", postRow = "", lineRow = "";
                bool isLastRow = m != _gameMetrics.GetM() - 1;

                for (int n = 0; n < _gameMetrics.GetN(); n++)
                {
                    bool isLastCol = n != _gameMetrics.GetN() - 1;

                    preRow += String.Format("{0}{1}", boardDisplay.LONG, isLastCol ? "|" : "");
                    row += String.Format("{0}{1}{2}{3}", boardDisplay.SHORT, board[m, n], boardDisplay.SHORT, isLastCol ? "|" : "");
                    postRow += String.Format("{0}{1}", boardDisplay.LONG, isLastCol ? "|" : "");

                    if (isLastRow)
                    {
                        lineRow += String.Format("{0}{1}", boardDisplay.SPLIT, isLastCol ? "-" : "");
                    }
                }

                boardDisplay.SetRowByIndex(m, new BoardDisplay.Row(preRow, row, postRow, lineRow));
            }

            return boardDisplay;
        }

        /**
         *  Helper for displaying current board state to console
         */
        private static void PrintBoardDisplay(BoardDisplay boardDisplay)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            for (int row = 0; row < boardDisplay.GetNumRows(); row++)
            {
                BoardDisplay.Row curRow = boardDisplay.GetRowByIndex(row);
                Console.WriteLine(curRow.GetPreRow());
                Console.WriteLine(curRow.GetRow());
                Console.WriteLine(curRow.GetPostRow());
                Console.WriteLine(curRow.GetLineRow());
            }
        }
    }
}