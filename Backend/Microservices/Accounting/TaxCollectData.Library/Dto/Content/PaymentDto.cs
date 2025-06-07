namespace TaxCollectData.Library.Dto.Content
{
    public record PaymentDto
    {
       /**
     * IinNumber
     */
       public string Iinn { get; init; }

        /**
     * acceptorNumber
     */
        public string Acn { get; init; }

        /**
     * terminalNumber
     */
        public string Trmn { get; init; }

        /**
     * trackingNumber
     */
        public string Trn { get; init; }

        /**
     * payerCardNumber
     */
        public string Pcn { get; init; }

        /**
     * payerId
     */
        public string Pid { get; init; }

        /**
     * payDateTime
     */
        public long? Pdt { get; init; }
        /**
     * paymentMethod
     */
        public int? Pmt { get; init; }
        /**
     * paymentValue
     */
        public long? Pv { get; init; }
    }
}