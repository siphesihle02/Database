using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dapper;
using System.Data.SqlClient;

namespace timeSheets
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private SqlConnection CreateConnection()
        {
            return new SqlConnection("Server = localhost\\SQLEXPRESS;Initial Catalog=Timesheets;Integrated Security=True");
        }
        private void updateoutput(IEnumerable<object> lists)
        {
            resultTxt.Clear();
            foreach (var list in lists) 
               

            {
                resultTxt.Text += list;


            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }


        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }



        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void label2_Click_1(object sender, EventArgs e)
        {


        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            var months = comboBox1.SelectedItem.ToString();
            var year = comboBox2.SelectedItem.ToString();


            using (var conn = CreateConnection())
            {

                var getHours = conn.Query<Users>(@"SELECT u.Username, t.TotalHoursCaptured FROM Users AS u
                                                    JOIN (SELECT UserId,SUM(HoursCaptured) AS TotalHoursCaptured FROM Timeslots
                                                    WHERE DATENAME(month, Date)=@month AND YEAR(Date) = @year 
                                                    GROUP BY UserId
                                                    ) AS t 
                                                    ON t.UserId = u.UserId"
                                                  , new { month = months, year = year });

                updateoutput(getHours);



            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var months = comboBox1.SelectedItem.ToString();
            var year = comboBox2.SelectedItem.ToString();


            using (var conn = CreateConnection())
            {
                var getHours = conn.Query<Projects>(@"SELECT u.Name, t.TotalHoursCaptured FROM Projects AS u
                                                    JOIN (SELECT ProjectId,SUM(HoursCaptured) AS TotalHoursCaptured FROM Timeslots
                                                    WHERE DATENAME(month, Date)=@month AND YEAR(Date) = @year 
                                                    GROUP BY ProjectId
                                                    ) AS t 
                                                    ON t.ProjectId = u.ProjectId"
                                                    , new { month = months, year = year });


                updateoutput(getHours);
            }

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void GetUsers_Click(object sender, EventArgs e)
        {

            using (var conn = CreateConnection())
            {
                var getString = (@"SELECT * FROM Users
                                        WHERE Username LIKE @name");
                updateoutput(conn.Query<Users>(getString, new { name = "%" + usersTxt.Text + "%" }));
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void getTimeslots_Click(object sender, EventArgs e)
        {
            using (var conn = CreateConnection())
            {
                {
                    var sqlGetTopMatching = (@"SELECT t.* FROM Timeslots AS t
                                        JOIN (SELECT TOP (1) * FROM Users
                                        WHERE Username LIKE @name) AS u ON t.UserId=u.UserId
                                        WHERE DATENAME(month, t.Date)=@month AND YEAR(t.Date) = @year");

                    updateoutput(conn.Query<Timeslots>(sqlGetTopMatching, new { name = "%" + nameTxt.Text + "%", month = comboBox3.SelectedItem.ToString(), year = comboBox4.SelectedItem.ToString()  }));
                }
            }
        }

        private void delete_Click(object sender, EventArgs e)
        {
           resultTxt.Clear();
        }
    }




    public class Users
    {

        public string Username { get; set; }
        public float TotalHoursCaptured { get; set; }
        public override string ToString()
        {
            return $"Username: {Username} - Hours: {TotalHoursCaptured}";

        }
    }


    public class Projects
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public float TotalHoursCaptured { get; set; }
        public override string ToString()
        {
            return $"Name:{Name} - Hours:{TotalHoursCaptured}  {"\n\n"}";
        }
    }

    public class Timeslots
    {


        public float HoursCaptured { get; set; }
        public int TimeslotId { get; set; }
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public override string ToString()
        {
            return $"Timeslots ID {TimeslotId}- Project ID{ProjectId}- User ID{UserId}-Date{Date}-Hours{HoursCaptured}";

        }
    }
}

