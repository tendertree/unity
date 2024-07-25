using UnityEngine;

[CreateAssetMenu(fileName = "Supabase", menuName = "Supabase/Supabase Settings", order = 1)]
public class SupabaseSettings : ScriptableObject
{
    public string SupabaseURL = "https://fklxmqeqvffhjvagzkla.supabase.co";


    public string SupabaseAnonKey =
        "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImZrbHhtcWVxdmZmaGp2YWd6a2xhIiwicm9sZSI6ImFub24iLCJpYXQiOjE2OTMxMTg1ODksImV4cCI6MjAwODY5NDU4OX0.A_xUy2NcJTbUa5rUWAv_zAPDlgynF4Dn38KWbqW6OQ8";
}