using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestioneSpese
{
    public class ConnectedMode
    {
        static IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

        static string connectionStringSQL = config.GetConnectionString("GestioneSpese");

        public static void ListaCategorie()
        {
            using SqlConnection conn = new SqlConnection(connectionStringSQL);

            try
            {
                conn.Open();
                SqlCommand selectCommand = conn.CreateCommand();
                selectCommand.CommandType = System.Data.CommandType.Text;
                selectCommand.CommandText = "SELECT * FROM Categorie";

                SqlDataReader reader = selectCommand.ExecuteReader();

                while (reader.Read())
                {
                    Console.WriteLine("{0,-5}{1,-40}", reader["Id"], reader["Descrizione"]);
                }
            }
            catch (Exception ex)
            {
                Console.Write($"Errore: {ex.Message}");
            }
            finally
            {
                conn.Close();
            }
        }

        public static void ListaSpeseConnectedMode()
        {
            using SqlConnection conn = new SqlConnection(connectionStringSQL);

            try
            {
                conn.Open();

                if (conn.State == System.Data.ConnectionState.Open)
                    Console.WriteLine("Connessi al DB");
                else
                    Console.WriteLine("CONNESSIONE FALLITA!");

                Console.Clear();
                Console.WriteLine("=== LISTA SPESE ===");

                string sqlStatement = "SELECT Spese.Id, Spese.Data, Spese.Descrizione, Categorie.Descrizione AS Categoria, Spese.Utente, Spese.Importo, Spese.Approvato " +
                    "FROM Categorie INNER JOIN " +
                    "Spese ON Categorie.Id = Spese.CategoriaId";

                SqlCommand readCommand = new SqlCommand();
                readCommand.Connection = conn;
                readCommand.CommandType = System.Data.CommandType.Text;
                readCommand.CommandText = sqlStatement;

                SqlDataReader reader = readCommand.ExecuteReader();

                Console.WriteLine("{0,-5}{1,-15}{2,-20}{3,-15}{4,-20}{5,-15}{6,-20}", "ID", "Data", "Descrizione", "Categoria", "Utente", "Importo", "Approvata?");
                Console.WriteLine(new string('-', 100));
                while (reader.Read())
                {

                    string data = ((DateTime)reader["Data"]).ToString("dd-MMM-yyyy");
                    Console.WriteLine("{0,-5}{1,-15}{2,-20}{3,-15}{4,-20}{5,-15}{6,-20}", reader["Id"], data, reader["Descrizione"],reader["Categoria"], reader["Utente"], reader["Importo"], reader["Approvato"]);
                }
                Console.WriteLine(new string('-', 100));
                Console.WriteLine();
                Console.WriteLine("Premi un tasto per tornare al menù principale.");
                Console.ReadKey();
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"ERRORE: {ex.Message}");
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
