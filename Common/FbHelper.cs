using System;
using System.Collections.Generic;
using System.Data;
using FirebirdSql.Data.FirebirdClient;

namespace Oper.Sql.Common
{
    public static class FbHelper
    {
        private static readonly Dictionary<Type, FbDbType> typeMap;

        // Create and populate the dictionary in the static constructor
        static FbHelper()
        {
            typeMap = new Dictionary<Type, FbDbType>();

            typeMap[typeof(string)] = FbDbType.VarChar;
            typeMap[typeof(byte)] = FbDbType.SmallInt;
            typeMap[typeof(short)] = FbDbType.SmallInt;
            typeMap[typeof(int)] = FbDbType.Integer;
            typeMap[typeof(long)] = FbDbType.BigInt;
            typeMap[typeof(byte[])] = FbDbType.Binary;
            typeMap[typeof(bool)] = FbDbType.SmallInt;
            typeMap[typeof(DateTime)] = FbDbType.TimeStamp;
            //typeMap[typeof(DateTimeOffset)] = FbDbType.DateTimeOffset;
            typeMap[typeof(decimal)] = FbDbType.Numeric;
            typeMap[typeof(float)] = FbDbType.Float;
            typeMap[typeof(double)] = FbDbType.Double;
            /* ... and so on ... */
        }

        public static string ToDbString(this FbDbType type)
        {
            switch (type)
            {
                case FbDbType.VarChar: return "VARCHAR(512)";
                case FbDbType.SmallInt: return "SMALLINT";
                case FbDbType.Integer: return "INTEGER";
                case FbDbType.BigInt: return "BIGINT";
                case FbDbType.TimeStamp: return "TIMESTAMP";
                case FbDbType.Numeric: return "NUMERIC(18,2)";
                case FbDbType.Float: return "FLOAT";
                case FbDbType.Double: return "DOUBLE PRECISION";
                case FbDbType.Binary: return "BLOB";
                default: return string.Empty;
            }
        }

        // Non-generic argument-based method
        public static FbDbType GetDbType(Type giveType)
        {
            // Allow nullable types to be handled
            giveType = Nullable.GetUnderlyingType(giveType) ?? giveType;

            if (typeMap.ContainsKey(giveType))
            {
                return typeMap[giveType];
            }

            throw new ArgumentException($"{giveType.FullName} is not a supported .NET class");
        }

        // Generic version
        public static FbDbType GetDbType<T>()
        {
            return GetDbType(typeof(T));
        }
    }
}