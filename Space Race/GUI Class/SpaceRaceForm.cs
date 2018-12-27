using System;
//  Uncomment  this using statement after you have remove the large Block Comment below 
using System.Drawing;
using System.Windows.Forms;
using Game_Logic_Class;
//  Uncomment  this using statement when you declare any object from Object Classes, eg Board,Square etc.
using Object_Classes;

namespace GUI_Class
{
    public partial class SpaceRaceForm : Form
    {
        // The numbers of rows and columns on the screen.
        const int NUM_OF_ROWS = 7;
        const int NUM_OF_COLUMNS = 8;

        // When we update what's on the screen, we show the movement of a player 
        // by removing them from their old square and adding them to their new square.
        // This enum makes it clear that we need to do both.
        enum TypeOfGuiUpdate { AddPlayer, RemovePlayer };


        public SpaceRaceForm()
        {
            InitializeComponent();
            Board.SetUpBoard();
            ResizeGUIGameBoard();
            SetUpGUIGameBoard();
            SetupPlayersDataGridView();
            DetermineNumberOfPlayers();
            SpaceRaceGame.SetUpPlayers();
            PrepareToPlay();
        }


        /// <summary>
        /// Handle the Exit button being clicked.
        /// Pre:  the Exit button is clicked.
        /// Post: the game is terminated immediately
        /// </summary>
        private void exitButton_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        /// <summary>
        /// Resizes the entire form, so that the individual squares have their correct size, 
        /// as specified by SquareControl.SQUARE_SIZE.  
        /// This method allows us to set the entire form's size to approximately correct value 
        /// when using Visual Studio's Designer, rather than having to get its size correct to the last pixel.
        /// Pre:  none.
        /// Post: the board has the correct size.
        /// </summary>
        private void ResizeGUIGameBoard()
        {
            const int SQUARE_SIZE = SquareControl.SQUARE_SIZE;
            int currentHeight = tableLayoutPanel.Size.Height;
            int currentWidth = tableLayoutPanel.Size.Width;
            int desiredHeight = SQUARE_SIZE * NUM_OF_ROWS;
            int desiredWidth = SQUARE_SIZE * NUM_OF_COLUMNS;
            int increaseInHeight = desiredHeight - currentHeight;
            int increaseInWidth = desiredWidth - currentWidth;
            this.Size += new Size(increaseInWidth, increaseInHeight);
            tableLayoutPanel.Size = new Size(desiredWidth, desiredHeight);
        }// ResizeGUIGameBoard


        /// <summary>
        /// Creates a SquareControl for each square and adds it to the appropriate square of the tableLayoutPanel.
        /// Pre:  none.
        /// Post: the tableLayoutPanel contains all the SquareControl objects for displaying the board.
        /// </summary>
        private void SetUpGUIGameBoard()
        {
            for (int squareNum = Board.START_SQUARE_NUMBER; squareNum <= Board.FINISH_SQUARE_NUMBER; squareNum++)
            {
                Square square = Board.Squares[squareNum];
                SquareControl squareControl = new SquareControl(square, SpaceRaceGame.Players);
                AddControlToTableLayoutPanel(squareControl, squareNum);
            }//endfor

        }// end SetupGameBoard

        private void AddControlToTableLayoutPanel(Control control, int squareNum)
        {
            int screenRow = 0;
            int screenCol = 0;
            MapSquareNumToScreenRowAndColumn(squareNum, out screenRow, out screenCol);
            tableLayoutPanel.Controls.Add(control, screenCol, screenRow);
        }// end Add Control


        /// <summary>
        /// For a given square number, tells you the corresponding row and column number
        /// on the TableLayoutPanel.
        /// Pre:  none.
        /// Post: returns the row and column numbers, via "out" parameters.
        /// </summary>
        /// <param name="squareNumber">The input square number.</param>
        /// <param name="rowNumber">The output row number.</param>
        /// <param name="columnNumber">The output column number.</param>
        private static void MapSquareNumToScreenRowAndColumn(int squareNum, out int screenRow, out int screenCol)
        {
            screenCol = 0;
            screenRow = 0;
            int count = 0;
            // For each row, position the squares accordingly
            for (int i = 48; i >= 0; i -= 8)
            {
                if (squareNum >= i)
                {
                    // If square number is increasing from left to right,
                    // Column is the difference between the squareNum and index.
                    if (i == 48 || i == 32 || i == 16 || i == 0)
                    {
                        screenCol = squareNum - i;
                    }
                    // If square number is increasing from right to left
                    else
                    {
                        screenCol = 7 - (squareNum - i);
                    }
                    screenRow = count;
                    break;
                }
                count++;
            }
        }//end MapSquareNumToScreenRowAndColumn


        private void SetupPlayersDataGridView()
        {
            // Stop the playersDataGridView from using all Player columns.
            playersDataGridView.AutoGenerateColumns = false;
            // Tell the playersDataGridView what its real source of data is.
            playersDataGridView.DataSource = SpaceRaceGame.Players;

        }// end SetUpPlayersDataGridView



