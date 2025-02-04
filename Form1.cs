using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using ManagerCont;

//Cambios realizados por Marco Hernandez. 

/*
Los cambios realizados en ésta versión han sido:

- Botón en "ToolStrip" para "comparar" en ambas grids.
- Botón para imprimir en grids (sea grid1 o grid2).

 */

namespace ManagerCont
{
    public partial class Form1 : Form
    {
        private List<DataGridViewCell> searchResults = new List<DataGridViewCell>();
        private int currentSearchIndex = -1;
        private conexion _conexion;
        struct Per
        {
            public string nom;
            public string ape1;
            public string ape2;
            public string RFC;
            public string imss;
            public string fal;
            public string fab;
            public decimal ingr;
            public decimal viat;
            public decimal otras;
            public decimal integrado;
        }
        public Form1()
        {
            InitializeComponent();
            /*  _conexion = new conexion();
              ProbarConexion();*/
            comboBox1.Items.Add("Seleccione un archivo");
            comboBox1.Items.Add("CATAUX");
            comboBox1.Items.Add("CATMAY");
            comboBox1.Items.Add("DATOS");
            comboBox1.Items.Add("OPERACIONES");
            comboBox1.Enabled = false; // Deshabilitar comboBox1 inicialmente
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
            comboBox3.Items.Add("EPE");
            comboBox3.Items.Add("CON");
            comboBox3.Items.Add("ING");
            comboBox3.Items.Add("SUP");
            comboBox3.Items.Add("CONS");
            comboBox3.Items.Add("COR");
            comboBox3.Items.Add("SAC");
            comboBox3.Items.Add("GEO");
            comboBox3.SelectedIndexChanged += ComboBox3_SelectedIndexChanged; // Agregar manejador de evento
            comboBox2.SelectedIndexChanged += ComboBox2_SelectedIndexChanged; // Agregar manejador de evento
            button7.Click += button7_Click; // Agregar manejador de evento para button7

            // Inicialmente ocultar todos los componentes que quieres controlar por menú
            dataGridView1.Visible = true;
            button1.Visible = false;
            button3.Visible = false;
            button8.Visible = false;
            button7.Visible = false;
            label1.Visible = true;
            label2.Visible = false;
            button2.Visible = false;
            comboBox3.Visible = false;
            comboBox2.Visible = false;
            comboBox1.Visible = false;
            button4.Visible = true;
            button5.Visible = true;
            textBox1.Visible = true;
            dataGridView2.Visible = true;
            button6.Visible = true;
            button9.Visible = false;
            label3.Visible = false;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            dataGridView1.KeyDown += new KeyEventHandler(dataGridView1_KeyDown);

            //Elementos ocultos añadidos a la función "Comparar" de la aplicación
            labelRuta1.Visible = false;
            labelRuta2.Visible = false;
            botonLimpiar1.Visible = false;
            botonLimpiar2.Visible = false;
            labelVista1.Visible = false;
            labelVista2.Visible = false;
            ButtonPrueba.Visible = false;
            button6.Visible = false;


        }

        private void rANDOMToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // Mostrar componentes para la opción "Random"
            dataGridView1.Visible = true;
            button8.Visible = false;
            button7.Visible = false;
            button6.Visible = true;
            label1.Visible = false;
            button1.Visible = false;
            button4.Visible = true;
            button5.Visible = true;
            textBox1.Visible = true;
            dataGridView2.Visible = true;

            // Ocultar otros componentes
            button3.Visible = false;
            label2.Visible = true;
            button2.Visible = false;
            comboBox3.Visible = false;
            comboBox2.Visible = false;
            comboBox1.Visible = false;
            label3.Visible = false;
        }

        //Variables para control de banderas
        int banderaGrid = 0, banderaGrid1 = 0, banderaGrid2 = 0;

        //Declaración de bitmap para imprimir
        Bitmap bitmap;

