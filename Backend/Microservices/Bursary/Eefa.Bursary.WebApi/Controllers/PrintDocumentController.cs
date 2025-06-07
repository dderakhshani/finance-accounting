using Eefa.Common.Web;
using Microsoft.Extensions.Logging;

namespace Eefa.Bursary.WebApi.Controllers
{
    public class PrintDocumentController : ApiControllerBase
    {
        ILogger<PrintDocumentController> _logger;


        //public ActionResult PrintPaymentDocument(int paymentId)
        //{

        // //   var payment = AccountingManager.GetRequestByRequestId(paymentId);



        //    var tmp = Image.FromFile(Server.MapPath("~/Images/PayemntDocument2_" + payment.PaymentStatus + ".jpg"));
        //    var img = (Image)tmp.Clone();
        //    tmp.Dispose();

        //    var g = Graphics.FromImage(img);
        //    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

        //    var font7 = new Font(new FontFamily("B Nazanin"), 7);
        //    var font6 = new Font(new FontFamily("B Nazanin"), 6);
        //    var font5 = new Font(new FontFamily("B Nazanin"), 5);

        //    var brush = new SolidBrush(Color.Black);
        //    StringFormat format = new StringFormat(StringFormatFlags.DirectionRightToLeft);
        //    if (payment.IsTemprory && payment.PaymentStatus != 2)
        //    {
        //        g.FillRectangle(Brushes.White, new Rectangle(915, 105, 220, 45));
        //        g.DrawString("فرم فاقد اعتبار", new Font(font7.FontFamily, 8, FontStyle.Bold), Brushes.Black, new Point(915, 105));
        //    }
        //    int circleWH = 27;

        //    if (payment.PaymentStatus == 1)//payment
        //        g.FillEllipse(brush, 323, 111, circleWH, circleWH);
        //    else if (payment.PaymentStatus == 2)//receive
        //        g.FillEllipse(brush, 216, 111, circleWH, circleWH);
        //    else if (payment.PaymentStatus == 3)//garantee
        //        g.FillEllipse(brush, 110, 111, circleWH, circleWH);
        //    else if (payment.PaymentStatus == 4)//swap
        //        g.FillEllipse(brush, 323, 156, circleWH, circleWH);
        //    else if (payment.PaymentStatus == 5)//facilities
        //        g.FillEllipse(brush, 216, 156, circleWH, circleWH);
        //    else if (payment.PaymentStatus == 6)//cancel
        //        g.FillEllipse(brush, 110, 156, circleWH, circleWH);

        //    if (payment.PaymentStatus == 1)//payment
        //    {
        //        if (payment.FeeType == 1)//Cash
        //            g.FillEllipse(brush, 244, 211, circleWH, circleWH);
        //        else if (payment.FeeType == 2)//Cheque
        //            g.FillEllipse(brush, 301, 211, circleWH, circleWH);
        //        else if (payment.FeeType == 3)//draft
        //            g.FillEllipse(brush, 175, 211, circleWH, circleWH);
        //    }

        //    if (payment.PaymentStatus == 3)//Grantee
        //    {
        //        if (payment.FeeType == 1)//چک
        //            g.FillEllipse(brush, 345, 211, circleWH, circleWH);
        //        else if (payment.FeeType == 2)//سفته
        //            g.FillEllipse(brush, 273, 211, circleWH, circleWH);
        //        else if (payment.FeeType == 3)//ضمانت نامه
        //            g.FillEllipse(brush, 168, 211, circleWH, circleWH);
        //        else // other
        //            g.FillEllipse(brush, 100, 211, circleWH, circleWH);
        //    }

        //    if (payment.PaymentStatus == 4)//swap
        //        g.FillEllipse(brush, 175, 211, circleWH, circleWH);

        //    if (payment.PaymentCurrency == 1)//Rial
        //        g.FillEllipse(brush, 660, 218, circleWH, circleWH);
        //    else
        //    {
        //        g.FillEllipse(brush, 660, 250, circleWH, circleWH);
        //        g.DrawString(payment.PaymentCurrencyTitle, font6, brush, new Point(570, 205), format);
        //    }

        //    if (payment.PaymentStatus == 1)//payment
        //    {
        //        if (payment.FactorType == 1)//Proforma
        //            g.FillEllipse(brush, 713, 370, circleWH, circleWH);
        //        else if (payment.FactorType == 2)//Invoice
        //            g.FillEllipse(brush, 616, 370, circleWH, circleWH);
        //        else if (payment.FactorType == 3)//draft
        //            g.FillEllipse(brush, 855, 370, circleWH, circleWH);
        //        else if (payment.FactorType == 4)//agreement
        //            g.FillEllipse(brush, 1008, 370, circleWH, circleWH);

