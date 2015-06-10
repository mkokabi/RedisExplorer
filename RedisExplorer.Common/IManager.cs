using System.Collections.Generic;

namespace RedisExplorer
{
	using RedisExplorer.DataTypes;

	public interface IManager
	{
		void Connect(string redisUrl);

		IReadOnlyCollection<string> GetDatabases();

		IReadOnlyCollection<RedisData> GetData(string database);
	}
}