        /// <summary>
        /// Obtains the current "selected item" from the ComboBox
        ///  and
        ///  sets the NumberOfPlayers in the SpaceRaceGame class.
        ///  Pre: none
        ///  Post: NumberOfPlayers in SpaceRaceGame class has been updated
        /// </summary>
        private void DetermineNumberOfPlayers()
        {
            // Store the SelectedItem property of the ComboBox in a string
            // Parse string to a number
            int input = Convert.ToInt32(comboBox1.Text);
            // Set the NumberOfPlayers in the SpaceRaceGame class to that number
            SpaceRaceGame.NumberOfPlayers = input;
        }//end DetermineNumberOfPlayers

        /// <summary>
        /// The players' tokens are placed on the Start square
        /// </summary>
        private void PrepareToPlay()
        {
            // Re-enables buttons and combo box to orignal state.
            comboBox1.Text = "6";
            comboBox1.Enabled = true;
            playersDataGridView.ReadOnly = false;
            resetGame.Enabled = false;
            rollDice.Enabled = false;

            // Re-adds the default number of players
            UpdatePlayersGuiLocations(TypeOfGuiUpdate.AddPlayer);

        }//end PrepareToPlay()


        /// <summary>
        /// Tells you which SquareControl object is associated with a given square number.
        /// Pre:  a valid squareNumber is specified; and
        ///       the tableLayoutPanel is properly constructed.
        /// Post: the SquareControl object associated with the square number is returned.
        /// </summary>
        /// <param name="squareNumber">The square number.</param>
        /// <returns>Returns the SquareControl object associated with the square number.</returns>
        private SquareControl SquareControlAt(int squareNum)
        {
            int screenRow;
            int screenCol;
            MapSquareNumToScreenRowAndColumn(squareNum, out screenRow, out screenCol);
            return (SquareControl)tableLayoutPanel.GetControlFromPosition(screenCol, screenRow);
        }


        /// <summary>
        /// Tells you the current square number of a given player.
        /// Pre:  a valid playerNumber is specified.
        /// Post: the square number of the player is returned.
        /// </summary>
        /// <param name="playerNumber">The player number.</param>
        /// <returns>Returns the square number of the player.</returns>
        private int GetSquareNumberOfPlayer(int playerNumber)
        {
            int counter = 0;
            int pos;
            // Foreach player in the game
            foreach (Object_Classes.Player person in SpaceRaceGame.Players)
            {
                pos = person.Position; // The square number of the player
                if (counter == playerNumber)
                {
                    return pos;
                }
                counter++;
            }

            return -1;
        }//end GetSquareNumberOfPlayer


        /// <summary>
        /// When the SquareControl objects are updated (when players move to a new square),
        /// the board's TableLayoutPanel is not updated immediately.  
        /// Each time that players move, this method must be called so that the board's TableLayoutPanel 
        /// is told to refresh what it is displaying.
        /// Pre:  none.
        /// Post: the board's TableLayoutPanel shows the latest information 
        ///       from the collection of SquareControl objects in the TableLayoutPanel.
        /// </summary>
        private void RefreshBoardTablePanelLayout()
        {
            tableLayoutPanel.Invalidate(true);
        }

        /// <summary>
        /// When the Player objects are updated (location, etc),
        /// the players DataGridView is not updated immediately.  
        /// Each time that those player objects are updated, this method must be called 
        /// so that the players DataGridView is told to refresh what it is displaying.
        /// Pre:  none.
        /// Post: the players DataGridView shows the latest information 
        ///       from the collection of Player objects in the SpaceRaceGame.
        /// </summary>
        private void UpdatesPlayersDataGridView()
        {
            SpaceRaceGame.Players.ResetBindings();
        }

        /// <summary>
        /// At several places in the program's code, it is necessary to update the GUI board,
        /// so that player's tokens are removed from their old squares
        /// or added to their new squares. E.g. at the end of a round of play or 
        /// when the Reset button has been clicked.
        /// 
        /// Moving all players from their old to their new squares requires this method to be called twice: 
        /// once with the parameter typeOfGuiUpdate set to RemovePlayer, and once with it set to AddPlayer.
        /// In between those two calls, the players locations must be changed. 
        /// Otherwise, you won't see any change on the screen.
        /// 
        /// Pre:  the Players objects in the SpaceRaceGame have each players' current locations
        /// Post: the GUI board is updated to match 
        /// </summary>
        private void UpdatePlayersGuiLocations(TypeOfGuiUpdate typeOfGuiUpdate)
        {
            int counter = 0;
            // Foreach player in the game
            foreach(Object_Classes.Player person in SpaceRaceGame.Players)
            {
                int location = GetSquareNumberOfPlayer(counter); //need to get the player number
                SquareControl item = SquareControlAt(location); 

                if (typeOfGuiUpdate == TypeOfGuiUpdate.AddPlayer)
                {
                    item.ContainsPlayers[counter] = true; //Places player token
                }
                if (typeOfGuiUpdate == TypeOfGuiUpdate.RemovePlayer)
                {
                    item.ContainsPlayers[counter] = false; //Removes players token
                }
                counter++;
            }

            RefreshBoardTablePanelLayout();//must be the last line in this method. Do not put inside above loop.
        } //end UpdatePlayersGuiLocations

