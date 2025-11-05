public class ParticipantListItemDto
{
    public int Id { get; set; }
    public string NombreCompleto { get; set; } = null!;
    public string? Twitter { get; set; }
    public string? Ocupacion { get; set; }
    public string? AvatarUrl { get; set; }
}
