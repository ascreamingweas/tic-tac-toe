using System;

namespace Tic_Tac_Toe
{
    public class Metrics
    {
        // Default values set for traditioanl Tic-Tac-Toe game
        private int m = 3, n = 3, k = 3;
        private string firstPlayer, secondPlayer;

        // No-args constructor for default game 
        public Metrics()
        {
        }

        public Metrics(int m, int n, int k)
        {
            this.m = m;
            this.n = n;
            this.k = k;
        }

        public int GetM()
        {
            return m;
        }
        
        public int GetN()
        {
            return n;
        }
        
        public int GetK()
        {
            return k;
        }

        /**
         *  Helper to determine the max number of moves possible given the metrics of the game for calculating a draw
         */
        public int GetMaxMoves()
        {
            return m * n;
        }

        /**
         *  Param for wasFirstPlayer is based on the last history record, current should be !wasFirstPlayer
         */
        public string GetCurrentPlayer(bool wasFirstPlayer)
        {
            return wasFirstPlayer ? secondPlayer : firstPlayer;
        }

        public string GetLastPlayer(bool wasFirstPlayer)
        {
            return wasFirstPlayer ? firstPlayer : secondPlayer;
        }

        public string FirstPlayer
        {
            get => firstPlayer;
            set => firstPlayer = value;
        }

        public string SecondPlayer
        {
            get => secondPlayer;
            set => secondPlayer = value;
        }
    }
}