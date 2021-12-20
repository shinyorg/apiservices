﻿using Shiny.Extensions.Localization;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;


namespace Shiny
{
    public static class LocalizationExtensions
    {
        /// <summary>
        /// This is a useful method if you want to serialize all sections for a specified culture to something like JSON
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> GetAllSectionsWithKeys(this ILocalizationManager manager, CultureInfo culture)
        {
            var dict = new Dictionary<string, IReadOnlyDictionary<string, string>>();
            foreach (var section in manager.Sections)
            {
                var values = section.GetValues(culture);
                dict.Add(section.Name, values);
            }
            return dict;
        }


        /// <summary>
        /// Quick shortcut for getting a sections key/values for a specified culture
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="sectionName"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static IReadOnlyDictionary<string, string> GetSectionValues(this ILocalizationManager manager, string sectionName, CultureInfo culture)
            => manager.GetSection(sectionName)!.GetValues(culture);


        public static void Bind(this ILocalizationSource localize, object obj)
        {
            var props = obj
                .GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(x => x.SetMethod != null && x.GetMethod != null)
                .ToList();

            foreach (var prop in props)
            {
                var value = localize[prop.Name];
                if (value != null)
                    prop.SetValue(obj, value);
            }
        }


        public static string? GetEnum<T>(this ILocalizationManager localize, string section, T value)
            => localize.GetSection(section)?.GetEnum(value);


        public static string? GetEnum<T>(this ILocalizationSource localize, T value)
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException(typeof(T).FullName + " is not an enum");

            var name = $"{typeof(T).Namespace}.{typeof(T).Name}.{value}";
            return localize[name];
        }
    }
}