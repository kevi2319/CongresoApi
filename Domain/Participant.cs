using System.ComponentModel.DataAnnotations;

public class Participant
{
    public int Id { get; set; }

    [Required, MaxLength(80)]
    public string Nombre { get; set; } = null!;

    [Required, MaxLength(80)]
    public string Apellidos { get; set; } = null!;

    [Required, EmailAddress, MaxLength(120)]
    public string Email { get; set; } = null!;

    // Usuario en Twitter sin @, solo el handle
    [MaxLength(30)]
    public string? Twitter { get; set; }

    [MaxLength(100)]
    public string? Ocupacion { get; set; }

    // URL del avatar (imagen)
    [MaxLength(250)]
    public string? AvatarUrl { get; set; }

    // Casilla de términos y condiciones
    public bool TerminosAceptados { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
