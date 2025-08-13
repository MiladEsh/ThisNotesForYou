using LiteDB;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ThisNotesForYou;

public static class Endpoints
{
    public static Task<Ok<List<NoteDto>>> GetNotes(int? pageSize, ILiteCollection<Note> col)
    {
        var size = pageSize is > 0 ? pageSize.Value : int.MaxValue;
        var items = col.Query()
            .OrderByDescending(x => x.CreatedAt)
            .Limit(size)
            .ToList()
            .Select(x => x.ToDto())
            .ToList();
        return Task.FromResult(TypedResults.Ok(items));
    }

    public static IResult CreateNote(CreateNote input, ILiteCollection<Note> col)
    {
        var title = input.Title?.Trim() ?? "";
        if (string.IsNullOrWhiteSpace(title))
            return TypedResults.BadRequest("Title is required");

        var note = new Note { Title = title, Text = input.Text?.Trim() ?? "" };
        col.Insert(note);
        var dto = note.ToDto();
        return TypedResults.Created($"/notes/{note.Id}", dto);
    }

    public static IResult DeleteNote(Guid id, ILiteCollection<Note> col)
    {
        var ok = col.Delete(id);
        return ok ? TypedResults.NoContent() : TypedResults.NotFound();
    }
}