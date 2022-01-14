using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.IO;
using System.Windows;

namespace a04_chop_fiandert
{
    class DataController
    {
        MySqlCommand genericCommand { get; set; }
        string commandString { get; set; }

        MySqlConnection dbConnection { get; set; }
        MySqlDataReader reader { get; set; }
        private const string connectionString = "SERVER=" + "localhost" + ";" + "DATABASE=" +
            "triviaGame" + ";" + "UID=" + "root" + ";" + "PASSWORD=" + "L29mkzz1!!" + ";";

        /*  -- Method Header Comment
        Name	:	DataController -- Constructor
        Purpose :	Initializes DB connection
        Inputs	:	Nothing
        Outputs	:	Nothing
        Returns	:	Nothing
        */
        public DataController()
        {
            //Set up connection
            dbConnection = new MySqlConnection(connectionString);

            try
            {
                //Connect
                dbConnection.Open();
            }
            catch (MySqlException e)
            {
                MessageBox.Show("Error: " + e.Message);
            }
        }

        /*  -- Method Header Comment
        Name	:	GetQuestion -- Helper Method
        Purpose :	Gets the current question
        Inputs	:	int questionNumber
        Outputs	:	Nothing
        Returns	:	string question or error message
        */
        public string GetQuestion(int questionNumber)
        {
            string question;
            string query = "SELECT * FROM questions WHERE QuestionID=" + questionNumber.ToString() + ";";
            genericCommand = new MySqlCommand(query, dbConnection);
            reader = genericCommand.ExecuteReader();

            while (reader.Read())
            {
                question = reader.GetString("Question");
                reader.Close();
                return question;
            }

            reader.Close();
            return "There was a problem getting the question from the database";
        }

        /*  -- Method Header Comment
        Name	:	GetCorrectAnswer -- Helper Method
        Purpose :	Gets the correct answer
        Inputs	:	int questionNumber
        Outputs	:	Nothing
        Returns	:	string answer or error message
        */
        public string GetCorrectAnswer(int questionNumber)
        {
            string answer;
            string query = "SELECT CorrectAnswer FROM questionanswers WHERE QuestionID=" + questionNumber.ToString() + ";";
            genericCommand = new MySqlCommand(query, dbConnection);
            reader = genericCommand.ExecuteReader();

            while (reader.Read())
            {
                answer = reader.GetString("CorrectAnswer");
                reader.Close();
                return answer;
            }

            reader.Close();
            return "There was a problem getting the answer from the database";
        }

        /*  -- Method Header Comment
        Name	:	GetListOfAnswers -- Helper Method
        Purpose :	Gets the list of answers
        Inputs	:	int questionNumber
        Outputs	:	Nothing
        Returns	:	List<string> answers
        */
        public List<string> GetListOfAnswers(int questionNumber)
        {
            List<string> answers = new List<string>();
            string query = "SELECT * FROM Answers WHERE QuestionID=" + questionNumber.ToString() + ";";
            genericCommand = new MySqlCommand(query, dbConnection);
            reader = genericCommand.ExecuteReader();

            while (reader.Read())
            {
                answers.Add(reader.GetString("Answer1"));
                answers.Add(reader.GetString("Answer2"));
                answers.Add(reader.GetString("Answer3"));
                answers.Add(reader.GetString("Answer4"));
                reader.Close();
                return answers;
            }

            reader.Close();
            return answers;
        }

        /*  -- Method Header Comment
        Name	:	UpdateGameTable -- Helper Method
        Purpose :	Updates the time take for each question
        Inputs	:	int GameID, int QTime, int roundNumber
        Outputs	:	Nothing
        Returns	:	Nothing
        */
        public void UpdateGameTable(int GameID, int QTime, int roundNumber)
        {
            //Build string that changes the QTime value for a certain user
            commandString = "UPDATE game SET Q" + roundNumber + "Time = " + " " + QTime.ToString() + " WHERE GameID = " + GameID.ToString() + ";";

            //Set up command
            genericCommand = new MySqlCommand(commandString, dbConnection);

            //Execute command
            genericCommand.ExecuteNonQuery();
        }

        /*  -- Method Header Comment
        Name	:	AddTotalTimeToGameTable -- Helper Method
        Purpose :	Sets final total score
        Inputs	:	int totalScore, int GameID
        Outputs	:	Nothing
        Returns	:	Nothing
        */
        public void AddTotalTimeToGameTable(int totalScore, int GameID)
        {
            commandString = "UPDATE game SET FinalScore = " + totalScore.ToString() + " WHERE GameID = " + GameID.ToString() + ";";
            genericCommand = new MySqlCommand(commandString, dbConnection);
            genericCommand.ExecuteNonQuery();
        }

        /*  -- Method Header Comment
        Name	:	InitializeGame -- Helper Method
        Purpose :	Create game entry in db
        Inputs	:	string username
        Outputs	:	Nothing
        Returns	:	Nothing
        */
        public void InitializeGame(string username)
        {
            commandString = "INSERT INTO game (UserName) VALUES ('" + username + "');";
            genericCommand = new MySqlCommand(commandString, dbConnection);
            genericCommand.ExecuteNonQuery();
        }

        /*  -- Method Header Comment
        Name	:	GetCurrentGameID -- Helper Method
        Purpose :	Gets the current game ID
        Inputs	:	Nothing
        Outputs	:	Nothing
        Returns	:	int GameID
        */
        public int GetCurrentGameID()
        {
            commandString = "SELECT * FROM game WHERE GameID=(SELECT MAX(GameID) FROM game);";
            genericCommand = new MySqlCommand(commandString, dbConnection);
            reader = genericCommand.ExecuteReader();

            while (reader.Read())
            {
                int GameID = reader.GetInt32("GameID");
                reader.Close();
                return GameID;
            }

            reader.Close();
            return 0;
        }

        /*  -- Method Header Comment
        Name	:	GetLeaderboardData -- Helper Method
        Purpose :	Gets the leaderboard information from game table
        Inputs	:	Nothing
        Outputs	:	Nothing
        Returns	:	List<Tuple<string, int>> leaderboardData
        */
        public List<Tuple<string, int>> GetLeaderboardData()
        {
            List<Tuple<string, int>> leaderboardData = new List<Tuple<string, int>>();

            // get all game data, ordered from highest to lowest final score
            commandString = "SELECT * FROM game ORDER BY FinalScore DESC;";
            genericCommand = new MySqlCommand(commandString, dbConnection);
            reader = genericCommand.ExecuteReader();

            // build a list of all the user/score pairs
            while (reader.Read())
            {
                int finalScore = reader.GetInt32("FinalScore");
                string user = reader.GetString("UserName");
                var row = Tuple.Create(user, finalScore);
                leaderboardData.Add(row);
            }

            reader.Close();
            return leaderboardData;
        }
    }
}
