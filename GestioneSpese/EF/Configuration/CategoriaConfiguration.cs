using GestioneSpese.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace GestioneSpese.EF.Configuration
{
    class CategoriaConfiguration : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Descrizione)
                .IsRequired()
                .HasMaxLength(100);

            builder
                 .HasMany(s => s.Spese)
                 .WithOne(c => c.Categoria);

        }
    }
}
