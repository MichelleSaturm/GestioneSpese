using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.IO;

namespace GestioneSpese
{
    public class DisconnectedMode
    {
        static IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

        static string connectionStringSQL = config.GetConnectionString("GestioneSpese");

        public static void FillDataSet()
        {
            DataSet spesaDs = new DataSet();

            using SqlConnection conn = new SqlConnection(connectionStringSQL);

            try
            {
                conn.Open();
                if (conn.State == ConnectionState.Open)
                    Console.WriteLine("Connessione effettuata con successo");
                else
                {
                    Console.WriteLine("Connessione Fallita");
                    return;
                }

                InitGestioneSpeseDataSetAndAdapter(spesaDs, conn);

                conn.Close();

            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"SQL Exception: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Generic Exception: {ex.Message}");
            }
            finally
            {
                conn.Close();
            }
        }


        public static void ModificaSpesa()
        {

            DataSet speseDs = new DataSet();

            using SqlConnection conn = new SqlConnection(connectionStringSQL);

            try
            {
                conn.Open();
                if (conn.State == ConnectionState.Open)
                    Console.WriteLine("Connessione effettuata con successo");
                else
                {
                    Console.WriteLine("Connessione Fallita");
                    return;
                }

                SqlDataAdapter gestioneSpeseAdapter = InitGestioneSpeseDataSetAndAdapter(speseDs, conn);


                conn.Close();

                Console.Clear();
                Console.WriteLine("=== MODIFICA SPESA ===");

                Console.WriteLine("Inserisci ID della spesa da modificare");

                Console.WriteLine();
                GestioneDelleSpese.ListaSpese();
                Console.Write(">> ");
                int spesaId = Helpers.CheckInt();
                DataRow rowToUpdate = speseDs.Tables["Spese"].Rows.Find(spesaId);

                if (rowToUpdate != null)
                {
                    Console.WriteLine("Inserisci nuova descrizione");
                    rowToUpdate["Descrizione"] = Console.ReadLine();

                    Console.WriteLine("Inserisci nuovo importo");
                    rowToUpdate["Importo"] = Helpers.CheckDecimal();

                    Console.WriteLine();
                    Console.WriteLine("Modifica eseguita con successo.\n" +
                 "Premi un tasto per tornare al menù principale.");
                    Console.ReadKey();
                }


                gestioneSpeseAdapter.Update(speseDs, "Spese");

            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"SQL Exception: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Generic Exception: {ex.Message}");
            }
            finally
            {
                conn.Close();
            }

        }


        #region Support Method
        private static SqlDataAdapter InitGestioneSpeseDataSetAndAdapter(DataSet speseDs, SqlConnection conn)
        {
            SqlDataAdapter spesaAdapter = new SqlDataAdapter();

            spesaAdapter.SelectCommand = GenerateSelectCommand(conn);
            spesaAdapter.UpdateCommand = GenerateUpdateCommand(conn);

            spesaAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            spesaAdapter.Fill(speseDs, "Spese");

            return spesaAdapter;
        }

        private static SqlCommand GenerateSelectCommand(SqlConnection conn)
        {
            SqlCommand speseSelectCommand = new SqlCommand();
            speseSelectCommand.Connection = conn;
            speseSelectCommand.CommandType = CommandType.Text;
            speseSelectCommand.CommandText = "SELECT * FROM Spese";

            return speseSelectCommand;
        }

        private static SqlCommand GenerateUpdateCommand(SqlConnection conn)
        {
            SqlCommand spesaUpdateCommand = new SqlCommand();
            spesaUpdateCommand.Connection = conn;
            spesaUpdateCommand.CommandType = CommandType.Text;
            spesaUpdateCommand.CommandText = "UPDATE Spese SET Descrizione = @Descrizione, Importo=@Importo WHERE Id = @Id";


            spesaUpdateCommand.Parameters.Add(new SqlParameter(
                "@Id",
                SqlDbType.Int,
                0,
                "Id"));
            spesaUpdateCommand.Parameters.Add(new SqlParameter(
                "@Descrizione",
                SqlDbType.NVarChar,
                500,
                "Descrizione"));
            spesaUpdateCommand.Parameters.Add(new SqlParameter(
                "@Importo",
                SqlDbType.Decimal,
                18,
                "Importo"));

            return spesaUpdateCommand;
        }



        #endregion

    }
}