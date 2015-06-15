using System.Collections.Generic;

namespace RedisExplorer.Common.DataTypes
{
	using StackExchange.Redis;

	/// <summary>
	/// Redis data.
	/// </summary>
	public class RedisData 
	{
		/// <summary>
		/// The redis data key.
		/// </summary>
		public string Key
		{
			get;
			set;
		}

		/// <summary>
		/// Redis data single value.
		/// </summary>
		public RedisValue Value
		{
			get;
			set;
		}

		/// <summary>
		/// Redis data list values.
		/// </summary>
		public RedisValue[] Values
		{
			get;
			set;
		}

		/// <summary>
		/// Redis data hash values.
		/// </summary>
		public HashEntry[] Hash
		{
			get;
			set;
		}

		/// <summary>
		/// Redis data sorted set.
		/// </summary>
		public IEnumerable<SortedSetEntry> SortedSet
		{
			get;
			set;
		}

		/// <summary>
		/// Redis data type.
		/// </summary>
		public RedisType Type
		{
			get;
			set;
		}

		/// <summary>
		/// Default constructor
		/// </summary>
		public RedisData()
		{
		}

		private RedisData(string key, RedisType type)
		{
			this.Key = key;
			this.Type = type;
		}

		/// <summary>
		/// Initialises a new instance of the <see cref="RedisData"/> class.
		/// </summary>
		/// <param name="key">
		/// The key.
		/// </param>
		/// <param name="value">
		/// The value.
		/// </param>
		public RedisData(string key, RedisValue value)
			: this(key, RedisType.String)
		{
			this.Value = value;
		}

		/// <summary>
		/// Initialises a new instance of the <see cref="RedisData"/> class including list.
		/// </summary>
		/// <param name="key">
		/// The key.
		/// </param>
		/// <param name="values">
		/// The values.
		/// </param>
		/// <param name="redisType">
		/// The redis type.
		/// </param>
		public RedisData(string key, RedisValue[] values, RedisType redisType = RedisType.List)
			: this(key, redisType)
		{
			this.Values = values;
		}

		/// <summary>
		/// Initialises a new instance of the <see cref="RedisData"/> class including hash.
		/// </summary>
		/// <param name="key">
		/// The key.
		/// </param>
		/// <param name="hash">
		/// The hash.
		/// </param>
		public RedisData(string key, HashEntry[] hash)
			: this(key, RedisType.Hash)
		{
			this.Hash = hash;
		}

		/// <summary>
		/// Initialises a new instance of the <see cref="RedisData"/> class including sorted set.
		/// </summary>
		/// <param name="key">
		/// The key.
		/// </param>
		/// <param name="sortedSet">
		/// The sorted set.
		/// </param>
		public RedisData(string key, IEnumerable<SortedSetEntry> sortedSet)
			: this(key, RedisType.SortedSet)
		{
			this.SortedSet = sortedSet;
		}
	}
}
