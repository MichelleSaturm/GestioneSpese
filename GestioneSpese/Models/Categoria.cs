using System.Collections.Generic;

namespace GestioneSpese.Models
{
    public class Categoria
    { 
        public int Id { get; set; }
        public string Descrizione { get; set; }

        public virtual IList<Spesa> Spese { get; set; }
    }
}