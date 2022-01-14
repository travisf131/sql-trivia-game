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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace a04_chop_fiandert
{
    public partial class StartPage : Window
    {
        /*  -- Window Constructor Header Comment
        Name	:	StartPage -- Constructor
        Purpose :	Initialize all components of the start page
        Inputs	:	Nothing
        Outputs	:	Nothing
        Returns	:	Nothing
        */
        public StartPage()
        {
            InitializeComponent();
        }

        /*  -- Event Header Comment
        Name	:	StartButton_Click -- Event
        Purpose :	Validate that the user entered a name, if so, start the game
        Inputs	:	object sender, RoutedEventArgs e
        Outputs	:	Nothing
        Returns	:	Nothing
        */
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            //Check if user has entered name
            if (String.IsNullOrEmpty(UserNameTextBox.Text))
            {
                MessageBox.Show("Error: You must enter a name to play the game!");
            }
            //If so, then continue to the game
            else
            {
                GamePage gp = new GamePage(UserNameTextBox.Text);
                gp.Show();
                this.Close();
            }


        }
    }
}
