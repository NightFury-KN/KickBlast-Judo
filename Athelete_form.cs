using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace KickBlast_Judo
{
    public partial class Athelete_form : Form
    {
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\ASUS TUF\Documents\KickBlastJudo DB.mdf"";Integrated Security=True;Connect Timeout=30;Encrypt=False";

        private void Athelete_form_Load_1(object sender, EventArgs e)
        {
            LoadAthleteNames();
            Loadcompwcat();
            LoadTrainingPlans();
            Months();
            Hours();



        }
        public Athelete_form()
        {
            InitializeComponent();
            trainingplancb.SelectedIndexChanged += trainingplancb_SelectedIndexChanged;
        }

        private void LoadTrainingPlans()
        {
            string query = "SELECT TPlan_ID, Tplan_name FROM Trainingplantb";  

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                trainingplancb.Items.Clear(); 

                while (reader.Read())
                {
                   
                    trainingplancb.Items.Add(new
                    {
                        Text = reader["Tplan_name"].ToString(), 
                        Value = reader["TPlan_ID"].ToString()   
                    });
                }

                
                trainingplancb.DisplayMember = "Text";
                trainingplancb.ValueMember = "Value";
            }
        }




       
        private void LoadAthleteNames()
        {
            string query = "SELECT * FROM Athelete"; 

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                comboBox5.Items.Clear(); 

                while (reader.Read())
                {
                    comboBox5.Items.Add(reader["AtheleteName"].ToString());
                }
            }
        }

       
        private void Loadcompwcat()
        {
            string query = "SELECT cat_ID, cat_name, weightlimit FROM W_category_table"; 

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                comboBox2.Items.Clear(); 

                while (reader.Read())
                {
                    comboBox2.Items.Add(new
                    {
                        Text = reader["cat_name"].ToString(),  
                        Value = reader["cat_ID"].ToString()  
                    });
                }

                comboBox2.DisplayMember = "Text";
                comboBox2.ValueMember = "Value";
            }
        }

        public void Months()
        {
            comboBox4.Items.Add("January");
            comboBox4.Items.Add("February");
            comboBox4.Items.Add("March");
            comboBox4.Items.Add("April");
            comboBox4.Items.Add("May");
            comboBox4.Items.Add("June");
            comboBox4.Items.Add("July");
            comboBox4.Items.Add("August");
            comboBox4.Items.Add("September");
            comboBox4.Items.Add("October");
            comboBox4.Items.Add("November");
            comboBox4.Items.Add("December");
        }

        public void Hours()
        {
            comboBox3.Items.Add(1);
            comboBox3.Items.Add(2);
            comboBox3.Items.Add(3);
            comboBox3.Items.Add(4);
            comboBox3.Items.Add(5);
            comboBox3.Items.Add(6);
            comboBox3.Items.Add(7);
            comboBox3.Items.Add(8);
            comboBox3.Items.Add(9);
            comboBox3.Items.Add(10);
            comboBox3.Items.Add(11);
            comboBox3.Items.Add(12);
            comboBox3.Items.Add(13);
            comboBox3.Items.Add(14);
            comboBox3.Items.Add(15);
            comboBox3.Items.Add(16);
            comboBox3.Items.Add(17);
            comboBox3.Items.Add(18);
            comboBox3.Items.Add(19);
            comboBox3.Items.Add(20);

        }
        
        private string GenerateAthleteID()
        {
            string query = "SELECT TOP 1 AtheleteID FROM Athelete ORDER BY AtheleteID DESC"; 

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                string lastAtheleteID = cmd.ExecuteScalar()?.ToString(); 

                if (lastAtheleteID != null)
                {
                    
                    int lastIDNumber = int.Parse(lastAtheleteID.Substring(1));  
                    return "A" + (lastIDNumber + 1); 
                }
                else
                {
                    
                    return "A1";
                }
            }
        }

        private string GenerateRecordID()
        {
            
            string query = "SELECT TOP 1 RecordID FROM Atheleterec ORDER BY RecordID DESC";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                string lastRecordID = cmd.ExecuteScalar()?.ToString();

                if (lastRecordID != null)
                {
                    int lastIDNumber = int.Parse(lastRecordID.Substring(1));  
                    return "R" + (lastIDNumber + 1);
                }
                else
                {
                    return "R1";  
                }
            }
        }


        
        private void button1_Click(object sender, EventArgs e)
        {
            string athleteID = GenerateAthleteID();
            string Month = comboBox4.Text;
            string Name = comboBox5.Text;
            int Age = int.Parse(textBox2.Text);
            decimal Weight = decimal.Parse(textBox1.Text);

            
            string Strainingplan = "";
            if (trainingplancb.SelectedItem != null)
            {
                Strainingplan = (trainingplancb.SelectedItem as dynamic).Value.ToString();  
            }
            else
            {
                MessageBox.Show("Please select a valid training plan.","Input Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation);
                return;
            }

            int Nofplans = 0;
            if (int.Parse(textBox3.Text) > 4)
            {
                MessageBox.Show("You can only have 4 weekly plans per month", "Input Error",MessageBoxButtons.RetryCancel,MessageBoxIcon.Exclamation );
                
                return;
            }
            else
            {
               
                Nofplans = int.Parse(textBox3.Text);
            }
                
            string scompWcat = null;
            int Nofcomps = 0;

           
            if (Strainingplan == "Beginner")
            {
                
                scompWcat = null;
                Nofcomps = 0; 
            }
            else
            {
                
                if (comboBox2.SelectedItem != null)
                {
                    scompWcat = (comboBox2.SelectedItem as dynamic)?.Value.ToString();  
                }

                
                if (!string.IsNullOrEmpty(scompWcat))
                {
                    
                    if (string.IsNullOrEmpty(textBox4.Text) || !int.TryParse(textBox4.Text, out Nofcomps) || Nofcomps < 1)
                    {
                        MessageBox.Show("Please enter a valid number of competitions.", "Input Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation);
                        return;
                    }
                }
                else
                {
                    
                    Nofcomps = 0;
                }
            }

            int NofPChours = 0;
            if (comboBox3.SelectedItem != null)
            {
                NofPChours = int.Parse(comboBox3.SelectedItem.ToString());
            }

          
            string checkAtheletequery = "SELECT COUNT(*) from Athelete where AtheleteName = @Name ";
            int atheleteExists = 0;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(checkAtheletequery, conn);
                cmd.Parameters.AddWithValue("@Name", Name);
                atheleteExists = (int)cmd.ExecuteScalar();
            }

            
            if (atheleteExists == 0)
            {
                string insertAtheletequery = "INSERT INTO Athelete(AtheleteID, AtheleteName, age, cweight) " +
                                "VALUES (@AtheleteID, @Name, @Age, @Weight)";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(insertAtheletequery, conn);
                    cmd.Parameters.AddWithValue("@AtheleteID", athleteID);
                    cmd.Parameters.AddWithValue("@Name", Name);
                    cmd.Parameters.AddWithValue("@Age", Age);
                    cmd.Parameters.AddWithValue("@weight", Weight);
                    cmd.ExecuteNonQuery();
                }
            }
            else
            {
                string fetchAtheleteIDQuery = "SELECT AtheleteID FROM Athelete WHERE AtheleteName = @Name";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(fetchAtheleteIDQuery, conn);
                    cmd.Parameters.AddWithValue("@Name", Name);
                    athleteID = cmd.ExecuteScalar().ToString();  
                }
            }

            
            string recordID = GenerateRecordID();
            string insertAthleteRecordQuery = "INSERT INTO Atheleterec (RecordID, recmonth, AtheleteID, Trainingplan,Noofplans, PThours, weight_cat, no_of_comp) " +
                                              "VALUES (@RecordID, @Month, @AtheleteID, @TrainingPlan,@Nofplans, @Pthours, @WeightCategory, @Competitions)";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(insertAthleteRecordQuery, conn);
                    cmd.Parameters.AddWithValue("@RecordID", recordID);
                    cmd.Parameters.AddWithValue("@Month", Month);
                    cmd.Parameters.AddWithValue("@AtheleteID", athleteID);  
                    cmd.Parameters.AddWithValue("@TrainingPlan", Strainingplan);  
                    cmd.Parameters.AddWithValue("@Nofplans", Nofplans);
                    cmd.Parameters.AddWithValue("@WeightCategory", (object)scompWcat ?? DBNull.Value);  
                    cmd.Parameters.AddWithValue("@Pthours", NofPChours);
                    cmd.Parameters.AddWithValue("@Competitions", Nofcomps);  
                    cmd.ExecuteNonQuery();  
                }
                MessageBox.Show("Athlete and Record successfully added.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }




        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
            Dashboard db = new Dashboard();
            db.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            Dashboard db = new Dashboard();
            db.Show();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void trainingplancb_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedPlan = trainingplancb.Text;

            if (selectedPlan == "Beginner")
            {
                comboBox2.Enabled = false; 
                textBox4.Enabled = false;
                
            }
            else
            {
                comboBox2.Enabled = true;
                textBox4.Enabled = true;
            }
        }
        
        

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                comboBox3.Enabled = true;
            }
            else
            {
                comboBox3.Enabled = false;
            }
        }
    }
}
