using System;
using System.Collections.Generic;
using System.Linq;

public enum Suit
{
    Hearts,
    Diamonds,
    Clubs,
    Spades
}

public enum Rank
{
    Six = 6,
    Seven,
    Eight,
    Nine,
    Ten,
    Jack,
    Queen,
    King,
    Ace
}

public class Card
{
    public Suit Suit { get; }
    public Rank Rank { get; }

    public Card(Suit suit, Rank rank)
    {
        Suit = suit;
        Rank = rank;
    }
}

public class Player
{
    public List<Card> Cards { get; } = new List<Card>();

    public void ShowCards()
    {
        foreach (var card in Cards)
        {
            Console.WriteLine($"{card.Rank} of {card.Suit}");
        }
    }
}

public class Game
{
    private List<Player> players;
    private List<Card> deck;

    public Game(int numberOfPlayers)
    {
        if (numberOfPlayers < 2)
        {
            throw new ArgumentException("Количество игроков должно быть не менее 2.");
        }

        players = new List<Player>();
        for (int i = 0; i < numberOfPlayers; i++)
        {
            players.Add(new Player());
        }

        deck = GenerateDeck();
        ShuffleDeck();
        DealCards();
    }

    private List<Card> GenerateDeck()
    {
        var deck = new List<Card>();
        foreach (Suit suit in Enum.GetValues(typeof(Suit)))
        {
            foreach (Rank rank in Enum.GetValues(typeof(Rank)))
            {
                deck.Add(new Card(suit, rank));
            }
        }
        return deck;
    }

    private void ShuffleDeck()
    {
        Random random = new Random();
        deck = deck.OrderBy(card => random.Next()).ToList();
    }

    private void DealCards()
    {
        int cardsPerPlayer = deck.Count / players.Count;
        for (int i = 0; i < players.Count; i++)
        {
            players[i].Cards.AddRange(deck.Skip(i * cardsPerPlayer).Take(cardsPerPlayer));
        }
    }

    public void PlayRound()
    {
        List<Card> roundCards = players.Select(player => player.Cards.First()).ToList();
        int maxRank = roundCards.Max(card => (int)card.Rank);

        int winnerIndex = roundCards.FindIndex(card => (int)card.Rank == maxRank);
        Player winner = players[winnerIndex];

        players.ForEach(player => player.Cards.RemoveAt(0));
        winner.Cards.AddRange(roundCards);

        Console.WriteLine($"Результат раунда:");
        for (int i = 0; i < players.Count; i++)
        {
            Console.WriteLine($"Игрок {i + 1}: {roundCards[i].Rank} of {roundCards[i].Suit}");
        }

        if (roundCards.Count(card => (int)card.Rank == maxRank) == 1)
        {
            Console.WriteLine($"Игрок {winnerIndex + 1} выигрывает раунд!");
        }
        else
        {
            Console.WriteLine("Это ничья!");
        }
    }

    public void PlayGame()
    {
        int round = 1;
        while (round <= 20 && players.Any(player => player.Cards.Count > 0))
        {
            Console.WriteLine($"Раунд {round}");
            PlayRound();
            round++;
        }

    }

}

class Program
{
    static void Main()
    {
            Console.Write("Введите количество игроков (минимум 2): ");
            int numberOfPlayers = int.Parse(Console.ReadLine());

            try
            {
                Game game = new Game(numberOfPlayers);
                game.PlayGame();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
    }
}
