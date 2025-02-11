using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class Dashboard : Form
    {

        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\ASUS TUF\Documents\KickBlastJudo DB.mdf"";Integrated Security=True;Connect Timeout=30;Encrypt=False";

        public Dashboard()
        {
            InitializeComponent();
        }
        private void Dashboard_Load(object sender, EventArgs e)
        {
            LoadAthleteNames();
            Months();
            
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

        public void ShowAthleteBasicDetails()
        {
            
            string name = comboBox5.Text;  

            
            if (!string.IsNullOrEmpty(name))
            {
                string query = "SELECT cweight, age FROM Athelete WHERE AtheleteName = @name";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@name", name);

                    SqlDataReader reader = cmd.ExecuteReader();

                  
                    if (reader.Read())
                    {
                        
                        decimal weight = reader.IsDBNull(reader.GetOrdinal("cweight")) ? 0 : reader.GetDecimal(reader.GetOrdinal("cweight"));
                        int age = reader.IsDBNull(reader.GetOrdinal("age")) ? 0 : reader.GetInt32(reader.GetOrdinal("age"));

                        textBox2.Text = name;
                        textBox2.ReadOnly = true;
                        textBox1.Text = age.ToString();
                        textBox1.ReadOnly = true;
                        textBox3.Text = weight.ToString();
                        textBox3.ReadOnly = true;
                    }
                    else
                    {
                        textBox2.Text = name;
                        textBox2.ReadOnly = true;
                        textBox1.Text = "No data";
                        textBox1.ReadOnly = true;
                        textBox3.Text = "No data";
                        textBox3.ReadOnly = true;
                    }
                }
            }
            else
            {
                textBox2.Text = name;
                textBox2.ReadOnly = true;
                textBox1.Text = "Select an athlete";
                textBox3.Text = "Select an athlete";
                textBox1.ReadOnly = true;
                textBox3.ReadOnly = true;
            }
        }

        void Showatheletecost()
        {
            string name = comboBox5.Text;
            string atheleteID;
            string month = comboBox4.Text;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

               
                string atheleteidqury = "SELECT AtheleteID FROM Athelete WHERE AtheleteName = @name";
                using (SqlCommand cmd = new SqlCommand(atheleteidqury, conn))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    object result = cmd.ExecuteScalar();
                    atheleteID = result != null ? result.ToString() : string.Empty;
                }

                if (string.IsNullOrEmpty(atheleteID))
                {
                    Console.WriteLine("Athlete ID not found.");
                    return;
                }

                
                string trainingPlanID = "";
                int noOfPlans = 0, ptHours = 0, noOfComps = 0;
                string weightCategoryName = "Not assigned";

                string recorddataqry = @"
            SELECT ar.Trainingplan, ar.Noofplans, ar.PThours, 
                   wc.cat_name AS weightCategoryName, ar.no_of_comp 
            FROM Atheleterec ar
            LEFT JOIN W_category_table wc ON ar.weight_cat = wc.cat_ID
            WHERE ar.recmonth = @month AND ar.AtheleteID = @atheleteID";

                using (SqlCommand cmd = new SqlCommand(recorddataqry, conn))
                {
                    cmd.Parameters.AddWithValue("@atheleteID", atheleteID);
                    cmd.Parameters.AddWithValue("@month", month);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            trainingPlanID = reader.IsDBNull(reader.GetOrdinal("Trainingplan")) ? "" : reader.GetString(reader.GetOrdinal("Trainingplan"));
                            noOfPlans = reader.IsDBNull(reader.GetOrdinal("Noofplans")) ? 0 : reader.GetInt32(reader.GetOrdinal("Noofplans"));
                            ptHours = reader.IsDBNull(reader.GetOrdinal("PThours")) ? 0 : reader.GetInt32(reader.GetOrdinal("PThours"));
                            weightCategoryName = reader.IsDBNull(reader.GetOrdinal("weightCategoryName")) ? "Not assigned" : reader.GetString(reader.GetOrdinal("weightCategoryName"));
                            noOfComps = reader.IsDBNull(reader.GetOrdinal("no_of_comp")) ? 0 : reader.GetInt32(reader.GetOrdinal("no_of_comp"));
                        }
                        else
                        {
                            Console.WriteLine("No records found for the given athlete and month.");
                            return;
                        }
                    }
                }

                if (string.IsNullOrEmpty(trainingPlanID))
                {
                    Console.WriteLine("Training Plan ID not found.");
                    return;
                }

                
                string trainingPlanName = "";
                decimal trainingPlanCost = 0;

                string trainingPlanQuery = "SELECT Tplan_name, cost FROM Trainingplantb WHERE TPlan_ID = @TPlanID";
                using (SqlCommand cmd = new SqlCommand(trainingPlanQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@TPlanID", trainingPlanID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            trainingPlanName = reader.IsDBNull(reader.GetOrdinal("Tplan_name")) ? "Unknown" : reader.GetString(reader.GetOrdinal("Tplan_name"));
                            trainingPlanCost = reader.IsDBNull(reader.GetOrdinal("cost")) ? 0 : reader.GetDecimal(reader.GetOrdinal("cost"));
                        }
                    }
                }

               
                decimal ptCost = 0, compCost = 0;

                string otherservqry = "SELECT Oservice_name, cost FROM other_services WHERE Oservice_name IN (@service1, @service2)";
                using (SqlCommand cmd = new SqlCommand(otherservqry, conn))
                {
                    cmd.Parameters.AddWithValue("@service1", "Personal Training");
                    cmd.Parameters.AddWithValue("@service2", "Competition Entry fee");

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string serviceName = reader.GetString(reader.GetOrdinal("Oservice_name"));
                            decimal cost = reader.GetDecimal(reader.GetOrdinal("cost"));

                            if (serviceName == "Personal Training")
                                ptCost = cost;
                            else if (serviceName == "Competition Entry fee")
                                compCost = cost;
                        }
                    }
                }

               
                textBox4.Text = weightCategoryName; 
                textBox5.Text = trainingPlanName;
                textBox6.Text = (noOfPlans * trainingPlanCost).ToString();
                textBox8.Text = (ptHours * ptCost).ToString();
                textBox9.Text = (noOfComps * compCost).ToString();
                decimal totalMonthlyCost = (noOfPlans * trainingPlanCost) + (ptHours * ptCost) + (noOfComps * compCost);
                textBox10.Text = ("Rs. " + totalMonthlyCost.ToString());

              
                textBox4.ReadOnly = true;
                textBox5.ReadOnly = true;
                textBox6.ReadOnly = true;
                textBox8.ReadOnly = true;
                textBox9.ReadOnly = true;
                textBox10.ReadOnly = true;
            }
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Athelete_form Athleteform = new Athelete_form();
            Athleteform.Show();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowAthleteBasicDetails();
            
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            Showatheletecost();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
