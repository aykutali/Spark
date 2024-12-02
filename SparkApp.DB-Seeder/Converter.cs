using System.Data;
using Newtonsoft.Json;

namespace SparkApp.DB_Seeder
{
	public class Converter
	{
		public string DataTableToJSONWithJSONNet(DataTable table)
		{
			string JSONString = string.Empty;
			JSONString = JsonConvert.SerializeObject(table);
			return JSONString;
		}
	}
}
