using System;
using System.Dynamic;

namespace attomdataloader
{
  public class Recorder {
    public int RTPropertyUniqueIdentifier { get; set; }
    public int RTDocumentIdentifier { get; set; }
    public string JurisdictionCountyFIPS { get; set; }
    public string DocumentNumber { get; set; }
    public string Book { get; set; }
    public string Page { get; set; }
    public DateTime DocumentDate { get; set; }
    public DateTime FilingDate { get; set; }
    public DateTime RecordingDate { get; set; }
    public string DocumentCategoryType { get; set; }
    public string DocumentTypeDescription { get; set; }
    public bool MultiParcel { get; set; }
    public bool ArmsLengthTransfer { get; set; }
    public string Legal { get; set; }
    public string Borrower { get; set; }
    public string Grantor { get; set; }
    public string Grantee { get; set; }
    public string LenderName { get; set; }
    public string CleanLenderName { get; set; }
    public string ParentLenderName { get; set; }
    public string MergerParentName { get; set; }
    public bool PartialInterest { get; set; }
    public int SalePrice { get; set; }
    public string LoanRateTypeDescription { get; set; }
    public string LoanTypeDescription { get; set; }
    public int NewLoan1Amount { get; set; }
    public int NewLoan2Amount { get; set; }
    public string NewLoanDocumentNumber { get; set; }
    public string NewLoanInterestRate { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime LastUpdated { get; set; }
    public DateTime PublicationDate { get; set; }
    public string ProcessIndicator { get; set; }
    public bool InMedianRange { get; set; }
    public bool ForeclosureAuctionSale { get; set; }
    public bool InvestorBulkBuy { get; set; }
    public bool AbsentOwner { get; set; }
    public string DistressCircumstanceDescription { get; set; }
    public string SaleTermsDescription { get; set; }
    public string SaleTypeDescription { get; set; }
    public string TransferTypeDescription { get; set; }
    public int RTUniqueFCIdentifier { get; set; }
    public int AVMatDocRecordingDate { get; set; }
    public string SourceCountyName { get; set; }
    public string SourceMunicipalityName { get; set; }
    public string DocumentNumberFormatted { get; set; }
    public string APNOriginal { get; set; }
    public string SiteAddressRaw { get; set; }
    public string MailAddressRaw { get; set; }
    public string MailAddressHouseNumber { get; set; }
    public string MailAddressFraction { get; set; }
    public string MailAddressDirection { get; set; }
    public string MailAddressStreetName { get; set; }
    public string MailAddressSuffix { get; set; }
    public string MailAddressPostDirection { get; set; }
    public string MailAddressUnitPrefix { get; set; }
    public string MailAddressUnitValue { get; set; }
    public string MailCity { get; set; }
    public string MailState { get; set; }
    public string MailZip { get; set; }
    public string MailZip4 { get; set; }
    public string MailCRRT { get; set; }
    public bool Quitclaim { get; set; }
    public string LegalBlock { get; set; }
    public string LegalLot { get; set; }
    public string LegalPlatBook { get; set; }
    public string LegalPlatPage { get; set; }
    public string LegalRange { get; set; }
    public string LegalSection { get; set; }
    public string LegalSubDivision { get; set; }
    public string LegalTownship { get; set; }
    public string LegalTract { get; set; }
    public string LegalUnit { get; set; }
    //public int FileID { get; set; }
  }
}
