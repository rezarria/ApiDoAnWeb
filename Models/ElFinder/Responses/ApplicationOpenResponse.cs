using elFinder.Net.Core.Models.Response;

namespace Api.Models.ElFinder.Responses;

public class ApplicationOpenResponse : OpenResponse
{
	public ApplicationOpenResponse(OpenResponse openResp)
	{
		cwd = openResp.cwd;
		files = openResp.files;
		options = openResp.options;
		uplMaxFile = openResp.uplMaxFile;
	}

	public long Usage { get; set; }
	public long Quota { get; set; }
}