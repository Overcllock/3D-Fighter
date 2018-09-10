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

    public class Match
    {
        //NOTE: time in seconds
        //NOTE: this is time for fast tests. Original time is 180 seconds
        const float MATCH_TIME = 45.0f;

        public uint round;
        public uint[] score;

        float start_timestamp;
        float end_timestamp;

        public Match()
        {
            round = 0;
            score = new uint[] { 0, 0 };
        }

        public void StartRound()
        {
            round++;
            start_timestamp = Time.time;
            end_timestamp = start_timestamp + MATCH_TIME;
            Debug.Log("Match started at " + start_timestamp);
        }
    }
}