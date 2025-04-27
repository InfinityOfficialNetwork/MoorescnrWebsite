using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Reflection;
using System.Text;

namespace MoorescnrWebsite.TagHelpers
{
	[HtmlTargetElement("include", TagStructure = TagStructure.NormalOrSelfClosing)]
	public class IncludeTagHelper : TagHelper
	{
		private readonly static IDictionary<string,string> cache = new Dictionary<string,string>();

		[HtmlAttributeName("tag")]
		public required string Tag { get; set; }
		[HtmlAttributeName("src")]
		public required string Src { get; set; }
		[HtmlAttributeName("optional")]
		public bool Optional { get; set; } = false;


		public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
		{
			if (cache.TryGetValue($"{Tag};{Src}", out string? content))
			{
				output.TagName = "";
				output.TagMode = TagMode.StartTagAndEndTag;
				output.PreContent.SetHtmlContent($"<{Tag}>");
				output.Content.SetHtmlContent(content);
				output.PostContent.SetHtmlContent($"</{Tag}>");
				return;
			}
			else
			{
				var assembly = Assembly.GetExecutingAssembly();
				using (var stream = assembly.GetManifestResourceStream(Src))
				{
					if (stream != null)
					{
						using (var reader = new StreamReader(stream, Encoding.UTF8))
						{
							content = await reader.ReadToEndAsync();
							cache.Add($"{Tag};{Src}", content);
							output.TagName = "";
							output.TagMode = TagMode.StartTagAndEndTag;
							output.PreContent.SetHtmlContent($"<{Tag}>");
							output.Content.SetHtmlContent(content);
							output.PostContent.SetHtmlContent($"</{Tag}>");
						}
						return;
					}
					else
					{
						if (!Optional)	
							throw new ArgumentException($"Embedded resource {Src} not found", nameof(Src));
						else
						{
							output.SuppressOutput();
							return;
						}
					}
				}
			}

		}
	}
}