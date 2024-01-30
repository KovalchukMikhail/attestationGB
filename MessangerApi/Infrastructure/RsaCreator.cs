using System.Security.Cryptography;

namespace MessangerApi.Infrastructure
{
    public class RsaCreator
    {
        public static RSA GetPublicKey()
        {
            var text = File.ReadAllText("rsa/public_key.pem");
            var rsa = RSA.Create();
            rsa.ImportFromPem(text);
            return rsa;
        }
    }
}
