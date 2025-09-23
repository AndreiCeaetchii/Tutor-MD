namespace Tutor.Application.Features.Users.Dtos;

public class EnableMFAResponse
{
    public string QrCodeImage { get; set; }
    public string SecretKey { get; set; }
    public string[] RecoveryCodes { get; set; }
}