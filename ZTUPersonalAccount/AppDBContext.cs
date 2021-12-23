using Microsoft.EntityFrameworkCore;
using System;
using ZTUPersonalAccount.Models;

namespace ZTUPersonalAccount
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<ChatModel> Chats { get; set; }
        public DbSet<PersonalAccountModel> PersonalAccounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChatModel>()
                .HasOne(a => a.PersonalAccount).WithOne(c => c.Chat)
                .HasForeignKey<PersonalAccountModel>(p => p.Id);
        }

        public static string GetConnectionString()
        {
            string connectionString = Environment.GetEnvironmentVariable("JAWSDB_URL");
            if (string.IsNullOrEmpty(connectionString))
                connectionString = "server=localhost;user=root;password=;database=schedule;";
            else
            {
                connectionString = connectionString.Split("//")[1];
                string user = connectionString.Split(':')[0];
                connectionString = connectionString.Replace(user, "").Substring(1);
                string password = connectionString.Split('@')[0];
                connectionString = connectionString.Replace(password, "").Substring(1);
                string server = connectionString.Split(':')[0];
                connectionString = connectionString.Replace(server, "").Substring(1);
                string port = connectionString.Split('/')[0];
                string database = connectionString.Split('/')[1];
                connectionString = $"server={server};database={database};user={user};password={password};port={port}";
            }
            return connectionString;
        }
    }
}
