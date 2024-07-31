using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace ManagerCont
{
    public partial class Form1 : Form
    {
        private string connectionString = "Server=WSTSEC-280\\MSSQLSERVER01;Database=Prueba;User Id=userPrueba;Password=3.1416Xpiuu;";


        private void ProcesarDatos()
        {
            string labelValue = label1.Text;

            // Utilizar switch para manejar diferentes casos basados en el valor de label1
            switch (labelValue)
            {

                case "CATMAY":
                    // Procesar datos de acuerdo con la estructura CATMAY
                    SubirCATMAY();
                    break;

                case "CATAUX":
                    // Procesar datos de acuerdo con la estructura CATMAY
                    SubirCATAUX();
                    break;
                case "DATOS":
                    // Procesar datos de acuerdo con la estructura CATMAY
                    SubirDatos();
                    break;

                default:
                    // Procesar según el prefijo del nombre del archivo
                    if (labelValue.StartsWith("SAC", StringComparison.OrdinalIgnoreCase) ||
                        labelValue.StartsWith("COR", StringComparison.OrdinalIgnoreCase) ||
                        labelValue.StartsWith("SUP", StringComparison.OrdinalIgnoreCase))
                    {
                        SubirOperaciones();
                    }
                    else
                    {
                        MessageBox.Show("El valor del Label1 no coincide con ninguna estructura conocida.");
                    }
                    break;
            }
        }

        private void SubirOperaciones()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (!row.IsNewRow) // Excluir la fila nueva (vacía)
                    {
                        string query = "INSERT INTO operaciones (CTA, descr, fe, impte, indenti, real) VALUES (@CTA, @descr, @fe, @impte, @indenti, @real)";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@CTA", row.Cells["CTA"].Value ?? DBNull.Value);
                            command.Parameters.AddWithValue("@descr", row.Cells["descr"].Value ?? DBNull.Value);
                            command.Parameters.AddWithValue("@fe", row.Cells["fe"].Value ?? DBNull.Value);
                            command.Parameters.AddWithValue("@impte", row.Cells["impte"].Value ?? DBNull.Value);
                            command.Parameters.AddWithValue("@indenti", row.Cells["indenti"].Value ?? DBNull.Value);
                            command.Parameters.AddWithValue("@real", row.Cells["real"].Value ?? DBNull.Value);

                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        private void SubirCATMAY()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (!row.IsNewRow) // Excluir la fila nueva (vacía)
                    {
                        string query = "INSERT INTO CATMAY (Cuenta, Nombre, Saldo, Rango_Inf, Rango_Sup) VALUES (@Cuenta, @Nombre, @Saldo, @Rango_Inf, @Rango_Sup)";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Cuenta", row.Cells["Cuenta"].Value ?? DBNull.Value);
                            command.Parameters.AddWithValue("@Nombre", row.Cells["Nombre"].Value ?? DBNull.Value);
                            command.Parameters.AddWithValue("@Saldo", row.Cells["Saldo"].Value ?? DBNull.Value);
                            command.Parameters.AddWithValue("@Rango_Inf", row.Cells["Rango_Inf"].Value ?? DBNull.Value);
                            command.Parameters.AddWithValue("@Rango_Sup", row.Cells["Rango_Sup"].Value ?? DBNull.Value);

                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        private void SubirCATAUX()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (!row.IsNewRow) // Excluir la fila nueva (vacía)
                    {
                        string query = "INSERT INTO CATAUX (Cuenta, Nombre, Saldo, Rango_Inf, Rango_Sup) VALUES (@Cuenta, @Nombre, @Saldo, @Rango_Inf, @Rango_Sup)";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Cuenta", row.Cells["Cuenta"].Value ?? DBNull.Value);
                            command.Parameters.AddWithValue("@Nombre", row.Cells["Nombre"].Value ?? DBNull.Value);
                            command.Parameters.AddWithValue("@Saldo", row.Cells["Saldo"].Value ?? DBNull.Value);
                            command.Parameters.AddWithValue("@Rango_Inf", row.Cells["Rango_Inf"].Value ?? DBNull.Value);
                            command.Parameters.AddWithValue("@Rango_Sup", row.Cells["Rango_Sup"].Value ?? DBNull.Value);

                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        private void SubirDatos()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (!row.IsNewRow) // Excluir la fila nueva (vacía)
                    {
                        string query = "INSERT INTO DATOS (D1, D2, D3, No_arch, a_o, others1, ultimaPol, ultimaOperacionReg, others) VALUES (@D1, @D2, @D3, @No_arch, @a_o, @others1, @ultimaPol, @ultimaOperacionReg, @others)";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@D1", row.Cells["D1"].Value ?? DBNull.Value);
                            command.Parameters.AddWithValue("@D2", row.Cells["D2"].Value ?? DBNull.Value);
                            command.Parameters.AddWithValue("@D3", row.Cells["D3"].Value ?? DBNull.Value);
                            command.Parameters.AddWithValue("@No_arch", row.Cells["No_arch"].Value ?? DBNull.Value);
                            command.Parameters.AddWithValue("@a_o", row.Cells["a_o"].Value ?? DBNull.Value);
                            command.Parameters.AddWithValue("@others1", row.Cells["others1"].Value ?? DBNull.Value);
                            command.Parameters.AddWithValue("@ultimapol", row.Cells["ultimaPol"].Value ?? DBNull.Value);
                            command.Parameters.AddWithValue("@ultimaOperacionReg", row.Cells["ultimaOperacionReg"].Value ?? DBNull.Value);
                            command.Parameters.AddWithValue("@others", row.Cells["others"].Value ?? DBNull.Value);

                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
        }
    }
}
