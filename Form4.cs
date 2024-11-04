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
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void InterpretarYMostrarDatos(string linea)
        {
            try
            {
                // Limpiar DataGridView antes de cargar nuevos datos
                dataGridView2.Rows.Clear();
                dataGridView2.Columns.Clear();

                // Configurar columnas del DataGridView
                DataGridViewColumn cuentaColumn = new DataGridViewTextBoxColumn { Name = "Cuentas", HeaderText = "Cuentas" };
                DataGridViewColumn subCuentaColumn = new DataGridViewTextBoxColumn { Name = "SubCuentas", HeaderText = "SubCuentas" };
                DataGridViewColumn subsubCuentaColumn = new DataGridViewTextBoxColumn { Name = "SubSubCuentas", HeaderText = "SubSubCuentas" };
 

                // Configurar todas las columnas para que no sean ordenables
                cuentaColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                subCuentaColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                subsubCuentaColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
              

                // Agregar columnas al DataGridView
                dataGridView2.Columns.Add(cuentaColumn);
                dataGridView2.Columns.Add(subCuentaColumn);
                dataGridView2.Columns.Add(subsubCuentaColumn);

                Font newFont = new Font("Arial", 12); // Cambia "Arial" y otros parámetros según tus preferencias
                dataGridView2.DefaultCellStyle.Font = newFont;

                int index = 0;
                while (index + 16 <= linea.Length)
                {
                    // Interpretar cada campo según los índices especificados
                    string Cuentas = linea.Substring(index, 5).Trim().PadRight(5);
                    string SubCuentas = linea.Substring(index + 5, 5).Trim().PadRight(5);
                    string SubSubCuentas = linea.Substring(index + 10, 6).Trim().PadRight(6);

                    // Agregar fila al DataGridView
                    dataGridView2.Rows.Add(Cuentas, SubCuentas, SubSubCuentas);

                    // Avanzar al siguiente conjunto de datos
                    index += 64;
                }

                // Ajustar automáticamente el tamaño de las columnas
                dataGridView2.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

                // Asignar números crecientes al encabezado de filas
                for (int u = 0; u < dataGridView2.Rows.Count; u++)
                {
                    dataGridView2.Rows[u].HeaderCell.Value = (u + 1).ToString();
                }

                // Ajustar el ancho del encabezado de fila para que se muestre correctamente
                dataGridView2.RowHeadersWidth = 65;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al interpretar y mostrar los datos: " + ex.Message);
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            // Define las longitudes para cada campo según tus requisitos
            int Cuentas = 5;         // Ejemplo de longitud para D1
            int SubCuentas = 5;         // Ejemplo de longitud para D2
            int SubSubCuentas = 6;         // Ejemplo de longitud para D3

            // Llama a la función GuardarDatos con los parámetros definidos
            GuardarDatos(Cuentas, SubCuentas, SubSubCuentas);
        }

        private void GuardarDatos(int Cuentas, int SubCuentas, int SubSubCuentas)
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
                            string g1 = Convert.ToString(row.Cells["Cuentas"].Value).PadRight(Cuentas).Substring(0, Cuentas);
                            string g2 = Convert.ToString(row.Cells["SubCuentas"].Value).PadRight(SubCuentas).Substring(0, SubCuentas);
                            string g3 = Convert.ToString(row.Cells["SubSubCuentas"].Value).PadRight(SubSubCuentas).Substring(0, SubSubCuentas);

                            // Combinar los campos en una línea y agregar al StringBuilder
                            string line = $"{g1}{g2}{g3}";
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

        private void button1_Click(object sender, EventArgs e)
        {
            // Crear una instancia del OpenFileDialog
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Configurar las propiedades del OpenFileDialog
            openFileDialog.Filter = "Archivos .CAT (*.CAT)|*.CAT"; // Permite solo archivos con extensión .CAT
            openFileDialog.Title = "Seleccionar archivo";
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

    }
}

