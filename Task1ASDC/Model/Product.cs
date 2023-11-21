using System.ComponentModel.DataAnnotations;

namespace Task1ASDC.Model
{
	public class Product
	{
		
		public int ID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Location { get; set; }
		public int Price { get; set; }
		public string Color { get; set; }
	}
}
