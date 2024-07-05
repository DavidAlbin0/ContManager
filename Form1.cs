using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace ManagerCont
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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
        }

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
                string archivosPath = @"C:\Users\david.albino\Desktop\CSVs\"; // Ruta literal al directorio donde están los archivos

                switch (comboBox1.SelectedItem.ToString())
                {
                    case "Seleccione un archivo":
                        break;
                    case "CATAUX":
                        CargarArchivoCSV(Path.Combine(archivosPath, "CATAUX.csv"));
                        break;
                    case "CATMAY":
                        CargarArchivoCSV(Path.Combine(archivosPath, "CATMAY.csv"));
                        break;
                    case "DATOS":
                        CargarArchivoCSV(Path.Combine(archivosPath, "DATOS.csv"));
                        break;
                    case "OPERACIONES":
                        if (comboBox2.SelectedIndex == -1 || comboBox2.SelectedItem.ToString() == "Valor Predeterminado")
                        {
                            MessageBox.Show("Debes elegir un mes primero para esta operación");
                            comboBox1.SelectedIndex = 0;
                        }
                        else
                        {
                            CargarArchivoCSV(Path.Combine(archivosPath, comboBox3.SelectedItem.ToString() + comboBox2.SelectedItem.ToString() + ".csv"));
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
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar el archivo: " + ex.Message);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0) // Verifica que se hizo clic en una celda válida
            {
                DataGridViewCell cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
            }
        }

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

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Crear una instancia del formulario Form2
            Form2 form2 = new Form2();

            // Mostrar Form2
            form2.Show();
        }

        // Método para interpretar una línea de texto según la estructura y cargar en el DataGridView
        private void InterpretarYMostrarDatos(string linea)
        {
            try
            {
                // Limpiar DataGridView antes de cargar nuevos datos
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();

                // Configurar columnas del DataGridView
                dataGridView1.Columns.Add("CTA", "CTA");
                dataGridView1.Columns.Add("descr", "descr");
                dataGridView1.Columns.Add("fe", "fe");
                dataGridView1.Columns.Add("impte", "impte");
                dataGridView1.Columns.Add("indenti", "indenti");
                dataGridView1.Columns.Add("real", "real");

                int index = 0;
                while (index + 64 <= linea.Length)
                {
                    // Interpretar cada campo según los índices especificados
                    string CTA = linea.Substring(index, 6).Trim().PadRight(6);
                    string descr = linea.Substring(index + 6, 30).Trim().PadRight(30);
                    string fe = linea.Substring(index + 36, 2).Trim().PadRight(2);
                    string impte = linea.Substring(index + 38, 16).Trim().PadRight(16);
                    string indenti = linea.Substring(index + 54, 1).Trim().PadRight(1);
                    string real = linea.Substring(index + 55, 9).Trim().PadRight(9);

                    // Agregar fila al DataGridView
                    dataGridView1.Rows.Add(CTA, descr, fe, impte, indenti, real);

                    // Avanzar al siguiente conjunto de datos
                    index += 64;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al interpretar y mostrar los datos: " + ex.Message);
            }
        }

        // Evento para manejar el botón que carga y muestra los datos en el DataGridView
        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Filter = "Archivos de Texto|*"; // Filtro para archivos de texto

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog1.FileName;

                    // Leer la línea completa del archivo
                    string lineaCompleta = File.ReadAllText(filePath, Encoding.UTF8);

                    // Llamar al método para interpretar y mostrar los datos
                    InterpretarYMostrarDatos(lineaCompleta);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir el archivo: " + ex.Message);
            }
        }

        // Evento para guardar los datos del DataGridView en un archivo de texto sin extensión
        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "Archivo de Texto|*."; // Filtro para archivos de texto sin extensión
                saveFileDialog1.Title = "Guardar datos en archivo de texto sin extensión";
                saveFileDialog1.FileName = "datos_guardados"; // Nombre base del archivo

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog1.FileName;

                    // Crear un StringBuilder para construir el contenido del archivo
                    StringBuilder sb = new StringBuilder();

                    // Construir el contenido del archivo a partir de los datos del DataGridView
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        // Verificar si la fila no está vacía
                        if (!row.IsNewRow)
                        {
                            string CTA = Convert.ToString(row.Cells["CTA"].Value).PadRight(6);
                            string descr = Convert.ToString(row.Cells["descr"].Value).PadRight(30);
                            string fe = Convert.ToString(row.Cells["fe"].Value).PadRight(2);
                            string impte = Convert.ToString(row.Cells["impte"].Value).PadRight(16);
                            string indenti = Convert.ToString(row.Cells["indenti"].Value).PadRight(1);
                            string real = Convert.ToString(row.Cells["real"].Value).PadRight(9);

                            // Combinar los campos en una línea y agregar al StringBuilder
                            string line = $"{CTA}{descr}{fe}{impte}{indenti}{real}";
                            sb.Append(line); // Usar Append en lugar de AppendLine para evitar saltos de línea adicionales
                        }
                    }

                    // Escribir el contenido del StringBuilder en el archivo de texto
                    File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);

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
    }
}
