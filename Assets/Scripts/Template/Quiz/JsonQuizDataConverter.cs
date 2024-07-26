namespace Quiz
{
    public static class JsonToQuizDataConverter
    {
        /*
        public static List<QuizData> ConvertJsonToQuizDataList(string jsonString)
        {
            List<QuizData> quizDataList = new List<QuizData>();

            JArray jsonArray = JArray.Parse(jsonString);

            foreach (JObject jsonObject in jsonArray)
            {
                QuizData quizData = new QuizData
                {
                    Word = new FixedString128Bytes(jsonObject["word"].ToString()),
                    Meaning = new FixedString128Bytes(jsonObject["meaning"].ToString()),
                    WrongAnswer = new FixedString128Bytes(jsonObject["wrong_answer"].ToString())
                };

                quizDataList.Add(quizData);
            }
            */

        // return quizDataList;
    }
}