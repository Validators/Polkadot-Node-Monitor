using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Validators.IO.Polkadot.Monitor.Authentication
{
	public class InMemoryUserStore : IUserStore<IdentityUser>, IUserPasswordStore<IdentityUser>
	{
		private readonly AppSettings settings;

		public InMemoryUserStore(IOptions<AppSettings> settings)
		{
			this.settings = settings.Value;
		}
		public Task<IdentityResult> CreateAsync(IdentityUser user, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public Task<IdentityResult> DeleteAsync(IdentityUser user, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public void Dispose()
		{
		}

		public Task<IdentityUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
		{
			return Task.FromResult(new IdentityUser("admin"));
		}

		public Task<IdentityUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
		{
			return Task.FromResult(new IdentityUser("admin"));
		}

		public Task<string> GetNormalizedUserNameAsync(IdentityUser user, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public Task<string> GetPasswordHashAsync(IdentityUser user, CancellationToken cancellationToken)
		{
			var hasher = new PasswordHasher<IdentityUser>();
			var identityuser = new IdentityUser("admin");

			var hashedPassword = hasher.HashPassword(identityuser, settings.AdminPassword);

			return Task.FromResult<string>(hashedPassword);
		}

		public Task<string> GetUserIdAsync(IdentityUser user, CancellationToken cancellationToken)
		{
			return Task.FromResult<string>("admin");
		}

		public Task<string> GetUserNameAsync(IdentityUser user, CancellationToken cancellationToken)
		{
			return Task.FromResult<string>("admin");
		}

		public Task<bool> HasPasswordAsync(IdentityUser user, CancellationToken cancellationToken)
		{
			return Task.FromResult<bool>(true);
		}

		public Task SetNormalizedUserNameAsync(IdentityUser user, string normalizedName, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public Task SetPasswordHashAsync(IdentityUser user, string passwordHash, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public Task SetUserNameAsync(IdentityUser user, string userName, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public Task<IdentityResult> UpdateAsync(IdentityUser user, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}
