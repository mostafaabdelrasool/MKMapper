﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace xMKMapper
{
    public class MapProperties
    {
        /// <summary>
        /// set all source properties to destination properties which name is equal
        /// </summary>
        /// <param name="SourceObject"></param>
        /// <param name="DesitnationObject"></param>
        /// <returns></returns>
        public T2 Map<T1, T2>(T1 SourceObject, T2 DesitnationObject, List<string> excludes = null)
        {
            if (SourceObject == null)
            {
                return DesitnationObject;
            }
            Type destType = DesitnationObject.GetType();
            parent.Clear();
            RootType = destType;
            if (SourceObject is IEnumerable && DesitnationObject is IEnumerable)
            {
                foreach (var item in (IEnumerable)SourceObject)
                {
                    destType.GetMethod("Add").Invoke(DesitnationObject,
                        new[] { GetProperties(item, CreateInstance(destType.GetProperties()[2], false), excludes) });
                    parent.Clear();
                }
            }
            else
            {
                GetProperties(SourceObject, DesitnationObject, excludes);
                maxDepth = 0;
            }
            return DesitnationObject;
        }

        private object GetProperties(object SourceObject, object DesitnationObject, List<string> excludes = null)
        {
            object data = null; Type sourceType = SourceObject.GetType();
            excludes = excludes == null ? new List<string>() : excludes;
            //check if source property is object or enumrable
            DesitnationObject.GetType().GetProperties().Where(x => !excludes.Contains(x.Name.ToLower())).ToList().ForEach(destProp =>
            {
                if (sourceType.GetProperties().Any(a => a.Name.ToLower() == destProp.Name.ToLower()))
                {
                    var sourceProp = sourceType.GetProperties().
                    Where(a => a.Name.ToLower() == destProp.Name.ToLower()).FirstOrDefault();

                    var SourceValues = sourceProp.GetValue(SourceObject);
                    if (sourceProp.PropertyType.IsInterface || (sourceProp.PropertyType.IsGenericType &&
                              sourceProp.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                    && SourceValues != null && MapCollection && maxDepth <= 3)
                    {
                        if (((IEnumerable)SourceValues).Cast<object>().Count() > 0)
                        {
                            if (!parent.Keys.Contains(sourceType))
                            {
                                parent.Add(sourceType, new List<string>());
                            }
                            if (!parent[sourceType].Contains(sourceProp.PropertyType.GetGenericArguments()[0].Name) && !parent.Keys.Contains(sourceProp.PropertyType.GetGenericArguments()[0]))
                            {
                                parent[sourceType].Add(sourceProp.PropertyType.GetGenericArguments()[0].Name);
                                data = CreateInstance(destProp, true);

                                foreach (var item in (IEnumerable)SourceValues)
                                {

                                    data.GetType().GetMethod("Add").Invoke(data,
                                        new[] { GetProperties(item, Activator.CreateInstance(destProp.PropertyType.GetGenericArguments()[0])) });
                                    parent.Remove(sourceProp.PropertyType.GetGenericArguments()[0]);
                                    maxDepth = 0;
                                }
                                destProp.SetValue(DesitnationObject, data);
                            }

                        }

                    }
                    else if (sourceProp.PropertyType.IsClass &&
                        !sourceProp.PropertyType.Name.Contains("String") && SourceValues != null && MapCollection
                        )
                    {
                        if (SourceValues != null)
                        {
                            if (!parent.Keys.Contains(sourceType))
                            {
                                parent.Add(sourceType, new List<string>());
                            }

                            if (!parent[sourceType].Contains(sourceProp.Name) && !parent.Keys.Contains(sourceProp.PropertyType))
                            {
                                parent[sourceType].Add(sourceProp.Name);
                                data = CreateInstance(destProp, false);
                                GetProperties(SourceValues, data);
                                parent.Remove(sourceProp.PropertyType);
                                maxDepth = 0;
                                destProp.SetValue(DesitnationObject, data);
                            }

                        }
                    }
                    else if (!sourceProp.PropertyType.IsClass && !sourceProp.PropertyType.IsInterface ||
                        sourceProp.PropertyType.Name.Contains("String"))
                    {
                        destProp.SetValue(DesitnationObject, SourceValues);
                    }
                    else if (!MapCollection)
                    {
                        destProp.SetValue(DesitnationObject, null);
                    }
                }

            });

            return DesitnationObject;
        }

        private object CreateInstance(PropertyInfo source, bool IsIEnumrable)
        {
            object Instance = null;
            if (IsIEnumrable)
            {
                Type Type = source.PropertyType.GetGenericArguments()[0];
                Type ListTpye = typeof(List<>).MakeGenericType(Type);
                Instance = Activator.CreateInstance(ListTpye);
            }
            else
            {
                Instance = Activator.CreateInstance(source.PropertyType);
            }

            return Instance;
        }

        public bool MapCollection = true;
        public int MaxRecursion = 0;
        private int maxDepth = 0;
        private Type RootType = null;
        private Dictionary<Type, List<string>> parent = new Dictionary<Type, List<string>>();
    }

}

