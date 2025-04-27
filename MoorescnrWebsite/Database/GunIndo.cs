namespace MoorescnrWebsite.Database
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
}
