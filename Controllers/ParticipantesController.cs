using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api")]
public class ParticipantesController : ControllerBase
{
    private readonly AppDbContext _db;
    public ParticipantesController(AppDbContext db) => _db = db;

    // GET /api/listado  (con soporte de búsqueda: ?q=)
    [HttpGet("listado")]
    public async Task<ActionResult<IEnumerable<ParticipantListItemDto>>> GetListado([FromQuery] string? q = null)
    {
        IQueryable<Participant> query = _db.Participants.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(q))
        {
            var term = q.Trim().ToLower();
            query = query.Where(p =>
                p.Nombre.ToLower().Contains(term) ||
                p.Apellidos.ToLower().Contains(term));
        }

        var data = await query
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => new ParticipantListItemDto
            {
                Id = p.Id,
                NombreCompleto = p.Nombre + " " + p.Apellidos,
                Twitter = p.Twitter,
                Ocupacion = p.Ocupacion,
                AvatarUrl = p.AvatarUrl
            })
            .ToListAsync();

        return Ok(data);
    }

    // GET /api/participante/{id}
    [HttpGet("participante/{id:int}")]
    public async Task<ActionResult<ParticipantDetailDto>> GetById([FromRoute] int id)
    {
        var p = await _db.Participants.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (p is null) return NotFound();

        var dto = new ParticipantDetailDto
        {
            Id = p.Id,
            Nombre = p.Nombre,
            Apellidos = p.Apellidos,
            Email = p.Email,
            Twitter = p.Twitter,
            Ocupacion = p.Ocupacion,
            AvatarUrl = p.AvatarUrl,
            TerminosAceptados = p.TerminosAceptados,
            CreatedAt = p.CreatedAt
        };
        return Ok(dto);
    }

    // POST /api/registro
    [HttpPost("registro")]
    public async Task<ActionResult<ParticipantDetailDto>> Registrar([FromBody] CreateParticipantDto dto)
    {
        if (dto.TerminosAceptados != true)
            return ValidationProblem("Debes aceptar términos y condiciones.");

        // opcional: validar duplicado por email
        var exists = await _db.Participants.AnyAsync(p => p.Email == dto.Email);
        if (exists) return Conflict("El email ya está registrado.");

        var p = new Participant
        {
            Nombre = dto.Nombre.Trim(),
            Apellidos = dto.Apellidos.Trim(),
            Email = dto.Email.Trim(),
            Twitter = string.IsNullOrWhiteSpace(dto.Twitter) ? null : dto.Twitter.Trim().TrimStart('@'),
            Ocupacion = string.IsNullOrWhiteSpace(dto.Ocupacion) ? null : dto.Ocupacion.Trim(),
            AvatarUrl = string.IsNullOrWhiteSpace(dto.AvatarUrl) ? null : dto.AvatarUrl.Trim(),
            TerminosAceptados = dto.TerminosAceptados!.Value
        };

        _db.Participants.Add(p);
        await _db.SaveChangesAsync();

        var outDto = new ParticipantDetailDto
        {
            Id = p.Id,
            Nombre = p.Nombre,
            Apellidos = p.Apellidos,
            Email = p.Email,
            Twitter = p.Twitter,
            Ocupacion = p.Ocupacion,
            AvatarUrl = p.AvatarUrl,
            TerminosAceptados = p.TerminosAceptados,
            CreatedAt = p.CreatedAt
        };

        return CreatedAtAction(nameof(GetById), new { id = p.Id }, outDto);
    }
}
