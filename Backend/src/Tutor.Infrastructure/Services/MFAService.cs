using OtpNet;
using QRCoder;
using System;
using System.Security.Cryptography;
using System.Text;
using Tutor.Application.Interfaces;

namespace Tutor.Infrastructure.Services;

public class MFAService : IMFAService
{
    public (string secretKey, string qrCodeImage) GenerateSetup(string email)
    {
        var secretKey = KeyGeneration.GenerateRandomKey(20);
        var base32Secret = Base32Encoding.ToString(secretKey);
        
        var totp = new Totp(secretKey);
        var issuer = "Tutor.Md";
        var provisionUrl = $"otpauth://totp/{issuer}:{email}?secret={base32Secret}&issuer={issuer}";
        
        // Generate QR code
        var qrGenerator = new QRCodeGenerator();
        var qrCodeData = qrGenerator.CreateQrCode(provisionUrl, QRCodeGenerator.ECCLevel.Q);
        var qrCode = new PngByteQRCode(qrCodeData);
        var qrCodeImage = qrCode.GetGraphic(20);
        
        return (base32Secret, Convert.ToBase64String(qrCodeImage));
    }

    public bool VerifyCode(string secretKey, string code)
    {
        var secretBytes = Base32Encoding.ToBytes(secretKey);
        var totp = new Totp(secretBytes);
        return totp.VerifyTotp(code, out _, new VerificationWindow(1, 1));
    }

    public string[] GenerateRecoveryCodes(int count = 10)
    {
        var codes = new string[count];
        var random = new Random();
        
        for (int i = 0; i < count; i++)
        {
            codes[i] = $"{random.Next(10000000, 99999999)}-{random.Next(10000000, 99999999)}";
        }
        
        return codes;
    }
    public string Encrypt(string plainText)
    {
        var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("MFA_ENCRYPTION_KEY") 
                                         ?? "Your32CharEncryptionKeyHere12345"); // 32 chars

        using var aes = Aes.Create();
        aes.Key = key;
        aes.GenerateIV();

        using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        var plainBytes = Encoding.UTF8.GetBytes(plainText);
        var cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

        var result = new byte[aes.IV.Length + cipherBytes.Length];
        Buffer.BlockCopy(aes.IV, 0, result, 0, aes.IV.Length);
        Buffer.BlockCopy(cipherBytes, 0, result, aes.IV.Length, cipherBytes.Length);

        return Convert.ToBase64String(result);
    }

    public string Decrypt(string cipherText)
    {
        var fullCipher = Convert.FromBase64String(cipherText);
        var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("MFA_ENCRYPTION_KEY") 
                                         ?? "Your32CharEncryptionKeyHere123456789012"); 

        using var aes = Aes.Create();
        aes.Key = key;

        var iv = new byte[aes.BlockSize / 8];
        var cipherBytes = new byte[fullCipher.Length - iv.Length];
        Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
        Buffer.BlockCopy(fullCipher, iv.Length, cipherBytes, 0, cipherBytes.Length);

        aes.IV = iv;

        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        var plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
        return Encoding.UTF8.GetString(plainBytes);
    }
    public string Hash(string input)
    {
        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(input);
        var hash = sha.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

}