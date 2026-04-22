using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Infrastructure
{
    public static class ConfigValidator
    {
        public static bool Validate<T>(T config, string fileName)
        {
            if (config == null)
            {
                Debug.LogError($"[Config] {fileName} — result is null");
                return false;
            }

            var errors = new List<string>();
            ValidateObject(config, typeof(T).Name, errors);

            if (errors.Count > 0)
            {
                foreach (var error in errors)
                    Debug.LogWarning($"[Config] {fileName} — {error}");

                Debug.LogError($"[Config] {fileName} loaded with {errors.Count} problem(s)");
                return false;
            }

            Debug.Log($"[Config] {fileName} loaded successfully");
            return true;
        }

        private static void ValidateObject(object obj, string path, List<string> errors)
        {
            if (obj == null)
            {
                errors.Add($"{path} is null");
                return;
            }

            var fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

            foreach (var field in fields)
            {
                var value = field.GetValue(obj);
                var fullPath = $"{path}.{field.Name}";

                if (value == null)
                {
                    errors.Add($"{fullPath} is null");
                    continue;
                }

                var type = field.FieldType;

                if (!type.IsPrimitive && type != typeof(string) && !type.IsEnum)
                {
                    ValidateObject(value, fullPath, errors);
                    continue;
                }

                if (value is int intVal && intVal == 0)
                    errors.Add($"{fullPath} = 0 (possibly not loaded)");
                else if (value is float floatVal && Mathf.Approximately(floatVal, 0f))
                    errors.Add($"{fullPath} = 0 (possibly not loaded)");
                else if (value is string strVal && string.IsNullOrEmpty(strVal))
                    errors.Add($"{fullPath} is empty");
            }
        }
    }
}
