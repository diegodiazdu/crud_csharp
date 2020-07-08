
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUD_Nota_Final
{
    public partial class Form2 : Form
    {

        public MySqlConnection conexion;
        public string sqlconexion;
        public Form2()
        {
            InitializeComponent();
            
            sqlconexion = ";server=localhost;user id=root;database=crud;password=";
            
        }
        

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 index = new Form1();
            this.Hide();
            index.Show();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
           
            try
            {
                conexion = new MySqlConnection();
                conexion.ConnectionString = sqlconexion;
                conexion.Open();
                cargarGrid2("");
                llenarCombo();
                
           
            }
            catch (MySqlException exc)
            {
                MessageBox.Show(exc.Message);
            }
          
        } 
       
        public void cargarGrid2(string cadena)
        {
            string select = "select * from empresas " + cadena;
            MySqlCommand opSelect = new MySqlCommand(select, conexion);
            MySqlDataAdapter datos = new MySqlDataAdapter(opSelect);
            DataSet ds = new DataSet();
            datos.Fill(ds);

            dataGridView1.DataSource = ds.Tables[0];
        }
        public void llenarCombo()
        {
            string llenar = "select idComuna from comunas";
            MySqlCommand opllenar = new MySqlCommand(llenar, conexion);
            MySqlDataAdapter datos = new MySqlDataAdapter(opllenar);
            DataSet ds = new DataSet();
            datos.Fill(ds);

            comboBox1.ValueMember = "idComuna";
            comboBox1.DisplayMember = "idComuna";
            comboBox1.DataSource = ds.Tables[0];
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "")
                {
                    MessageBox.Show("No pueden ingresar campos en blanco",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cargarGrid2("");
                }
                else
                {
                    string insert = "INSERT INTO empresas(idEmpresa, nomEmpresa, dirEmpresa, idComuna)VALUES(@param1, @param2,@param3,@param4)";
                    MySqlCommand opInsert = new MySqlCommand(insert, conexion);
                    opInsert.Parameters.AddWithValue("@param1", Convert.ToInt32(textBox1.Text));
                    opInsert.Parameters.AddWithValue("@param2", textBox2.Text);
                    opInsert.Parameters.AddWithValue("@param3", textBox3.Text);
                    opInsert.Parameters.AddWithValue("@param4", comboBox1.SelectedValue);
                    opInsert.ExecuteNonQuery();
                    cargarGrid2("");
                    this.Controls.OfType<TextBox>().ToList().ForEach(o => o.Text = "");
                }

                

            }
            else if (radioButton2.Checked)
            {
                if(textBox1.Text == "")
                {
                    MessageBox.Show("No se a seleccionado un item",
            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("No se puede eliminar el indice seleccionado, ya que está sujeto a una clave foranea." +
                                    "Diríjase al panel de comunas para eliminar el indice deseado",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //Metodo para limpiar todas las cajas de texto
                    this.Controls.OfType<TextBox>().ToList().ForEach(o => o.Text = "");
                }
            

            }
            else if (radioButton3.Checked)
            {
                

                if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "")
                {
                    MessageBox.Show("No se seleccionó un registro",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cargarGrid2("");
                }
                else { 
                //No se puede editar el IDComuna ya que al ser clave foranea debe ser editada en cascada desde comunas
                    string update = "update empresas set nomEmpresa=@param1,dirEmpresa=@param2 where idEmpresa=@param4";
                    MySqlCommand opUpdate = new MySqlCommand(update, conexion);
                    opUpdate.Parameters.AddWithValue("@param1", textBox2.Text);
                    opUpdate.Parameters.AddWithValue("@param2", textBox3.Text);
                   //opUpdate.Parameters.AddWithValue("@param3", comboBox1.SelectedItem);
                    opUpdate.Parameters.AddWithValue("@param4", Convert.ToInt32(textBox1.Text));
                    opUpdate.ExecuteNonQuery();
                    cargarGrid2(" ");
                    this.Controls.OfType<TextBox>().ToList().ForEach(o => o.Text = "");
                }
            }
            else if (radioButton4.Checked)
            {
                textBox2.Focus();
                cargarGrid2("where nomEmpresa like " + "'%" + textBox2.Text + "%'");
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
            {
                comboBox1.Enabled = false;
            }
            else
            {
                comboBox1.Enabled = true;
            }
        }
    }
}