        private void UpdatePlayerGuiLocation(TypeOfGuiUpdate typeOfGuiUpdate, int index)
        {
            int location = GetSquareNumberOfPlayer(index);
            SquareControl item = SquareControlAt(location);

            if (typeOfGuiUpdate == TypeOfGuiUpdate.AddPlayer)
            {
                item.ContainsPlayers[index] = true;
            }
            if (typeOfGuiUpdate == TypeOfGuiUpdate.RemovePlayer)
            {
                item.ContainsPlayers[index] = false;
            }
            RefreshBoardTablePanelLayout();
        }

        private int turnIndex = 0;

        private void rollDice_Click(object sender, EventArgs e)
        {
            // Stop exiting during round
            exitButton.Enabled = false;
            comboBox1.Enabled = false;
            playersDataGridView.ReadOnly = true;        

            // If playing one round at a time          
            if (stepN.Checked == true)
            {
                // Remove players, play the round and update player location
                UpdatePlayersGuiLocations(TypeOfGuiUpdate.RemovePlayer);
                SpaceRaceGame.PlayOneRound();
                UpdatePlayersGuiLocations(TypeOfGuiUpdate.AddPlayer);
            }
            // If playing one turn at a time      
            else if (stepY.Checked == true)
            {
                // Remove player, play the turn and update the players location
                UpdatePlayerGuiLocation(TypeOfGuiUpdate.RemovePlayer, turnIndex);
                SpaceRaceGame.PlayOneTurn(turnIndex);
                UpdatePlayerGuiLocation(TypeOfGuiUpdate.AddPlayer, turnIndex);
                turnIndex++; // individual player turn increments
                if (turnIndex == SpaceRaceGame.NumberOfPlayers)
                {
                    turnIndex = 0; // After round is completed, prepare for round 2
                }
            }
            //Update the player data table
            UpdatesPlayersDataGridView();

            //At the end of round re-enable controls
            exitButton.Enabled = true;
            resetGame.Enabled = true;
            checkGameOver();
        }

        private void resetGame_Click(object sender, EventArgs e)
        {
            groupBox1.Enabled = true; // Allow players to select single turn or not
            stepN.Checked = false; // Reset single turn radio buttons
            stepY.Checked = false;
            turnIndex = 0; // Reset turn index to 0, allows 

            UpdatePlayersGuiLocations(TypeOfGuiUpdate.RemovePlayer); //Remove all current players
            SpaceRaceGame.Players.Clear(); //Remove all current players from BindingList

            // Re-setup the entire game with default values
            SetupPlayersDataGridView(); 
            DetermineNumberOfPlayers();
            SpaceRaceGame.SetUpPlayers();
            PrepareToPlay();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdatePlayersGuiLocations(TypeOfGuiUpdate.RemovePlayer); //Remove all current players
            comboBox1.Enabled = false; //Disable combo box
            SpaceRaceGame.Players.Clear(); //Remove all current players from BindingList
            DetermineNumberOfPlayers(); //Check the new amount of players selected in combobox
            SpaceRaceGame.SetUpPlayers(); //Re-setup all of the players to the new number
            UpdatePlayersGuiLocations(TypeOfGuiUpdate.AddPlayer); //Add new players to the list
        }

        /// <summary>
        /// Checks each time playOneRound is called to see if someone has reached final square
        /// Pre:  playOneRound called.
        /// Post: wcreates a message box with the winning players names.
        /// </summary>
        private void checkGameOver()
        {
            bool game_over = false;
            string winners = "";

            foreach (Object_Classes.Player person in SpaceRaceGame.Players)
            {
                if (person.AtFinish == true)
                {
                    //Checks if one person has landed on the final square to end the game
                    game_over = true;
                    // The foreach loop still records all of the players names who won
                    // Recording them to a string for the message box
                    winners += person.Name + ", ";
                }
            }

            if (game_over)
            {
                rollDice.Enabled = false;
                // Displays a combobox with the winners names on it
                MessageBox.Show("The following player(s) finished the game:\n\n\t" + winners);
            }
        }

        // When Single Step -> yes is pressed
        private void stepY_Click(object sender, EventArgs e)
        {
            groupBox1.Enabled = false;
            rollDice.Enabled = true;

            // Read the part B instructions
           
        }

        // When Single Step -> no is pressed
        private void stepN_Click(object sender, EventArgs e)
        {
            groupBox1.Enabled = false;
            rollDice.Enabled = true;
        }
    }// end class
}
