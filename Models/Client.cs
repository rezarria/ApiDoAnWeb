using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public class Client
{
	[Key]
	public Guid Id { get; set; }
	public string Url { get; set; }
	public bool Active { get; set; } = false;
}