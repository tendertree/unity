using System;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

// Postgrest.Models.BaseModels
public class Quiz_DB : BaseModel
{
    [PrimaryKey("id")] public long Id { get; set; }

    [Column("inserted_at")] public DateTime InsertedAt { get; set; }

    [Column("updated_at")] public DateTime UpdatedAt { get; set; }

    [Column("word")] public string Word { get; set; }

    [Column("meaning")] public string Meaning { get; set; }

    [Column("wrong_answer")] public string WrongAnswer { get; set; }

    /*
    public override bool Equals(object obj)
    {
        return obj is quiz quiz &&
               Id == quiz.Id;
    }
    */

    public override int GetHashCode()
    {
        return HashCode.Combine(Id);
    }
}