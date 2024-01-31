using System;
using System.Collections.Generic;
using System.Reflection;

namespace OneBlock.Utils
{
    public class ReflectionHelper
    {
        private Type type;
        private object obj;
        public ReflectionHelper(object obj)
        {
            this.obj = obj;
            type = obj.GetType();
        }
        private SortedDictionary<string, FieldInfo> fieldInfos = new();
        private void CheckValidField(ref FieldInfo fieldInfo, string fieldName)
        {
            if (fieldInfo == null)
            {
                throw new ArgumentException($"Private field '{fieldName}' not found on type '{type.FullName}'.");
            }
        }
        public T GetField<T>(string fieldName, BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance)
        {
            FieldInfo fieldInfo;
            if (!fieldInfos.TryGetValue(fieldName, out fieldInfo))
            {
                fieldInfo = type.GetField(fieldName, bindingFlags);
            }

            CheckValidField(ref fieldInfo, fieldName);

            return (T)fieldInfo.GetValue(obj);
        }
        public void SetField<T>(string fieldName, T value, BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance)
        {
            FieldInfo fieldInfo;
            if (!fieldInfos.TryGetValue(fieldName, out fieldInfo))
            {
                fieldInfo = type.GetField(fieldName, bindingFlags);
            }

            CheckValidField(ref fieldInfo, fieldName);

            fieldInfo.SetValue(obj, value);
        }
        private void CheckValidProperty(ref PropertyInfo propertyInfo, string propertyName)
        {
            if (propertyInfo == null)
            {
                throw new ArgumentException($"Private property '{propertyName}' not found on type '{type.FullName}'.");
            }
        }
        public T GetProperty<T>(string propertyName, BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance)
        {
            PropertyInfo propertyInfo = type.GetProperty(propertyName, bindingFlags);
            if (propertyInfo == null)
            {
                throw new ArgumentException($"Private property '{propertyName}' not found on type '{type.FullName}'.");
            }

            CheckValidProperty(ref propertyInfo, propertyName);

            return (T)propertyInfo.GetValue(obj);
        }
        public void SetProperty<T>(string propertyName, T value, BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance)
        {
            PropertyInfo propertyInfo = type.GetProperty(propertyName, bindingFlags);
            if (propertyInfo == null)
            {
                throw new ArgumentException($"Private property '{propertyName}' not found on type '{type.FullName}'.");
            }

            CheckValidProperty(ref propertyInfo, propertyName);

            propertyInfo.SetValue(obj, value);
        }
        public static T GetNonPublicStaticClassField<T>(string NameSpace, string className, string fieldName, BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Static)
        {
            Assembly assembly = Assembly.Load(NameSpace);
            Type backupIOType = assembly.GetType($"{NameSpace}.{className}");
            FieldInfo fieldInfo = backupIOType.GetField(fieldName, bindingFlags);
            return (T)fieldInfo.GetValue(null);
        }
        public static void SetNonPublicStaticClassField<T>(string NameSpace, string className, string fieldName, T value, BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Static)
        {
            Assembly assembly = Assembly.Load(NameSpace);
            Type backupIOType = assembly.GetType($"{NameSpace}.{className}");
            FieldInfo fieldInfo = backupIOType.GetField(fieldName, bindingFlags);
            fieldInfo.SetValue(null, value);
        }
        public static void CallNonPublicStaticMethod(string NameSpace, string className, string methodName, BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Static)
        {
            Assembly assembly = Assembly.Load(NameSpace);
            Type backupIOType = assembly.GetType($"{NameSpace}.{className}");
            MethodInfo methodInfo = backupIOType.GetMethod(methodName, bindingFlags);
            methodInfo.Invoke(null, null);
        }
    }
}
