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
    public partial class Form3 : Form
    {
        public MySqlConnection con;
        public string url;
        public Form3()
        {
            InitializeComponent();
            url = ";server=localhost;user id=root;database=crud;password=";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form3 index = new Form3();
            this.Hide();
            index.Show();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            try
            {
                con = new MySqlConnection();
                con.ConnectionString = url;
                con.Open();
                cargarGrid(" ");
             
            }
            catch (MySqlException exc)
            {
                MessageBox.Show($"Error al conectar a la base de datos {exc}");
            }

        }

        public void cargarGrid(String cadena)
        {
            string select = "select * from comunas " + cadena;
            MySqlCommand opSelect = new MySqlCommand(select, con);
            MySqlDataAdapter datos = new MySqlDataAdapter(opSelect);
            //dataset = resultset
            DataSet dataset = new DataSet();
            //datos.fill = rs.next();
            datos.Fill(dataset);
            dataGridView1.DataSource = dataset.Tables[0];
        }
        

        private void button1_Click(object sender, EventArgs e)
        {
          
            if (btnInsert.Checked)
            {
                if(textBox1.Text == "" || textBox2.Text == "")
                {
                    MessageBox.Show("No se pueden ingresar datos en blanco",
                      "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cargarGrid(" ");
                    textBox1.Focus();
                }
                else
                {
                    string insert = "insert into comunas(idComuna, nomComuna)VALUES(@param1,@param2)";
                    MySqlCommand opInsert = new MySqlCommand(insert, con);
                    opInsert.Parameters.AddWithValue("@param1", Convert.ToInt32(textBox1.Text));
                    opInsert.Parameters.AddWithValue("@param2", textBox2.Text);
                    opInsert.ExecuteNonQuery();
                    cargarGrid(" ");
                    this.Controls.OfType<TextBox>().ToList().ForEach(o => o.Text = "");
                }
               
            }
            else if (btnDelete.Checked)
            {
                if (textBox1.Text == "")
                {
                      MessageBox.Show("No se seleccionó un ID de comuna",
                      "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                       cargarGrid(" ");
                    textBox1.Focus();
                }
                else
                {
                    DialogResult resul = MessageBox.Show("¿Seguro que desea eliminar el registro seleccionado?", "Eliminar registro", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (resul == DialogResult.Yes)
                    {
                        string delete = "delete from comunas where idComuna=@param1";
                        //string delete = "DELETE c, emp FROM comunas c LEFT JOIN empresas emp ON c.idComuna = emp.idComuna  WHERE c.idComuna=@param1;";
                        MySqlCommand opDelete = new MySqlCommand(delete, con);
                        opDelete.Parameters.AddWithValue("@param1", Convert.ToInt32(textBox1.Text));
                        opDelete.ExecuteNonQuery();
                        cargarGrid("");
                    }
                    else
                    {
                        MessageBox.Show("Registro no eliminado");
                        cargarGrid(" ");
                    }
                }
                    
                }
           
            else if (btnUpdate.Checked)
            {
                if (textBox1.Text == "" || textBox2.Text == "")
                {
                    MessageBox.Show("No se seleccionó nada que editar","Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cargarGrid(" ");
                    textBox1.Focus();
                }
                else
                {
                    string update = "update comunas set nomComuna=@param1 where idComuna=@param2";
                    MySqlCommand opUpdate = new MySqlCommand(update, con);
                    opUpdate.Parameters.AddWithValue("@param1", textBox2.Text);
                    opUpdate.Parameters.AddWithValue("@param2", Convert.ToInt32(textBox1.Text));
                    opUpdate.ExecuteNonQuery();
                    cargarGrid(" ");
                    this.Controls.OfType<TextBox>().ToList().ForEach(o => o.Text = "");
                }

            }else if (btnFind.Checked)
            {
                textBox2.Focus();
                cargarGrid("where nomComuna like " + "'%" + textBox2.Text + "%'");
            }
        }
    }
}
