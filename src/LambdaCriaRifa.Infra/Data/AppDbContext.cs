using LambdaCriaRifa.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace LambdaCriaRifa.Infra.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Rifa> Rifas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Rifa>(entity =>
        {
            entity.ToTable("rifas");
            
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Id)
                .HasColumnName("id");
            
            entity.Property(e => e.Titulo)
                .HasColumnName("titulo")
                .IsRequired()
                .HasMaxLength(200);
            
            entity.Property(e => e.Descricao)
                .HasColumnName("descricao")
                .HasMaxLength(1000);
            
            entity.Property(e => e.ValorBilhete)
                .HasColumnName("valor_bilhete")
                .HasColumnType("decimal(18,2)");
            
            entity.Property(e => e.QuantidadeBilhetes)
                .HasColumnName("quantidade_bilhetes");
            
            entity.Property(e => e.DataSorteio)
                .HasColumnName("data_sorteio");
            
            entity.Property(e => e.DataCriacao)
                .HasColumnName("data_criacao");
            
            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasMaxLength(50);
            
            entity.Property(e => e.CriadoPor)
                .HasColumnName("criado_por")
                .HasMaxLength(100);
        });
    }
}
