using GestioneSpese.EF;
using GestioneSpese.Models;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace GestioneSpese
{
    public class GestioneDelleSpese
    {
        public static void InserisciNuovaSpesa()
        {
            using GestioneSpeseContext ctx = new();

            Console.Clear();
            Console.WriteLine("=== INSERIMENTO NUOVA SPESA ===");

            Console.WriteLine("Inserisci la descrizione della spesa");
            string descrizione = Console.ReadLine();

            Console.WriteLine("Inserisci l'utente che ha sostenuto la spesa");
            string utente = Console.ReadLine();

            Console.WriteLine("Inserisci l'importo");
            decimal importo = Helpers.CheckDecimal();

            Console.WriteLine("Scegli la categoria tra quelle in elenco");
            Console.WriteLine();
            ConnectedMode.ListaCategorie();
            Console.Write(">> ");
            int categoryId = Helpers.CheckInt();

            var categoryToSelect = ctx.Categorie.Find(categoryId);


            Spesa nuovaSpesa = new()
            {
                Data = DateTime.Now,
                Descrizione = descrizione,
                Utente = utente,
                Importo = importo,
                Approvato = false,
                Categoria = categoryToSelect
            };

            ctx.Spese.Add(nuovaSpesa);

            ctx.SaveChanges();

            Console.WriteLine();
            Console.WriteLine("Inserimento Eseguito.\n" +
                "Premi un tasto per tornare al menù principale.");
            Console.ReadKey();
        }

        public static void CancellaSpese()
        {
            using GestioneSpeseContext ctx = new();

            Console.Clear();
            Console.WriteLine("=== CANCELLA SPESA ===");

            Console.WriteLine("Inserisci ID della spesa da approvare");

            Console.WriteLine();
            GestioneDelleSpese.ListaSpese();
            Console.Write(">> ");
            int spesaId = Helpers.CheckInt();

            var spesaDaCancellare = ctx.Spese.Find(spesaId);

            if (spesaDaCancellare != null)
                ctx.Spese.Remove(spesaDaCancellare);
           
            ctx.SaveChanges();

            Console.WriteLine();
            Console.WriteLine("Eliminazione eseguita con successo.\n" +
         "Premi un tasto per tornare al menù principale.");
            Console.ReadKey();
        }


        public static void TotaleSpesePerCategoria()
        {
            using GestioneSpeseContext ctx = new();


            var spesePerCategoria = ctx.Spese.GroupBy(
                c => new { c.Categoria.Id, c.Categoria.Descrizione },
                (k, g) => new
                {
                    CategoriaId = k.Id,
                    Descrizione = k.Descrizione,
                    SommaSpese = g.Sum(t => t.Importo)
                }
            );

            Console.Clear();
            Console.WriteLine("=== SOMMA DELLE SPESE PER CATEGORIA ===");

            Console.WriteLine("{0,-15} {1,-20}", "Categoria", "Somma");
            Console.WriteLine(new string('-', 35));
            foreach (var somma in spesePerCategoria)
            {
                Console.WriteLine("{0,-15} {1,-20}",
                    somma.Descrizione, somma.SommaSpese);
            }
            Console.WriteLine(new string('-', 35));
            Console.WriteLine();
            Console.WriteLine("Premi un tasto per tornare al menù principale.");
            Console.ReadKey();
        }

        public static void ElencoSpeseUtente()
        {
            using GestioneSpeseContext ctx = new();

            Console.Clear();
            Console.WriteLine("=== ELENCO SPESE PER UN DATO UTENTE ===");

            Console.WriteLine("Inserisci il nome dell'utente");

            string userName = Console.ReadLine();

            Console.WriteLine("{0,-5}{1,-15}{2,-20}{3,-15}{4,-20}{5,-15}{6,-20}", "ID", "Data", "Descrizione", "Categoria", "Utente", "Importo", "Approvata?");
            Console.WriteLine(new string('-', 100));

            foreach (var item in ctx.Spese.Where(u => u.Utente == userName).Include(c => c.Categoria))
                Console.WriteLine("{0,-5}{1,-15}{2,-20}{3,-15}{4,-20}{5,-15}{6,-20}",
                    item.Id, item.Data.ToString("dd-MMM-yyyy"), item.Descrizione, item.Categoria.Descrizione, item.Utente, item.Importo, item.Approvato);
            Console.WriteLine(new string('-', 100));

            Console.WriteLine();
            Console.WriteLine("Premi un tasto per tornare al menù principale.");
            Console.ReadKey();

        }


        public static void ElencoSpeseApprovate()
        {
            Console.Clear();
            Console.WriteLine("=== ELENCO SPESE APPROVATE ===");

            using GestioneSpeseContext ctx = new();

            Console.WriteLine("{0,-5}{1,-15}{2,-20}{3,-15}{4,-20}{5,-15}{6,-20}", "ID", "Data", "Descrizione", "Categoria", "Utente", "Importo", "Approvata?");
            Console.WriteLine(new string('-', 100));

            foreach (var item in ctx.Spese.Include(c => c.Categoria).Where(s => s.Approvato == true))
                Console.WriteLine("{0,-5}{1,-15}{2,-20}{3,-15}{4,-20}{5,-15}{6,-20}",
                    item.Id, item.Data.ToString("dd-MMM-yyyy"), item.Descrizione, item.Categoria.Descrizione, item.Utente, item.Importo, item.Approvato);
            Console.WriteLine(new string('-', 100));
            Console.WriteLine();
            Console.WriteLine("Premi un tasto per tornare al menù principale.");
            Console.ReadKey();
        }

        public static void ApprovaSpese()
        {
            using GestioneSpeseContext ctx = new();

            Console.Clear();
            Console.WriteLine("=== APPROVAZIONE SPESE ===");

            Console.WriteLine("Inserisci ID della spesa da approvare");

            Console.WriteLine();
            GestioneDelleSpese.ListaSpese();
            Console.Write(">> ");
            int spesaId = Helpers.CheckInt();
            

            var spesaDaApprovare = ctx.Spese.Find(spesaId);

            if (spesaDaApprovare != null)
                spesaDaApprovare.Approvato = true;
            else
                Console.WriteLine("ID inserito non corretto");
            
            ctx.SaveChanges();

            Console.WriteLine();
            Console.WriteLine("Approvazione spesa eseguita con successo.\n" +
         "Premi un tasto per tornare al menù principale.");
            Console.ReadKey();

        }


        
        public static void ListaSpese()
        {
            using GestioneSpeseContext ctx = new();

           
            Console.WriteLine("{0,-5}{1,-15}{2,-20}{3,-15}{4,-20}{5,-15}{6,-20}", "ID", "Data", "Descrizione", "Categoria", "Utente", "Importo", "Approvata?");
            Console.WriteLine(new string('-', 100));

            foreach (var item in ctx.Spese.Include(c => c.Categoria))
                Console.WriteLine("{0,-5}{1,-15}{2,-20}{3,-15}{4,-20}{5,-15}{6,-20}",
                    item.Id,item.Data.ToString("dd-MMM-yyyy"),item.Descrizione,item.Categoria.Descrizione,item.Utente,item.Importo,item.Approvato);
            Console.WriteLine(new string('-', 100));

           
            Console.WriteLine();
            //Console.ReadLine();

        }
    }
}
