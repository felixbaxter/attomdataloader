using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using LumenWorks.Framework.IO.Csv;

namespace attomdataloader
{
  public class ForeclosureFileDataReader : IDataReader {

    private List<Foreclosure> _objectList = null;
    private int _currentIndex = -1;
    public ForeclosureFileDataReader(string sFileName) {
      _objectList = new List<Foreclosure>();
      // open the file "data.csv" which is a CSV file with headers
      using (CsvReader csv =
             new CsvReader(new StreamReader(sFileName), true)) {
        int fieldCount = csv.FieldCount;

        string[] headers = csv.GetFieldHeaders();
        while (csv.ReadNextRecord())
        {
          var f = new Foreclosure();
          for (int i = 0; i < fieldCount; i++)
            
          Console.WriteLine();
        }
      }

      //_objectList = o;
    }

    public bool IsDBNull(int i)
    {
      throw new NotImplementedException();
    }

    public int FieldCount {
      get { return 3; }
    }

    object IDataRecord.this[int i]
    {
      get { throw new NotImplementedException(); }
    }

    object IDataRecord.this[string name]
    {
      get { throw new NotImplementedException(); }
    }

    public void Close() {
      throw new NotImplementedException();
    }

    public DataTable GetSchemaTable()
    {
      throw new NotImplementedException();
    }

    public bool NextResult()
    {
      throw new NotImplementedException();
    }

    public void Dispose() {
      throw new NotImplementedException();
    }

    public string GetName(int i) {
      switch (i) {
        case 0:
          return "RTPropertyUniqueIdentifier";
        case 1:
          return "RTUniqueFCIdentifier";
        case 2:
          return "JurisdictionCountyFIPS";
        case 3:
          return "RecordType";
        case 4:
          return "BorrowersNameOrOwner";
        case 5:
          return "LenderName";
        case 6:
          return "LenderAddress";
        case 7:
          return "LenderCity";
        case 8:
          return "LenderState";
        case 9:
          return "LenderZip";
        case 10:
          return "LenderPhone";
        case 11:
          return "CleanLenderName";
        case 12:
          return "ParentLenderName";
        case 13:
          return "MergerParentName";
        case 14:
          return "ServicerName";
        case 15:
          return "ServicerAddress";
        case 16:
          return "ServicerCity";
        case 17:
          return "ServicerPhone";
        case 18:
          return "ServicerState";
        case 19:
          return "ServicerZip";
        case 20:
          return "TrusteeName";
        case 21:
          return "TrusteeAddress";
        case 22:
          return "TrusteeCity";
        case 23:
          return "TrusteeState";
        case 24:
          return "TrusteeZip";
        case 25:
          return "TrusteePhone";
        case 26:
          return "FCDocRecordingDate";
        case 27:
          return "FCDocInstrumentNumber";
        case 28:
          return "FCDocBookPage";
        case 29:
          return "FCDocInstrumentDate";
        case 30:
          return "RelatedDocumentInstrumentNumber";
        case 31:
          return "RelatedDocDocumentBookPage";
        case 32:
          return "RelatedDocumentRecordingDate";
        case 33:
          return "RecordedAuctionDate";
        case 34:
          return "RecordedOpeningBid";
        case 35:
          return "CaseNumber";
        case 36:
          return "TrusteeReferenceNumber";
        case 37:
          return "Payment";
        case 38:
          return "LoanNumber";
        case 39:
          return "LoanMaturityDate";
        case 40:
          return "DefaultAmount";
        case 41:
          return "OriginalLoanAmount";
        case 42:
          return "PenaltyInterest";
        case 43:
          return "LoanBalance";
        case 44:
          return "InterestRate";
        case 45:
          return "JudgementDate";
        case 46:
          return "JudgmentAmount";
        case 47:
          return "AuctionCourthouse";
        case 48:
          return "AuctionAddress";
        case 49:
          return "AuctionCityState";
        case 50:
          return "AuctionTime";
        case 51:
          return "CreateDate";
        case 52:
          return "LastUpdated";
        case 53:
          return "PublicationDate";
        case 54:
          return "ProcessIndicator";
        case 55:
          return "EstimatedValue";
        default:
          return string.Empty;
      }
    }

    public string GetDataTypeName(int i)
    {
      throw new NotImplementedException();
    }

    public Type GetFieldType(int i)
    {
      throw new NotImplementedException();
    }

