using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ManagerCont
{
    public partial class Form2 : Form
    {

        public string EmpresaSeleccionada { get; private set; }
        public string MesSeleccionado { get; private set; }
        public string AñoSeleccionado { get; private set; }
        public Form2()
        {
            InitializeComponent();

            comboBox1.Items.Add("Seleccione una empresa");
            comboBox1.Items.Add("CONTROL");
            comboBox1.Items.Add("EPESA");
            comboBox1.Items.Add("INGENIAL");
            comboBox1.Items.Add("SUPERVISA");
            comboBox1.Items.Add("CONSULTE");
            comboBox1.Items.Add("CORDINA");
            comboBox1.Items.Add("SACMAG");
            comboBox1.Items.Add("GEOAMBIENTE");
            comboBox1.Enabled = false; // Deshabilitar comboBox1 inicialmente

            comboBox2.Items.Add("Seleccione un mes");
            comboBox2.Items.Add("01");
            comboBox2.Items.Add("02");
            comboBox2.Items.Add("03");
            comboBox2.Items.Add("04");
            comboBox2.Items.Add("05");
            comboBox2.Items.Add("06");
            comboBox2.Items.Add("07");
            comboBox2.Items.Add("08");
            comboBox2.Items.Add("09");
            comboBox2.Items.Add("10");
            comboBox2.Items.Add("11");
            comboBox2.Items.Add("12");
            comboBox2.Enabled = false; // Deshabilitar comboBox2 inicialmente

            comboBox3.Items.Add("2024");
            comboBox3.Items.Add("2023");
            comboBox3.Items.Add("2022");
            comboBox3.Items.Add("2021");
            comboBox3.Items.Add("2020");
            comboBox3.Items.Add("2019");
            comboBox3.Items.Add("2018");
            comboBox3.Items.Add("2017");
            comboBox3.Items.Add("2016");
            comboBox3.Items.Add("2015");
            comboBox3.Items.Add("2014");
            comboBox3.Items.Add("2013");
            comboBox3.Items.Add("2012");
            comboBox3.Items.Add("2011");
            comboBox3.Items.Add("2010");
            comboBox3.Items.Add("2009");

            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            comboBox2.SelectedIndexChanged += comboBox2_SelectedIndexChanged;
            comboBox3.SelectedIndexChanged += comboBox3_SelectedIndexChanged;

        }



        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > 0)
            {
                EmpresaSeleccionada = comboBox1.SelectedItem.ToString();
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Habilitar comboBox1 solo si comboBox2 y comboBox3 están seleccionados
            if (comboBox2.SelectedIndex > 0 && comboBox3.SelectedIndex > 0)
            {
                comboBox1.Enabled = true;
            }
            else
            {
                comboBox1.Enabled = false;
            }

            if (comboBox2.SelectedIndex > 0)
            {
                EmpresaSeleccionada = comboBox2.SelectedItem.ToString();
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Habilitar comboBox2 si comboBox3 está seleccionado
            if (comboBox3.SelectedIndex > 0)
            {
                comboBox2.Enabled = true;
            }
            else
            {
                comboBox2.Enabled = false;
            }
            if (comboBox3.SelectedIndex > 0)
            {
                EmpresaSeleccionada = comboBox3.SelectedItem.ToString();
            }
            // Reiniciar comboBox1 y comboBox2 si se cambia comboBox3
            comboBox1.SelectedIndex = -1;
            comboBox1.Enabled = false;
            comboBox2.SelectedIndex = -1;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

    }

}
