// Guids.cs
// MUST match guids.h
using System;

namespace RedisExplorer
{
    static class GuidList
    {
        public const string guidRedisExplorerPkgString = "27da9abe-6c36-4802-a5ef-6cfee788304d";
        public const string guidRedisExplorerCmdSetString = "27bde98c-16b7-407f-9fd6-ba61e1225bdf";
        public const string guidToolWindowPersistanceString = "9db37d7f-adb5-4aed-858f-addeabe2fe41";

        public static readonly Guid guidRedisExplorerCmdSet = new Guid(guidRedisExplorerCmdSetString);
    };
}