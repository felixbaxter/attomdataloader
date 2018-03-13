using System;

namespace attomdataloader
{
  public class Foreclosure {
    public int RTPropertyUniqueIdentifier { get; set; }
    public int RTUniqueFCIdentifier { get; set; }
    public string JurisdictionCountyFIPS { get; set; }
    public string RecordType { get; set; }
    public string BorrowersNameOrOwner { get; set; }
    public string LenderName { get; set; }
    public string LenderAddress { get; set; }
    public string LenderCity { get; set; }
    public string LenderState { get; set; }
    public string LenderZip { get; set; }
    public string LenderPhone { get; set; }
    public string CleanLenderName { get; set; }
    public string ParentLenderName { get; set; }
    public string MergerParentName { get; set; }
    public string ServicerName { get; set; }
    public string ServicerAddress { get; set; }
    public string ServicerCity { get; set; }
    public string ServicerPhone { get; set; }
    public string ServicerState { get; set; }
    public string ServicerZip { get; set; }
    public string TrusteeName { get; set; }
    public string TrusteeAddress { get; set; }
    public string TrusteeCity { get; set; }
    public string TrusteeState { get; set; }
    public string TrusteeZip { get; set; }
    public string TrusteePhone { get; set; }
    public DateTime FCDocRecordingDate { get; set; }
    public string FCDocInstrumentNumber { get; set; }
    public string FCDocBookPage { get; set; }
    public DateTime FCDocInstrumentDate { get; set; }
    public string RelatedDocumentInstrumentNumber { get; set; }
    public string RelatedDocDocumentBookPage { get; set; }
    public DateTime RelatedDocumentRecordingDate { get; set; }
    public DateTime RecordedAuctionDate { get; set; }
    public int RecordedOpeningBid { get; set; }
    public string CaseNumber { get; set; }
    public string TrusteeReferenceNumber { get; set; }
    public int Payment { get; set; }
    public string LoanNumber { get; set; }
    public DateTime LoanMaturityDate { get; set; }
    public int DefaultAmount { get; set; }
    public int OriginalLoanAmount { get; set; }
    public int PenaltyInterest { get; set; }
    public int LoanBalance { get; set; }
    public decimal InterestRate { get; set; }
    public DateTime JudgementDate { get; set; }
    public int JudgmentAmount { get; set; }
    public string AuctionCourthouse { get; set; }
    public string AuctionAddress { get; set; }
    public string AuctionCityState { get; set; }
    public string AuctionTime { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime LastUpdated { get; set; }
    public DateTime PublicationDate { get; set; }
    public string ProcessIndicator { get; set; }
    public int EstimatedValue { get; set; }
    //public int FileID { get; set; }

  }
}
