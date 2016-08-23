using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Task2.Common.Helpers
{
    public static class DataExtensions
    {
        public static List<T> MapToList<T>(this DbDataReader dr) where T : new()
        {
            if (dr == null || !dr.HasRows)
                return null;

            var entity = typeof(T);
            var entities = new List<T>();
            var props = entity.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var propDict = props.ToDictionary(p => p.Name.ToUpper(), p => p);

            while (dr.Read())
            {
                var newObject = new T();

                for (var index = 0; index < dr.FieldCount; index++)
                {
                    if (!propDict.ContainsKey(dr.GetName(index).ToUpper()))
                        continue;

                    var info = propDict[dr.GetName(index).ToUpper()];

                    if ((info == null) || !info.CanWrite)
                        continue;

                    var val = dr.GetValue(index);

                    info.SetValue(newObject, (val == DBNull.Value) ? null : val, null);

                }

                entities.Add(newObject);
            }

            return entities;
        }
    }
}
