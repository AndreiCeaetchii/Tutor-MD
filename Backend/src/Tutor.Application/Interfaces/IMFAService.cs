namespace Tutor.Application.Interfaces;

public interface IMFAService
{
    (string secretKey, string qrCodeImage) GenerateSetup(string email);
    bool VerifyCode(string secretKey, string code);
    string[] GenerateRecoveryCodes(int count = 10);
    string Hash(string input);
    string Decrypt(string cipherText);
    string Encrypt(string plainText);
}