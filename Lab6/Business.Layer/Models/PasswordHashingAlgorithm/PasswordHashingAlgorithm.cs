using System.Security.Cryptography;
using System.Text;

namespace Business.Layer.Models.PasswordHashingAlgorithm;

public class PasswordHashingAlgorithm
{
    public string GetHash(string input)
    {
        var result = new StringBuilder();
        var hashingAlgorithm = SHA256.Create();
        byte[] password = hashingAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

        foreach (byte passwordByte in password)
        {
            result.Append(passwordByte.ToString("X2"));
        }

        return result.ToString();
    }
}