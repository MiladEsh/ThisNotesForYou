namespace ThisNotesForYou;

public record NoteDto(Guid Id, string Title, string Text, DateTime CreatedAt);

public static class NoteMapping
{
    public static NoteDto ToDto(this Note n) => new(n.Id, n.Title, n.Text, n.CreatedAt);
}