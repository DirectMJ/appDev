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
using MySql;
using MySql.Data.MySqlClient;

namespace _046_Pattaguan_Caranguian_F2
{
    public partial class Form1 : Form
    {
        string connectionString = "server=localhost; database=dbpirates; uid=root; pwd=Daenerys@26; port=3306";
        string query;
        MySqlConnection conn;
        DataTable dt = new DataTable();
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            query = "select piratename as ALIAS, givenname as NAME, prgname as PIRATEGROUP, bounty as BOUNTY, age from crew c join pirategroup p on c.prgid = p.prgid";
            conn = new MySqlConnection(connectionString);
            conn.Open();
            MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
            adapter.Fill(dt);
            conn.Close();
            dtgRecord.DataSource = dt;

            dtgRecord.Columns["age"].Visible = false;

            disable();

            
            //populate combobox
            query = "select prgname from pirategroup";
            conn = new MySqlConnection(connectionString);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(query, conn);
            MySqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                cboGroup.Items.Add(dr["prgname"].ToString());
                cboPrtGrp.Items.Add(dr["prgname"].ToString());
            }

            dr.Close();
            conn.Close();


        }


        void disable()
        {
            txtAlias.Enabled = false;
            txtAge.Enabled = false;
            txtBounty.Enabled = false;
            txtName.Enabled = false;
            cboPrtGrp.Enabled = false;
            btnSave.Enabled = false;
        }
        //search
        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearch.Text) || string.IsNullOrEmpty(cboGroup.Text))
            {
                MessageBox.Show("All input fields are required!");
            }
            else
            {
                dt = new DataTable();

                query = "select piratename, givenname, prgname, bounty, age from crew c join pirategroup p on c.prgid = p.prgid where piratename = '"+txtSearch.Text+"' or givenname = '"+txtSearch.Text+"'";
                conn = new MySqlConnection(connectionString);
                conn.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                adapter.Fill(dt);
                conn.Close();
                dtgRecord.DataSource = dt;
            }


        }
        //view
        private void btnView_Click(object sender, EventArgs e)
        {
            txtAlias.Text = dtgRecord.SelectedCells[0].Value.ToString();
            txtName.Text = dtgRecord.SelectedCells[1].Value.ToString();
            cboPrtGrp.Text = dtgRecord.SelectedCells[2].Value.ToString();
            txtBounty.Text = dtgRecord.SelectedCells[3].Value.ToString();
            txtAge.Text = dtgRecord.CurrentRow.Cells["age"].Value.ToString();



            txtAlias.Enabled = true;
            txtAge.Enabled = true;
            txtBounty.Enabled = true;
            txtName.Enabled = true;
            cboPrtGrp.Enabled = true;
            btnSave.Enabled =true;
            btnNewRec.Enabled = false;
        }
        //update
        private void btnSave_Click(object sender, EventArgs e)
        {
            query = "select prgid from pirategroup where prgname = '" + cboPrtGrp.SelectedItem.ToString() + "'";
            conn = new MySqlConnection(connectionString);
            conn.Open();
            MySqlCommand mdc = new MySqlCommand(query, conn);
            int result = (int)mdc.ExecuteScalar();
            conn.Close();


            query = "update crew set piratename = '" + txtAlias.Text + "', givenname = '" + txtName.Text + "', age = '" + txtAge.Text + "', prgid = '"+result.ToString()+"',bounty = '" + txtBounty.Text + "' where crewid = '" + txtcrewID.Text + "'";
            conn = new MySqlConnection(connectionString);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
           
        }


        //delete
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dtgRecord.SelectedRows.Count > 0)
            {
                int rowIndex = dtgRecord.SelectedRows[0].Index;
                dtgRecord.Rows.RemoveAt(rowIndex);

                dtgRecord.DataSource = null;
                dtgRecord.DataSource = dt;
            }
            else
            {
                MessageBox.Show("Please select a row to delete!!");
            }

        }


        //insert

    }
}
