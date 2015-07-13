using System.Collections.Generic;
using System.Linq;

namespace RedisExplorer.Common.DataTypes
{
	using StackExchange.Redis;

	/// <summary>
	/// Redis data.
	/// </summary>
	public class RedisData 
	{
		readonly bool loaded;

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
		public IList<RedisValue> Values
		{
			get;
			set;
		}

		/// <summary>
		/// Redis data hash values.
		/// </summary>
		public IList<HashEntry> Hash
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
		/// A flag indicating whether the value is loaded.
		/// </summary>
		public bool Loaded
		{
			get
			{
				return this.loaded;
			}
		}

		/// <summary>
		/// Default constructor
		/// </summary>
		public RedisData()
		{
			this.loaded = false;
		}

		/// <summary>
		/// Initialises a new instance of the <see cref="RedisData"/> class.
		/// </summary>
		/// <param name="key">
		/// The key.
		/// </param>
		/// <param name="type">
		/// The type.
		/// </param>
		public RedisData(string key, RedisType type)
		{
			this.Key = key;
			this.Type = type;
			switch (this.Type)
			{
				case RedisType.String:
					{
						this.Value = default(string);
						break;
					}
				case RedisType.List:
					{
						this.Values = new RedisValue[0].ToList();
						break;
					}
				case RedisType.Hash:
					{
						this.Hash = new HashEntry[0];
						break;
					}
				case RedisType.Set:
					{
						this.Values = new RedisValue[0].ToList();
						break;
					}
				case RedisType.SortedSet:
					{
						this.SortedSet = new List<SortedSetEntry>();
						break;
					}
			}
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
			this.loaded = true;
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
			this.Values = values.ToList();
			this.loaded = true;
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
			this.loaded = true;
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
			this.loaded = true;
		}
	}
}
