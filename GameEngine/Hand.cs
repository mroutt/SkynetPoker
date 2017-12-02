using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine
{
    public class Hand
    {
        public static void Play(GameState gameState, List<string> log, Deck deck)
        {
            int currentPot = 0;
            var players = gameState.Players;

            log.Add("Dealing a new hand.");

            var seats = FillSeatsWithPlayers(players, gameState.DealerPosition);

            currentPot += CollectBlinds(seats);

            DealCardsToSeats(seats, deck);

            foreach (var seat in seats)
            {
                var action = seat.RequestAction();
                if (action.Action == "Fold")
                    seat.StillInHand = false;
                else
                {
                    seat.TakeChipsFromPlayer(action.ChipAmount);
                    currentPot += action.ChipAmount;
                }

                NotifyPlayersOfAction(action, players);
                log.Add(FormatPlayerActionEventMessage(action, seat.Player));

                if (OnlyOneSeatInTheHand(seats))
                    break;
            }

            var winningPlayer = DetermineWinningPlayer(seats, players);

            AwardPotToWinningPlayer(winningPlayer, currentPot);

            log.Add(FormatPlayerWonHandMessage(winningPlayer, currentPot));
        }

        public static void Play(GameState gameState, List<string> log)
        {
            Play(gameState, log, new Deck());         
        }

        private static int CollectBlinds(List<Seat> seats)
        {
            int smallBlind = 50;
            int bigBlind = 100;

            seats.Where(x => x.SmallBlind).Single().TakeChipsFromPlayer(smallBlind);
            seats.Where(x => x.BigBlind).Single().TakeChipsFromPlayer(bigBlind);

            return smallBlind + bigBlind;
        }

        private static List<Seat> FillSeatsWithPlayers(List<Player> players, int dealerPosition)
        {
            var seats = new List<Seat>();
            int indexOfDealer = dealerPosition - 1;
            int indexOfFirstAction = indexOfDealer + 3;

            for (int i = 0; i < players.Count; i++)
            {
                int indexOfPlayerToAddNext = (indexOfFirstAction + i) % players.Count; 

                seats.Add(new Seat(players[indexOfPlayerToAddNext]));
            }

            SetBlindSeats(seats);

            return seats;
        }

        private static void SetBlindSeats(List<Seat> seats)
        {
            int smallBlindIndex = (seats.Count + 2) % seats.Count;
            int bigBlindIndex = (seats.Count + 3) % seats.Count;

            seats[smallBlindIndex].SmallBlind = true;
            seats[bigBlindIndex].BigBlind = true;
        }

        private static bool OnlyOneSeatInTheHand(List<Seat> seats)
        {
            return seats.Where(x => x.StillInHand).Count() == 1;
        }

        private static void DealCardsToSeats(List<Seat> seats, Deck deck)
        {
            foreach (var seat in seats)
            {
                var card1 = deck.DrawCard();
                var card2 = deck.DrawCard();
                seat.DealCards(card1, card2);
            }
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

        private static Player DetermineWinningPlayer(List<Seat> seats, List<Player> players)
        {
            return DetermineWinningSeat(seats, players).Player;
        }

        private static Seat DetermineWinningSeat(List<Seat> seats, List<Player> players)
        {
            if (OnlyOneSeatInTheHand(seats))
                return seats.Where(x => x.StillInHand).Single();

            var highPairSeat = FindSeatWithHighPair(seats);
            if (highPairSeat != null)
                return highPairSeat;

            return FindSeatWithHighCard(seats);
        }

        private static Seat FindSeatWithHighPair(List<Seat> seats)
        {
            int highPairValue = 0;
            Seat highPairSeat = null;

            foreach (var seat in seats)
            {
                if (seat.Card1.Value == seat.Card2.Value)
                {
                    if (seat.Card1.Value > highPairValue)
                    {
                        highPairValue = seat.Card1.Value;
                        highPairSeat = seat;
                    }
                }
            }

            return highPairSeat;
        }

        private static Seat FindSeatWithHighCard(List<Seat> seats)
        {
            int highValue = 0;
            Seat highValueSeat = null;
            foreach (var seat in seats)
            {
                int highValueForSeat = Math.Max(seat.Card1.Value, seat.Card2.Value);

                if (highValueForSeat > highValue)
                {
                    highValue = highValueForSeat;
                    highValueSeat = seat;
                }
            }

            return highValueSeat;
        }
    }
}
