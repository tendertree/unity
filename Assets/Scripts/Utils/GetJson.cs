using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
//
public static class JsonHelper
{
    public static List<T> GetJsonObject<T>(string filePath, string propertyName)
    {
        try
        {
            // JSON 파일 읽기
            string jsonText = File.ReadAllText(filePath);

            // JSON 파싱
            JObject jsonObject = JObject.Parse(jsonText);

            // 지정된 프로퍼티에서 배열 가져오기
            JArray jsonArray = (JArray)jsonObject[propertyName];

            // 배열을 List<T>로 변환
            return jsonArray.ToObject<List<T>>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"JSON 파일 읽기 오류: {ex.Message}");
            return null;
        }
    }
}