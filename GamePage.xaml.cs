using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using a04_chop_fiandert;

namespace a04_chop_fiandert
{
    /// Relational Databases Assignment 4 - Trivia Game
    /// Class: EndPage : Window
    /// Purpose: Display end page of game, showing leaderboard, correct answers, user score, and options to play again, or exit.
    public partial class GamePage : Window
    {
        DispatcherTimer LiveTime = new DispatcherTimer();
        private string UserName { get; set; }
        private int GameID { get; set; }
        private int Countdown { get; set; }
        private int TotalScore = 0;
        private bool AnswerRegistered = false;
        private bool AnswerIsCorrect { get; set; }
        private int QuestionCounter = 0;

        private string CurrentCorrectAnswer { get; set; }
        private string CurrentQuestion { get; set; }
        private List<string> CurrentAnswers = new List<string>();
        private string SelectedAnswer { get; set; }

        /*  -- Window Constructor Header Comment
        Name	:	Game -- Constructor
        Purpose :	Initialize all components of the game page, add new entry to game table, get game id, show first question, start timer
        Inputs	:	string name
        Outputs	:	Nothing
        Returns	:	Nothing
        */
        public GamePage(string name)
        {
            InitializeComponent();

            UserName = name;
            Countdown = 20;

            // add a new game to the game table
            DataController dc = new DataController();
            dc.InitializeGame(name);

            // get gameID so we know which row in the game table to update after each round
            GameID = dc.GetCurrentGameID();

            // display the first question
            DisplayQuestion();

            // set up and start the ticker
            LiveTime.Interval = TimeSpan.FromSeconds(1);
            LiveTime.Tick += timer_Tick;
            LiveTime.Start();
        }

        /*  -- Event Header Comment
        Name	:	timer_Tick -- Event
        Purpose :	Check if the user answered, or if they ran out of time
                    This function is executed every second until LiveTime.Stop() is called.
        Inputs	:	object sender, RoutedEventArgs e
        Outputs	:	Nothing
        Returns	:	Nothing
        */
        void timer_Tick(object sender, EventArgs e)
        {
            // decrement the Countdown every second. 
            Countdown--;
            TimerLabel.Content = Countdown.ToString();

            // check every second if the question has been answered
            if (AnswerRegistered)
            {
                // check if question is correct
                if (AnswerIsCorrect)
                {
                    TotalScore += Countdown;
                    UpdateGameTable();
                    Countdown = 20;
                    DisplayQuestion();
                }
                else
                {
                    Countdown = 20;
                    DisplayQuestion();
                }
            }

            if (Countdown == 0)
            {
                // Don't add anything to total score for this question, reset timer, move to next question.
                Countdown = 20;
                UpdateGameTable();
                DisplayQuestion();
            }
        }

        /*  -- Method Header Comment
        Name	:	DisplayQuestion -- Helper Method
        Purpose :	Display each question's answers
        Inputs	:	object sender, RoutedEventArgs e
        Outputs	:	Nothing
        Returns	:	Nothing
        */
        private void DisplayQuestion()
        {
            // reset answer variables
            TimerLabel.Content = 20.ToString();
            AnswerRegistered = false;
            AnswerIsCorrect = false;
            QuestionCounter++;

            // check if we've reached the end of the game. If so, go to leaderboard page
            if (QuestionCounter > 10)
            {
                HandleGameOver();
                return;
            }

            // use the question counter to grab info from the database
            DataController dc = new DataController();
            CurrentAnswers = dc.GetListOfAnswers(QuestionCounter);
            CurrentQuestion = dc.GetQuestion(QuestionCounter);
            CurrentCorrectAnswer = dc.GetCorrectAnswer(QuestionCounter);

            // display question
            QuestionLabel.Text = CurrentQuestion;

            // display answers in the buttons
            AnswerButton1.Content = CurrentAnswers[0];
            AnswerButton2.Content = CurrentAnswers[1];
            AnswerButton3.Content = CurrentAnswers[2];
            AnswerButton4.Content = CurrentAnswers[3];
        }

        /*  -- Method Header Comment
        Name	:	AnswerSelected -- Helper Method
        Purpose :	Check if the user answered correctly
        Inputs	:	string selectedAnswer
        Outputs	:	Nothing
        Returns	:	Nothing
        */
        private void AnswerSelected(string selectedAnswer)
        {
            // check if the answer if correct or not 
            // set the AnswerRegistered flag to true
            // set the AnswerIsCorrect flag

            SelectedAnswer = selectedAnswer;

            if (selectedAnswer == CurrentCorrectAnswer)
            {
                AnswerRegistered = true;
                AnswerIsCorrect = true;
            }
            else
            {
                AnswerRegistered = true;
                AnswerIsCorrect = false;
            }
        }

        /*  -- Method Header Comment
        Name	:	UpdateGameTable -- Helper Method
        Purpose :	Updates the game session table
        Inputs	:	Nothing
        Outputs	:	Nothing
        Returns	:	Nothing
        */
        private void UpdateGameTable()
        {
            int timeTaken = 20 - Countdown;

            DataController dc = new DataController();
            dc.UpdateGameTable(GameID, timeTaken, QuestionCounter);
        }

        /*  -- Method Header Comment
        Name	:	HandleGameOver -- Helper Method
        Purpose :	Handle end of game
        Inputs	:	Nothing
        Outputs	:	Nothing
        Returns	:	Nothing
        */
        private void HandleGameOver()
        {
            LiveTime.Stop();

            // add total score to game table
            DataController dc = new DataController();
            dc.AddTotalTimeToGameTable(TotalScore, GameID);

            // open leaderboard window and send the total score over 
            EndPage ep = new EndPage(TotalScore);
            ep.Show();
            this.Close();
        }

        /*  -- Event Header Comment
        Name	:	AnswerButton_Click -- Event
        Purpose :	Gets the users answer
        Inputs	:	object sender, RoutedEventArgs e
        Outputs	:	Nothing
        Returns	:	Nothing
        */
        private void AnswerButton_Click(object sender, RoutedEventArgs e)
        {
            string selection = (sender as Button).Content.ToString();
            AnswerSelected(selection);
        }
    }
}
