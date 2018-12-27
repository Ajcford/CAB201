using System.Drawing;
using System.ComponentModel;
using Object_Classes;
using System;


namespace Game_Logic_Class
{
    public static class SpaceRaceGame
    {
        // Minimum and maximum number of players.
        public const int MIN_PLAYERS = 2;
        public const int MAX_PLAYERS = 6;
   
        private static int numberOfPlayers;  //default value for test purposes only 
        public static int NumberOfPlayers
        {
            get
            {
                return numberOfPlayers;
            }
            set
            {
                numberOfPlayers = value;
            }
        }

        private static bool endGame = false;
        public static bool Endgame
        {
            get
            {
                return endGame;
            }
            set
            {
                endGame = value;
            }
        }

        public static string[] names = { "One", "Two", "Three", "Four", "Five", "Six" };  // default values
        
        // Only used in Part B - GUI Implementation, the colours of each player's token
        private static Brush[] playerTokenColours = new Brush[MAX_PLAYERS] { Brushes.Yellow, Brushes.Red,
                                                                       Brushes.Orange, Brushes.White,
                                                                      Brushes.Green, Brushes.DarkViolet};
        /// <summary>
        /// A BindingList is like an array which grows as elements are added to it.
        /// </summary>
        private static BindingList<Player> players = new BindingList<Player>();
        public static BindingList<Player> Players
        {
            get
            {
                return players;
            }
        }

        // The pair of die
        private static Die die1 = new Die(), die2 = new Die();
       

        /// <summary>
        /// Set up the conditions for this game as well as
        ///   creating the required number of players, adding each player 
        ///   to the Binding List and initialize the player's instance variables
        ///   except for playerTokenColour and playerTokenImage in Console implementation.
        ///   
        ///     
        /// Pre:  none
        /// Post:  required number of players have been initialsed for start of a game.
        /// </summary>
        public static void SetUpPlayers() 
        {
            for (int i = 0; i < NumberOfPlayers; i++)
            {
                Player person = new Player(names[i]);// Create player object
                //Initialise instance variables for new game
                person.Name = names[i];
                person.Location = Board.StartSquare;
                person.Position = Board.START_SQUARE_NUMBER;
                person.RocketFuel = Player.INITIAL_FUEL_AMOUNT;
                person.HasPower = true;
                person.AtFinish = false;
                person.PlayerTokenColour = playerTokenColours[i]; // Set player colour
                players.Add(person); // Add new player to binding list
            }
        }

        /// <summary>
        ///  Plays one round of a game
        /// </summary>
        public static void PlayOneRound() 
        {
            Console.WriteLine("     Next Round \n");
            // Loops through each Player object in the BindingList
            foreach (Object_Classes.Player person in players)
            {
                person.Play(die1, die2);
                Console.WriteLine("      " + person.Name + " on square " + person.Position + " with " + person.RocketFuel + " yattowatt of power remaining");
            }
        }

        /// <summary>
        ///  Plays one turn of a game
        /// </summary>
        public static void PlayOneTurn(int index)
        {
            players[index].Play(die1, die2); // Play individual turn
        }

    }//end SnakesAndLadders
}