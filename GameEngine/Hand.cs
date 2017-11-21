using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEngine
{
    public class Hand
    {
        public static void Play(GameState gameState, List<string> log)
        {
            var players = gameState.Players;

            log.Add("Dealing a new hand.");

            var cards = DealCards(players);
            int currentPot = 0;

            foreach(var player in players)
            {
                var action = player.GetPlayerAction();
                currentPot = ApplyPlayerActionAndReturnUpdatedPot(action, player, currentPot);
                NotifyPlayersOfAction(action, players);
                log.Add(FormatPlayerActionEventMessage(action, player));
            }

            var winningPlayer = DetermineWinningPlayer(cards, players);

            AwardPotToWinningPlayer(winningPlayer, currentPot);

            log.Add(FormatPlayerWonHandMessage(winningPlayer, currentPot));
        }

        private static List<PlayerCards> DealCards(List<Player> players)
        {
            var cards = new List<PlayerCards>();
            int cardToDeal = 1;

            foreach (var player in players)
            {
                cards.Add(new PlayerCards(player.Seat, cardToDeal, cardToDeal));
                player.DealCards(cardToDeal, cardToDeal);
                cardToDeal += 1;
            }

            return cards;
        }

        private static int ApplyPlayerActionAndReturnUpdatedPot(PlayerAction action, Player player, int currentPot)
        {
            if (action.Action == "Raise" || action.Action == "Call")
            {
                player.Chips -= action.ChipAmount;
                currentPot += action.ChipAmount;
            }

            return currentPot;
        }

        private static void NotifyPlayersOfAction(PlayerAction action, List<Player> players)
        {
            foreach (var player in players)
            {
                player.NotifyOfPlayerAction(action);
            }
        }

        private static string FormatPlayerWonHandMessage(Player player, int potAmount)
        {
            return string.Format("{0} in seat {1} won the hand and took down a ${2} pot.", player.Name, player.Seat, potAmount);
        }

        private static string FormatPlayerActionEventMessage(PlayerAction action, Player player)
        {
            string actionDescription = "";
            switch (action.Action)
            {
                case "Raise":
                    actionDescription = "raises " + action.ChipAmount;
                    break;

                case "Call":
                    actionDescription = "calls " + action.ChipAmount;
                    break;
            }

            return string.Format("{0} in seat {1} {2}", player.Name, player.Seat, actionDescription);
        }

        private static void AwardPotToWinningPlayer(Player player, int currentPot)
        {
            player.Chips += currentPot;
        }

        private static Player DetermineWinningPlayer(List<PlayerCards> cards, List<Player> players)
        {
            int maxScore = 0;
            int maxScoreSeat = 0;
            foreach (var card in cards)
            {
                int score = card.Card1 + card.Card2;

                if (score > maxScore)
                {
                    maxScore = score;
                    maxScoreSeat = card.Seat;
                }
            }

            return players.Where(x => x.Seat == maxScoreSeat).Single();
        }
    }
}
