namespace RedisExplorer.Common
{
	using System.Collections.Generic;

	using RedisExplorer.Common.DataTypes;
	using RedisExplorer.DataTypes;

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
		/// Get the data from the database.
		/// </summary>
		/// <param name="database">
		/// The database.
		/// </param>
		/// <returns>
		/// The <see cref="IReadOnlyCollection"/>.
		/// </returns>
		IReadOnlyCollection<RedisData> GetData(string database);

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