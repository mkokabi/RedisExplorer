using System.Collections.Generic;

namespace RedisExplorer.Common
{
	using RedisExplorer.Common.DataTypes;

	using StackExchange.Redis;

	/// <summary>
	/// The Manager interface.
	/// </summary>
	public interface IManager
	{
		/// <summary>
		/// Connect to the redis server.
		/// </summary>
		/// <param name="url">
		/// The url.
		/// </param>
		void Connect(string url);

		/// <summary>
		/// Return the databases name.
		/// </summary>
		/// <returns>
		/// The <see cref="IReadOnlyCollection"/>.
		/// </returns>
		IReadOnlyCollection<string> GetDatabases();
		
		/// <summary>
		/// Return the value of selected key in a database
		/// </summary>
		/// <param name="database">
		/// database name.
		/// </param>
		/// <param name="redisType">
		/// the type
		/// </param>
		/// <param name="key">
		/// the key
		/// </param>
		/// <returns>
		/// The value as RedisData
		/// </returns>
		RedisData GetValue(string database, RedisType redisType, string key);

		/// <summary>
		/// Delete key.
		/// </summary>
		/// <param name="database">
		/// The database.
		/// </param>
		/// <param name="key">
		/// The key.
		/// </param>
		void DeleteKey(string database, string key);

		/// <summary>
		/// Get the data from the database.
		/// </summary>
		/// <param name="database">
		/// The database.
		/// </param>
		/// <returns>
		/// The <see cref="IReadOnlyCollection"/>.
		/// </returns>
		IReadOnlyCollection<RedisData> GetKeys(string database);

		/// <summary>
		/// Update database values.
		/// </summary>
		/// <param name="database">
		/// The database.
		/// </param>
		/// <param name="data">
		/// The data.
		/// </param>
		void Update(string database, RedisData data);
	}
}