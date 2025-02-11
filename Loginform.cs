using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace KickBlast_Judo
{
    public partial class Loginform : Form
    {
        
        public Loginform()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Exit Application", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==DialogResult.Yes)
                {
                Application.Exit();
                }
        }

        private void button_login_Click(object sender, EventArgs e)
        {
            string username = textBox_Username.Text;
            string password = textBox_password.Text;
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\ASUS TUF\Documents\KickBlastJudo DB.mdf"";Integrated Security=True;Connect Timeout=30;Encrypt=False";

            string query = "SELECT COUNT(*) FROM Usertable WHERE username = @username AND upassword = @password";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);

                    int userCount = (int)cmd.ExecuteScalar();

                    if (userCount>0)
                    {
                        MessageBox.Show("Login Successfull", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                        this.Hide();
                        Dashboard dashboard = new Dashboard();
                        dashboard.Show();
                    }
                    else
                    {
                        MessageBox.Show("Invalied Username or Password", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }

            }
        }

        private void textBox_Username_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
