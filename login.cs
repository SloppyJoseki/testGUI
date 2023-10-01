using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace testGUI
{
    public partial class loginWindow : Form
    {
        public loginWindow()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void confirmButton_Click(object sender, EventArgs e)
        {
            // Set up a string with the MySQL database login information
            string testdbConnectionString = "Server=localhost;Database=testdb;User ID=root;Password=frog11";
            MySqlConnection connection = new MySqlConnection(testdbConnectionString);

            // Try connecting to the database if this fails print the error message in a box
            try 
            {
                connection.Open();

                string username = txtUserName.Text;
                string password = txtPassword.Text;

                // This SELECT statement searches the table users for a row with a matching username and password
                // the BINARY command just makes sure its case sensitive
                string loginQuery = "SELECT * FROM users WHERE BINARY username = @username AND BINARY password = @password";

                // This object executes the MySQL commands
                MySqlCommand cmd = new MySqlCommand(loginQuery, connection);

                // @username and @password are just placeholders in the statement this puts in the actual values
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);

                /* The ExecuteScalar command returns the first row that matches the username and password
                 * this means that if it does not find a match the object will be set to null which we can test
                 * with a simple if statement
                 */
                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    new dashboard().Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Fucking moron");
                    txtUserName.Clear();
                    txtPassword.Clear();
                }

                // Closing the connection to the database before moving on
                connection.Close();
            }

            catch (MySqlException ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
            }

        }

        private void loginWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                // Ensure that the form is being closed by the user (not programmatically).
                Application.Exit();
            }
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = '•';
        }
    }
}
