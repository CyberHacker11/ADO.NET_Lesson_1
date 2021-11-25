using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
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
        public Form1()
        {
            InitializeComponent();
            path = ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString;
            MultiQueries();
        }

        public void MultiQueries()
        {
            try
            {
                using (var connection = new SqlConnection(path))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Authors; SELECT * FROM Categories; SELECT * FROM Books", connection);

                    using (var reader = cmd.ExecuteReader())
                    {
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
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButtons.OK);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (authors)
            {
                lbxLists.Items.Clear();
                try
                {
                    using (var connection = new SqlConnection(path))
                    {
                        connection.Open();
                        SqlCommand cmd = new SqlCommand("SELECT * FROM Books JOIN Authors ON Authors.Id = Books.Id_Author WHERE Authors.FirstName = @p1", connection);                       
                        cmd.Parameters.AddWithValue("@p1", cbxAuthors.SelectedItem);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read()) lbxLists.Items.Add(reader.GetString(1));
                            authors = categories = books = false;
                        }
                        
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButtons.OK);
                }
            }
            else if (categories)
            {
                lbxLists.Items.Clear();
                try
                {
                    using (var connection = new SqlConnection(path))
                    {
                        connection.Open();
                        SqlCommand cmd = new SqlCommand("SELECT * FROM Books JOIN Categories ON Categories.Id = Books.Id_Category WHERE Categories.Name = @p1", connection);
                        
                        cmd.Parameters.AddWithValue("@p1", cbxCategories.SelectedItem);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read()) lbxLists.Items.Add(reader.GetString(1));
                            authors = categories = books = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButtons.OK);
                }
            }
            else if (books)
            {
                lbxLists.Items.Clear();
                try
                {
                    using (var connection = new SqlConnection(path))
                    {
                        connection.Open();
                        SqlCommand cmd = new SqlCommand("SELECT * FROM Books JOIN Categories ON Categories.Id = Books.Id_Category JOIN Authors ON Authors.Id = Books.Id_Author WHERE Books.Name = @p1", connection);
                        
                        cmd.Parameters.AddWithValue("@p1", cbxBooks.SelectedItem);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read()) lbxLists.Items.Add(reader.GetString(11) + " |  " + reader.GetString(13) + " " + reader.GetString(14));
                            authors = categories = books = false;
                        }                    
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButtons.OK);
                }
            }
        }

        private void cbxAuthors_SelectedIndexChanged(object sender, EventArgs e)
        {
            authors = true;
            categories = books = false;
        }

        private void cbxBooks_SelectedIndexChanged(object sender, EventArgs e)
        {
            books = true;
            authors = categories = false;
        }

        private void cbxCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            categories = true;
            authors = books = false;
        }

        private void tbxCode_TextChanged(object sender, EventArgs e)
        {
            if (tbxCode.Text != "") btnUpdate.Enabled = true;
            else btnUpdate.Enabled = false;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(path))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand(tbxCode.Text, connection);

                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show($"Code executed successfully", "Welcome", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbxAuthors.Items.Clear();
                cbxCategories.Items.Clear();
                cbxBooks.Items.Clear();
                MultiQueries();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButtons.OK);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (authors) 
            {
                using (var connection = new SqlConnection(path))
                {
                    connection.Open();
                    var cmd = new SqlCommand($"DELETE FROM Authors WHERE Authors.FirstName = @p1", connection);
                    cmd.Parameters.AddWithValue("@p1", $"{cbxAuthors.SelectedItem}");
                    cmd.ExecuteNonQuery();
                    cbxAuthors.Items.Clear();
                    cbxCategories.Items.Clear();
                    cbxBooks.Items.Clear();
                    MultiQueries();
                }
            }
            else if (categories)
            {
                using (var connection = new SqlConnection(path))
                {
                    connection.Open();
                    var cmd = new SqlCommand($"DELETE FROM Categories WHERE Categories.Name = @p1", connection);
                    cmd.Parameters.AddWithValue("@p1", $"{cbxCategories.SelectedItem}");
                    cmd.ExecuteNonQuery();
                    cbxAuthors.Items.Clear();
                    cbxCategories.Items.Clear();
                    cbxBooks.Items.Clear();
                    MultiQueries();
                }
            }
            else if (books)
            {
                using (var connection = new SqlConnection(path))
                {
                    connection.Open();
                    var cmd = new SqlCommand($"DELETE FROM Books WHERE Books.Name = @p1", connection);
                    cmd.Parameters.AddWithValue("@p1", $"{cbxBooks.SelectedItem}");
                    cmd.ExecuteNonQuery();
                    cbxAuthors.Items.Clear();
                    cbxCategories.Items.Clear();
                    cbxBooks.Items.Clear();
                    MultiQueries();
                }
            }
        }

        bool authors = false, categories = false, books = false;
        string path;
    }
}
