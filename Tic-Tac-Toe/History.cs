namespace Tic_Tac_Toe
{
    public class History
    {
        private Move[,] _board;
        private BoardDisplay _boardDisplay;
        private bool _playerX;
        private int _moveCounter;
        
        public History() {}

        public History(Move[,] board, BoardDisplay boardDisplay, bool playerX, int moveCounter)
        {
            _board = board;
            _boardDisplay = boardDisplay;
            _playerX = playerX;
            _moveCounter = moveCounter;
        }

        public Move[,] GetBoard()
        {
            return _board;
        }

        public BoardDisplay GetBoardDisplay()
        {
            return _boardDisplay;
        }

        public bool WasPlayerX()
        {
            return _playerX;
        }

        public int GetMoveCount()
        {
            return _moveCounter;
        }
    }
}