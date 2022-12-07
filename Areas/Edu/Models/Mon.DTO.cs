using Api.Models.Interfaces;

namespace Api.Models.DTO;

/// <summary>
/// 
/// </summary>
public class MonDTO : IMonInfo
{
    public string Ten { get; set; } = string.Empty;
    public string? MieuTa { get; set; }
}