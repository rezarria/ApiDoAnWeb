using elFinder.Net.Core.Models.Response;

namespace Api.Models.ElFinder.Responses;

public class ApplicationInitResponse : InitResponse
{
	public ApplicationInitResponse(InitResponse initResp)
	{
		cwd = initResp.cwd;
		files = initResp.files;
		options = initResp.options;
		uplMaxFile = initResp.uplMaxFile;
	}

	public long Usage { get; set; }
	public long Quota { get; set; }
}