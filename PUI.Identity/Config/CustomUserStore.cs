using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PUI.Identity.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PUI.Identity.Config
{
    public class CustomUserStore : IUserPasswordStore<Usuario>
    {
        private readonly PuiIdentityDbContext _context;

        public CustomUserStore(PuiIdentityDbContext context)
        {
            _context = context;
        }

        public void Dispose() { }

        public Task<string?> GetUserIdAsync(Usuario user, CancellationToken cancellationToken)
            => Task.FromResult(user.Id);

        public Task<string?> GetUserNameAsync(Usuario user, CancellationToken cancellationToken)
            => Task.FromResult(user.UserName);

        public Task SetUserNameAsync(Usuario user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public Task<string?> GetNormalizedUserNameAsync(Usuario user, CancellationToken cancellationToken)
            => Task.FromResult(user.UserName?.ToUpper());

        public Task SetNormalizedUserNameAsync(Usuario user, string normalizedName, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> CreateAsync(Usuario user, CancellationToken cancellationToken)
        {
            _context.Add(user);
            await _context.SaveChangesAsync(cancellationToken);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateAsync(Usuario user, CancellationToken cancellationToken)
        {
            _context.Update(user);
            await _context.SaveChangesAsync(cancellationToken);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(Usuario user, CancellationToken cancellationToken)
        {
            _context.Remove(user);
            await _context.SaveChangesAsync(cancellationToken);
            return IdentityResult.Success;
        }

        public Task<Usuario?> FindByIdAsync(string userId, CancellationToken cancellationToken)
            => _context.Set<Usuario>().FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        public Task<Usuario?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
            => _context.Set<Usuario>().FirstOrDefaultAsync(u => u.UserName.ToUpper() == normalizedUserName, cancellationToken);

        public Task SetPasswordHashAsync(Usuario user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public Task<string?> GetPasswordHashAsync(Usuario user, CancellationToken cancellationToken)
            => Task.FromResult(user.PasswordHash);

        public Task<bool> HasPasswordAsync(Usuario user, CancellationToken cancellationToken)
            => Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));
    }
}
