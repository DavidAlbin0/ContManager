using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace ManagerCont
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            dataGridView2.KeyDown += new KeyEventHandler(dataGridView2_KeyDown);

        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void InterpretarYMostrarDatos(string linea)
        {
            try
            {
                // Limpiar DataGridView antes de cargar nuevos datos
                dataGridView2.Rows.Clear();
                dataGridView2.Columns.Clear();

                // Agregar columna "Mes" al inicio del DataGridView
                DataGridViewColumn mesColumn = new DataGridViewTextBoxColumn { Name = "Mes", HeaderText = "Mes" };
                dataGridView2.Columns.Add(mesColumn);

                // Configurar las demás columnas del DataGridView
                DataGridViewColumn d1Column = new DataGridViewTextBoxColumn { Name = "D1", HeaderText = "D1" };
                DataGridViewColumn d2Column = new DataGridViewTextBoxColumn { Name = "D2", HeaderText = "D2" };
                DataGridViewColumn d3Column = new DataGridViewTextBoxColumn { Name = "D3", HeaderText = "D3" };
                DataGridViewColumn noArchColumn = new DataGridViewTextBoxColumn { Name = "No_arch", HeaderText = "No_arch" };
                DataGridViewColumn aoColumn = new DataGridViewTextBoxColumn { Name = "a_o", HeaderText = "a_o" };
                DataGridViewColumn others1Column = new DataGridViewTextBoxColumn { Name = "others1", HeaderText = "others1" };
                DataGridViewColumn ultimaPol1Column = new DataGridViewTextBoxColumn { Name = "ultimaPol1", HeaderText = "ultimaPol1" };
                DataGridViewColumn ultimaOperacionRegColumn = new DataGridViewTextBoxColumn { Name = "ultimaOperacionReg", HeaderText = "ultimaOperacionReg" };
                DataGridViewColumn othersColumn = new DataGridViewTextBoxColumn { Name = "others", HeaderText = "others" };

                // Agregar las demás columnas al DataGridView
                dataGridView2.Columns.Add(d1Column);
                dataGridView2.Columns.Add(d2Column);
                dataGridView2.Columns.Add(d3Column);
                dataGridView2.Columns.Add(noArchColumn);
                dataGridView2.Columns.Add(aoColumn);
                dataGridView2.Columns.Add(others1Column);
                dataGridView2.Columns.Add(ultimaPol1Column);
                dataGridView2.Columns.Add(ultimaOperacionRegColumn);
                dataGridView2.Columns.Add(othersColumn);

                // Definir los nombres de los meses
                string[] meses = new string[]
                {
            "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
            "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"
                };

                Font newFont = new Font("Arial", 7, FontStyle.Bold);
                dataGridView2.DefaultCellStyle.Font = newFont;

                int index = 0;
                int mesIndex = 0;
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
                    string ultimaOperacionReg = linea.Substring(index + 219, 6).Trim().PadRight(6);
                    string others = linea.Substring(index + 225, 11).Trim().PadRight(11);

                    // Determinar el valor para la columna "Mes"
                    string mes;
                    if (mesIndex < 12)
                    {
                        mes = meses[mesIndex];
                    }
                    else
                    {
                        mes = "Incorporación";
                    }

                    // Agregar fila al DataGridView
                    dataGridView2.Rows.Add(mes, D1, D2, D3, No_arch, a_o, Others1, ultimaPol1, ultimaOperacionReg, others);

                    // Avanzar al siguiente mes y al siguiente conjunto de datos
                    mesIndex++;
                    index += 236;
                }

                dataGridView2.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al interpretar y mostrar los datos: " + ex.Message);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            // Crear una instancia del OpenFileDialog
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Configurar las propiedades del OpenFileDialog
            openFileDialog.Filter = "Archivos Datos|Datos"; // Permite solo archivos que se llamen "Datos"
            openFileDialog.Title = "Seleccionar archivo Datos";
            openFileDialog.Multiselect = false; // Permite seleccionar solo un archivo

            // Mostrar el diálogo y verificar si el usuario seleccionó un archivo
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Leer el contenido del archivo
                string fileContent = File.ReadAllText(openFileDialog.FileName);

                // Llamar a la función InterpretarYMostrarDatos con el contenido del archivo
                InterpretarYMostrarDatos(fileContent);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Define las longitudes para cada campo según tus requisitos
            int D1_Length = 64;         // Ejemplo de longitud para D1
            int D2_Length = 60;         // Ejemplo de longitud para D2
            int D3_Length = 45;         // Ejemplo de longitud para D3
            int No_arch_Length = 15;    // Ejemplo de longitud para No_arch
            int a_o_Length = 5;         // Ejemplo de longitud para a_o
            int Others1_Length = 25;    // Ejemplo de longitud para Others1
            int ultimaPol1_Length = 5;  // Ejemplo de longitud para ultimaPol1
            int ultimoReg_Length = 6;   // Ejemplo de longitud para ultimaOperacionReg
            int others_Length = 11;     // Ejemplo de longitud para others

            // Llama a la función GuardarDatos con los parámetros definidos
            GuardarDatos(D1_Length, D2_Length, D3_Length, No_arch_Length, a_o_Length, Others1_Length, ultimaPol1_Length, ultimoReg_Length, others_Length);
        }

        private void GuardarDatos(int D1_Length, int D2_Length, int D3_Length, int No_arch_Length, int a_o_Length, int Others1_Length, int ultimaPol1_Length, int ultimoReg_Length, int others_Length)
        {
            try
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "Archivo de Texto|*"; // Filtro para archivos de texto con extensión .txt
                saveFileDialog1.Title = "Guardar datos en archivo de texto";
                saveFileDialog1.FileName = "datos_guardados"; // Nombre base del archivo

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog1.FileName;

                    // Crear un StringBuilder para construir el contenido del archivo
                    StringBuilder sb = new StringBuilder();

                    // Construir el contenido del archivo a partir de los datos del DataGridView
                    foreach (DataGridViewRow row in dataGridView2.Rows)
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

                            // Combinar los campos en una línea y agregar al StringBuilder
                            string line = $"{D1}{D2}{D3}{No_arch}{a_o}{Others1}{ultimaPol1}{ultimoReg}{others}";
                            sb.Append(line); // Usar Append en lugar de AppendLine para evitar saltos de línea adicionales
                        }
                    }

                    // Escribir el contenido del StringBuilder en el archivo de texto
                    File.WriteAllText(filePath, sb.ToString());

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


        private void dataGridView2_KeyDown(object sender, KeyEventArgs e)
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
            DataObject dataObj = dataGridView2.GetClipboardContent();
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

            int row = dataGridView2.CurrentCell.RowIndex;
            int col = dataGridView2.CurrentCell.ColumnIndex;

            foreach (string line in lines)
            {
                if (row < dataGridView2.RowCount && line.Length > 0)
                {
                    string[] cells = line.Split('\t');
                    for (int i = 0; i < cells.Length; ++i)
                    {
                        if (col + i < dataGridView2.ColumnCount)
                        {
                            dataGridView2[col + i, row].Value = Convert.ChangeType(cells[i], dataGridView2[col + i, row].ValueType);
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
    }
}