    public int GetValues(object[] values)
    {
      throw new NotImplementedException();
    }

    public int GetOrdinal(string name) {
      switch (name) {
        case "RTPropertyUniqueIdentifier":
          return 0;
        case "RTUniqueFCIdentifier":
          return 1;
        case "JurisdictionCountyFIPS":
          return 2;
        case "RecordType":
          return 3;
        case "BorrowersNameOrOwner":
          return 4;
        case "LenderName":
          return 5;
        case "LenderAddress":
          return 6;
        case "LenderCity":
          return 7;
        case "LenderState":
          return 8;
        case "LenderZip":
          return 9;
        case "LenderPhone":
          return 10;
        case "CleanLenderName":
          return 11;
        case "ParentLenderName":
          return 12;
        case "MergerParentName":
          return 13;
        case "ServicerName":
          return 14;
        case "ServicerAddress":
          return 15;
        case "ServicerCity":
          return 16;
        case "ServicerPhone":
          return 17;
        case "ServicerState":
          return 18;
        case "ServicerZip":
          return 19;
        case "TrusteeName":
          return 20;
        case "TrusteeAddress":
          return 21;
        case "TrusteeCity":
          return 22;
        case "TrusteeState":
          return 23;
        case "TrusteeZip":
          return 24;
        case "TrusteePhone":
          return 25;
        case "FCDocRecordingDate":
          return 26;
        case "FCDocInstrumentNumber":
          return 27;
        case "FCDocBookPage":
          return 28;
        case "FCDocInstrumentDate":
          return 29;
        case "RelatedDocumentInstrumentNumber":
          return 30;
        case "RelatedDocDocumentBookPage":
          return 31;
        case "RelatedDocumentRecordingDate":
          return 32;
        case "RecordedAuctionDate":
          return 33;
        case "RecordedOpeningBid":
          return 34;
        case "CaseNumber":
          return 35;
        case "TrusteeReferenceNumber":
          return 36;
        case "Payment":
          return 37;
        case "LoanNumber":
          return 38;
        case "LoanMaturityDate":
          return 39;
        case "DefaultAmount":
          return 40;
        case "OriginalLoanAmount":
          return 41;
        case "PenaltyInterest":
          return 42;
        case "LoanBalance":
          return 43;
        case "InterestRate":
          return 44;
        case "JudgementDate":
          return 45;
        case "JudgmentAmount":
          return 46;
        case "AuctionCourthouse":
          return 47;
        case "AuctionAddress":
          return 48;
        case "AuctionCityState":
          return 49;
        case "AuctionTime":
          return 50;
        case "CreateDate":
          return 51;
        case "LastUpdated":
          return 52;
        case "PublicationDate":
          return 53;
        case "ProcessIndicator":
          return 54;    
        case "EstimatedValue":
          return 55;          
        default:
          return -1;
      }
    }

    public bool GetBoolean(int i)
    {
      throw new NotImplementedException();
    }

    public byte GetByte(int i)
    {
      throw new NotImplementedException();
    }

    public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
    {
      throw new NotImplementedException();
    }

    public char GetChar(int i)
    {
      throw new NotImplementedException();
    }

    public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
    {
      throw new NotImplementedException();
    }

    public Guid GetGuid(int i)
    {
      throw new NotImplementedException();
    }

    public short GetInt16(int i)
    {
      throw new NotImplementedException();
    }

    public int GetInt32(int i)
    {
      throw new NotImplementedException();
    }

    public long GetInt64(int i)
    {
      throw new NotImplementedException();
    }

    public float GetFloat(int i)
    {
      throw new NotImplementedException();
    }

    public double GetDouble(int i)
    {
      throw new NotImplementedException();
    }

    public string GetString(int i)
    {
      throw new NotImplementedException();
    }

    public decimal GetDecimal(int i)
    {
      throw new NotImplementedException();
    }

    public DateTime GetDateTime(int i)
    {
      throw new NotImplementedException();
    }

    public IDataReader GetData(int i)
    {
      throw new NotImplementedException();
    }

