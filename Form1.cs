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

namespace Lesson_1
{
    public partial class Form1 : Form
    {
        public Form1() { InitializeComponent(); }

        private void Form1_Load(object sender, EventArgs e)
        {
            path = new SqlConnection("Data Source=CYBER-HACKER\\SQLEXPRESS;Initial Catalog=Library;Integrated Security=True;");
            MultiQueries();

        }

        public void SelectQuery()
        {
            SqlDataReader reader = null;

            try
            {
                path.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM Authors", path);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    cbxAuthors.Items.Add(reader.GetString(1) + " " + reader.GetString(2));
                }
            }
            finally
            {
                path?.Close();
                reader?.Close();
            }

        }

        public void MultiQueries()
        {
            try
            {
                path.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM Authors; SELECT * FROM Categories; SELECT * FROM Books", path);
                reader = cmd.ExecuteReader();

                int step = 0;

                do
                {
                    while (reader.Read())
                    {
                        if (step == 0) cbxAuthors.Items.Add(reader.GetString(1));
                        else if (step == 1) cbxCategories.Items.Add(reader.GetString(1));
                        else if (step == 2) cbxBooks.Items.Add(reader.GetString(1));
                    }
                    step++;

                } while (reader.NextResult());
            }
            finally
            {
                path?.Close();
                reader?.Close();
            }
        }
        SqlDataReader reader = null;
        SqlConnection path = null;

        private void cbxAuthors_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlDataReader reader = null;
            lbxLists.Items.Clear();
            try
            {
                path.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM Books JOIN Authors ON Authors.Id = Books.Id_Author WHERE Authors.FirstName = @p1", path);

                string aaa = cbxAuthors.SelectedItem.ToString();

                cmd.Parameters.AddWithValue("@p1", aaa);

                reader = cmd.ExecuteReader();

                while (reader.Read()) lbxLists.Items.Add(reader.GetString(1));
            }
            finally
            {
                path?.Close();
                reader?.Close();
            }
        }
    }
}
