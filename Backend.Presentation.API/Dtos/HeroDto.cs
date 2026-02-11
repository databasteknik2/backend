namespace Backend.Presentation.API.Dtos;

public sealed record HeroDto
(
    int Id,
    string Title,
    string Description,
    string? ImageUrl = null
);
