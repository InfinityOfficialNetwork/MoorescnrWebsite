using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MoorescnrWebsite.Database
{
	public class ImageEntity
	{
		[Key]
		public int Id { get; set; }

		[Required]
		required public byte[] Data { get; set; }

		[Required]
		required public int GunId { get; set; }

		[Required]
		[Comment("The order which the pictures are displayed")]
		required public int Order { get; set; }
	}
}
