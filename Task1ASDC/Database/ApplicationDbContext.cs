using Microsoft.EntityFrameworkCore;
using Task1ASDC.Model;

namespace Task1ASDC.Database
{
	namespace UploadExcel.WebApi
	{
		public class ApplicationDbContext : DbContext
		{

			public ApplicationDbContext(DbContextOptions option) : base(option)
			{

			}
			public DbSet<Product> products { get; set; }
		}
	}
}
