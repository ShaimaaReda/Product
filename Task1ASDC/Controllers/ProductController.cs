using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task1ASDC.Database.UploadExcel.WebApi;
using Task1ASDC.Model;

namespace Task1ASDC.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public ProductController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
		{
			_context = context;
			_webHostEnvironment = webHostEnvironment;
		}

		[HttpPost("upload")]
		public async Task<ActionResult> Upload(CancellationToken ct)
		
		{
			if (Request.Form.Files.Count == 0) return NoContent();

			var file = Request.Form.Files[0];
			var filePath = SaveFile(file);

			// load product requests from excel file
			var productRequests = UploadController.Import<ProductRequest>(filePath);

			// save product requests to database
			foreach (var productRequest in productRequests)
			{
				var product = new Product
				{
					//ID = Guid.NewGuid(),
					Name = productRequest.Name,
					Description = productRequest.Description,
					Location = productRequest.Location,
					Price = productRequest.Price,
					Color = productRequest.Color
					
				};
				await _context.AddAsync(product, ct);
			}
			await _context.SaveChangesAsync(ct);

			return Ok();
		}

		// save the uploaded file into wwwroot/uploads folder
		private string SaveFile(IFormFile file)
		{
			if (file.Length == 0)
			{
				throw new BadHttpRequestException("File is empty.");
			}

			var extension = Path.GetExtension(file.FileName);

			var webRootPath = _webHostEnvironment.WebRootPath;
			if (string.IsNullOrWhiteSpace(webRootPath))
			{
				webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
			}

			var folderPath = Path.Combine(webRootPath, "uploads");
			if (!Directory.Exists(folderPath))
			{
				Directory.CreateDirectory(folderPath);
			}

			var fileName = $"{Guid.NewGuid()}.{extension}";
			var filePath = Path.Combine(folderPath, fileName);
			using var stream = new FileStream(filePath, FileMode.Create);
			file.CopyTo(stream);

			return filePath;
		}

		[HttpGet("Id")]
		public ActionResult Getbyid(int Id)
		{
			Product pro = _context.products.FirstOrDefault(s => s.ID == Id);

			if (pro == null)
			{
				return NotFound("notfound");
			}

			return Ok(pro);
		}

		[HttpGet]
		public IActionResult GetAll()
		{
			var AllPro = _context.products.Select(x=> new  Product()
			{
				ID=x.ID,
				Name=x.Name,
				Description=x.Description,
				Location = x.Location,
				Price = x.Price,
				Color =x.Color
			}).ToList();

			return Ok(AllPro);
		}

	}
}

