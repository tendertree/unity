using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using com.example;
using Unity.Logging;
using UnityEngine;
using Client = Supabase.Client;

public class SupabaseQuizManager : MonoBehaviour
{
    public SupabaseManager _manager;
    private Client client;

    public async Task<quiz> CreateQuiz(string word, string meaning, string wrongAnswer)
    {
        client = _manager.Supabase();
        try

        {
            var newQuiz = new quiz { Word = word, Meaning = meaning, WrongAnswer = wrongAnswer };
            var response = await client.From<quiz>().Insert(newQuiz);
            Log.Info($"Quiz created: {newQuiz.Word}");
            return response.Model;
        }
        catch
            (Exception e)
        {
            Log.Error($"Error creating quiz: {e.Message}");
            return null;
        }
    }

// Read
    public async Task<List<quiz>> LoadQuizzes()
    {
        try
        {
            var result = await client.From<quiz>().Select("*").Get();
            var quizzes = result.Models;
            foreach (var quiz in quizzes)
                Log.Info($"Quiz: Word={quiz.Word}, Meaning={quiz.Meaning}, Wrong Answer={quiz.WrongAnswer}");
            return quizzes;
        }
        catch (Exception e)
        {
            Log.Error($"Error loading quizzes: {e.Message}");
            return new List<quiz>();
        }
    }

// Update
    public async Task<bool> UpdateQuiz(int id, string word, string meaning, string wrongAnswer)
    {
        try
        {
            var updatedQuiz = new quiz { Id = id, Word = word, Meaning = meaning, WrongAnswer = wrongAnswer };
            await client.From<quiz>().Update(updatedQuiz);
            Log.Info($"Quiz updated: {updatedQuiz.Word}");
            return true;
        }
        catch (Exception e)
        {
            Log.Error($"Error updating quiz: {e.Message}");
            return false;
        }
    }

// Delete
    public async Task<bool> DeleteQuiz(int id)
    {
        try
        {
            await client.From<quiz>().Where(x => x.Id == id)
                .Delete();
            Log.Info($"Quiz deleted: ID {id}");
            return true;
        }
        catch (Exception e)
        {
            Log.Error($"Error deleting quiz: {e.Message}");
            return false;
        }
    }
}