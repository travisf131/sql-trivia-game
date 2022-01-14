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

namespace a04_chop_fiandert
{
    public partial class EndPage : Window
    {
        /*  -- Window Constructor Header Comment
        Name	:	EndPage -- Constructor
        Purpose :	Initialize all components of the end page, populating the leaderboard, and the correct answers
        Inputs	:	int totalScore
        Outputs	:	Nothing
        Returns	:	Nothing
        */
        public EndPage(int totalScore)
        {
            InitializeComponent();

            // show total score
            ScoreLabel.Text = "Your Score: " + totalScore.ToString();

            PopulateCorrectAnswers();
            PopulateLeaderBoard();
        }

        /*  -- Event Header Comment
        Name	:	PlayAgainButton_Click -- Event
        Purpose :	Close current window, start the game again
        Inputs	:	object sender, RoutedEventArgs e
        Outputs	:	Nothing
        Returns	:	Nothing
        */
        private void PlayAgainButton_Click(object sender, RoutedEventArgs e)
        {
            StartPage startPage = new StartPage();
            this.Close();
            startPage.Show();
        }

        /*  -- Event Header Comment
        Name	:	ExitButton_Click -- Event
        Purpose :	Closes the game
        Inputs	:	object sender, RoutedEventArgs e
        Outputs	:	Nothing
        Returns	:	Nothing
        */
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /*  -- Method Header Comment
        Name	:	PopulateLeaderBoard -- Helper Method
        Purpose :	Populates the leader board on the end page
        Inputs	:	Nothing
        Outputs	:	Nothing
        Returns	:	Nothing
        */
        private void PopulateLeaderBoard()
        {
            // populate leaderboard
            DataController dc = new DataController();
            List<Tuple<string, int>> leaderboardData = dc.GetLeaderboardData();

            foreach (Tuple<string, int> score in leaderboardData)
            {
                LeaderboardListBox.Items.Add(score.Item1 + " --- High Score: " + score.Item2.ToString());
            }
        }

        /*  -- Method Header Comment
        Name	:	PopulateCorrectAnswers -- Helper Method
        Purpose :	Populates the correct answers list on the end page
        Inputs	:	Nothing
        Outputs	:	Nothing
        Returns	:	Nothing
        */
        private void PopulateCorrectAnswers()
        {
            List<string> AllQuestions = new List<string>();
            List<string> AllCorrectAnswers = new List<string>();
            DataController dc = new DataController();

            for (int i = 1; i <= 10; i++)
            {
                AllQuestions.Add(dc.GetQuestion(i));
                AllCorrectAnswers.Add(dc.GetCorrectAnswer(i));
            }

            for (int i = 0; i < 10; i++)
            {
                CorrectAnswersList.Items.Add(AllQuestions[i]);
                CorrectAnswersList.Items.Add(AllCorrectAnswers[i]);
                CorrectAnswersList.Items.Add("");
            }
        }

    }
}
