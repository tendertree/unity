using System;

[Serializable]
public class WordData
{
    public string word;
    public string meaning;
    public string wrongAnswer;
}

[Serializable]
public class WordDataList
{
    public WordData[] words;
}