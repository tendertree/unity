using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;

public static class JsonHelper
{
    public static List<T> GetJsonObject<T>(string filePath, string propertyName)
    {
        try
        {
            if (!File.Exists(filePath)) return null;

            var jsonContent = File.ReadAllText(filePath);
            var jsonObject = JObject.Parse(jsonContent);

            if (!jsonObject.ContainsKey(propertyName)) return null;

            var jsonArray = (JArray)jsonObject[propertyName];
            return jsonArray.ToObject<List<T>>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading JSON file: {ex.Message}");
            return null;
        }
    }
}