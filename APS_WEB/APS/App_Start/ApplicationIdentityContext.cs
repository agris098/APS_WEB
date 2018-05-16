namespace APS
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using AspNet.Identity.MongoDB;
	using Models;
	using MongoDB.Driver;
    using System.Configuration;

    public class ApplicationIdentityContext : IDisposable
	{
		public static ApplicationIdentityContext Create()
		{
            // todo add settings where appropriate to switch server & database in your own application
            var _connectionString = ConfigurationManager.ConnectionStrings["MongoDB"].ToString();
            var _databaseName = MongoUrl.Create(_connectionString).DatabaseName;
            var database = new MongoClient(_connectionString).GetDatabase(_databaseName);
			var users = database.GetCollection<ApplicationUser>("users");
			var roles = database.GetCollection<IdentityRole>("roles");
			return new ApplicationIdentityContext(users, roles);
		}

		private ApplicationIdentityContext(IMongoCollection<ApplicationUser> users, IMongoCollection<IdentityRole> roles)
		{
			Users = users;
			Roles = roles;
		}

		public IMongoCollection<IdentityRole> Roles { get; set; }

		public IMongoCollection<ApplicationUser> Users { get; set; }

		public Task<List<IdentityRole>> AllRolesAsync()
		{
			return Roles.Find(r => true).ToListAsync();
		}

		public void Dispose()
		{
		}
	}
}