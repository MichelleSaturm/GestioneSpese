using GestioneSpese.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestioneSpese.EF.Configuration
{
    public class SpesaConfiguration : IEntityTypeConfiguration<Spesa>
    {
        public void Configure(EntityTypeBuilder<Spesa> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Data)
                .IsRequired();

            builder.Property(s => s.Descrizione)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(s => s.Utente)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.Importo)
                .IsRequired();

            builder.Property(s => s.Approvato)
                .IsRequired();


            builder
                .HasOne(c => c.Categoria)
                .WithMany(s => s.Spese);

        }
    }
}