        /*--------------------------------- Sección de Métodos/Funciones ---------------------------------*/
        private void InterpretarYMostrarOperaciones(string linea)
        {
            string filePath = labelRuta1.Text;
            int recordLength = 64; // Longitud fija de cada registro
            dataGridView2.Rows.Clear();
            dataGridView2.Columns.Clear();

            try
            {
                // Definir columnas de ejemplo (modifica según los datos en el archivo)
                dataGridView2.Columns.Add("CTA", "CTA");
                dataGridView2.Columns.Add("descr", "descr");
                dataGridView2.Columns.Add("fe", "fe");
                dataGridView2.Columns.Add("impte", "impte");
                dataGridView2.Columns.Add("indenti", "indenti");
                dataGridView2.Columns.Add("real", "real");

                Font newFont = new Font("Arial", 7, FontStyle.Bold); // Cambia "Arial" y otros parámetros según tus preferencias
                dataGridView2.DefaultCellStyle.Font = newFont;
                // Abrir el archivo para lectura
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                using (BinaryReader reader = new BinaryReader(fs, Encoding.Default))
                {
                    long fileLength = fs.Length;
                    long recordCount = fileLength / recordLength;

                    // Leer cada registro
                    for (int i = 0; i < recordCount; i++)
                    {
                        byte[] buffer = reader.ReadBytes(recordLength);
                        string record = Encoding.Default.GetString(buffer);

                        // Separar los campos según la estructura (ajustar a tu formato)                                       
                        string CTA = record.Substring(0, 6).Trim();
                        string descr = record.Substring(6, 30).Trim();
                        string fe = record.Substring(36, 2).Trim();
                        string impte = record.Substring(38, 16).Trim();
                        string indenti = record.Substring(54, 1).Trim();
                        string real = record.Substring(55, 9).Trim();

                        // Agregar fila a la DataGridView

                        dataGridView2.Rows.Add(CTA, descr, fe, impte, indenti, real);

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al leer el archivo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InterpretarYMostrarCatmay(string linea)
        {
            string filePath = labelRuta1.Text;
            int recordLength = 64; // Longitud fija de cada registro
            dataGridView2.Rows.Clear();
            dataGridView2.Columns.Clear();

            try
            {
                // Definir columnas de ejemplo (modifica según los datos en el archivo)
                dataGridView2.Columns.Add("Cuenta", "Cuenta");
                dataGridView2.Columns.Add("Nombre", "Nombre");
                dataGridView2.Columns.Add("Saldo", "Saldo");
                dataGridView2.Columns.Add("Rango_Inf", "Rango_Inf");
                dataGridView2.Columns.Add("Rango_Sup", "Rango_Sup");

                Font newFont = new Font("Arial", 7, FontStyle.Bold); // Cambia "Arial" y otros parámetros según tus preferencias
                dataGridView2.DefaultCellStyle.Font = newFont;
                // Abrir el archivo para lectura
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                using (BinaryReader reader = new BinaryReader(fs, Encoding.Default))
                {
                    long fileLength = fs.Length;
                    long recordCount = fileLength / recordLength;

                    // Leer cada registro
                    for (int i = 0; i < recordCount; i++)
                    {
                        byte[] buffer = reader.ReadBytes(recordLength);
                        string record = Encoding.Default.GetString(buffer);

                        // Separar los campos según la estructura (ajustar a tu formato)
                        string Cuenta = record.Substring(0, 6).Trim();
                        string Nombre = record.Substring(6, 32).Trim();
                        string Saldo = record.Substring(38, 16).Trim();
                        string Rango_Inf = record.Substring(54, 5).Trim();
                        string Rango_Sup = record.Substring(59, 5).Trim();

                        // Agregar fila a la DataGridView

                        dataGridView2.Rows.Add(Cuenta, Nombre, Saldo, Rango_Inf, Rango_Sup);

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al leer el archivo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void InterpretarYMostrarPersonal(string linea)
        {
            string filePath = labelRuta1.Text;
            int recordLength = 152; // Longitud fija de cada registro
            dataGridView2.Rows.Clear();
            dataGridView2.Columns.Clear();

            try
            {
                // Definir columnas de ejemplo (modifica según los datos en el archivo)
                dataGridView2.Columns.Add("Nom", "Nom");
                dataGridView2.Columns.Add("Ape1", "Ape1");
                dataGridView2.Columns.Add("Ape2", "Ape2");
                dataGridView2.Columns.Add("RFC", "RFC");
                dataGridView2.Columns.Add("Imss", "Imss");
                dataGridView2.Columns.Add("Fal", "Fal");
                dataGridView2.Columns.Add("Fab", "Fab");
                dataGridView2.Columns.Add("Ingr", "Ingr");
                dataGridView2.Columns.Add("Viat", "Viat");
                dataGridView2.Columns.Add("Otras", "Otras");
                dataGridView2.Columns.Add("Integrado", "Integrado");

                Font newFont = new Font("Arial", 7, FontStyle.Bold);
                dataGridView2.DefaultCellStyle.Font = newFont;

                // Abrir el archivo para lectura
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                using (BinaryReader reader = new BinaryReader(fs, Encoding.Default))
                {
                    long fileLength = fs.Length;
                    long recordCount = fileLength / recordLength;

                    // Leer cada registro
                    for (int i = 0; i < recordCount; i++)
                    {
                        byte[] buffer = reader.ReadBytes(recordLength);

                        // Leer los valores de tipo string
                        string Nom = Encoding.Default.GetString(buffer, 0, 20).Trim();
                        string Ape1 = Encoding.Default.GetString(buffer, 20, 20).Trim();
                        string Ape2 = Encoding.Default.GetString(buffer, 40, 20).Trim();
                        string RFC = Encoding.Default.GetString(buffer, 60, 18).Trim();
                        string Imss = Encoding.Default.GetString(buffer, 78, 18).Trim();
                        string Fal = Encoding.Default.GetString(buffer, 96, 12).Trim();
                        string Fab = Encoding.Default.GetString(buffer, 108, 12).Trim();

                        // Leer los valores de tipo Currency (8 bytes cada uno)
                        long ingrRaw = BitConverter.ToInt64(buffer, 120);
                        long viatRaw = BitConverter.ToInt64(buffer, 128);
                        long otrasRaw = BitConverter.ToInt64(buffer, 136);
                        long integradoRaw = BitConverter.ToInt64(buffer, 144);

                        // Convertir a decimal dividiendo por 10000m (manteniendo precisión de 4 decimales)
                        decimal Ingr = ingrRaw / 10000m;
                        decimal Viat = viatRaw / 10000m;
                        decimal Otras = otrasRaw / 10000m;
                        decimal Integrado = integradoRaw / 10000m;

                        // Agregar fila a la DataGridView
                        dataGridView2.Rows.Add(Nom, Ape1, Ape2, RFC, Imss, Fal, Fab, Ingr, Viat, Otras, Integrado);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al leer el archivo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void InterpretarYMostrarDatos(string linea)
        {

            string filePath = labelRuta1.Text;
            int recordLength = 236; // Longitud fija de cada registro
            dataGridView2.Rows.Clear();
            dataGridView2.Columns.Clear();

            try
            {
                // Definir columnas de ejemplo (modifica según los datos en el archivo)
                dataGridView2.Columns.Add("D1", "D1");
                dataGridView2.Columns.Add("D2", "D2");
                dataGridView2.Columns.Add("D3", "D3");
                dataGridView2.Columns.Add("No_arch", "No_arch");
                dataGridView2.Columns.Add("a_o", "a_o");
                dataGridView2.Columns.Add("Others1", "Others1");
                dataGridView2.Columns.Add("ultimaPol1", "ultimaPol1");
                dataGridView2.Columns.Add("ultimaOperacionReg", "ultimaOperacionReg");
                dataGridView2.Columns.Add("others", "others");


                Font newFont = new Font("Arial", 7, FontStyle.Bold); // Cambia "Arial" y otros parámetros según tus preferencias
                dataGridView2.DefaultCellStyle.Font = newFont;
                // Abrir el archivo para lectura
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                using (BinaryReader reader = new BinaryReader(fs, Encoding.Default))
                {
                    long fileLength = fs.Length;
                    long recordCount = fileLength / recordLength;

                    // Leer cada registro
                    for (int i = 0; i < recordCount; i++)
                    {
                        byte[] buffer = reader.ReadBytes(recordLength);
                        string record = Encoding.Default.GetString(buffer);

                        // Separar los campos según la estructura (ajustar a tu formato)
                        string D1 = record.Substring(0, 64).Trim();
                        string D2 = record.Substring(64, 60).Trim();
                        string D3 = record.Substring(124, 45).Trim();
                        string No_arch = record.Substring(169, 15).Trim();
                        string a_o = record.Substring(184, 5).Trim();
                        string Others1 = record.Substring(189, 25).Trim();
                        string ultimaPol1 = record.Substring(214, 5).Trim();
                        string ultimaOperacionReg = record.Substring(219, 6).Trim();
                        string others = record.Substring(225, 11).Trim();

                        // Agregar fila a la DataGridView

                        dataGridView2.Rows.Add(D1, D2, D3, No_arch, a_o, Others1, ultimaPol1, ultimaOperacionReg, others);

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al leer el archivo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarArchivoCSV(string archivoPath)
        {
            try
            {
                if (!File.Exists(archivoPath))
                {
                    MessageBox.Show("El archivo no existe en la ruta especificada.");
                    return;
                }

                // Leer todas las líneas del archivo CSV
                string[] lines = File.ReadAllLines(archivoPath);

                // Crear un DataTable para almacenar los datos del archivo CSV
                DataTable dt = new DataTable();

                // Si hay líneas, asumimos que la primera línea contiene los nombres de las columnas
                if (lines.Length > 0)
                {
                    // Dividir la primera línea por ';' para obtener los nombres de las columnas
                    string[] headers = lines[0].Split(';');

                    // Agregar las columnas al DataTable
                    foreach (string header in headers)
                    {
                        dt.Columns.Add(header);
                    }

                    // Leer las filas restantes y agregarlas al DataTable
                    for (int i = 1; i < lines.Length; i++)
                    {
                        string[] data = lines[i].Split(';');
                        dt.Rows.Add(data);
                    }
                }

                // Asignar el DataTable como origen de datos del DataGridView
                dataGridView2.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar el archivo: " + ex.Message);
            }
        }

        //Métodos "COPIA" de los anteriores en éste apartado, para mopstrar en Grid's sin editar (solo comparar).
        private void InterpretarYMostrarOperacionesDisabled(string linea)
        {
            string filePath = labelRuta2.Text;
            int recordLength = 64; // Longitud fija de cada registro
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            dataGridView1.ReadOnly = true;
            try
            {
                // Definir columnas de ejemplo (modifica según los datos en el archivo)
                dataGridView1.Columns.Add("CTA", "CTA");
                dataGridView1.Columns.Add("descr", "descr");
                dataGridView1.Columns.Add("fe", "fe");
                dataGridView1.Columns.Add("impte", "impte");
                dataGridView1.Columns.Add("indenti", "indenti");
                dataGridView1.Columns.Add("real", "real");

                Font newFont = new Font("Arial", 7, FontStyle.Bold); // Cambia "Arial" y otros parámetros según tus preferencias
                dataGridView1.DefaultCellStyle.Font = newFont;
                // Abrir el archivo para lectura
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                using (BinaryReader reader = new BinaryReader(fs, Encoding.Default))
                {
                    long fileLength = fs.Length;
                    long recordCount = fileLength / recordLength;

                    // Leer cada registro
                    for (int i = 0; i < recordCount; i++)
                    {
                        byte[] buffer = reader.ReadBytes(recordLength);
                        string record = Encoding.Default.GetString(buffer);

                        // Separar los campos según la estructura (ajustar a tu formato)                                       
                        string CTA = record.Substring(0, 6).Trim();
                        string descr = record.Substring(6, 30).Trim();
                        string fe = record.Substring(36, 2).Trim();
                        string impte = record.Substring(38, 16).Trim();
                        string indenti = record.Substring(54, 1).Trim();
                        string real = record.Substring(55, 9).Trim();

                        // Agregar fila a la DataGridView

                        dataGridView1.Rows.Add(CTA, descr, fe, impte, indenti, real);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al leer el archivo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InterpretarYMostrarCatmayDisabled(string linea)
        {
            string filePath = labelRuta2.Text;
            int recordLength = 64; // Longitud fija de cada registro
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            dataGridView1.ReadOnly = true;

            try
            {
                // Definir columnas de ejemplo (modifica según los datos en el archivo)
                dataGridView1.Columns.Add("Cuenta", "Cuenta");
                dataGridView1.Columns.Add("Nombre", "Nombre");
                dataGridView1.Columns.Add("Saldo", "Saldo");
                dataGridView1.Columns.Add("Rango_Inf", "Rango_Inf");
                dataGridView1.Columns.Add("Rango_Sup", "Rango_Sup");

                Font newFont = new Font("Arial", 7, FontStyle.Bold); // Cambia "Arial" y otros parámetros según tus preferencias
                dataGridView1.DefaultCellStyle.Font = newFont;
                // Abrir el archivo para lectura
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                using (BinaryReader reader = new BinaryReader(fs, Encoding.Default))
                {
                    long fileLength = fs.Length;
                    long recordCount = fileLength / recordLength;

                    // Leer cada registro
                    for (int i = 0; i < recordCount; i++)
                    {
                        byte[] buffer = reader.ReadBytes(recordLength);
                        string record = Encoding.Default.GetString(buffer);

                        // Separar los campos según la estructura (ajustar a tu formato)
                        string Cuenta = record.Substring(0, 6).Trim();
                        string Nombre = record.Substring(6, 32).Trim();
                        string Saldo = record.Substring(38, 16).Trim();
                        string Rango_Inf = record.Substring(54, 5).Trim();
                        string Rango_Sup = record.Substring(59, 5).Trim();

                        // Agregar fila a la DataGridView
                        dataGridView1.Rows.Add(Cuenta, Nombre, Saldo, Rango_Inf, Rango_Sup);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al leer el archivo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InterpretarYMostrarDatosDisabled(string linea)
        {
            string filePath = labelRuta2.Text;
            int recordLength = 236; // Longitud fija de cada registro
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            dataGridView1.ReadOnly = true;

            try
            {
                // Definir columnas de ejemplo (modifica según los datos en el archivo)
                dataGridView1.Columns.Add("D1", "D1");
                dataGridView1.Columns.Add("D2", "D2");
                dataGridView1.Columns.Add("D3", "D3");
                dataGridView1.Columns.Add("No_arch", "No_arch");
                dataGridView1.Columns.Add("a_o", "a_o");
                dataGridView1.Columns.Add("Others1", "Others1");
                dataGridView1.Columns.Add("ultimaPol1", "ultimaPol1");
                dataGridView1.Columns.Add("ultimaOperacionReg", "ultimaOperacionReg");
                dataGridView1.Columns.Add("others", "others");

                Font newFont = new Font("Arial", 7, FontStyle.Bold); // Cambia "Arial" y otros parámetros según tus preferencias
                dataGridView1.DefaultCellStyle.Font = newFont;
                // Abrir el archivo para lectura
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                using (BinaryReader reader = new BinaryReader(fs, Encoding.Default))
                {
                    long fileLength = fs.Length;
                    long recordCount = fileLength / recordLength;

                    // Leer cada registro
                    for (int i = 0; i < recordCount; i++)
                    {
                        byte[] buffer = reader.ReadBytes(recordLength);
                        string record = Encoding.Default.GetString(buffer);

                        // Separar los campos según la estructura (ajustar a tu formato)
                        string D1 = record.Substring(0, 64).Trim();
                        string D2 = record.Substring(64, 60).Trim();
                        string D3 = record.Substring(124, 45).Trim();
                        string No_arch = record.Substring(169, 15).Trim();
                        string a_o = record.Substring(184, 5).Trim();
                        string Others1 = record.Substring(189, 25).Trim();
                        string ultimaPol1 = record.Substring(214, 5).Trim();
                        string ultimaOperacionReg = record.Substring(219, 6).Trim();
                        string others = record.Substring(225, 11).Trim();

                        // Agregar fila a la DataGridView
                        dataGridView1.Rows.Add(D1, D2, D3, No_arch, a_o, Others1, ultimaPol1, ultimaOperacionReg, others);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al leer el archivo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Funciónes para guardar los datos con la estructura especificada
        private void GuardarOpers(int CTA_Length, int descr_Length, int fe_Length, int impte_Length, int indenti_Length, int real_Length)
        {
            try
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "Archivo Binario|*"; // Filtro para archivos binarios
                saveFileDialog1.Title = "Guardar datos";
                saveFileDialog1.FileName = "OPER"; // Nombre base del archivo

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog1.FileName;

                    // Crear un archivo binario para guardar los datos
                    using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    using (BinaryWriter writer = new BinaryWriter(fs, Encoding.Default))
                    {
                        // Iterar sobre las filas del DataGridView
                        foreach (DataGridViewRow row in dataGridView2.Rows)
                        {
                            // Verificar si la fila no está vacía
                            if (!row.IsNewRow)
                            {
                                // Obtener y formatear los valores de las celdas
                                string CTA = Convert.ToString(row.Cells["CTA"].Value).PadRight(CTA_Length).Substring(0, CTA_Length);
                                string descr = Convert.ToString(row.Cells["descr"].Value).PadRight(descr_Length).Substring(0, descr_Length);
                                string fe = Convert.ToString(row.Cells["fe"].Value).PadRight(fe_Length).Substring(0, fe_Length);
                                string impte = Convert.ToString(row.Cells["impte"].Value).PadRight(impte_Length).Substring(0, impte_Length);
                                string indenti = Convert.ToString(row.Cells["indenti"].Value).PadRight(indenti_Length).Substring(0, indenti_Length);
                                string real = Convert.ToString(row.Cells["real"].Value).PadRight(real_Length).Substring(0, real_Length);

                                // Combinar los campos en una línea binaria
                                string record = $"{CTA}{descr}{fe}{impte}{indenti}{real}";

                                // Escribir los bytes en el archivo binario
                                byte[] buffer = Encoding.Default.GetBytes(record);
                                writer.Write(buffer);
                            }
                        }
                    }

                    MessageBox.Show("Datos guardados correctamente en: " + filePath, "Guardar datos",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar los datos: " + ex.Message, "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GuardarPer()
        {
            try
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "Archivo Binario|*"; // Filtro para archivos binarios
                saveFileDialog1.Title = "Guardar datos PERSONAL en archivo binario";
                saveFileDialog1.FileName = "PERSONAL"; // Nombre base del archivo

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog1.FileName;

                    using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    using (BinaryWriter writer = new BinaryWriter(fs, Encoding.Default))
                    {
                        foreach (DataGridViewRow row in dataGridView2.Rows)
                        {
                            if (!row.IsNewRow)
                            {
                                // Leer y formatear las cadenas (paddings y longitudes específicas)
                                string Nom = Convert.ToString(row.Cells["Nom"].Value).PadRight(20).Substring(0, 20);
                                string Ape1 = Convert.ToString(row.Cells["Ape1"].Value).PadRight(20).Substring(0, 20);
                                string Ape2 = Convert.ToString(row.Cells["Ape2"].Value).PadRight(20).Substring(0, 20);
                                string RFC = Convert.ToString(row.Cells["RFC"].Value).PadRight(18).Substring(0, 18);
                                string Imss = Convert.ToString(row.Cells["Imss"].Value).PadRight(18).Substring(0, 18);
                                string Fal = Convert.ToString(row.Cells["Fal"].Value).PadRight(12).Substring(0, 12);
                                string Fab = Convert.ToString(row.Cells["Fab"].Value).PadRight(12).Substring(0, 12);

                                // Leer y convertir los valores de tipo long
                                long Ingr = Convert.ToInt64(row.Cells["Ingr"].Value);
                                long Viat = Convert.ToInt64(row.Cells["Viat"].Value);
                                long Otras = Convert.ToInt64(row.Cells["Otras"].Value);
                                long Integrado = Convert.ToInt64(row.Cells["Integrado"].Value);

                                // Escribir cadenas en el archivo binario
                                writer.Write(Encoding.Default.GetBytes(Nom));
                                writer.Write(Encoding.Default.GetBytes(Ape1));
                                writer.Write(Encoding.Default.GetBytes(Ape2));
                                writer.Write(Encoding.Default.GetBytes(RFC));
                                writer.Write(Encoding.Default.GetBytes(Imss));
                                writer.Write(Encoding.Default.GetBytes(Fal));
                                writer.Write(Encoding.Default.GetBytes(Fab));

                                // Escribir valores numéricos en el archivo binario
                                writer.Write(Ingr);
                                writer.Write(Viat);
                                writer.Write(Otras);
                                writer.Write(Integrado);
                            }
                        }
                    }

                    MessageBox.Show("Datos PERSONAL guardados correctamente en: " + filePath, "Guardar datos",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar los datos PERSONAL: " + ex.Message, "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void GuardarCats(int Cuenta_Length, int Nombre_Length, int Saldo_Length, int Rango_Inf_Length, int Rango_Sup_Length)
        {
            try
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "Archivo Binario|*"; // Filtro para archivos binarios
                saveFileDialog1.Title = "Guardar datos CATMAY en archivo binario";
                saveFileDialog1.FileName = "CATMAY"; // Nombre base del archivo

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog1.FileName;

                    // Crear un archivo binario para guardar los datos
                    using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    using (BinaryWriter writer = new BinaryWriter(fs, Encoding.Default))
                    {
                        // Iterar sobre las filas del DataGridView
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            // Verificar si la fila no está vacía
                            if (!row.IsNewRow)
                            {
                                // Obtener y formatear los valores de las celdas según los parámetros proporcionados
                                string Cuenta = Convert.ToString(row.Cells["Cuenta"].Value).PadRight(Cuenta_Length).Substring(0, Cuenta_Length);
                                string Nombre = Convert.ToString(row.Cells["Nombre"].Value).PadRight(Nombre_Length).Substring(0, Nombre_Length);

                                // Obtener el valor de Saldo y quitar puntos si es necesario
                                string SaldoValue = Convert.ToString(row.Cells["Saldo"].Value);
                                SaldoValue = SaldoValue.Replace(".", ""); // Quitar puntos
                                string Saldo = SaldoValue.PadRight(Saldo_Length).Substring(0, Saldo_Length);

                                string Rango_Inf = Convert.ToString(row.Cells["Rango_Inf"].Value).PadRight(Rango_Inf_Length).Substring(0, Rango_Inf_Length);
                                string Rango_Sup = Convert.ToString(row.Cells["Rango_Sup"].Value).PadRight(Rango_Sup_Length).Substring(0, Rango_Sup_Length);

                                // Combinar los campos en un registro binario
                                string record = $"{Cuenta}{Nombre}{Saldo}{Rango_Inf}{Rango_Sup}";

                                // Escribir los bytes en el archivo binario
                                byte[] buffer = Encoding.Default.GetBytes(record);
                                writer.Write(buffer);
                            }
                        }
                    }

                    MessageBox.Show("Datos CATMAY guardados correctamente en: " + filePath, "Guardar datos",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar los datos CATMAY: " + ex.Message, "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void GuardarDatos(int D1_Length, int D2_Length, int D3_Length, int No_arch_Length, int a_o_Length, int Others1_Length, int ultimaPol1_Length, int ultimoReg_Length, int others_Length)
        {
            try
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "Archivo Binario|*"; // Filtro para archivos binarios
                saveFileDialog1.Title = "Guardar datos en archivo binario";
                saveFileDialog1.FileName = "DATOS"; // Nombre base del archivo

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog1.FileName;

                    // Crear un archivo binario para guardar los datos
                    using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    using (BinaryWriter writer = new BinaryWriter(fs, Encoding.Default))
                    {
                        // Construir el contenido del archivo a partir de los datos del DataGridView
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            // Verificar si la fila no está vacía
                            if (!row.IsNewRow)
                            {
                                // Obtener y formatear los valores de las celdas según los parámetros proporcionados
                                string D1 = Convert.ToString(row.Cells["D1"].Value).PadRight(D1_Length).Substring(0, D1_Length);
                                string D2 = Convert.ToString(row.Cells["D2"].Value).PadRight(D2_Length).Substring(0, D2_Length);
                                string D3 = Convert.ToString(row.Cells["D3"].Value).PadRight(D3_Length).Substring(0, D3_Length);
                                string No_arch = Convert.ToString(row.Cells["No_arch"].Value).PadRight(No_arch_Length).Substring(0, No_arch_Length);
                                string a_o = Convert.ToString(row.Cells["a_o"].Value).PadRight(a_o_Length).Substring(0, a_o_Length);
                                string Others1 = Convert.ToString(row.Cells["others1"].Value).PadRight(Others1_Length).Substring(0, Others1_Length);
                                string ultimaPol1 = Convert.ToString(row.Cells["ultimaPol1"].Value).PadRight(ultimaPol1_Length).Substring(0, ultimaPol1_Length);
                                string ultimoReg = Convert.ToString(row.Cells["ultimaOperacionReg"].Value).PadRight(ultimoReg_Length).Substring(0, ultimoReg_Length);
                                string others = Convert.ToString(row.Cells["others"].Value).PadRight(others_Length).Substring(0, others_Length);

                                // Combinar los campos en un registro binario
                                string record = $"{D1}{D2}{D3}{No_arch}{a_o}{Others1}{ultimaPol1}{ultimoReg}{others}";

                                // Escribir los bytes en el archivo binario
                                byte[] buffer = Encoding.Default.GetBytes(record);
                                writer.Write(buffer);
                            }
                        }
                    }

                    MessageBox.Show("Datos guardados correctamente en: " + filePath, "Guardar datos",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar los datos: " + ex.Message, "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /*------------------------------------------------------------------------------------------------*/

        /*------------------------------------ Eventos para ToolStrip ------------------------------------*/
        private void gUARDARRANDOMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string label1Content = label1.Text.ToUpper(); // Obtener el contenido del label 1 y convertirlo a mayúsculas

                if (label1Content.Contains("DATOS"))
                {
                    // Llamar a la función GuardarDatos con las longitudes específicas
                    GuardarDatos(64, 60, 45, 15, 5, 25, 5, 6, 11);
                }
                else if (label1Content.Contains("CATAUX") || label1Content.Contains("CATMAY"))
                {
                    // Llamar a la función GuardarCats con las longitudes específicas
                    GuardarCats(6, 32, 16, 5, 5);
                    // Formatear y alinear la columna en el índice 2
                }
                else if (label1Content.StartsWith("SAC") || label1Content.StartsWith("COR") || label1Content.StartsWith("SUP"))
                {
                    // Determinar qué función llamar basado en el nombre del archivo sin extensión
                    if (label1Content.StartsWith("SAC"))
                    {
                        GuardarOpers(6, 30, 2, 16, 1, 9);
                    }
                    else if (label1Content.StartsWith("COR"))
                    {
                        GuardarOpers(6, 30, 2, 16, 1, 9);
                    }
                    else if (label1Content.StartsWith("SUP"))
                    {
                        GuardarOpers(6, 30, 2, 16, 1, 9);
                    }
                    else if (label1Content.StartsWith("CON"))
                    {
                        GuardarOpers(6, 30, 2, 16, 1, 9);
                    }
                    else if (label1Content.StartsWith("EPE"))
                    {
                        GuardarOpers(6, 30, 2, 16, 1, 9);
                    }
                    else if (label1Content.StartsWith("GEO"))
                    {
                    }
                    else if (label1Content.StartsWith("ING"))
                    {
                        // Llamar a la función GuardarOpers con las longitudes específicas para SUP
                        GuardarOpers(6, 30, 2, 16, 1, 9);
                    }
                    else
                    {
                        MessageBox.Show("Nombre de archivo no reconocido para guardar datos.", "Error",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Nombre de archivo no reconocido para guardar datos.", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar los datos: " + ex.Message, "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gUARDARCSVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
            saveFileDialog.Title = "Save as CSV file";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                StringBuilder csvContent = new StringBuilder();

                // Adding the column headers
                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    csvContent.Append(dataGridView1.Columns[i].HeaderText);
                    if (i < dataGridView1.Columns.Count - 1)
                        csvContent.Append(";");
                }
                csvContent.AppendLine();

                // Adding the rows
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    for (int i = 0; i < dataGridView1.Columns.Count; i++)
                    {
                        csvContent.Append(row.Cells[i].Value?.ToString());
                        if (i < dataGridView1.Columns.Count - 1)
                            csvContent.Append(";");
                    }
                    csvContent.AppendLine();
                }

                // Writing to the file
                File.WriteAllText(saveFileDialog.FileName, csvContent.ToString());
                MessageBox.Show("CSV file saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void borrarSaldosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Verifica si la columna "Saldo" existe en el DataGridView
            if (!dataGridView1.Columns.Contains("Saldo"))
            {
                MessageBox.Show("La columna 'Saldo' no existe en el DataGridView.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Sale del método si la columna no existe
            }

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                try
                {
                    // Intenta establecer el valor en 0 en la celda correspondiente
                    row.Cells["Saldo"].Value = "0";
                }
                catch
                {
                    // Si ocurre una excepción en una fila específica, muestra un MessageBox
                    MessageBox.Show("Error al intentar actualizar una fila. Asegúrate de que todas las celdas 'Saldo' son válidas.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; // Sale del método si ocurre un error
                }
            }
        }

        private void modificarDatosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Crear una instancia de Form3
            Form3 form3 = new Form3();

            // Mostrar el Formulario 3
            form3.Show(); // Usa form3.ShowDialog(); si quieres abrirlo como un formulario modal
        }

        private void subirArchivoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (banderaGrid == 0 && banderaGrid1 == 0 && banderaGrid2 == 0)
            {
                banderaGrid = 1;
                try
                {
                    OpenFileDialog openFileDialog1 = new OpenFileDialog();
                    openFileDialog1.Filter = "Archivos de Texto|*"; // Filtro para archivos de texto

                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        //Habilitar boton para modificar.
                        button6.Visible = true;

                        string filePath = openFileDialog1.FileName;
                        labelRuta1.Text = filePath;

                        // Extraer el nombre del archivo sin la extensión
                        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);

                        // Asignar el nombre al texto del label (label1)
                        label1.Text = fileNameWithoutExtension;

                        // Leer la línea completa del archivo usando StreamReader
                        string lineaCompleta = ReadFileWithStreamReader(filePath);

                        // Aquí debe de llevar una instrucción dependiendo el label
                        if (fileNameWithoutExtension.StartsWith("CATMAY", StringComparison.CurrentCultureIgnoreCase))
                        {
                            InterpretarYMostrarCatmay(filePath);
                        }
                        else if (fileNameWithoutExtension.StartsWith("CATAUX", StringComparison.CurrentCultureIgnoreCase))
                        {
                            InterpretarYMostrarCatmay(filePath);
                        }
                        else if (fileNameWithoutExtension.StartsWith("PERSONAL", StringComparison.CurrentCultureIgnoreCase))
                        {
                            InterpretarYMostrarPersonal(filePath);
                        }
                        else if (fileNameWithoutExtension.Equals("DATOS", StringComparison.CurrentCultureIgnoreCase))
                        {
                            InterpretarYMostrarDatos(filePath);
                        }
                        else if (fileNameWithoutExtension.StartsWith("SAC", StringComparison.CurrentCultureIgnoreCase) ||
                                 fileNameWithoutExtension.StartsWith("COR", StringComparison.CurrentCultureIgnoreCase) ||
                                 fileNameWithoutExtension.StartsWith("EPE", StringComparison.CurrentCultureIgnoreCase) ||
                                 fileNameWithoutExtension.StartsWith("ING", StringComparison.CurrentCultureIgnoreCase) ||
                                 fileNameWithoutExtension.StartsWith("CON", StringComparison.CurrentCultureIgnoreCase) ||
                                 fileNameWithoutExtension.StartsWith("GEO", StringComparison.CurrentCultureIgnoreCase) ||
                                 fileNameWithoutExtension.StartsWith("COS", StringComparison.CurrentCultureIgnoreCase) ||
                                 fileNameWithoutExtension.StartsWith("SUP", StringComparison.CurrentCultureIgnoreCase))
                        {
                            InterpretarYMostrarOperaciones(filePath);
                        }
                        else
                        {
                            MessageBox.Show("Archivo no reconocido.");
                        }
                        button1_Click(sender, e);

                    }
                    else
                    {
                        banderaGrid = 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al abrir el archivo: " + ex.Message);
                }
            }
            else
            {
                DialogResult result = MessageBox.Show("Hay un archivo cargado. Limpia las ventanas con la opción 'Borrar'.",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void verControlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Crear una instancia de Form3
            Form4 form4 = new Form4();

            // Mostrar el Formulario 3
            form4.Show(); // Usa form3.ShowDialog(); si quieres abrirlo como un formulario modal
        }

        private void todoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // Mostrar un cuadro de diálogo de confirmación
                DialogResult result = MessageBox.Show(
                    "¿Estás seguro de que deseas borrar todos los datos?",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                // Si el usuario selecciona "Sí", proceder a borrar los datos
                if (result == DialogResult.Yes)
                {
                    dataGridView1.Rows.Clear();
                    // Si también quieres borrar las columnas, descomenta la siguiente línea:
                    dataGridView1.Columns.Clear();
                    dataGridView2.Rows.Clear();
                    // Si también quieres borrar las columnas, descomenta la siguiente línea:
                    dataGridView2.Columns.Clear();

                    //Devuelve los valores de las banderas para validaciones
                    banderaGrid = 0;
                    banderaGrid1 = 0;
                    banderaGrid2 = 0;

                    //"Esconde" los labales y botones para limpiar.
                    labelRuta1.Visible = false;
                    botonLimpiar1.Visible = false;
                    labelRuta2.Visible = false;
                    botonLimpiar2.Visible = false;
                    labelVista1.Visible = false;
                    labelVista2.Visible = false;
                    button6.Visible = false;
                }
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción que ocurra
                MessageBox.Show("Ocurrió un error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void originalToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (banderaGrid1 == 0 && banderaGrid == 0)
            {
                banderaGrid1 = 1;
                try
                {
                    OpenFileDialog gridCopia = new OpenFileDialog();
                    gridCopia.Filter = "Archivos de Texto|*";
                    if (gridCopia.ShowDialog() == DialogResult.OK)
                    {
                        //Bloquear edicion en el Grid
                        dataGridView2.ReadOnly = true;

                        //Inicia declaración de string para obtener ruta del archivo seleccionado
                        string filePath = gridCopia.FileName;

                        // Asignar el nombre al texto del label (labelRuta2 al ser "COPIA")                       
                        labelRuta1.Text = filePath;
                        labelRuta1.Visible = true;

                        //Visibilizar label de señalización
                        labelVista1.Visible = true;

                        //Visibiliza el botón limpiar gridOriginal
                        botonLimpiar1.Visible = true;

                        //Colocar datos en el GridView 1
                        // Extraer el nombre del archivo sin la extensión
                        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);

                        // Leer la línea completa del archivo usando StreamReader
                        string lineaCompleta = ReadFileWithStreamReader(filePath);

                        if (fileNameWithoutExtension.StartsWith("CATMAY", StringComparison.CurrentCultureIgnoreCase))
                        {
                            InterpretarYMostrarCatmay(lineaCompleta);
                        }
                        else if (fileNameWithoutExtension.StartsWith("CATAUX", StringComparison.CurrentCultureIgnoreCase))
                        {
                            InterpretarYMostrarCatmay(lineaCompleta);
                        }
                        else if (fileNameWithoutExtension.Equals("DATOS", StringComparison.CurrentCultureIgnoreCase))
                        {
                            InterpretarYMostrarDatos(lineaCompleta);
                        }
                        else if (fileNameWithoutExtension.StartsWith("SAC", StringComparison.CurrentCultureIgnoreCase) ||
                                 fileNameWithoutExtension.StartsWith("COR", StringComparison.CurrentCultureIgnoreCase) ||
                                 fileNameWithoutExtension.StartsWith("EPE", StringComparison.CurrentCultureIgnoreCase) ||
                                 fileNameWithoutExtension.StartsWith("ING", StringComparison.CurrentCultureIgnoreCase) ||
                                 fileNameWithoutExtension.StartsWith("CON", StringComparison.CurrentCultureIgnoreCase) ||
                                 fileNameWithoutExtension.StartsWith("GEO", StringComparison.CurrentCultureIgnoreCase) ||
                                 fileNameWithoutExtension.StartsWith("COS", StringComparison.CurrentCultureIgnoreCase) ||
                                 fileNameWithoutExtension.StartsWith("SUP", StringComparison.CurrentCultureIgnoreCase))
                        {
                            InterpretarYMostrarOperaciones(lineaCompleta);
                        }
                    }
                    else
                    {
                        banderaGrid1 = 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ha ocurrido un problema: " + ex);
                }
            }
            else
            {
                // Mostrar aviso de Archivo en vista.
                DialogResult result = MessageBox.Show("Hay un archivo cargado. Primero debes limpiar la vista.",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void copiaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (banderaGrid2 == 0 && banderaGrid == 0)
            {
                banderaGrid2 = 1;
                try
                {
                    OpenFileDialog gridCopia = new OpenFileDialog();
                    gridCopia.Filter = "Archivos de Texto|*";
                    if (gridCopia.ShowDialog() == DialogResult.OK)
                    {
                        //Bloquear edicion en el Grid
                        dataGridView1.ReadOnly = true;

                        //Inicia declaración de string para obtener ruta del archivo seleccionado
                        string filePath = gridCopia.FileName;

                        // Asignar el nombre al texto del label (labelRuta2 al ser "COPIA")                       
                        labelRuta2.Text = filePath;
                        labelRuta2.Visible = true;

                        //Visibilizar label de señalización
                        labelVista2.Visible = true;

                        //Visibiliza el botón limpiar gridOriginal
                        botonLimpiar2.Visible = true;

                        //Colocar datos en el GridView 1
                        // Extraer el nombre del archivo sin la extensión
                        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);

                        // Leer la línea completa del archivo usando StreamReader
                        string lineaCompleta = ReadFileWithStreamReader(filePath);

                        if (fileNameWithoutExtension.StartsWith("CATMAY", StringComparison.CurrentCultureIgnoreCase))
                        {
                            InterpretarYMostrarCatmayDisabled(lineaCompleta);
                        }
                        else if (fileNameWithoutExtension.StartsWith("CATAUX", StringComparison.CurrentCultureIgnoreCase))
                        {
                            InterpretarYMostrarCatmayDisabled(lineaCompleta);
                        }
                        else if (fileNameWithoutExtension.Equals("DATOS", StringComparison.CurrentCultureIgnoreCase))
                        {
                            InterpretarYMostrarDatosDisabled(lineaCompleta);
                        }
                        else if (fileNameWithoutExtension.StartsWith("SAC", StringComparison.CurrentCultureIgnoreCase) ||
                                 fileNameWithoutExtension.StartsWith("COR", StringComparison.CurrentCultureIgnoreCase) ||
                                 fileNameWithoutExtension.StartsWith("EPE", StringComparison.CurrentCultureIgnoreCase) ||
                                 fileNameWithoutExtension.StartsWith("ING", StringComparison.CurrentCultureIgnoreCase) ||
                                 fileNameWithoutExtension.StartsWith("CON", StringComparison.CurrentCultureIgnoreCase) ||
                                 fileNameWithoutExtension.StartsWith("GEO", StringComparison.CurrentCultureIgnoreCase) ||
                                 fileNameWithoutExtension.StartsWith("COS", StringComparison.CurrentCultureIgnoreCase) ||
                                 fileNameWithoutExtension.StartsWith("SUP", StringComparison.CurrentCultureIgnoreCase))
                        {
                            InterpretarYMostrarOperacionesDisabled(lineaCompleta);
                        }
                    }
                    else
                    {
                        banderaGrid2 = 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ha ocurrido un problema: " + ex);
                }
            }
            else
            {
                // Mostrar aviso de Archivo en vista.
                DialogResult result = MessageBox.Show("Hay un archivo cargado. Primero debes limpiar la vista.",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void imprimirGrid1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Valida si se encuentra algún archivo abierto en la Grid indicada.
            if (dataGridView2.RowCount > 0)
            {

                /*Guarda la altura que se muestra en el Grid. La ajusta temporalmente para acomodar las filas,
                multiplicando e número de filas por la alutra de la plantilla de fila, 
                asegurandose que hay suficiente espacio para todas las filas.*/
                int height = dataGridView2.Height;
                dataGridView2.Height = dataGridView2.RowCount * dataGridView2.RowTemplate.Height * 2;

                /* Se crea un objeto Bitmap con el ancho y la altura ajustada del DataGridView, usando DrawToBitmap
                 para renderizar el contenido del DataGridView en el Bitmap*/
                bitmap = new Bitmap(dataGridView2.Width, dataGridView2.Height);
                dataGridView2.DrawToBitmap(bitmap, new Rectangle(0, 0, dataGridView2.Width, dataGridView2.Height));

                //Restablece la altura original del DataGridView
                dataGridView2.Height = height;

                //Vista previa de como se ve el documento ajustado a la página.
                printPreviewDialog1.ShowDialog();
            }
            else
            {
                MessageBox.Show("No hay ningún elemento en el Grid que desea imprimir.", "Ningún documento seleccionado");
            }
        }

        private void imprimirGrid2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount > 0)
            {
                int height = dataGridView2.Height;
                dataGridView1.Height = dataGridView1.RowCount * dataGridView1.RowTemplate.Height * 2;
                bitmap = new Bitmap(dataGridView1.Width, dataGridView1.Height);
                dataGridView1.DrawToBitmap(bitmap, new Rectangle(0, 0, dataGridView1.Width, dataGridView1.Height));
                dataGridView1.Height = height;
                printPreviewDialog1.ShowDialog();
            }
            else
            {
                MessageBox.Show("No hay ningún elemento en el Grid que desea imprimir.", "Ningún documento seleccionado");
            }
        }

        private void clasificarCostosToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        /*------------------------------------------------------------------------------------------------*/

        /*-------------------------------------- ComboBox ------------------------------------------------*/

        private void ComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Habilitar comboBox2 si se selecciona un elemento en comboBox3, de lo contrario, deshabilitarlo
            comboBox2.Enabled = comboBox3.SelectedIndex != -1;
            // Llamar a método para actualizar el estado de comboBox1
            ActualizarEstadoComboBox1();
        }

        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Llamar a método para actualizar el estado de comboBox1
            ActualizarEstadoComboBox1();
        }

        private void ActualizarEstadoComboBox1()
        {
            // Habilitar comboBox1 si ambos comboBox2 y comboBox3 tienen una selección, de lo contrario, deshabilitarlo
            comboBox1.Enabled = comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                string archivosPath = label3.Text; // Ruta literal al directorio donde están los archivos
                string selectedFilePath = string.Empty;

                switch (comboBox1.SelectedItem.ToString())
                {
                    case "Seleccione un archivo":
                        label1.Text = string.Empty; // Limpiar el texto del label si no se selecciona un archivo válido
                        break;
                    case "CATAUX":

                        selectedFilePath = Path.Combine(archivosPath, "CATAUX.csv");
                        CargarArchivoCSV(selectedFilePath);
                        label1.Text = "CATAUX"; // Asignar la ruta completa al label
                        break;
                    case "CATMAY":
                        selectedFilePath = Path.Combine(archivosPath, "CATMAY.csv");
                        CargarArchivoCSV(selectedFilePath);
                        label1.Text = "CATMAY"; // Asignar la ruta completa al label
                        break;
                    case "DATOS":
                        selectedFilePath = Path.Combine(archivosPath, "DATOS.csv");
                        CargarArchivoCSV(selectedFilePath);
                        label1.Text = "DATOS"; // Asignar la ruta completa al label
                        break;
                    case "OPERACIONES":
                        if (comboBox2.SelectedIndex == -1 || comboBox2.SelectedItem.ToString() == "Valor Predeterminado")
                        {
                            MessageBox.Show("Debes elegir un mes primero para esta operación");
                            comboBox1.SelectedIndex = 0;
                        }
                        else
                        {
                            selectedFilePath = Path.Combine(archivosPath, comboBox3.SelectedItem.ToString() + comboBox2.SelectedItem.ToString() + ".csv");
                            CargarArchivoCSV(selectedFilePath);
                            label1.Text = comboBox3.SelectedItem.ToString() + comboBox2.SelectedItem.ToString(); // Asignar la ruta completa al label
                        }
                        break;
                    default:
                        MessageBox.Show("Selección no válida");
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Se ha producido un error: " + ex.Message);
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        /*------------------------------------------------------------------------------------------------*/

        /*------------------------------------------- Botones  -------------------------------------------*/
        
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string archivosPath = @"C:\Users\david.albino\Desktop\CSVs\"; // Ruta literal al directorio donde están los archivos
                string archivoPath = Path.Combine(archivosPath, comboBox1.SelectedItem.ToString() + ".csv"); // Obtener ruta completa del archivo

                // Obtener los datos actuales del DataGridView
                DataTable dt = (DataTable)dataGridView1.DataSource;

                // Verificar que el DataGridView tenga datos y que el archivo exista
                if (dt != null && File.Exists(archivoPath))
                {
                    // Crear una lista de líneas para escribir en el archivo CSV
                    List<string> lines = new List<string>();

                    // Agregar encabezados como primera línea
                    string headers = string.Join(";", dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName));
                    lines.Add(headers);

                    // Agregar cada fila del DataTable como una línea en el archivo CSV
                    foreach (DataRow row in dt.Rows)
                    {
                        string line = string.Join(";", row.ItemArray);
                        lines.Add(line);
                    }

                    // Escribir todas las líneas al archivo CSV
                    File.WriteAllLines(archivoPath, lines);

                    MessageBox.Show("Archivo actualizado correctamente.");
                }
                else
                {
                    MessageBox.Show("No hay datos para guardar o el archivo no existe.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar el archivo: " + ex.Message);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Filter = "Archivos de Texto|*"; // Filtro para archivos de texto

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog1.FileName;

                    // Extraer el nombre del archivo sin la extensión
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);

                    // Asignar el nombre al texto del label (label1)
                    label1.Text = fileNameWithoutExtension;

                    // Leer la línea completa del archivo
                    string lineaCompleta = File.ReadAllText(filePath);

                    // Aquí debe de llevar una instrucción dependiendo el label
                    if (fileNameWithoutExtension.StartsWith("CATMAY", StringComparison.OrdinalIgnoreCase))
                    {
                        InterpretarYMostrarCatmay(lineaCompleta);
                    }
                    else if (fileNameWithoutExtension.StartsWith("CATAUX", StringComparison.OrdinalIgnoreCase))
                    {
                        InterpretarYMostrarCatmay(lineaCompleta);
                    }
                    else if (fileNameWithoutExtension.Equals("DATOS", StringComparison.OrdinalIgnoreCase))
                    {
                        InterpretarYMostrarDatos(lineaCompleta);
                    }
                    else if (fileNameWithoutExtension.Equals("PERSONAL", StringComparison.OrdinalIgnoreCase))
                    {
                        InterpretarYMostrarPersonal(lineaCompleta);
                    }
                    else if (fileNameWithoutExtension.StartsWith("SAC", StringComparison.OrdinalIgnoreCase) ||

                             fileNameWithoutExtension.StartsWith("COR", StringComparison.OrdinalIgnoreCase) ||
                             fileNameWithoutExtension.StartsWith("EPE", StringComparison.OrdinalIgnoreCase) ||
                             fileNameWithoutExtension.StartsWith("ING", StringComparison.OrdinalIgnoreCase) ||
                             fileNameWithoutExtension.StartsWith("CON", StringComparison.OrdinalIgnoreCase) ||
                             fileNameWithoutExtension.StartsWith("GEO", StringComparison.OrdinalIgnoreCase) ||
                             fileNameWithoutExtension.StartsWith("SUP", StringComparison.OrdinalIgnoreCase))
                    {
                        InterpretarYMostrarOperaciones(lineaCompleta);
                    }
                    else
                    {
                        MessageBox.Show("Archivo no reconocido.");
                    }
                    button1_Click(sender, e);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir el archivo: " + ex.Message);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {

                string label1Content = label1.Text.ToUpper(); // Obtener el contenido del label 1 y convertirlo a mayúsculas

                if (label1Content.Contains("DATOS"))
                {
                    // Llamar a la función GuardarDatos con las longitudes específicas
                    GuardarDatos(64, 60, 45, 15, 5, 25, 5, 6, 11);
                }
                else if (label1Content.Contains("CATAUX") || label1Content.Contains("CATMAY"))
                {
                    // Llamar a la función GuardarCats con las longitudes específicas
                    GuardarCats(6, 32, 16, 5, 5);
                    // Formatear y alinear la columna en el índice 2
                }
                else if (label1Content.Contains("PERSONAL") || label1Content.Contains("PERSONAL"))
                {
                    // Llamar a la función GuardarCats con las longitudes específicas
                    GuardarPer();
                    // Formatear y alinear la columna en el índice 2
                }
                else if (label1Content.StartsWith("SAC") || label1Content.StartsWith("COR") || label1Content.StartsWith("SUP"))
                {
                    // Determinar qué función llamar basado en el nombre del archivo sin extensión
                    if (label1Content.StartsWith("SAC"))
                    {
                        // Llamar a la función GuardarOpers con las longitudes específicas para SAC
                        GuardarOpers(6, 30, 2, 16, 1, 9);
                    }
                    else if (label1Content.StartsWith("COR"))
                    {
                        // Llamar a la función GuardarOpers con las longitudes específicas para COR
                        GuardarOpers(6, 30, 2, 16, 1, 9);
                    }
                    else if (label1Content.StartsWith("SUP"))
                    {
                        // Llamar a la función GuardarOpers con las longitudes específicas para SUP
                        GuardarOpers(6, 30, 2, 16, 1, 9);
                    }
                    else
                    {
                        MessageBox.Show("Nombre de archivo no reconocido para guardar datos.", "Error",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Nombre de archivo no reconocido para guardar datos.", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar los datos: " + ex.Message, "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Obtener el valor del label1
                string label1Value = label1.Text;

                // Determinar la columna a utilizar según el valor de label1
                int columnIndex;
                if (label1Value == "CATAUX" || label1Value == "CATMAY")
                {
                    // Usar la columna "Saldo"
                    if (!dataGridView2.Columns.Contains("Saldo"))
                    {
                        MessageBox.Show("La columna 'Saldo' no existe en el DataGridView.", "Error",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    columnIndex = dataGridView2.Columns["Saldo"].Index;
                }
                else if (label1Value.StartsWith("SAC") || label1Value.StartsWith("COR") || label1Value.StartsWith("EPE"))
                {
                    // Usar la columna "impte"
                    if (!dataGridView2.Columns.Contains("impte"))
                    {
                        MessageBox.Show("La columna 'impte' no existe en el DataGridView.", "Error",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    columnIndex = dataGridView2.Columns["impte"].Index;
                }
                else if (label1Value.StartsWith("DAT"))
                {
                    // Usar la columna "impte"
                    if (!dataGridView2.Columns.Contains("others"))
                    {
                        MessageBox.Show("Datos");
                        return;
                    }
                    columnIndex = dataGridView2.Columns["others"].Index;
                }
                else
                {
                    MessageBox.Show("No se puede determinar la columna adecuada para el valor de label1: " + label1Value, "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Convertir los valores de la columna a tipo double y formatear
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    if (row.Cells[columnIndex].Value != null)
                    {
                        string cellValue = row.Cells[columnIndex].Value.ToString().Trim();

                        // Reemplazar solo las comas que no sean parte de números decimales
                        if (cellValue.Contains(","))
                        {
                            // Comprobar si la coma es un separador decimal y no un separador de miles
                            if (cellValue.Count(c => c == ',') == 1 && cellValue.IndexOf(',') == cellValue.Length - 3)
                            {
                                // Es un número decimal con coma, reemplazar comas por puntos
                                cellValue = cellValue.Replace(",", ".");
                            }
                            else
                            {
                                // Es un número con separador de miles, eliminar comas
                                cellValue = cellValue.Replace(",", "");
                            }
                        }

                        if (double.TryParse(cellValue, NumberStyles.Number, CultureInfo.InvariantCulture, out double value))
                        {
                            row.Cells[columnIndex].Value = value;
                        }
                    }
                }

                // Aplicar el formato y alineación
                dataGridView2.Columns[columnIndex].DefaultCellStyle.Format = "#,##0.00";
                dataGridView2.Columns[columnIndex].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al formatear la columna según el valor de label1: " + ex.Message, "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                // Obtener el valor seleccionado del ComboBox
                string selectedValue = comboBox1.SelectedItem?.ToString();

                // Determinar la columna a utilizar según el valor seleccionado del ComboBox
                int columnIndex;
                if (selectedValue == "CATAUX" || selectedValue == "CATMAY")
                {
                    // Usar la columna "B3"
                    if (!dataGridView1.Columns.Contains("B3"))
                    {
                        MessageBox.Show("La columna 'B3' no existe en el DataGridView.", "Error",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    columnIndex = dataGridView1.Columns["B3"].Index;
                }
                else if (selectedValue != null && (selectedValue.StartsWith("SAC") || selectedValue.StartsWith("COR") || selectedValue.StartsWith("EPE")))
                {
                    // Usar la columna "impte"
                    if (!dataGridView1.Columns.Contains("impte"))
                    {
                        MessageBox.Show("La columna 'impte' no existe en el DataGridView.", "Error",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    columnIndex = dataGridView1.Columns["impte"].Index;
                }
                else
                {
                    MessageBox.Show("No se puede determinar la columna adecuada para el valor seleccionado del ComboBox.", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Convertir los valores de la columna a tipo double y formatear
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells[columnIndex].Value != null)
                    {
                        string cellValue = row.Cells[columnIndex].Value.ToString().Trim();

                        // Reemplazar solo las comas que no sean parte de números decimales
                        if (cellValue.Contains(","))
                        {
                            // Comprobar si la coma es un separador decimal y no un separador de miles
                            if (cellValue.Count(c => c == ',') == 1 && cellValue.IndexOf(',') == cellValue.Length - 3)
                            {
                                // Es un número decimal con coma, reemplazar comas por puntos
                                cellValue = cellValue.Replace(",", ".");
                            }
                            else
                            {
                                // Es un número con separador de miles, eliminar comas
                                cellValue = cellValue.Replace(",", "");
                            }
                        }

                        if (double.TryParse(cellValue, NumberStyles.Number, CultureInfo.InvariantCulture, out double value))
                        {
                            row.Cells[columnIndex].Value = value;
                        }
                    }
                }

                // Aplicar el formato y alineación
                dataGridView1.Columns[columnIndex].DefaultCellStyle.Format = "#,##0.00";
                dataGridView1.Columns[columnIndex].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al formatear la columna según el valor seleccionado del ComboBox: " + ex.Message, "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string searchText = textBox1.Text;
            if (string.IsNullOrWhiteSpace(searchText))
            {
                MessageBox.Show("Por favor, ingresa el texto a buscar.");
                return;
            }

            searchResults.Clear();
            currentSearchIndex = -1;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value != null && cell.Value.ToString().IndexOf(searchText, StringComparison.Ordinal) >= 0)
                    {
                        searchResults.Add(cell);
                    }
                }
            }

            if (searchResults.Count == 0)
            {
                MessageBox.Show("No se encontraron coincidencias.");
            }
            else
            {
                currentSearchIndex = 0;
                HighlightCurrentSearchResult();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (searchResults.Count == 0)
            {
                MessageBox.Show("No hay coincidencias para navegar.");
                return;
            }

            currentSearchIndex++;
            if (currentSearchIndex >= searchResults.Count)
            {
                currentSearchIndex = 0; // Volver al primer resultado
            }

            HighlightCurrentSearchResult();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Limpiar dataGridView1 antes de copiar las filas de dataGridView2
            button6.UseWaitCursor = true;

            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            // Copiar las columnas de dataGridView2 a dataGridView1
            foreach (DataGridViewColumn column in dataGridView2.Columns)
            {
                // Clonar la columna y ajustar el ancho
                DataGridViewColumn newColumn = (DataGridViewColumn)column.Clone();
                newColumn.Width = column.Width; // Copiar el ancho de la columna
                dataGridView1.Columns.Add(newColumn);
            }

            // Copiar las filas de dataGridView2 a dataGridView1
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                // Verificar que la fila no sea una fila nueva vacía
                if (!row.IsNewRow)
                {
                    // Crear una nueva fila para dataGridView1
                    DataGridViewRow newRow = (DataGridViewRow)row.Clone();
                    newRow.Height = row.Height; // Copiar la altura de la fila

                    // Copiar los valores de las celdas de la fila original a la nueva fila
                    for (int i = 0; i < row.Cells.Count; i++)
                    {
                        newRow.Cells[i].Value = row.Cells[i].Value;
                        newRow.Cells[i].Style = row.Cells[i].Style; // Copiar el estilo de la celda
                    }

                    // Agregar la nueva fila a dataGridView1
                    dataGridView1.Rows.Add(newRow);
                }
            }

            // Asignar la numeración en los encabezados de fila (RowHeaders)
            int rowIndex = 1;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (!row.IsNewRow)
                {
                    row.HeaderCell.Value = rowIndex.ToString();
                    rowIndex++;
                }
            }

            // Configurar el estilo de la fuente para dataGridView1
            Font newFont = new Font("Arial", 7, FontStyle.Bold); // Cambia "Arial" y otros parámetros según tus preferencias
            dataGridView1.DefaultCellStyle.Font = newFont;

            // Deshabilitar la ordenación de las columnas
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            // Ajustar el ancho del encabezado de fila para mostrar el número completo
            dataGridView1.RowHeadersWidth = 65; // Ajusta el ancho según sea necesario
            button6.UseWaitCursor = false;

        }

        private void ButtonPrueba_Click_1(object sender, EventArgs e)
        {
            {
                string filePath = @"C:\Users\david.albino\Desktop\Contas y costos\Contabilidades\CORDINA\COR2024\CATAUX"; // Cambia esta ruta según tu archivo
                int recordLength = 64; // Longitud fija de cada registro
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();

                try
                {
                    // Definir columnas de ejemplo (modifica según los datos en el archivo)
                    dataGridView1.Columns.Add("Cuenta", "Cuenta");
                    dataGridView1.Columns.Add("Nombre", "Nombre");
                    dataGridView1.Columns.Add("Dineros", "Dineros");
                    dataGridView1.Columns.Add("RangoInf", "RangoInf");
                    dataGridView1.Columns.Add("RangoSup", "RangoSup");

                    // Abrir el archivo para lectura
                    using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    using (BinaryReader reader = new BinaryReader(fs, Encoding.Default))
                    {
                        long fileLength = fs.Length;
                        long recordCount = fileLength / recordLength;

                        // Leer cada registro
                        for (int i = 0; i < recordCount; i++)
                        {
                            byte[] buffer = reader.ReadBytes(recordLength);
                            string record = Encoding.Default.GetString(buffer);

                            // Separar los campos según la estructura (ajustar a tu formato)
                            string Cuenta = record.Substring(0, 6).Trim();
                            string Nombre = record.Substring(6, 32).Trim();
                            string Dineros = record.Substring(38, 16).Trim();
                            string RangoInf = record.Substring(54, 5).Trim();
                            string RangoSup = record.Substring(59, 5).Trim();

                            // Agregar fila a la DataGridView
                            dataGridView1.Rows.Add(Cuenta, Nombre, Dineros, RangoInf, RangoSup);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al leer el archivo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void botonLimpiar1_Click(object sender, EventArgs e)
        {
            try
            {
                // Mostrar un cuadro de diálogo de confirmación
                DialogResult result = MessageBox.Show(
                    "Se quitará el archivo seleccionado en vista 1, ¿Desea continuar?",
                    "Confirmar limpiar Grid 1",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                // Si el usuario selecciona "Sí", proceder a borrar los datos
                if (result == DialogResult.Yes)
                {
                    //Se elimina el contenido en Grid 2 (Correspondiente a "Original").
                    dataGridView2.Rows.Clear();
                    dataGridView2.Columns.Clear();
                    labelRuta1.Visible = false;
                    botonLimpiar1.Visible = false;
                    banderaGrid1 = 0;
                    banderaGrid = 0;
                    labelVista1.Visible = false;
                }
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción que ocurra
                MessageBox.Show("Ocurrió un error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void botonLimpiar2_Click(object sender, EventArgs e)
        {
            try
            {
                // Mostrar un cuadro de diálogo de confirmación
                DialogResult result = MessageBox.Show(
                    "Se quitará el archivo seleccionado en vista 2, ¿Desea continuar?",
                    "Confirmar limpiar Grid 2",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                // Si el usuario selecciona "Sí", proceder a borrar los datos
                if (result == DialogResult.Yes)
                {
                    //Se elimina el contenido en Grid 2 (Correspondiente a "Original").
                    dataGridView1.Rows.Clear();
                    dataGridView1.Columns.Clear();
                    labelRuta2.Visible = false;
                    botonLimpiar2.Visible = false;
                    banderaGrid2 = 0;
                    banderaGrid = 0;
                    labelVista2.Visible = false;
                }
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción que ocurra
                MessageBox.Show("Ocurrió un error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /*------------------------------------------------------------------------------------------------*/


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0) // Verifica que se hizo clic en una celda válida
            {
                DataGridViewCell cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
            }
        }              

        private void HighlightCurrentSearchResult()
        {
            DataGridViewCell cell = searchResults[currentSearchIndex];
            dataGridView1.ClearSelection();
            cell.Selected = true;
            dataGridView1.CurrentCell = cell;
            dataGridView1.FirstDisplayedScrollingRowIndex = cell.RowIndex;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                CopyToClipboard();
            }
            if (e.Control && e.KeyCode == Keys.V)
            {
                PasteFromClipboard();
            }
        }

        private void CopyToClipboard()
        {
            DataObject dataObj = dataGridView1.GetClipboardContent();
            if (dataObj != null)
            {
                Clipboard.SetDataObject(dataObj);
            }
        }

        // Método para pegar datos desde el portapapeles a las celdas seleccionadas
        private void PasteFromClipboard()
        {
            string s = Clipboard.GetText();
            string[] lines = s.Split('\n');

            int row = dataGridView1.CurrentCell.RowIndex;
            int col = dataGridView1.CurrentCell.ColumnIndex;

            foreach (string line in lines)
            {
                if (row < dataGridView1.RowCount && line.Length > 0)
                {
                    string[] cells = line.Split('\t');
                    for (int i = 0; i < cells.Length; ++i)
                    {
                        if (col + i < dataGridView1.ColumnCount)
                        {
                            dataGridView1[col + i, row].Value = Convert.ChangeType(cells[i], dataGridView1[col + i, row].ValueType);
                        }
                        else
                        {
                            break;
                        }
                    }
                    row++;
                }
                else
                {
                    break;
                }
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private string ReadFileWithStreamReader(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("El archivo no existe.", filePath);
            }

            // Intenta leer el archivo con varias codificaciones
            foreach (var encoding in new[] { Encoding.UTF8, Encoding.GetEncoding("iso-8859-1"), Encoding.GetEncoding("windows-1252") })
            {
                try
                {
                    using (var reader = new StreamReader(filePath, encoding))
                    {
                        string content = reader.ReadToEnd();
                        content = CleanContent(content); // Limpiar contenido de caracteres incorrectos
                        Console.WriteLine($"Contenido leído con codificación {encoding.WebName}: {content}");
                        return content;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al leer con codificación {encoding.WebName}: {ex.Message}");
                }
            }

            throw new Exception("No se pudo leer el archivo con las codificaciones disponibles.");
        }

        private string CleanContent(string content)
        {
            return content.Replace("�", "-"); // Reemplazar caracteres incorrectos
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(bitmap, 0, 0);
        }

    }
}
