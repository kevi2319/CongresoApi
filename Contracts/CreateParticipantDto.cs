using System.ComponentModel.DataAnnotations;

public class CreateParticipantDto
{
    [Required, MaxLength(80)]
    public string Nombre { get; set; } = null!;

    [Required, MaxLength(80)]
    public string Apellidos { get; set; } = null!;

    [Required, EmailAddress, MaxLength(120)]
    public string Email { get; set; } = null!;

    [MaxLength(30)]
    public string? Twitter { get; set; }

    [MaxLength(100)]
    public string? Ocupacion { get; set; }

    [Url, MaxLength(250)]
    public string? AvatarUrl { get; set; }

    [Required]
    public bool? TerminosAceptados { get; set; } // requerido true
}
