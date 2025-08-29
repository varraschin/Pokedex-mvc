using Microsoft.EntityFrameworkCore;
using Pokedex.Models;

namespace Pokedex.Data;

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
            
        }
        public DbSet<Genero> Generos { get; set; }
        public DbSet<Pokemon> Pokemons { get; set; }
        public DbSet<PokemonTipo> PokemonTipos { get; set; }
        public DbSet<Regiao> Regioes { get; set; }
        public DbSet<Tipo> Tipos { get; set; }

    protected override void OnModelCreating(ModelBuilder Builder)
    {
        base.OnModelCreating(Builder);

        #region Muitos para Muitos do Pokemon Tipo
        // Chave da primária composta
        Builder.Entity<PokemonTipo>()
        .HasKey(pt => new { pt.PokemonNumero, pt.TipoId });

        // Chave estrangeira PokemonTipo - Pokemon
        Builder.Entity<PokemonTipo>()
        .HasOne(pt => pt.Pokemon)
        .WithMany(p => p.Tipos)
        .HasForeignKey(pt => pt.PokemonNumero);

        // Chave Estrangeira PokemonTipo - Tipo
        Builder.Entity<PokemonTipo>()
        .HasOne(pt => pt.Tipo)
        .WithMany(t => t.Pokemons)
        .HasForeignKey(pt => pt.TipoId);
        #endregion
    }
}
