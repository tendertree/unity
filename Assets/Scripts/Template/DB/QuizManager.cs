using System;
using System.Threading.Tasks;
using Supabase;
using Unity.Logging;
using UnityEngine;
using Client = Supabase.Client;

public class SupabaseQuizManager : MonoBehaviour
{
    public SupabaseSettings SupabaseSettings = null!;
    private Client client;

    private async void Start()
    {
        var options = new SupabaseOptions
        {
            AutoConnectRealtime = true
        };


        try
        {
            client = new Client(SupabaseSettings.SupabaseURL, SupabaseSettings.SupabaseAnonKey, options);
            await client.InitializeAsync();
            await LoadQuizzes();
        }
        catch (Exception e)
        {
            Debug.LogError($"Error initializing Supabase: {e.Message}");
        }
    }

    private async Task LoadQuizzes()
    {
        try
        {
            var result = await client.From<quiz>().Select("*").Get();
            var quizzes = result.Models;

            foreach (var quiz in quizzes)
                Log.Info($"Quiz: Word={quiz.Word}, Meaning={quiz.Meaning}, Wrong Answer={quiz.WrongAnswer}");
        }
        catch (Exception e)
        {
            Log.Error($"Error loading quizzes: {e.Message}");
        }
    }
}