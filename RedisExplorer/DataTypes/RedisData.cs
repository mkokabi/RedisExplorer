using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DimensionData.RedisExplorer
{
	using StackExchange.Redis;

	public class RedisData
	{
		public string Key
		{
			get;
			set;
		}

		public RedisValue Value
		{
			get;
			set;
		}

		public RedisValue[] Values
		{
			get;
			set;
		}

		public HashEntry[] Hash
		{
			get;
			set;
		}

		public RedisType Type
		{
			get;
			set;
		}

		public RedisData()
		{
		}

		private RedisData(string key, RedisType type)
		{
			this.Key = key;
			this.Type = type;
		}

		public RedisData(string key, RedisValue value)
			: this(key, RedisType.String)
		{
			this.Value = value;
		}

		public RedisData(string key, RedisValue[] values)
			: this(key, RedisType.List)
		{
			this.Values = values;
		}

		public RedisData(string key, HashEntry[] hash)
			: this(key, RedisType.Hash)
		{
			this.Hash = hash;
		}

		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>
		/// A string that represents the current object.
		/// </returns>
		public override string ToString()
		{
			return Text;
		}

		public string Text
		{
			get
			{
				if (Type == RedisType.String)
				{
					return this.Value;
				}
				if (Type == RedisType.List)
				{
					return String.Join(",", this.Values.ToStringArray());
				}
				if (Type == RedisType.Hash)
				{
					return String.Join(",", this.Hash.Select(item => string.Format("{0}:{1}", item.Name, item.Value)).ToArray());
				}
				return base.ToString();
			}
		}
	}
}
