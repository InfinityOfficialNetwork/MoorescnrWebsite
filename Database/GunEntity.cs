using System.ComponentModel.DataAnnotations;

namespace MoorescnrWebsite.Database
{
	public class GunEntity
	{
		[Key]
		public required int Id { get; set; }

		[Required]
		public required string Name { get; set; }

		[Required]
		public required string Description { get; set; }

		[Required]
		public required bool IsAvailable { get; set; }

		public short ManufacturedYear { get; set; }

		public double Price { get; set; }

		public double SalePrice { get; set; }

		public override string ToString()
		{
			return $"Id: {Id}\nName: {Name}\nDescription: {Description}\nIsAvailable: {IsAvailable}\nManufacturedDate: {ManufacturedYear}\nPrice: {Price}\nSalePrice: {SalePrice}";
		}
	}
}
