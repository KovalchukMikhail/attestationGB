using System.Security.Cryptography;

namespace UserAPI.Infrastructure
{
    public static class RsaCreator
    {
        public static RSA GetPublicKey()
        {
            return GetKey("rsa/public_key.pem");
        }
        public static RSA GetPrivetKey()
        {
            return GetKey("rsa/private_key.pem");
        }
        private static RSA GetKey(string keyPath)
        {
            var text = File.ReadAllText(keyPath);
            var rsa = RSA.Create();
            rsa.ImportFromPem(text);
            return rsa;
        }

    }
}
