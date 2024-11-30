using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkApp.Web.ViewModels.User
{
	public class UserAllViewModel
	{
		public string Id { get; set; }

		public string UserName { get; set; }

		public IList<string> Roles { get; set; }

	}
}
