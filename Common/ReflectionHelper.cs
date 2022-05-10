using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Sql.Olap.Common
{
    /// <summary>
    /// Вспомогательный класс для задач Reflection.
    /// </summary>
    public class ReflectionHelper
    {
        /// <summary>
        /// Gets the value of the member with the specified name.
        /// </summary>
        /// <param name="sourceObject">An object instance.</param>
        /// <param name="memberName">The member name. Could be separated by "." to access internal members: "ParentObject.ChildObject.ChildObjectProperty"</param>
        /// <returns>The member value.</returns>
        public static object GetMemberValue(object sourceObject, string memberName)
        {
            string[] propertyChain = memberName.Split('.');
            object currentObject = sourceObject;
            foreach (string propertyName in propertyChain)
            {
                PropertyInfo property;
                currentObject = GetPropertyValue(currentObject, propertyName, out property);
            }
            return currentObject;
        }

        /// <summary>
        /// Gets the value of the property with the specified name.
        /// </summary>
        /// <param name="sourceObject">An object instance.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="property">A reference to the <see cref="PropertyInfo"/> object.</param>
        /// <returns>The property value.</returns>
        public static object GetPropertyValue(object sourceObject, string propertyName, out PropertyInfo property)
        {
            object value = null;
            property = GetPropertyInfo(sourceObject, propertyName);

            if (property != null && property.CanRead)
            {
                value = property.GetValue(sourceObject, null);
            }

            return value;
        }

        /// <summary>
        /// Gets a <see cref="PropertyInfo"/> object representing the property
        /// belonging to the object having the specified name.
        /// </summary>
        /// <param name="sourceObject">An object instance.</param>
        /// <param name="propertyName">The property name.</param>
        /// <returns>A <see cref="PropertyInfo"/> object, or null if the object
        /// instance does not have a property with the specified name.</returns>
        public static PropertyInfo GetPropertyInfo(object sourceObject, string propertyName)
        {
            PropertyInfo prop = null;

            if (sourceObject != null)
            {
                prop = GetPropertyInfo(sourceObject.GetType(), propertyName);
            }

            return prop;
        }

        /// <summary>
        /// Gets a <see cref="PropertyInfo"/> object representing the property
        /// belonging to the runtime type having the specified name.
        /// </summary>
        /// <param name="type">The runtime type.</param>
        /// <param name="propertyName">The property name.</param>
        /// <returns>A <see cref="PropertyInfo"/> object, or null if the runtime
        /// type does not have a property with the specified name.</returns>
        public static PropertyInfo GetPropertyInfo(Type type, String propertyName)
        {
            PropertyInfo prop = null;

            if (type != null && !String.IsNullOrEmpty(propertyName))
            {
                prop = type.GetProperty(propertyName);
            }

            return prop;
        }

        /// <summary>
        /// Пытается присвоить <paramref name="value"/> в свойство <paramref name="propertyName"/> для объекта <paramref name="destinationObject"/>/>
        /// </summary>
        /// <param name="destinationObject"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns>True, если присвоение удалось. False в противном случае.</returns>
        public static bool SetPropertyValue(object destinationObject, string propertyName, object value)
        {
            PropertyInfo propertyInfo = GetPropertyInfo(destinationObject, propertyName);
            if (propertyInfo == null ||
                !propertyInfo.CanWrite ||
                (value != null && !propertyInfo.PropertyType.IsInstanceOfType(value)) ||
                (value == null && !IsTypeNullable(propertyInfo.PropertyType)))
                return false;
            propertyInfo.SetValue(destinationObject, value, null); // value == null ? null : value - данная махинация нужна для того, чтобы установился null нужного типа
            return true;
        }

        public static string GetPropertyNameFromExpression<T>(Expression<Func<T>> property)
        {
            var lambda = (LambdaExpression)property;
            MemberExpression memberExpression;

            if (lambda.Body is UnaryExpression)
            {
                var unaryExpression = (UnaryExpression)lambda.Body;
                memberExpression = (MemberExpression)unaryExpression.Operand;
            }
            else
            {
                memberExpression = (MemberExpression)lambda.Body;
            }
            return memberExpression.Member.Name;
        }

        public static IList<KeyValuePair<string, object>> GetObjectsChainFromExpression<T>(object rootObject, Expression<Func<T>> expression)
        {
            var result = new List<KeyValuePair<string, object>>();
            var names = GetNamesFromExpression(expression);
            string currentFullName = "";
            var keyValuePair = new KeyValuePair<string, object>(names.First(), rootObject);
            result.Add(keyValuePair);
            foreach (string name in names.Skip(1))
            {
                currentFullName += (currentFullName == "" ? "" : ".") + name;
                keyValuePair = new KeyValuePair<string, object>(name, GetMemberValue(rootObject, currentFullName));
                result.Add(keyValuePair);
            }
            return result;
        }

        public static string GetNamesStringFromExpression<T>(Expression<Func<T>> expression)
        {
            return String.Join(".", GetNamesFromExpression(expression));
        }

        public static IEnumerable<string> GetNamesFromExpression<T>(Expression<Func<T>> expression)
        {
            var lambda = (LambdaExpression)expression;
            MemberExpression memberExpression;

            if (lambda.Body is UnaryExpression)
            {
                var unaryExpression = (UnaryExpression)lambda.Body;
                memberExpression = (MemberExpression)unaryExpression.Operand;
            }
            else
            {
                memberExpression = (MemberExpression)lambda.Body;
            }
            var result = new List<string>();
            Expression currentExpression = memberExpression;
            while (currentExpression is MemberExpression)
            {
                string memberName = ((MemberExpression)currentExpression).Member.Name;
                result.Insert(0, memberName);
                currentExpression = ((MemberExpression)currentExpression).Expression;
            }
            return result;
        }

        public static bool IsTypeNullable(Type type)
        {
            return (!type.IsValueType || Nullable.GetUnderlyingType(type) != null);
        }

        /// <summary>
        /// GetPath helps to get path to object via lambda expressions.
        /// Example: 
        /// RaisePropertyChanged(PropertyHelper.GetPath&lt;SomeClass&gt;(e => e.Name)
        /// Grabbed from VI.Mvvm
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression">Lambda expression.</param>
        /// <returns></returns>
        public static string GetPath<T>(Expression<Func<T, object>> expression)
        {
            string result = "";
            Expression expr = expression.Body;
            while (true)
            {
                if (expr is ParameterExpression)
                    return result;
                if (expr is UnaryExpression)
                    expr = (expr as UnaryExpression).Operand;
                else if (expr is MemberExpression)
                {
                    var me = expr as MemberExpression;
                    var pi = me.Member as PropertyInfo;
                    result = pi.Name + (String.IsNullOrEmpty(result) ? "" : ("." + result));
                    expr = me.Expression;
                }
                else if (expr is MethodCallExpression)
                {
                    var methodCallExpession = (MethodCallExpression)expr;
                    return methodCallExpession.Method.Name;
                }
                else
                    throw new ArgumentException(String.Format("Can not use expression '{0}' as property path. Check all path elements to be properties.", expression), "expression");
            }
        }

        /// <summary>
        /// Возвращает массив интерфейсов, которые реализуются именно заданным типом <see cref="type"/>, но не его предками
        /// </summary>
        public static Type[] GetThisTypeInterfaces(Type type)
        {
            Type[] allInterfaces = type.GetInterfaces();
            if (allInterfaces.Length == 0 || type.BaseType == null) return allInterfaces;
            return allInterfaces.Where(intf => !intf.IsAssignableFrom(type.BaseType)).ToArray();
        }

        /// <summary>
        /// Возвращает коллекцию PropertyInfo для свойств объекта типа <see cref="objectType"/>, которые помечены
        /// атрибутом <see cref="attributeType"/>.
        /// Учитываются также интерфейсы, которые реализует этот объект. Если в интерфейсе свойство помечено 
        /// атрибутом <see cref="attributeType"/>, то оно тоже попадет в возвращаемый результат.
        /// </summary>
        public static IEnumerable<PropertyInfo> GetPropertiesWithAttribute(Type objectType, Type attributeType)
        {
            IEnumerable<Type> objectTypeAndInterfaces = objectType.GetInterfaces().Union(new[] { objectType });
            return objectTypeAndInterfaces
                .SelectMany(typeOrInterface =>
                    typeOrInterface.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .Where(p => Attribute.GetCustomAttributes(p, attributeType, true).Any()));
        }

        /// <summary>
        /// Возвращает коллекцию PropertyInfo для свойств объекта <see cref="obj"/>, которые помечены
        /// атрибутом <see cref="attributeType"/>.
        /// Учитываются также интерфейсы, которые реализует этот объект. Если в интерфейсе свойство помечено 
        /// атрибутом <see cref="attributeType"/>, то оно тоже попадет в возвращаемый результат.
        /// </summary>
        public static IEnumerable<PropertyInfo> GetPropertiesWithAttribute(object obj, Type attributeType)
        {
            return GetPropertiesWithAttribute(obj.GetType(), attributeType);
        }

        #region Moved from VI.Mvvm.PropertyHelper
        // TODO: Объединить функциональность этих методов с функциональностью оригинальных

        private const string indexedPropertyName = "Item";
        private const char propertySeparator = '.';

        /// <summary>
        /// Возвращает значение свойства объекта по заданному пути
        /// </summary>
        /// <param name="source">Исходный объект</param>
        /// <param name="propertyPathName">
        /// Путь к искомому свойству, например, "Property1.SomeCollectionProperty[0].Name"
        /// </param>
        /// <returns>Значение свойства объекта по заданному пути</returns>
        public static object GetPropertyValue(object source, string propertyPathName)
        {
            if (propertyPathName == null)
                throw new ArgumentNullException("propertyPathName");

            if (propertyPathName.Equals(Char.ToString(propertySeparator)))
                return source;

            string[] propertyPathParts = propertyPathName.Split(propertySeparator);
            var properties = new List<PropertyData>();
            foreach (string propertyPathPart in propertyPathParts)
            {
                IEnumerable<PropertyData> partProperties = ParseProperty(propertyPathPart);
                properties.AddRange(partProperties);
            }
            return properties.Aggregate(source, GetSimplePropertyValue);
        }

        /// <summary>
        /// Разбирает свойство в список структур <see cref="PropertyData"/>. Если свойство обычное,
        /// то список содержит только один элемент. Несколько элементов появляются в том случае,
        /// если свойство является индексным.
        /// </summary>
        /// <param name="property">Имя свойства, возможно с индексом</param>
        /// <returns>
        /// Список структур <see cref="PropertyData"/>, соответствующий переданному имени свойства
        /// </returns>
        private static IEnumerable<PropertyData> ParseProperty(string property)
        {
            string[] parts = property.Split('[', ']').Where(s => !String.IsNullOrEmpty(s)).ToArray();

            var result = new List<PropertyData> { new PropertyData { PropertyName = parts[0] } };
            for (int i = 1; i < parts.Length; i++)
            {
                string[] index = parts[i].Split(',');
                result.Add(new PropertyData { PropertyName = indexedPropertyName, IndexParameters = index });
            }

            return result;
        }

        /// <summary>
        /// Возвращает значение указанного свойства для заданного объекта
        /// </summary>
        /// <param name="source">Объект</param>
        /// <param name="propertyData">Описатель свойства</param>
        /// <returns>Значение указанного свойства для заданного объекта</returns>
        private static object GetSimplePropertyValue(object source, PropertyData propertyData)
        {
            if (source == null)
            {
                return null;
                // throw new ArgumentNullException("source");
            }

            bool isArrayIndexer = source.GetType().IsArray && propertyData.PropertyName == indexedPropertyName && propertyData.IndexParameters != null;
            Type sourceType = isArrayIndexer ? typeof(IList) : source.GetType();

            object[] index =
                propertyData.IndexParameters != null
                    ? ParseIndexParameters(sourceType, propertyData.PropertyName, propertyData.IndexParameters)
                    : null;

            object value;
            try
            {
                value = sourceType.GetProperty(propertyData.PropertyName).GetValue(source, index);
            }
            catch (Exception)
            {
                value = null;
            }

            return value;
        }

        /// <summary>
        /// Разбирает строковое представление индексных параметров свойства в их настоящий тип
        /// </summary>
        /// <param name="sourceType">Тип, содержащий свойство</param>
        /// <param name="propertyName">Имя свойства</param>
        /// <param name="stringIndexParameters">
        /// Массив, содержащий строковое представление индексных параметров
        /// </param>
        /// <returns>Массив строго типизированных индексных параметров</returns>
        private static object[] ParseIndexParameters(Type sourceType, string propertyName, string[] stringIndexParameters)
        {
            ParameterInfo[] indexParametersInfo = sourceType.GetProperty(propertyName).GetIndexParameters();
            object[] indexParameters =
                stringIndexParameters.Length > 0
                    ? ConvertToTypedParameters(stringIndexParameters, indexParametersInfo)
                    : null;
            return indexParameters;
        }

        /// <summary>
        /// Преобразует набор параметров из их строкового представления в указанные типы
        /// </summary>
        /// <param name="stringParameters">Массив параметров в строковом представлении</param>
        /// <param name="parameterInfos">Массив описателей параметров</param>
        /// <returns>Массив значений, приведённых к указанным в описателях типах</returns>
        private static object[] ConvertToTypedParameters(string[] stringParameters, ParameterInfo[] parameterInfos)
        {
            if (stringParameters.Length != parameterInfos.Length)
                throw new TargetParameterCountException();

            var typedParameters = new object[stringParameters.Length];
            for (int i = 0; i < stringParameters.Length; i++)
            {
                typedParameters[i] = Convert.ChangeType(stringParameters[i], parameterInfos[i].ParameterType);
            }
            return typedParameters;
        }

        #endregion
    }
}