        //        if (payment.PaymentType == 1)//علی احساب
        //            g.FillEllipse(brush, 1008, 403, circleWH, circleWH);
        //        else if (payment.PaymentType == 2)//پیش پرداخت
        //            g.FillEllipse(brush, 713, 403, circleWH, circleWH);
        //        else if (payment.PaymentType == 3)//تسویه
        //            g.FillEllipse(brush, 855, 403, circleWH, circleWH);
        //        else //سایر
        //            g.FillEllipse(brush, 615, 403, circleWH, circleWH);
        //    }

        //    if (payment.PaymentStatus == 3)//grantee
        //    {
        //        if (payment.FactorType == 1)//Proforma
        //            g.FillEllipse(brush, 713, 370, circleWH, circleWH);
        //        else if (payment.FactorType == 2)//Invoice
        //            g.FillEllipse(brush, 616, 370, circleWH, circleWH);
        //        else if (payment.FactorType == 3)//draft
        //            g.FillEllipse(brush, 855, 370, circleWH, circleWH);
        //        else if (payment.FactorType == 4)//agreement
        //            g.FillEllipse(brush, 1008, 370, circleWH, circleWH);

        //        if (payment.PaymentType == 1)//علی احساب
        //            g.FillEllipse(brush, 1008, 403, circleWH, circleWH);
        //        else if (payment.PaymentType == 2)//پیش پرداخت
        //            g.FillEllipse(brush, 713, 403, circleWH, circleWH);
        //        else if (payment.PaymentType == 3)//تسویه
        //            g.FillEllipse(brush, 855, 403, circleWH, circleWH);
        //        else //سایر
        //            g.FillEllipse(brush, 615, 403, circleWH, circleWH);
        //    }

        //    if (payment.PaymentStatus == 4)//swap
        //        g.FillEllipse(brush, 944, 375, circleWH, circleWH);

        //    if (payment.ForeignTrading == true)//بازرگانی خارجی
        //        g.FillEllipse(brush, 97, 537, circleWH, circleWH);

        //    g.DrawString(payment.PaymentNumber.ToString(), font7, brush, new Point(92, 67));

        //    //  if (payment.PaymentStatus != 2)
        //    g.DrawString(payment.InvoiceIssuanceDate, font7, brush, new Point(1066, 150), format);
        //    //   else
        //    //  g.DrawString(payment.ReceicePersianDate, font7, brush, new Point(1066, 150), format);

        //    var fullName = payment.FullName;
        //    if (!string.IsNullOrEmpty(payment.Name))
        //    {
        //        fullName += "-" + payment.Name + " " + payment.LastName + " (" + payment.SSN + ")";
        //    }
        //    var fullNameWords = fullName.Split(' ');
        //    var x = 745;
        //    var y = 105;
        //    var font = font6;
        //    foreach (var w in fullNameWords)
        //    {
        //        var size = g.MeasureString(w, font);
        //        if (x - size.Width < 420 && y == 105)
        //        {
        //            font = font5;
        //            x = 880;
        //            y += 30;
        //            g.DrawString(w, font, brush, new Point(x, y), format);
        //            x -= (int)Math.Round(size.Width);

        //        }
        //        else
        //        {
        //            g.DrawString(w, font, brush, new Point(x, y), format);
        //            x -= (int)Math.Round(size.Width);
        //        }
        //    }

        //    var fullName2 = payment.TargetAccount;

        //    if (payment.PaymentStatus == 2)
        //    {
        //        g.DrawString(fullName2, font, brush, new Point(985, 815), format);
        //        g.DrawString(payment.TargetAccountCode, font, brush, new Point(250, 815), format);

        //    }
        //    else
        //    {
        //        g.DrawString(fullName2, font, brush, new Point(985, 912), format);
        //        g.DrawString(payment.TargetAccountCode, font, brush, new Point(250, 912), format);
        //    }
        //    g.DrawString(payment.Code, font7, brush, new Point(715, 152), format);
        //    g.DrawString(payment.AmountPay.ToString("N0"), font7, brush, new Point(1010, 225), format);
        //    g.DrawString(payment.StringAmount, font7, brush, new Point(1030, 275), format);
        //    g.DrawString(payment.BankName, font7, brush, new Point(1070, 440), format);
        //    g.DrawString(payment.AccountSheba.Replace("IR", ""), font7, brush, new Point(490, 445));
        //    g.DrawString("به نام " + payment.AccountOwner +
        //        (!string.IsNullOrEmpty(payment.CustomerPaymentIdentifier) ? "-شناسه واریز:" + payment.CustomerPaymentIdentifier : ""),
        //        font5, brush, new Point(1130, 477), format);
        //    g.DrawString(payment.Description, font6, brush, new RectangleF(190, 495, 860, 85), format);

