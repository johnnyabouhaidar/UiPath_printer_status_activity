using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Activities;
using System.ComponentModel;
using System.Printing;

namespace PrinterQueueStatus
{
    public class GetQueueStatus : CodeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        public InArgument<String> PrinterName { get; set; }

        [Category("Output")]
        public OutArgument<Boolean> ErrorStatus { get; set; }

        [Category("Output")]
        public OutArgument<Boolean> OutOfPaperStatus { get; set; }

        [Category("Output")]
        public OutArgument<Boolean> PaperJammedStatus { get; set; }

        [Category("Output")]
        public OutArgument<Boolean> HasTonerStatus { get; set; }

        [Category("Output")]
        public OutArgument<String> PrinterLocation { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            try
            {
                var server = new LocalPrintServer();
                var printer = PrinterName.Get(context);
                PrintQueue queue = server.GetPrintQueue(printer, new string[0] { });
                queue.Refresh();

                bool isInError = queue.IsInError;
                bool isOutOfPaper = queue.IsOutOfPaper;
                bool isPaperJammed = queue.IsPaperJammed;
                bool hasToner = queue.HasToner;
                string location = queue.Location;


                ErrorStatus.Set(context, isInError);
                OutOfPaperStatus.Set(context, isOutOfPaper);
                PaperJammedStatus.Set(context, isPaperJammed);
                HasTonerStatus.Set(context, hasToner);
                PrinterLocation.Set(context, location);
            }
            catch (Exception e)
            {
               
            }
        }
    }
}
