namespace TaxCollectData.Library.Dto.Transfer
{
    public record SignedRequest
    {
        internal SignedRequest()
        {
        }

        public SignedRequest(string signature, string signatureKeyId)
        {
            Signature = signature;
            SignatureKeyId = signatureKeyId;
        }

        public string Signature { get; }

        public string SignatureKeyId { get; }
    }
}