using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DimensionData.RedisExplorer
{
	public class RedisData
	{
		public string Key
		{
			get;
			set;
		}

		public string Value
		{
			get;
			set;
		}

		public RedisData()
		{
		}

		public RedisData(string key, string value)
		{
			this.Key = key;
			this.Value = value;
		}
	}
}
