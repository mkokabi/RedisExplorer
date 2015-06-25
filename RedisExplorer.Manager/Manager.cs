using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

// ReSharper disable CSharpWarnings::CS1584
namespace RedisExplorer.Manager
{
	using RedisExplorer.Common;
	using RedisExplorer.Common.DataTypes;
	using RedisExplorer.DataTypes;

	using StackExchange.Redis;

	/// <summary>
	/// RedisExplorer manager.
	/// </summary>
	public class Manager : IManager
	{
		#region private fields
		ConnectionMultiplexer redisConnection;

		IServer redisServer;

		string redisUrl;

		string[] databases;
		#endregion

		/// <summary>
		/// Connect to the redis server.
		/// </summary>
		/// <param name="url">
		/// Redis server url.
		/// </param>
		public void Connect(string url)
		{
			if (url == null)
			{
				throw new ArgumentNullException("url");
			}
			this.redisUrl = url;
			this.redisConnection = ConnectionMultiplexer.Connect(this.redisUrl + ",allowAdmin=true");
			EndPoint[] endpoints = this.redisConnection.GetEndPoints();
			this.redisServer = this.redisConnection.GetServer(endpoints[0]);
			IGrouping<string, KeyValuePair<string, string>>[] infos = this.redisServer.Info();
			var keyspace = infos.FirstOrDefault(info => info.Key == "Keyspace");
			if (keyspace != null)
			{
				var dbsKeyspaceInfo = keyspace.Where(info => info.Key.StartsWith("db"));
				this.databases = dbsKeyspaceInfo.Select(db => db.Key).ToArray();
			}
		}

		/// <summary>
		/// Return the databases name.
		/// </summary>
		/// <returns>
		/// The <see cref="IReadOnlyCollection"/>.
		/// </returns>
		public IReadOnlyCollection<string> GetDatabases()
		{
			return this.databases.ToList().AsReadOnly();
		}

		/// <summary>
		/// Get the data from the database.
		/// </summary>
		/// <param name="database">
		/// database name.
		/// </param>
		/// <returns>
		/// The <see cref="IReadOnlyCollection"/>.
		/// </returns>
		public IReadOnlyCollection<RedisData> GetData(string database)
		{
			if (database == null)
			{
				throw new ArgumentNullException("database");
			}
			IDatabase redisDatabase = this.redisConnection.GetDatabase(int.Parse(database.Replace("db", string.Empty)));

			IEnumerable<RedisKey> keys = this.redisServer.Keys(pageSize: 10, database: redisDatabase.Database);
			RedisDataCollection KeyValueCollection = new RedisDataCollection();
			keys.ToList().ForEach(key =>
				{
					try
					{
						switch (redisDatabase.KeyType(key))
						{
							case RedisType.String:
								{
									KeyValueCollection.Add(new RedisData(key, redisDatabase.StringGet(key)));
									break;
								}
							case RedisType.List:
								{
									KeyValueCollection.Add(new RedisData(key, redisDatabase.ListRange(key)));
									break;
								}
							case RedisType.Hash:
								{
									KeyValueCollection.Add(new RedisData(key, redisDatabase.HashGetAll(key)));
									break;
								}
							case RedisType.Set:
								{
									KeyValueCollection.Add(new RedisData(key, redisDatabase.SetMembers(key), RedisType.Set));
									break;
								}
							case RedisType.SortedSet:
								{
									KeyValueCollection.Add(new RedisData(key, redisDatabase.SortedSetScan(key)));
									break;
								}
						}
					}
					catch (Exception ex)
					{
						KeyValueCollection.Add(new RedisData(key, ex.Message));
					}
				});
			return KeyValueCollection;
		}

		/// <summary>
		/// Update database values.
		/// </summary>
		/// <param name="database">
		/// Database name.
		/// </param>
		/// <param name="data">
		/// Redis data.
		/// </param>
		// TODO: Not completed
		public void Update(string database, RedisData data)
		{
			if (database == null)
			{
				throw new ArgumentNullException("database");
			}
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			IDatabase redisDatabase = this.redisConnection.GetDatabase(int.Parse(database.Replace("db", string.Empty)));
			switch (data.Type)
			{
				case RedisType.String:
					{
						redisDatabase.StringSet(data.Key, data.Value);
						break;
					}
				case RedisType.List:
					{
						var list = redisDatabase.ListRange(data.Key);
						int index = 0;
						list.ToList().ForEach(value => redisDatabase.ListSetByIndex(data.Key, index, data.Values[index++]));
						break;
					}
				case RedisType.Hash:
					{
						redisDatabase.KeyDelete(data.Key);
						data.Hash.ToList().ForEach(entry => redisDatabase.HashSet(data.Key, data.Hash));
						break;
					}
			}
		}
	}
	// ReSharper restore CSharpWarnings::CS1584
}
