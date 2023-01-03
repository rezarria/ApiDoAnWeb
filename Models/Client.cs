using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public class Client
{
	[Key]
	public Guid Id { get; set; }
	public string Url { get; set; } = string.Empty;
	public bool Active { get; set; } = false;
}