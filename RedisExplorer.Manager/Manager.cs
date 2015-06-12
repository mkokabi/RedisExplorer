using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace RedisExplorer
{
	using RedisExplorer.DataTypes;

	using StackExchange.Redis;
	
	public class Manager : IManager
	{
		#region private fields
		ConnectionMultiplexer redisConnection;

		IServer redisServer;

		string redisUrl;

		string[] databases;
		#endregion
		
		public void Connect(string redisUrl)
		{
			this.redisUrl = redisUrl;
			redisConnection = ConnectionMultiplexer.Connect(this.redisUrl + ",allowAdmin=true");
			EndPoint[] endpoints = redisConnection.GetEndPoints();
			redisServer = redisConnection.GetServer(endpoints[0]);
			IGrouping<string, KeyValuePair<string, string>>[] infos = redisServer.Info();
			var keyspace = infos.FirstOrDefault(info => info.Key == "Keyspace");
			var dbsKeyspaceInfo = keyspace.Where(info => info.Key.StartsWith("db"));
			databases = dbsKeyspaceInfo.Select(db => db.Key).ToArray();
		}

		public IReadOnlyCollection<string> GetDatabases()
		{
			return databases.ToList().AsReadOnly();
		}

		public IReadOnlyCollection<RedisData> GetData(string database)
		{
			IDatabase redisDatabase = redisConnection.GetDatabase(int.Parse(database.Replace("db", string.Empty)));

			IEnumerable<RedisKey> keys = redisServer.Keys(pageSize: 10, database: redisDatabase.Database);
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

		public void Update(string database, RedisData data)
		{
			IDatabase redisDatabase = redisConnection.GetDatabase(int.Parse(database.Replace("db", string.Empty)));
			switch (data.Type)
			{
				case RedisType.String:
					{
						redisDatabase.StringSet(data.Key, data.Text);
						break;
					}
			}
		}
	}
}
