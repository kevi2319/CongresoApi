public class ParticipantDetailDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public string Apellidos { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Twitter { get; set; }
    public string? Ocupacion { get; set; }
    public string? AvatarUrl { get; set; }
    public bool TerminosAceptados { get; set; }
    public DateTime CreatedAt { get; set; }
}
