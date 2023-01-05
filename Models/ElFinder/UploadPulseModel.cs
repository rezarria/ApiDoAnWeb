using Timer=System.Timers.Timer;

namespace Api.Models.ElFinder;

public class UploadPulseModel
{
	public List<string> UploadedFiles { get; set; } = new();
	public DateTimeOffset LastPulse { get; set; }
	public Timer? Timer { get; set; }
}