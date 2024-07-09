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
                string selectedFilePath = string.Empty;

                switch (comboBox1.SelectedItem.ToString())
                {
                    case "Seleccione un archivo":
                        label2.Text = string.Empty; // Limpiar el texto del label si no se selecciona un archivo válido
                        break;
                    case "CATAUX":
                        selectedFilePath = Path.Combine(archivosPath, "CATAUX.csv");
                        CargarArchivoCSV(selectedFilePath);
                        label2.Text = selectedFilePath; // Asignar la ruta completa al label
                        break;
                    case "CATMAY":
                        selectedFilePath = Path.Combine(archivosPath, "CATMAY.csv");
                        CargarArchivoCSV(selectedFilePath);
                        label2.Text = selectedFilePath; // Asignar la ruta completa al label
                        break;
                    case "DATOS":
                        selectedFilePath = Path.Combine(archivosPath, "DATOS.csv");
                        CargarArchivoCSV(selectedFilePath);
                        label2.Text = selectedFilePath; // Asignar la ruta completa al label
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
                            label2.Text = selectedFilePath; // Asignar la ruta completa al label
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

        // Método para interpretar una línea de texto según la estructura y cargar en el DataGridView
        private void InterpretarYMostrarOperaciones(string linea)
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
                dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al interpretar y mostrar los datos: " + ex.Message);
            }
        }

        private void InterpretarYMostrarCatmay(string linea)
        {
            try
            {
                // Limpiar DataGridView antes de cargar nuevos datos
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();

                // Configurar columnas del DataGridView
                dataGridView1.Columns.Add("Cuenta", "Cuenta");
                dataGridView1.Columns.Add("Nombre", "Nombre");
                dataGridView1.Columns.Add("Saldo", "Saldo");
                dataGridView1.Columns.Add("Rango_Inf", "Rango_Inf");
                dataGridView1.Columns.Add("Rango_Sup", "Rango_Sup");

                int index = 0;
                while (index + 64 <= linea.Length)
                {
                    // Interpretar cada campo según los índices especificados
                    string Cuenta = linea.Substring(index, 6).Trim().PadRight(6);
                    string Nombre = linea.Substring(index + 6, 32).Trim().PadRight(32);
                    string Saldo = linea.Substring(index + 38, 16).Trim().PadRight(16);
                    string Rango_Inf = linea.Substring(index + 54, 5).Trim().PadRight(5);
                    string Rango_Sup = linea.Substring(index + 59, 5).Trim().PadRight(5);

                    // Agregar fila al DataGridView
                    dataGridView1.Rows.Add(Cuenta, Nombre, Saldo, Rango_Inf, Rango_Sup);

                    // Avanzar al siguiente conjunto de datos
                    index += 64;
                }
                dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al interpretar y mostrar los datos: " + ex.Message);
            }
        }


        private void InterpretarYMostrarDatos(string linea)
        {
            try
            {
                // Limpiar DataGridView antes de cargar nuevos datos
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();

                // Configurar columnas del DataGridView
                dataGridView1.Columns.Add("D1", "D1");
                dataGridView1.Columns.Add("D2", "D2");
                dataGridView1.Columns.Add("D3", "D3");
                dataGridView1.Columns.Add("No_arch", "No_arch");
                dataGridView1.Columns.Add("a_o", "a_o");
                dataGridView1.Columns.Add("others1", "others1");
                dataGridView1.Columns.Add("ultimaPol1", "ultimaPol1");
                dataGridView1.Columns.Add("ultimoReg", "ultimoReg");
                dataGridView1.Columns.Add("others", "others");


                int index = 0;
                while (index + 236 <= linea.Length)
                {
                    // Interpretar cada campo según los índices especificados
                    string D1 = linea.Substring(index, 64).Trim().PadRight(64);
                    string D2 = linea.Substring(index + 64, 60).Trim().PadRight(60);
                    string D3 = linea.Substring(index + 124, 45).Trim().PadRight(45);
                    string No_arch = linea.Substring(index + 169, 15).Trim().PadRight(15);
                    string a_o = linea.Substring(index + 184, 5).Trim().PadRight(5);
                    string Others1 = linea.Substring(index + 189, 25).Trim().PadRight(25);
                    string ultimaPol1 = linea.Substring(index + 214, 5).Trim().PadRight(5);
                    string ultimoReg = linea.Substring(index + 219, 6).Trim().PadRight(6);
                    string others = linea.Substring(index + 225, 11).Trim().PadRight(11);


                    // Agregar fila al DataGridView
                    dataGridView1.Rows.Add(D1, D2, D3, No_arch, a_o, Others1, ultimaPol1, ultimoReg, others);

                    // Avanzar al siguiente conjunto de datos
                    index += 236;
                }
                dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al interpretar y mostrar los datos: " + ex.Message);
            }
        }
        private void InterpretarYMostrarCataux(string linea)
        {
            try
            {
                // Limpiar DataGridView antes de cargar nuevos datos
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();

                // Configurar columnas del DataGridView
                dataGridView1.Columns.Add("Cuenta", "Cuenta");
                dataGridView1.Columns.Add("Nombre", "Nombre");
                dataGridView1.Columns.Add("Saldo", "Saldo");
                dataGridView1.Columns.Add("Rango_Inf", "Rango_Inf");
                dataGridView1.Columns.Add("Rango_Sup", "Rango_Sup");

                int index = 0;
                while (index + 64 <= linea.Length)
                {
                    // Interpretar cada campo según los índices especificados
                    string Cuenta = linea.Substring(index, 6).Trim().PadRight(6);
                    string Nombre = linea.Substring(index + 6, 32).Trim().PadRight(32);
                    string Saldo = linea.Substring(index + 38, 16).Trim().PadRight(16);
                    string Rango_Inf = linea.Substring(index + 54, 5).Trim().PadRight(5);
                    string Rango_Sup = linea.Substring(index + 59, 5).Trim().PadRight(5);

                    // Agregar fila al DataGridView
                    dataGridView1.Rows.Add(Cuenta, Nombre, Saldo, Rango_Inf, Rango_Sup);

                    // Avanzar al siguiente conjunto de datos
                    index += 64;
                }
                dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

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

                    // Extraer el nombre del archivo sin la extensión
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);

                    // Asignar el nombre al texto del label (label1)
                    label1.Text = fileNameWithoutExtension;

                    // Leer la línea completa del archivo
                    string lineaCompleta = File.ReadAllText(filePath, Encoding.UTF8);

                    // Aquí debe de llevar una instrucción dependiendo el label
                    if (fileNameWithoutExtension.StartsWith("CATMAY", StringComparison.OrdinalIgnoreCase))
                    {
                        InterpretarYMostrarCatmay(lineaCompleta);
                    }
                    else if (fileNameWithoutExtension.StartsWith("CATAUX", StringComparison.OrdinalIgnoreCase))
                    {
                        InterpretarYMostrarCataux(lineaCompleta);
                    }
                    else if (fileNameWithoutExtension.Equals("DATOS", StringComparison.OrdinalIgnoreCase))
                    {
                        InterpretarYMostrarDatos(lineaCompleta);
                    }
                    else if (fileNameWithoutExtension.StartsWith("SAC", StringComparison.OrdinalIgnoreCase) ||

                             fileNameWithoutExtension.StartsWith("COR", StringComparison.OrdinalIgnoreCase) ||
                             /*  fileNameWithoutExtension.StartsWith("COR", StringComparison.OrdinalIgnoreCase) ||
                               fileNameWithoutExtension.StartsWith("COR", StringComparison.OrdinalIgnoreCase) ||
                               fileNameWithoutExtension.StartsWith("COR", StringComparison.OrdinalIgnoreCase) ||
                               fileNameWithoutExtension.StartsWith("COR", StringComparison.OrdinalIgnoreCase) ||
                               fileNameWithoutExtension.StartsWith("COR", StringComparison.OrdinalIgnoreCase) ||
                               fileNameWithoutExtension.StartsWith("COR", StringComparison.OrdinalIgnoreCase) ||
                               fileNameWithoutExtension.StartsWith("COR", StringComparison.OrdinalIgnoreCase) ||*/
                             fileNameWithoutExtension.StartsWith("SUP", StringComparison.OrdinalIgnoreCase))
                    {
                        InterpretarYMostrarOperaciones(lineaCompleta);
                    }
                    else
                    {
                        MessageBox.Show("Archivo no reconocido.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir el archivo: " + ex.Message);
            }
        }


        // Funciónes para guardar los datos con la estructura especificada
        private void GuardarOpers(int CTA_Length, int descr_Length, int fe_Length, int impte_Length, int indenti_Length, int real_Length)
        {
            try
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "Archivo de Texto|*.txt"; // Filtro para archivos de texto con extensión .txt
                saveFileDialog1.Title = "Guardar datos en archivo de texto";
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
                            // Obtener y formatear los valores de las celdas
                            string CTA = Convert.ToString(row.Cells["CTA"].Value).PadRight(CTA_Length).Substring(0, CTA_Length);
                            string descr = Convert.ToString(row.Cells["descr"].Value).PadRight(descr_Length).Substring(0, descr_Length);
                            string fe = Convert.ToString(row.Cells["fe"].Value).PadRight(fe_Length).Substring(0, fe_Length);
                            string impte = Convert.ToString(row.Cells["impte"].Value).PadRight(impte_Length).Substring(0, impte_Length);
                            string indenti = Convert.ToString(row.Cells["indenti"].Value).PadRight(indenti_Length).Substring(0, indenti_Length);
                            string real = Convert.ToString(row.Cells["real"].Value).PadRight(real_Length).Substring(0, real_Length);

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

        private void GuardarCats(int Cuenta_Length, int Nombre_Length, int Saldo_Length, int Rango_Inf_Length, int Rango_Sup_Length)
        {
            try
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "Archivo de Texto|*.txt"; // Filtro para archivos de texto con extensión .txt
                saveFileDialog1.Title = "Guardar datos CATMAY en archivo de texto";
                saveFileDialog1.FileName = "datos_catmay"; // Nombre base del archivo

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
                            // Obtener y formatear los valores de las celdas según los parámetros proporcionados
                            string Cuenta = Convert.ToString(row.Cells["Cuenta"].Value).PadRight(Cuenta_Length).Substring(0, Cuenta_Length);
                            string Nombre = Convert.ToString(row.Cells["Nombre"].Value).PadRight(Nombre_Length).Substring(0, Nombre_Length);
                            string Saldo = Convert.ToString(row.Cells["Saldo"].Value).PadRight(Saldo_Length).Substring(0, Saldo_Length);
                            string Rango_Inf = Convert.ToString(row.Cells["Rango_Inf"].Value).PadRight(Rango_Inf_Length).Substring(0, Rango_Inf_Length);
                            string Rango_Sup = Convert.ToString(row.Cells["Rango_Sup"].Value).PadRight(Rango_Sup_Length).Substring(0, Rango_Sup_Length);

                            // Combinar los campos en una línea y agregar al StringBuilder
                            string line = $"{Cuenta}{Nombre}{Saldo}{Rango_Inf}{Rango_Sup}";
                            sb.Append(line); // Usar Append en lugar de AppendLine para evitar saltos de línea adicionales
                        }
                    }

                    // Escribir el contenido del StringBuilder en el archivo de texto
                    File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);

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
                saveFileDialog1.Filter = "Archivo de Texto|*.txt"; // Filtro para archivos de texto con extensión .txt
                saveFileDialog1.Title = "Guardar datos en archivo de texto";
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
                            // Obtener y formatear los valores de las celdas según los parámetros proporcionados
                            string D1 = Convert.ToString(row.Cells["D1"].Value).PadRight(D1_Length).Substring(0, D1_Length);
                            string D2 = Convert.ToString(row.Cells["D2"].Value).PadRight(D2_Length).Substring(0, D2_Length);
                            string D3 = Convert.ToString(row.Cells["D3"].Value).PadRight(D3_Length).Substring(0, D3_Length);
                            string No_arch = Convert.ToString(row.Cells["No_arch"].Value).PadRight(No_arch_Length).Substring(0, No_arch_Length);
                            string a_o = Convert.ToString(row.Cells["a_o"].Value).PadRight(a_o_Length).Substring(0, a_o_Length);
                            string Others1 = Convert.ToString(row.Cells["others1"].Value).PadRight(Others1_Length).Substring(0, Others1_Length);
                            string ultimaPol1 = Convert.ToString(row.Cells["ultimaPol1"].Value).PadRight(ultimaPol1_Length).Substring(0, ultimaPol1_Length);
                            string ultimoReg = Convert.ToString(row.Cells["ultimoReg"].Value).PadRight(ultimoReg_Length).Substring(0, ultimoReg_Length);
                            string others = Convert.ToString(row.Cells["others"].Value).PadRight(others_Length).Substring(0, others_Length);

                            // Combinar los campos en una línea y agregar al StringBuilder
                            string line = $"{D1}{D2}{D3}{No_arch}{a_o}{Others1}{ultimaPol1}{ultimoReg}{others}";
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



        // Función del evento del botón 8 para guardar con la estructura específica
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

    }
}
