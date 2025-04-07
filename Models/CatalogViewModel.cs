using MoorescnrWebsite.Database;

namespace MoorescnrWebsite.Models
{
	public class CatalogViewModel
	{
		public class GunInfo
		{
			public required int Id { get; set; }
			public required string Name { get; set; }
			public required string Description { get; set; }
			public required bool IsAvailable { get; set; }
			public short? ManufacturedYear { get; set; }
			public double? Price { get; set; }
			public double? SalePrice { get; set; }
			public required int[] ImageIds { get; set; }

			public const int PageCount = 5;
		}

		public required IList<GunInfo> GunInfos { get; set; }

		public required int Page { get; set; }

		public required int NumberOfGuns { get; set; }
	}
}
