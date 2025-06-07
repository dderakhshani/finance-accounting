using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Admin.Application.Common.Extensions
{
    public class RsaKeyGenerator
    {
        public static (string publicKey, string privateKey) GenerateRsaKeyPair(string companyEconomicCode)
        {
            using (var rsa = RSA.Create(2048)) 
            {
                
                string privateKey = ExportPrivateKey(rsa);

                
                string publicKey = ExportPublicKey(rsa);

                
                string csr = GenerateCsr(rsa, companyEconomicCode);

                return (publicKey, privateKey);
            }
        }

        private static string ExportPrivateKey(RSA rsa)
        {
            var privateKeyBytes = rsa.ExportPkcs8PrivateKey();
            return ConvertToPem("PRIVATE KEY", privateKeyBytes);
        }

        private static string ExportPublicKey(RSA rsa)
        {
            var publicKeyBytes = rsa.ExportSubjectPublicKeyInfo();
            return ConvertToPem("PUBLIC KEY", publicKeyBytes);
        }

        private static string GenerateCsr(RSA rsa, string commonName)
        {
            
            var request = new CertificateRequest(
                $"CN={commonName}",
                rsa,
                HashAlgorithmName.SHA256,
                RSASignaturePadding.Pkcs1);

            
            

            byte[] csrBytes = request.CreateSigningRequest();
            return ConvertToPem("CERTIFICATE REQUEST", csrBytes);
        }

        private static string ConvertToPem(string type, byte[] data)
        {
            //StringBuilder builder = new StringBuilder();
            ////builder.AppendLine($"-----BEGIN {type}-----");
            //Convert.ToBase64String(data, Base64FormattingOptions.InsertLineBreaks);
            ////builder.AppendLine($"-----END {type}-----");
            return Convert.ToBase64String(data, Base64FormattingOptions.InsertLineBreaks);
        }
    }
}
