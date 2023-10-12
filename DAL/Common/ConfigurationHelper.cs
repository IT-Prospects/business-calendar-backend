﻿using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace DAL.Common
{
    public static class ConfigurationHelper
    {
        private static IConfiguration _configuration;
        private static readonly string _configNotInitialized = "Конфигурация не инициализирована";
        private static readonly string _notFountMessage = "Не найдено значение {0} в конфигурации приложения";
        private static readonly string _invalidFormatMessage = "При попытке преобразовать значение {0} произошла ошибка";
        public static void SetConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private static void CheckConfiguration()
        {
            if (_configuration == null)
                throw new ApplicationException(_configNotInitialized);
        }

        public static string GetString(string key)
        {
            CheckConfiguration();

            var value = _configuration[key];
            return value == null ? throw new ArgumentException(string.Format(_notFountMessage, key)) : value.ToString();
        }

        public static long GetLong(string key)
        {
            CheckConfiguration();

            var value = _configuration[key] ?? throw new ArgumentException(string.Format(_notFountMessage, key));
            if (!long.TryParse(value, out long parsedValue))
                throw new FormatException(string.Format(_invalidFormatMessage, key));

            return parsedValue;
        }

        public static int GetInt(string key)
        {
            CheckConfiguration();

            var value = _configuration[key] ?? throw new ArgumentException(string.Format(_notFountMessage, key));
            if (!int.TryParse(value, out int parsedValue))
                throw new FormatException(string.Format(_invalidFormatMessage, key));

            return parsedValue;
        }
    }
}
