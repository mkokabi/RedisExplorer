using System;
using System.Collections.Generic;
using System.Linq;

namespace RedisExplorer.DataTypes
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

		public IEnumerable<SortedSetEntry> SortedSet
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

		public RedisData(string key, RedisValue[] values, RedisType redisType = RedisType.List)
			: this(key, redisType)
		{
			this.Values = values;
		}

		public RedisData(string key, HashEntry[] hash)
			: this(key, RedisType.Hash)
		{
			this.Hash = hash;
		}

		public RedisData(string key, IEnumerable<SortedSetEntry> sortedSet)
			: this(key, RedisType.SortedSet)
		{
			this.SortedSet = sortedSet;
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
				switch (Type)
				{
					case RedisType.String:
						{
							return this.Value;
						}
					case RedisType.List:
					case RedisType.Set:
						{
							return String.Join(",", this.Values.ToStringArray());
						}
					case RedisType.Hash:
						{
							return String.Join(",", this.Hash.Select(item => string.Format("{0}:{1}", item.Name, item.Value)).ToArray());
						}
					case RedisType.SortedSet:
						{
							return String.Join(",", this.SortedSet.Select(item => string.Format("{0}:{1}", item.Element, item.Score)).ToArray());
						}
				}
				return base.ToString();
			}
		}
	}
}
