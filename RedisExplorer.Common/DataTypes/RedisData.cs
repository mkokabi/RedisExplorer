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
	}
}
