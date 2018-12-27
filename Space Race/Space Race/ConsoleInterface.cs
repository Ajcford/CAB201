using System;
//DO NOT DELETE the two following using statements *********************************
using Game_Logic_Class;
using Object_Classes;

namespace Space_Race
{
    class Console_Class
    {
        /// <summary>
        /// Algorithm below currently plays only one game
        /// 
        /// when have this working correctly, add the abilty for the user to 
        /// play more than 1 game if they choose to do so.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {      
             DisplayIntroductionMessage();
            /*                    
             Set up the board in Board class (Board.SetUpBoard)
             Determine number of players - initally play with 2 for testing purposes 
             Create the required players in Game Logic class
              and initialize players for start of a game             
             loop  until game is finished           
                call PlayGame in Game Logic class to play one round
                Output each player's details at end of round
             end loop
             Determine if anyone has won
             Output each player's details at end of the game
           */



            //SpaceRaceGame.NumberOfPlayers = user input (remember to remove "= 2" in SpaceRaceGame.cs)

            Board.SetUpBoard();

            SpaceRaceGame.SetUpPlayers();

            bool game_over = false;
            do
            {
                SpaceRaceGame.PlayOneRound();

                Console.WriteLine("\nPress Enter to play round ...");
                Console.ReadLine();

                foreach (Object_Classes.Player person in SpaceRaceGame.Players)
                {
                    if (person.AtFinish == true)
                    {
                        game_over = true;
                    }
                }

            } while (!game_over); 

            FinalInfo();

            PressEnter();

        }//end Main

   
        /// <summary>
        /// Display a welcome message to the console
        /// Pre:    none.
        /// Post:   A welcome message is displayed to the console.
        /// </summary>
        static void DisplayIntroductionMessage()
        {
            string input;
            int number;

            Console.WriteLine("     Welcome to Space Race.\n");
            Console.WriteLine("     This game is for 2 to 6 players.");
            Console.WriteLine("     How many players (2-6): ");
            input = Console.ReadLine();
            int.TryParse(input, out number);

            while (!int.TryParse(input, out number) || Convert.ToInt16(input) > 6 || Convert.ToInt16(input) < 2)
            {
                Console.WriteLine("Error: invalid number of players entered.");
                Console.WriteLine("     This game is for 2 to 6 players.");
                Console.WriteLine("     How many players (2-6): ");

                input = Console.ReadLine();
                int.TryParse(input, out number);
            }

            SpaceRaceGame.NumberOfPlayers = number;

        } //end DisplayIntroductionMessage

        /// <summary>
        /// Displays a prompt and waits for a keypress.
        /// Pre:  none
        /// Post: a key has been pressed.
        /// </summary>
        static void PressEnter()
        {
            Console.Write("\nPress Enter to terminate program ...");
            Console.ReadLine();
        } // end PressAny

        static void FinalInfo()
        {
            Console.WriteLine("The following player(s) finished the game\n");
            foreach (Object_Classes.Player person in SpaceRaceGame.Players)
            {
                if (person.AtFinish == true)
                {
                    Console.WriteLine("      " + person.Name);
                }
            }

            Console.WriteLine("\nIndividual player finished at the locations specified.\n");
            foreach (Object_Classes.Player person in SpaceRaceGame.Players)
            {
                Console.WriteLine("      " + person.Name + " with " + person.RocketFuel + " yattowatt of power at square " + person.Position + "\n");
            }
        }



    }//end Console class
}
