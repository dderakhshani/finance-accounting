using System.Collections.Generic;

public class MoadianInquiryResultModel
{
    public List<MoadianInquiryErrorModel> Error { get; set; }
    public List<MoadianInquiryErrorModel> Warning { get; set; }
}

public class MoadianInquiryErrorModel
{
    public string Code { get; set; }
    public string Message { get; set; }
}