using System;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

namespace Web.Api.Infrastructure.Data
{
    public class AppDbContext : IDisposable
    {
        public MySqlConnection Connection;

        public AppDbContext(string connectionString)
        {
            Connection = new MySqlConnection(connectionString);
        }

        public void Dispose()
        {
            Connection.Close();
        }

    }
}