    public object GetValue(int i) {
      switch (i) {
        case 0:
          return _objectList[_currentIndex].RTPropertyUniqueIdentifier;
        case 1:
          return _objectList[_currentIndex].RTUniqueFCIdentifier;
        case 2:
          return _objectList[_currentIndex].JurisdictionCountyFIPS;
        case 3:
          return _objectList[_currentIndex].RecordType;
        case 4:
          return _objectList[_currentIndex].BorrowersNameOrOwner;
        case 5:
          return _objectList[_currentIndex].LenderName;
        case 6:
          return _objectList[_currentIndex].LenderAddress;
        case 7:
          return _objectList[_currentIndex].LenderCity;
        case 8:
          return _objectList[_currentIndex].LenderState;
        case 9:
          return _objectList[_currentIndex].LenderZip;
        case 10:
          return _objectList[_currentIndex].LenderPhone;
        case 11:
          return _objectList[_currentIndex].CleanLenderName;
        case 12:
          return _objectList[_currentIndex].ParentLenderName;
        case 13:
          return _objectList[_currentIndex].MergerParentName;
        case 14:
          return _objectList[_currentIndex].ServicerName;
        case 15:
          return _objectList[_currentIndex].ServicerAddress;
        case 16:
          return _objectList[_currentIndex].ServicerCity;
        case 17:
          return _objectList[_currentIndex].ServicerPhone;
        case 18:
          return _objectList[_currentIndex].ServicerState;
        case 19:
          return _objectList[_currentIndex].ServicerZip;
        case 20:
          return _objectList[_currentIndex].TrusteeName;
        case 21:
          return _objectList[_currentIndex].TrusteeAddress;
        case 22:
          return _objectList[_currentIndex].TrusteeCity;
        case 23:
          return _objectList[_currentIndex].TrusteeState;
        case 24:
          return _objectList[_currentIndex].TrusteeZip;
        case 25:
          return _objectList[_currentIndex].TrusteePhone;
        case 26:
          return _objectList[_currentIndex].FCDocRecordingDate;
        case 27:
          return _objectList[_currentIndex].FCDocInstrumentNumber;
        case 28:
          return _objectList[_currentIndex].FCDocBookPage;
        case 29:
          return _objectList[_currentIndex].FCDocInstrumentDate;
        case 30:
          return _objectList[_currentIndex].RelatedDocumentInstrumentNumber;
        case 31:
          return _objectList[_currentIndex].RelatedDocDocumentBookPage;
        case 32:
          return _objectList[_currentIndex].RelatedDocumentRecordingDate;
        case 33:
          return _objectList[_currentIndex].RecordedAuctionDate;
        case 34:
          return _objectList[_currentIndex].RecordedOpeningBid;
        case 35:
          return _objectList[_currentIndex].CaseNumber;
        case 36:
          return _objectList[_currentIndex].TrusteeReferenceNumber;
        case 37:
          return _objectList[_currentIndex].Payment;
        case 38:
          return _objectList[_currentIndex].LoanNumber;
        case 39:
          return _objectList[_currentIndex].LoanMaturityDate;
        case 40:
          return _objectList[_currentIndex].DefaultAmount;
        case 41:
          return _objectList[_currentIndex].OriginalLoanAmount;
        case 42:
          return _objectList[_currentIndex].PenaltyInterest;
        case 43:
          return _objectList[_currentIndex].LoanBalance;
        case 44:
          return _objectList[_currentIndex].InterestRate;
        case 45:
          return _objectList[_currentIndex].JudgementDate;
        case 46:
          return _objectList[_currentIndex].JudgmentAmount;
        case 47:
          return _objectList[_currentIndex].AuctionCourthouse;
        case 48:
          return _objectList[_currentIndex].AuctionAddress;
        case 49:
          return _objectList[_currentIndex].AuctionCityState;
        case 50:
          return _objectList[_currentIndex].AuctionTime;
        case 51:
          return _objectList[_currentIndex].CreateDate;
        case 52:
          return _objectList[_currentIndex].LastUpdated;
        case 53:
          return _objectList[_currentIndex].PublicationDate;
        case 54:
          return _objectList[_currentIndex].ProcessIndicator;
        case 55:
          return _objectList[_currentIndex].EstimatedValue;      
        default:
          return null;
      }
    }

    public bool Read() {
      if ((_currentIndex + 1) < _objectList.Count) {
        _currentIndex++;
        return true;
      }
      else {
        return false;
      }
    }

    public int Depth { get; private set; }
    public bool IsClosed { get; private set; }
    public int RecordsAffected { get; private set; }
  }
}
