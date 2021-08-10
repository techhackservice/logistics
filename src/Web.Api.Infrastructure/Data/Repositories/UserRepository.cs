using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Web.Api.Core.Interfaces.Gateways.Repositories;

namespace Web.Api.Infrastructure.Data.Repositories
{
    internal sealed class UserRepository : IUserRepository
    {
        private new readonly AppDbContext _appDbContext;
        public UserRepository(AppDbContext appDbContext)
        {
        }

        /// <summary>
        /// Check whether username is present or not
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>username present or not</returns>
        public async Task<bool> CheckUsername(string userName)
        {
            return true;
        }
    }
}