using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Sql.Olap.Models;

namespace Sql.Olap.Common
{
    public class InParametersBase
    {
        #region Ctor

        public InParametersBase()
        {
        }

        #endregion

        #region Fields

        private readonly Dictionary<string, object> defaultValues = new Dictionary<string, object>();
        public readonly Dictionary<string, string> WhereClauses = new Dictionary<string, string>();

        #endregion

        #region Properties

        public bool Recalc { get; set; }
        public STDMTypeList STDM { get; set; }

        [Filter(0)]
        public DateTime StartDate { get; set; }
        [Filter(1)]
        public DateTime EndDate { get; set; }
        [Filter(2)]
        public string Firm { get; set; }

        private string hash;
        public string Hash
        {
            get
            {
                if (hash == null)
                {
                    var hashValues = new Dictionary<string, string>();
                    var properties = ReflectionHelper.GetPropertiesWithAttribute(GetType(), typeof(HashAttribute)).ToList();
                    foreach (var info in properties)
                    {
                        var value = info.GetValue(this, null);
                        hashValues.Add(info.Name, value.ToString());
                    }
                    hash = JsonConvert.SerializeObject(hashValues);
                }
                return hash;
            }
        }

        #endregion

        #region Methods

        public void InitFilterParameter(string key, object defaultValue, string whereClause)
        {
            AddDefaultValue(key, defaultValue);
            AddWhereClause(key, whereClause);
        }

        public void AddDefaultValue(string key, object value)
        {
            defaultValues.Add(key, value);
        }

        public object GetDefaultValue(string key)
        {
            return defaultValues[key];
        }

        public bool CheckExistsDefaultValue(string key)
        {
            return defaultValues.ContainsKey(key);
        }

        public void AddWhereClause(string key, string value)
        {
            WhereClauses.Add(key, value);
        }

        public bool CheckExistsWhereClause(string key)
        {
            return WhereClauses.ContainsKey(key);
        }

        //private byte[] GetHash(string inputString)
        //{
        //    HashAlgorithm algorithm = SHA256.Create();
        //    return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        //}

        //public string GetHashString()
        //{
        //    string inputString = $"{TimeStamp ?? String.Empty}|{TelegramName ?? String.Empty}|{Section ?? String.Empty}|{Floor ?? String.Empty}|{Apartment ?? String.Empty}|{Phone ?? String.Empty}|{AdditionalPhone ?? String.Empty}";
        //    StringBuilder sb = new StringBuilder();
        //    foreach (byte b in GetHash(inputString))
        //        sb.Append(b.ToString("X2"));
        //    return sb.ToString();
        //}

        #endregion
    }
}