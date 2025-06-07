namespace Eefa.Bursary.Application.UseCases.Payables.Documents.Models
{
    public class DocumentAccountsModel
    {
        public int? Id { get; set; }
        public int DocumentId { get; set; }
        public int ReferenceId { get; set; }
        public int? AccountHeadId { get; set; }
        public int? ReferenceGroupId { get; set; }
        public int? RexpId { get; set; }
        public string Descp { get; set; }
        public long Amount { get; set; }
    }
}
