using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace game 
{
    public static class Matching
    {
        public struct MinLeagueRate
        {
            public static readonly uint Bronze = 1300;
            public static readonly uint Silver = 1350;
            public static readonly uint Gold = 1600;
            public static readonly uint Platinum = 1900;
        }

        public static uint GetRateDifference(Account player, Account opponent)
        {
            return (uint)Math.Abs(player.rate - opponent.rate);
        }

        public static uint GetResultRate(Account player, Account opponent, bool is_victory)
        {
            //TODO: Place here system to generate rate points after match
            return 0;
        }
    }
}