        //    x = 370;
        //    foreach (var c in payment.listCheckDate)
        //    {
        //        var size = g.MeasureString(c.CheckDate + ",", font6);
        //        if (x - size.Width < 90)
        //        {
        //            g.DrawString("....", font6, brush, new Point(x, 245), format);
        //            break;
        //        }

        //        g.DrawString(c.CheckDate + ",", font6, brush, new Point(x, 248), format);
        //        x -= (int)Math.Round(size.Width);
        //    }



        //    var requestNumbers = "";
        //    x = 827;
        //    y = 320;
        //    var requestNumbers2 = "";
        //    foreach (var r in payment.listRequestNumber)
        //    {
        //        requestNumbers += r + ", ";
        //        var size = g.MeasureString(r + ", ", font6);
        //        if (x - size.Width < 97)
        //        {
        //            if (x > 0)//first time
        //                g.DrawString(".... ", font6, brush, new Point(x, y), format);
        //            x = 0;
        //            y = 0;
        //            requestNumbers2 += r + ", ";
        //        }
        //        else if (y == 320)
        //        {
        //            g.DrawString(r + ", ", font6, brush, new Point(x, y), format);// new Point(825, 320,
        //            x -= (int)Math.Round(size.Width);
        //        }

        //    }
        //    //g.DrawString(requestNumbers, font6, brush, new RectangleF(97, 320,730,30), format);// new Point(825, 320,

        //    var preinvoiceNumbers = "";
        //    var preinvoiceNumbers2 = "";
        //    x = 380;
        //    y = 360;
        //    foreach (var r in payment.listPreinvoiceNumber)
        //    {
        //        preinvoiceNumbers += r + ", ";
        //        var size = g.MeasureString(r + ", ", font6);
        //        if (x - size.Width < 97 && y < 410)
        //        {
        //            x = 560;
        //            y += 30;
        //            g.DrawString(r + ", ", font6, brush, new Point(x, y), format);// new Point(825, 320,
        //            x -= (int)Math.Round(size.Width);
        //        }
        //        else if (x - size.Width < 97 && y >= 410)
        //        {
        //            if (x > 0)//first time
        //                g.DrawString(".... ", font6, brush, new Point(x, y), format);
        //            x = 0;
        //            preinvoiceNumbers2 += r + ", ";
        //        }
        //        else
        //        {
        //            g.DrawString(r + ", ", font6, brush, new Point(x, y), format);// new Point(825, 320,
        //            x -= (int)Math.Round(size.Width);
        //        }
        //    }

        //    if (payment.Verifiers.Count > 1)
        //        if (payment.Verifiers[payment.Verifiers.Count - 2].UserId == 1220)
        //        {
        //            var sign1220 = Image.FromFile(Server.MapPath("~/Images/PersonelSigns/u_1220.png"));
        //            g.DrawImage(sign1220, 942, 670, 200, 100);
        //        }
        //    //-----------------------Draw Cheques On Document if exist-------------------
        //    DrawChequesOnDocument(payment, g, img);

        //    //-----------------------Draw Letter For Swap Request-------------------------------
        //    if (payment.PaymentStatus == 2)  //recieve{
        //    {
        //        var context = ContextFactory.CreateContext();
        //        var RecieveChequeId = context.BaseValues.FirstOrDefault(b => b.UniqueName == "RecieveCheque").Id;
        //        if (payment.SubjectBaseId == RecieveChequeId)
        //            DrawAttachmentOnDocument(payment, 11, g, 0);
        //        else
        //            DrawAttachmentOnDocument(payment, 100, g, 0);

        //    }
        //    else if (payment.PaymentStatus == 4)//swap{
        //        DrawAttachmentOnDocument(payment, 5, g, 0);

        //    using (var streak = new MemoryStream())
        //    {
        //        img.Save(streak, System.Drawing.Imaging.ImageFormat.Png);
        //        return File(streak.ToArray(), "image/jpeg");
        //    }
        //}




    }
   
}
