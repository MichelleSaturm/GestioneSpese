using System;

namespace GestioneSpese
{
    public class PannelloDiControllo
    {
        public static void Menu()
        {
            bool continuare = true;
            do
            {
                Console.Clear();
                Console.WriteLine("=== ESERCITAZIONE - GESTIONE SPESE ===");

                Console.WriteLine("[1] Inserisci Nuova Spesa");
                Console.WriteLine("[2] Approva Spese Esistenti");
                Console.WriteLine("[3] Cancella Spese Esistenti");
                Console.WriteLine("[4] Elenco Spese Approvate");
                Console.WriteLine("[5] Elenco Spese di un Utente");
                Console.WriteLine("[6] Totale delle Spese per una Categoria");
                Console.WriteLine("[7] Lista di tutte le spese {Connected Mode}");
                Console.WriteLine("[8] Modifica una spesa {Disonnected Mode}");
                Console.WriteLine();
                Console.WriteLine("[0] Esci");
                Console.WriteLine();
                Console.Write("Inserisci la tua scelta: ");

                int choice;
                bool isInt;
                do
                {
                    isInt = int.TryParse(Console.ReadLine(), out choice);
                } while (!isInt);


                switch (choice)
                {
                    case 1:
                        GestioneDelleSpese.InserisciNuovaSpesa();
                        break;
                    case 2:
                        GestioneDelleSpese.ApprovaSpese();
                        break;
                    case 3:
                        GestioneDelleSpese.CancellaSpese();
                        break;
                    case 4:
                        GestioneDelleSpese.ElencoSpeseApprovate();
                        break;
                    case 5:
                        GestioneDelleSpese.ElencoSpeseUtente();
                        break;
                    case 6:
                        GestioneDelleSpese.TotaleSpesePerCategoria();
                        break;
                    case 7:
                        ConnectedMode.ListaSpeseConnectedMode();
                        break;
                    case 8:
                        DisconnectedMode.ModificaSpesa();
                        break;
                    case 0:
                        Console.WriteLine("See ya!!");
                        continuare = false;
                        break;
                    default:
                        Console.WriteLine("La scelta è sbagliata. Riprova.");
                        break;
                }
            } while (continuare);
        }
    }
}