using MoorescnrWebsite.Database;

namespace MoorescnrWebsite.Models
{
	public class CatalogViewModel
	{


		public required IList<GunInfo> GunInfos { get; set; }

		public required int Page { get; set; }

		public required int NumberOfGuns { get; set; }
	}
}
