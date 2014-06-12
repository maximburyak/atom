using System.ComponentModel;

namespace Atom.Areas.Suppliers.Domain
{
    public enum HandlerTypeEnum
    {
        [Description("Supplier Feed")]
        FeedFile = 0,
        [Description("Supplier Invoice")]
        InvoiceFile = 1,
        [Description("Supplier PO Confirmation")]
        DfDespatchFile = 2,
        [Description("Supplier Charges")]
        FeedChargeFile = 3,
        [Description("Wholesale Trade")]
        WholesaleTradeFile = 4,
        [Description("Closure Reports")]
        ClosureReportFile = 5,
        [Description("Excess Invoice")]
        ExcessInvoiceFile = 6,
        [Description("File Download")]        
        FileDownload = 7,
        [Description("Opticard File")]        
        OpticardFile = 8,
		[Description("Inspection File")]        
        InspectionFile = 9,
		[Description("FIS Files")]        
        FisFiles = 10,
		[Description("FIS Error Files")]
		FisErrorFiles = 11,
		[Description("PP Extended Range Files")]
		PpExtendedRange = 12,
		[Description("Unknown")]
		Unknown = 99
    }
}