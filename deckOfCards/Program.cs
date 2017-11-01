using System;
using System.Collections.Generic;

namespace deckOfCards
{

    public class Card
    {
        string face;
        string suit;
        int val;
    public Card(string cardFace, string cardSuit, int cardVal){
        face = cardFace;
        suit = cardSuit;
        val = cardVal;
    }
    }

    public class Deck
    {
        List<Card> deckCards = new List<Card>();

        string[] cardSuits = {"Hearts", "Diamonds", "Clubs", "Spades"};
        string[] cardFaces = {"A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "Jack", "Queen", "King"};
        // int[] cardVals = {1,2,3,4,5,6,7,8,9,10,11,12,13};

        public void makeDeck(){
            for(var suit=0; suit < cardSuits.Length; suit++){
                for(var face = 0; face < cardFaces.Length; face++){
                     Card newCard = new Card(cardSuits[suit], cardFaces[face], face+1);
                     deckCards.Add(newCard);
                }
            }
        }
        
        public Card deal(){
            Card dealtCard = deckCards[deckCards.Count-1];
            deckCards.RemoveAt(deckCards.Count-1);
            return dealtCard;
        }

        public void reset(){
            deckCards.Clear();
            makeDeck();
        }

        public void shuffle(){
            Random rand = new Random();
            for(int i=0; i < deckCards.Count; i++){
                Card shuffledCard = deckCards[i];
                int shuffleSpot = rand.Next(0, deckCards.Count-2);
                deckCards.RemoveAt(i);
                deckCards.Insert(shuffleSpot, shuffledCard);
            }

        }

    }

    public class Player
    {
        string name;
        List<Card> hand = new List<Card>();

        public Card draw(Deck theDeck){
            Card drawnCard = theDeck.deal();
            hand.Add(drawnCard);
            return drawnCard;
        }

        public Card discard(int index){
            if(index < hand.Count){
                Card disCard = hand[index];
                hand.RemoveAt(index);
                return disCard;
            }
            else{
                return null;
            }
        }
    }






    // -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    class Program
    {
        static void Main(string[] args)
        {
            Deck newDeck = new Deck();
            newDeck.makeDeck();
            newDeck.reset();
            newDeck.shuffle();


            Player newPlayer = new Player();
            newPlayer.draw(newDeck);
            newPlayer.draw(newDeck);
            newPlayer.draw(newDeck);
            newPlayer.draw(newDeck);
            newPlayer.draw(newDeck);

            newPlayer.discard(3);
            newPlayer.discard(1);
            newPlayer.discard(5);
        }
    }
}
