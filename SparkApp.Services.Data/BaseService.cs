
using SparkApp.Services.Data.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Web;

namespace SparkApp.Services.Data
{
	public class BaseService : IBaseService
	{
		public bool IsGuidValid(string? id, ref Guid parsedGuid)
		{
			// Non-existing parameter in the URL
			if (String.IsNullOrWhiteSpace(id))
			{
				return false;
			}

			// Invalid parameter in the URL
			bool isGuidValid = Guid.TryParse(id, out parsedGuid);
			if (!isGuidValid)
			{
				return false;
			}

			return true;
		}

		public bool IsModelValid(object obj)
		{
			List<ValidationResult> validationResults = new List<ValidationResult>();

			var context = new ValidationContext(obj);
			var isValid = Validator.TryValidateObject(obj, context, validationResults);

			return isValid;
		}

		public string Sanitize(string input)
		{
			string[] blackListTags = { "script", "iframe", "object", "embed", "form" };
			string[] blackListAttributes = { "onload", "onclick", "onerror", "href", "src" };

			if (string.IsNullOrEmpty(input))
				return string.Empty;

			// Remove blacklisted tags
			foreach (var tag in blackListTags)
			{
				var tagRegex = new Regex($"<\\/?\\s*{tag}\\s*[^>]*>", RegexOptions.IgnoreCase);
				input = tagRegex.Replace(input, string.Empty);
			}

			// Remove blacklisted attributes
			foreach (var attr in blackListAttributes)
			{
				var attrRegex = new Regex($"{attr}\\s*=\\s*['\"].*?['\"]", RegexOptions.IgnoreCase);
				input = attrRegex.Replace(input, string.Empty);
			}

			// Remove javascript: links
			var jsLinkRegex = new Regex(@"href\s*=\s*['""]javascript:[^'""]*['""]", RegexOptions.IgnoreCase);
			input = jsLinkRegex.Replace(input, string.Empty);

			// Remove plain http and https links
			var plainLinkRegex = new Regex(@"(http|https):\/\/[^\s<>]+", RegexOptions.IgnoreCase);
			input = plainLinkRegex.Replace(input, string.Empty);

			// Encode any remaining HTML
			return HttpUtility.HtmlEncode(input);
		}
	}
}
