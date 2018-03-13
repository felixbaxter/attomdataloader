using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq.Expressions;
using System.Net.Mime;
using System.Reflection;
using LumenWorks.Framework.IO.Csv;

namespace attomdataloader
{
  public class LoadData {

    public static bool TruncateTaxAssessorImport() {
      try {
        using (var destinationConnection = new SqlConnection(GlobalVariables.DbConnection)) {
          destinationConnection.Open();
          var command = new SqlCommand("TRUNCATE TABLE TaxAssessorImport", destinationConnection);
          command.ExecuteNonQuery();
          destinationConnection.Close();
        }
        return true;
      }
      catch (Exception ex) {
        return false;
      }
    }

    public static bool DropTaxAssessorImportPrimaryKey() {
      try {
        using (var destinationConnection = new SqlConnection(GlobalVariables.DbConnection)) {
          destinationConnection.Open();
          var command = new SqlCommand("ALTER TABLE [dbo].[TaxAssessorImport] DROP CONSTRAINT PK_TaxAssessorImport", destinationConnection);
          command.ExecuteNonQuery();
          destinationConnection.Close();
        }
        return true;
      }
      catch (Exception ex) {
        return false;
      }
    }

    

    public static bool TruncateRecorderImport() {
      try {
        using (var destinationConnection = new SqlConnection(GlobalVariables.DbConnection)) {
          destinationConnection.Open();
          var command = new SqlCommand("TRUNCATE TABLE RecorderImport", destinationConnection);
          command.ExecuteNonQuery();
          destinationConnection.Close();
        }
        return true;
      }
      catch (Exception ex) {
        return false;
      }
    }

    public static bool TruncateForeclosureImport() {
      try {
        using (var destinationConnection = new SqlConnection(GlobalVariables.DbConnection)) {
          destinationConnection.Open();
          var command = new SqlCommand("TRUNCATE TABLE ForeclosureImport", destinationConnection);
          command.ExecuteNonQuery();
          destinationConnection.Close();
        }
        return true;
      }
      catch (Exception ex) {
        return false;
      }
    }

    public static bool TruncateAVMEquityImport() {
      try {
        using (var destinationConnection = new SqlConnection(GlobalVariables.DbConnection)) {
          destinationConnection.Open();
          var command = new SqlCommand("TRUNCATE TABLE AVMEquityImport", destinationConnection);
          command.ExecuteNonQuery();
          destinationConnection.Close();
        }
        return true;
      }
      catch (Exception ex) {
        return false;
      }
    }

    public static bool LoadTaxAccessorFile(string sRoot, string sFile) {
      bool bSussess = true;
      GlobalVariables.iTotalRecords = 0;
      if (!Directory.Exists(sRoot + "\\Logs")) Directory.CreateDirectory(sRoot + "\\Logs");
      TextWriter tw = new StreamWriter(sRoot + "\\Logs" + "\\taxassessor-debug-" + sFile + ".txt");
      using (var destinationConnection = new SqlConnection(GlobalVariables.DbConnection)) {
        destinationConnection.Open();
        SqlTransaction transaction = destinationConnection.BeginTransaction();
        try {
          tw.WriteLine("=================================================");
          tw.WriteLine("Debug Start - " + DateTime.Now.ToString());
          tw.WriteLine("=================================================");

          using (var file = new StreamReader(sRoot + "\\" + sFile))
          using (var csv = new CsvReader(file, true, '|', '"', '"', '#', ValueTrimmingOptions.All))
          // true = has header row          
          using (
            var bcp = new SqlBulkCopy(destinationConnection, SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.KeepNulls,
              transaction)) {
            csv.MissingFieldAction = MissingFieldAction.ParseError;
            csv.DefaultParseErrorAction = ParseErrorAction.RaiseEvent;
            csv.ParseError += csv_ParseError;
            csv.SkipEmptyLines = true;

            bcp.DestinationTableName = Properties.Settings.Default.TaxAssessorTable;
            lock (GlobalVariables.locker) {
              bcp.NotifyAfter = GlobalVariables.iNotifyAfter;
              bcp.BatchSize = GlobalVariables.iBatchSize;
            }
            bcp.BulkCopyTimeout = 1800;
            bcp.SqlRowsCopied += bcp_SqlRowsCopied;
            var t = new TaxAssessor();
            Type type = t.GetType();
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo property in properties) {
              bcp.ColumnMappings.Add(property.Name, property.Name);
            }
            bcp.WriteToServer(csv);
            transaction.Commit();
          }
        }
        catch (Exception e) {
          tw.WriteLine(e.Message);
          bSussess = false;
          lock (GlobalVariables.locker) {
            GlobalVariables.iNumberOfErrors++;
            GlobalVariables.sTaxAssessorErrors += Environment.NewLine + e.Message;
          }
        }
        finally {
          tw.Close();
          tw.Dispose();
          transaction.Dispose();
        }
      }
      return bSussess;
    }


    public static bool LoadForeclosureFile(string sRoot, string sFile) {
      bool bSussess = true;
      GlobalVariables.iTotalRecords = 0;
      if (!Directory.Exists(sRoot + "\\Logs")) Directory.CreateDirectory(sRoot + "\\Logs");
      TextWriter tw = new StreamWriter(sRoot + "\\Logs" + "\\foreclosure-debug-" + sFile + ".txt");
      using (var destinationConnection = new SqlConnection(GlobalVariables.DbConnection)) {
        destinationConnection.Open();
        SqlTransaction transaction = destinationConnection.BeginTransaction();
        try {
          tw.WriteLine("=================================================");
          tw.WriteLine("Debug Start - " + DateTime.Now.ToString());
          tw.WriteLine("=================================================");
          using (var file = new StreamReader(sRoot + "\\" + sFile))
          using (var csv = new CsvReader(file, true, '|', '"', '"', '#', ValueTrimmingOptions.All)) // true = has header row          
          using (var bcp = new SqlBulkCopy(destinationConnection, SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.KeepNulls, transaction)) {
            csv.MissingFieldAction = MissingFieldAction.ParseError;
            csv.DefaultParseErrorAction = ParseErrorAction.RaiseEvent;
            csv.ParseError += csv_ParseError;
            csv.SkipEmptyLines = true;
            bcp.DestinationTableName = Properties.Settings.Default.ForeclosureTable;
            lock (GlobalVariables.locker) {
              bcp.NotifyAfter = GlobalVariables.iNotifyAfter;
              bcp.BatchSize = GlobalVariables.iBatchSize;
            }
            bcp.BulkCopyTimeout = 1800;
            bcp.SqlRowsCopied += bcp_SqlRowsCopied;
            var t = new Foreclosure();
            Type type = t.GetType();
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo property in properties) {
              bcp.ColumnMappings.Add(property.Name, property.Name);
            }
            bcp.WriteToServer(csv);
            transaction.Commit();
          }
        }
        catch (Exception e) {
          tw.WriteLine(e.Message);
          bSussess = false;
          transaction.Rollback();
          lock (GlobalVariables.locker) {
            GlobalVariables.iNumberOfErrors++;
            GlobalVariables.sForeclosureErrors += Environment.NewLine + e.Message;
          }
        }
        finally {
          tw.Close();
          tw.Dispose();
          transaction.Dispose();
        }
      }
      return bSussess;
    }

    public static bool LoadRecorderFile(string sRoot, string sFile) {
      bool bSussess = true;
      GlobalVariables.iTotalRecords = 0;
      if (!Directory.Exists(sRoot + "\\Logs")) Directory.CreateDirectory(sRoot + "\\Logs");
      TextWriter tw = new StreamWriter(sRoot + "\\Logs" + "\\recorder-debug-" + sFile + ".txt");
      using (var destinationConnection = new SqlConnection(GlobalVariables.DbConnection)) {
        destinationConnection.Open();
        SqlTransaction transaction = destinationConnection.BeginTransaction();
        try {
          tw.WriteLine("=================================================");
          tw.WriteLine("Debug Start - " + DateTime.Now.ToString());
          tw.WriteLine("=================================================");
          using (var file = new StreamReader(sRoot + "\\" + sFile))
          using (var csv = new CsvReader(file, true, '|', '"', '"', '#', ValueTrimmingOptions.All)) // true = has header row          
          using (var bcp = new SqlBulkCopy(destinationConnection, SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.KeepNulls, transaction)) {
            csv.MissingFieldAction = MissingFieldAction.ParseError;
            csv.DefaultParseErrorAction = ParseErrorAction.RaiseEvent;
            csv.ParseError += csv_ParseError;
            csv.SkipEmptyLines = true;
            bcp.DestinationTableName = Properties.Settings.Default.RecorderTable;
            lock (GlobalVariables.locker) {
              bcp.NotifyAfter = GlobalVariables.iNotifyAfter;
              bcp.BatchSize = GlobalVariables.iBatchSize;
            }
            bcp.BulkCopyTimeout = 1800;
            bcp.SqlRowsCopied += bcp_SqlRowsCopied;
            var t = new Recorder();
            Type type = t.GetType();
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo property in properties) {
              bcp.ColumnMappings.Add(property.Name, property.Name);
            }

            bcp.WriteToServer(csv);
            transaction.Commit();
          }
        }
        catch (Exception e) {
          tw.WriteLine(e.Message);
          bSussess = false;
          transaction.Rollback();
          lock (GlobalVariables.locker) {
            GlobalVariables.iNumberOfErrors++;
            GlobalVariables.sRecorderErrors += Environment.NewLine + e.Message;
          }
        }
        finally {
          tw.Close();
          tw.Dispose();
          transaction.Dispose();
        }
      }
      return bSussess;
    }

    public static bool LoadAVMFile(string sRoot, string sFile) {
      bool bSussess = true;
      TruncateAVMEquityImport();
      GlobalVariables.iTotalRecords = 0;
      if (!Directory.Exists(sRoot + "\\Logs")) Directory.CreateDirectory(sRoot + "\\Logs");
      TextWriter tw = new StreamWriter(sRoot + "\\Logs" + "\\avm-debug-" + sFile + ".txt");
      using (var destinationConnection = new SqlConnection(GlobalVariables.DbConnection)) {
        destinationConnection.Open();
        SqlTransaction transaction = destinationConnection.BeginTransaction();
        try {
          tw.WriteLine("=================================================");
          tw.WriteLine("Debug Start - " + DateTime.Now.ToString());
          tw.WriteLine("=================================================");
          using (var file = new StreamReader(sRoot + "\\" + sFile))
          using (var csv = new CsvReader(file, true, '|', '"', '"', '#', ValueTrimmingOptions.All)) // true = has header row          
          using (var bcp = new SqlBulkCopy(destinationConnection, SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.KeepNulls, transaction)) {
            csv.MissingFieldAction = MissingFieldAction.ParseError;
            csv.DefaultParseErrorAction = ParseErrorAction.RaiseEvent;
            csv.ParseError += csv_ParseError;
            csv.SkipEmptyLines = true;
            bcp.DestinationTableName = Properties.Settings.Default.AVMTable;
            lock (GlobalVariables.locker) {
              bcp.NotifyAfter = GlobalVariables.iNotifyAfter;
              bcp.BatchSize = GlobalVariables.iBatchSize;
            }
            bcp.BulkCopyTimeout = 1800;
            bcp.SqlRowsCopied += bcp_SqlRowsCopied;
            var t = new AVMEquity();
            Type type = t.GetType();
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo property in properties) {
              bcp.ColumnMappings.Add(property.Name, property.Name);
            }
            bcp.WriteToServer(csv);
            transaction.Commit();
          }
        }
        catch (Exception e) {
          tw.WriteLine(e.Message);
          bSussess = false;
          transaction.Rollback();
          lock (GlobalVariables.locker) {
            GlobalVariables.iNumberOfErrors++;
            GlobalVariables.sAvmErrors += Environment.NewLine + e.Message;
          }
        }
        finally {
          tw.Close();
          tw.Dispose();
          transaction.Dispose();
        }
      }
      return bSussess;
    }



    public static bool ProcessTaxAccessorImport(int iFileId) {
      try {
        using (var destinationConnection = new SqlConnection(GlobalVariables.DbConnection)) {          
          destinationConnection.Open();
          destinationConnection.InfoMessage += destinationConnection_InfoMessage;
          using (var command = new SqlCommand("LoadTaxAssessorImport", destinationConnection))
          {
            command.CommandTimeout = 43200;
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@FileID", SqlDbType.Int).Value = iFileId;
            command.ExecuteNonQuery();
          }
          destinationConnection.Close();
        }
        return true;
      }
      catch (Exception ex) {
        lock (GlobalVariables.locker) {
          GlobalVariables.iNumberOfErrors++;
          GlobalVariables.sTaxAssessorErrors += Environment.NewLine + ex.Message;
        }
        return false;
      }
    }

    public static bool ProcessForeclosureImport(int iFileId) {
      try {
        using (var destinationConnection = new SqlConnection(GlobalVariables.DbConnection)) {          
          destinationConnection.Open();
          destinationConnection.InfoMessage += destinationConnection_InfoMessage;
          using (var command = new SqlCommand("LoadForeclosureImport", destinationConnection))
          {
            command.CommandTimeout = 43200;
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@FileID", SqlDbType.Int).Value = iFileId;
            command.ExecuteNonQuery();
          }
          destinationConnection.Close();
        }
        return true;
      }
      catch (Exception ex) {
        lock (GlobalVariables.locker) {
          GlobalVariables.iNumberOfErrors++;
          GlobalVariables.sForeclosureErrors += Environment.NewLine + ex.Message;
        }
        return false;
      }
    }

    public static bool ProcessRecorderImport(int iFileId) {
      try {
        using (var destinationConnection = new SqlConnection(GlobalVariables.DbConnection)) {          
          destinationConnection.Open();
          destinationConnection.InfoMessage += destinationConnection_InfoMessage;
          using (var command = new SqlCommand("LoadRecorderImport", destinationConnection))
          {
            command.CommandTimeout = 43200;
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@FileID", SqlDbType.Int).Value = iFileId;
            command.ExecuteNonQuery();
          }
          destinationConnection.Close();
        }
        return true;
      }
      catch (Exception ex) {
        lock (GlobalVariables.locker) {
          GlobalVariables.iNumberOfErrors++;
          GlobalVariables.sRecorderErrors += Environment.NewLine + ex.Message;
        }
        return false;
      }
    }

    public static bool ProcessAVMImport(int iFileId) {
      try {
        using (var destinationConnection = new SqlConnection(GlobalVariables.DbConnection)) {
          destinationConnection.Open();
          destinationConnection.InfoMessage += destinationConnection_InfoMessage;
          using (var command = new SqlCommand("LoadAVMImport", destinationConnection))
          {
            command.CommandTimeout = 43200;
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@FileID", SqlDbType.Int).Value = iFileId;
            command.ExecuteNonQuery();
          }
          destinationConnection.Close();
        }
        return true;
      }
      catch (Exception ex) {
        lock (GlobalVariables.locker) {
          GlobalVariables.iNumberOfErrors++;
          GlobalVariables.sAvmErrors += Environment.NewLine + ex.Message;
        }
        return false;
      }
    }    

    public static void destinationConnection_InfoMessage(object sender, SqlInfoMessageEventArgs e)
    {
      GlobalServices.SetText("frmMain", "lStatus", e.Message);
    }

    public static bool CleanFileOfQutoes(string sRoot, string sFile, out int iCount) {
      bool bSussess = true;     
      TextWriter tw = new StreamWriter(sRoot + "\\Logs" + "\\clean-quotes-debug-" + sFile + ".txt");
      TextWriter twOut = new StreamWriter(sRoot + "\\" + Path.GetFileNameWithoutExtension(sFile) + "_P.txt");
      try {
        tw.WriteLine("=================================================");
        tw.WriteLine("Debug Start - " + DateTime.Now.ToString());
        tw.WriteLine("=================================================");
        using (var file = new StreamReader(sRoot + "\\" + sFile)) {
          int iProcessed = 0;
          int iTotal = 0;
          string line;
          while ((line = file.ReadLine()) != null) {
            iProcessed++;
            iTotal++;
            //var orgline = line;
            line = line.Replace("\"|\"", "®").Replace("\"", "").Replace("|", ",");
            line = "\"" + line.Replace("®", "\"|\"") + "\"";
            twOut.WriteLine(line);
          }
          iCount = iTotal - 1;
        }
      }
      catch (Exception e) {
        tw.WriteLine(e.Message);
        bSussess = false;
        iCount = 0;
        lock (GlobalVariables.locker)
        {
          GlobalVariables.iNumberOfErrors++;
        }
      }
      finally {
        tw.Close();
        tw.Dispose();
        twOut.Close();
        twOut.Dispose();
      }
      return bSussess;
    }

    public static bool CheckRecorderDataForErrors(string sRoot, string sFile) {
      bool bSussess = true;
      TextWriter tw = new StreamWriter(sRoot + "\\Logs" + "\\file-errors-debug-" + sFile + ".txt");
      TextWriter twOut = new StreamWriter(sRoot + "\\" + Path.GetFileNameWithoutExtension(sFile) + "2.txt");
      TextWriter twError = new StreamWriter(sRoot + "\\" + Path.GetFileNameWithoutExtension(sFile) + "_E.txt");
      try {
        tw.WriteLine("=================================================");
        tw.WriteLine("Debug Start - " + DateTime.Now.ToString());
        tw.WriteLine("=================================================");
        using (var file = new StreamReader(sRoot + "\\" + sFile)) {
          int iProcessed = 1;
          int iTotal = 1;
          string line;
          line = file.ReadLine();//skip first row b/c it has header information
          twOut.WriteLine(line);
          while ((line = file.ReadLine()) != null) {
            iProcessed++;
            iTotal++;
            //We are going to split out each record into an array of columns and test data
            var cdata = line.Split('|');
            if (CheckRecorderRecord(cdata, twError)) {
              twOut.WriteLine(line);
            }
            if (iProcessed == 100000) {
              iProcessed = 0;
              GlobalServices.AppendText("frmMain", "txtRecorder", iTotal.ToString("N0"));
            }
          }
        }
      }
      catch (Exception e) {
        tw.WriteLine(e.Message);
        bSussess = false;
        lock (GlobalVariables.locker) {
          GlobalVariables.iNumberOfErrors++;
        }
      }
      finally {
        tw.Close();
        tw.Dispose();
        twOut.Close();
        twOut.Dispose();
        twError.Close();
        twError.Dispose();
      }
      return bSussess;
    }

    public static bool CheckTaxAssessorDataForErrors(string sRoot, string sFile) {
      bool bSussess = true;
      TextWriter tw = new StreamWriter(sRoot + "\\Logs" + "\\file-errors-debug-" + sFile + ".txt");
      TextWriter twOut = new StreamWriter(sRoot + "\\" + Path.GetFileNameWithoutExtension(sFile) + "2.txt");
      TextWriter twError = new StreamWriter(sRoot + "\\" + Path.GetFileNameWithoutExtension(sFile) + "_E.txt");
      try {
        tw.WriteLine("=================================================");
        tw.WriteLine("Debug Start - " + DateTime.Now.ToString());
        tw.WriteLine("=================================================");
        using (var file = new StreamReader(sRoot + "\\" + sFile)) {
          int iProcessed = 1;
          int iTotal = 1;
          string line;
          line = file.ReadLine();//skip first row b/c it has header information
          twOut.WriteLine(line);
          while ((line = file.ReadLine()) != null) {
            iProcessed++;
            iTotal++;
            //We are going to split out each record into an array of columns and test data
            var cdata = line.Split('|');
            if (CheckTaxAssessorRecord(cdata, twError)) {
              twOut.WriteLine(line);
            }
          }
        }
      }
      catch (Exception e) {
        tw.WriteLine(e.Message);
        bSussess = false;
        lock (GlobalVariables.locker) {
          GlobalVariables.iNumberOfErrors++;
        }
      }
      finally {
        tw.Close();
        tw.Dispose();
        twOut.Close();
        twOut.Dispose();
        twError.Close();
        twError.Dispose();
      }
      return bSussess;
    }

    

    public static bool RewriteRecorderHeader(string sRoot, string sFile) {
      bool bSussess = true;
      TextWriter tw = new StreamWriter(sRoot + "\\Logs" + "\\recorder-rewrite-debug-" + sFile + ".txt");
      TextWriter twOut = new StreamWriter(sRoot + "\\" + Path.GetFileNameWithoutExtension(sFile) + "1.txt");
      try {
        tw.WriteLine("=================================================");
        tw.WriteLine("Debug Start - " + DateTime.Now.ToString());
        tw.WriteLine("=================================================");
        using (var file = new StreamReader(sRoot + "\\" + sFile)) {
          twOut.WriteLine("RTPropertyUniqueIdentifier|RTDocumentIdentifier|JurisdictionCountyFIPS|DocumentNumber|Book|Page|DocumentDate|FilingDate|RecordingDate|DocumentCategoryType|DocumentTypeDescription|MultiParcel|ArmsLengthTransfer|Legal|Borrower|Grantor|Grantee|LenderName|CleanLenderName|ParentLenderName|MergerParentName|PartialInterest|SalePrice|LoanRateTypeDescription|LoanTypeDescription|NewLoan1Amount|NewLoan2Amount|NewLoanDocumentNumber|NewLoanInterestRate|CreateDate|LastUpdated|PublicationDate|ProcessIndicator|InMedianRange|ForeclosureAuctionSale|InvestorBulkBuy|AbsentOwner|DistressCircumstanceDescription|SaleTermsDescription|SaleTypeDescription|TransferTypeDescription|RTUniqueFCIdentifier|AVMatDocRecordingDate|SourceCountyName|SourceMunicipalityName|DocumentNumberFormatted|APNOriginal|SiteAddressRaw|MailAddressRaw|MailAddressHouseNumber|MailAddressFraction|MailAddressDirection|MailAddressStreetName|MailAddressSuffix|MailAddressPostDirection|MailAddressUnitPrefix|MailAddressUnitValue|MailCity|MailState|MailZip|MailZip4|MailCRRT|Quitclaim|LegalBlock|LegalLot|LegalPlatBook|LegalPlatPage|LegalRange|LegalSection|LegalSubDivision|LegalTownship|LegalTract|LegalUnit");
          int iProcessed = 1;
          int iTotal = 1;
          string line;
          while ((line = file.ReadLine()) != null) {
            iProcessed++;
            iTotal++;
            twOut.WriteLine(line);
          }
        }
      }
      catch (Exception e) {
        tw.WriteLine(e.Message);
        bSussess = false;
      }
      finally {
        tw.Close();
        tw.Dispose();
        twOut.Close();
        twOut.Dispose();
      }
      return bSussess;
    }

    private static void csv_ParseError(object sender, ParseErrorEventArgs e) {
      // if the error is that a field is missing, then skip to next line
      if (e.Error is MissingFieldCsvException) {
        //Log.Write(e.Error, "--MISSING FIELD ERROR OCCURRED!" + Environment.NewLine);
        e.Action = ParseErrorAction.AdvanceToNextLine;
      }
      else if (e.Error is MalformedCsvException) {
        //Log.Write(e.Error, "--MALFORMED CSV ERROR OCCURRED!" + Environment.NewLine);
        e.Action = ParseErrorAction.AdvanceToNextLine;
      }
      else {
        //Log.Write(e.Error, "--UNKNOWN PARSE ERROR OCCURRED!" + Environment.NewLine);
        e.Action = ParseErrorAction.AdvanceToNextLine;
      }
    }

    public static void bcp_SqlRowsCopied(object sender, SqlRowsCopiedEventArgs e)
    {

    }



    private static bool CheckTaxAssessorRecord(string[] data, TextWriter tw) {

      //column count check
      if (data.Length != 196) {
        lock (GlobalVariables.locker) {
          tw.WriteLine("incorrect column length - " + string.Join("|", data));
        }
        return false;
      }

      //NULLS and Empty Strings are for all columns because they are nullable.


      //check int columns
      if (!TestInt(data[0]) ||
          !TestInt(data[1]) ||
          !TestInt(data[35]) ||
          !TestInt(data[37]) ||
          !TestInt(data[39]) ||
          !TestInt(data[40]) ||
          !TestInt(data[47]) ||
          !TestInt(data[48]) ||
          !TestInt(data[49]) ||
          !TestInt(data[50]) ||
          !TestInt(data[51]) ||
          !TestInt(data[52]) ||
          !TestInt(data[53]) ||
          !TestInt(data[54]) ||
          !TestInt(data[55]) ||
          !TestInt(data[56]) ||
          !TestInt(data[62]) ||
          !TestInt(data[63]) ||
          !TestInt(data[64]) ||
          !TestInt(data[65]) ||
          !TestInt(data[66]) ||
          !TestInt(data[70]) ||
          !TestInt(data[71]) ||
          !TestInt(data[72]) ||
          !TestInt(data[73]) ||
          !TestInt(data[74]) ||
          !TestInt(data[75]) ||
          !TestInt(data[76]) ||
          !TestInt(data[77]) ||
          !TestInt(data[78]) ||
          !TestInt(data[79]) ||
          !TestInt(data[80]) ||
          !TestInt(data[81]) ||
          !TestInt(data[82]) ||
          !TestInt(data[83]) ||
          !TestInt(data[84]) ||
          !TestInt(data[85]) ||
          !TestInt(data[126]) ||
          !TestInt(data[128]) ||
          !TestInt(data[130]) ||
          !TestInt(data[131]) ||
          !TestInt(data[132]) ||
          !TestInt(data[133]) ||
          !TestInt(data[134]) ||
          !TestInt(data[136]) ||
          !TestInt(data[137]) ||
          !TestInt(data[151]) ||
          !TestInt(data[152]) ||
          !TestInt(data[156]) ||
          !TestInt(data[157]) ||
          !TestInt(data[160]) ||
          !TestInt(data[169]) ||
          !TestInt(data[170]) ||
          !TestInt(data[172]) ||
          !TestInt(data[174]) ||
          !TestInt(data[180]) ||
          !TestInt(data[195]))
      {
        tw.WriteLine("int column failed - " + string.Join("|", data));
        return false;
      }

      //check decimal      
      if (!TestDecimal(data[22]) ||
          !TestDecimal(data[23]) ||
          !TestDecimal(data[36]) ||
          !TestDecimal(data[38]) ||
          !TestDecimal(data[46]) ||
          !TestDecimal(data[155]))
      {
        tw.WriteLine("decimal column failed - " + string.Join("|", data));
        return false;
      }


      //check for date columns
      if (!TestDate(data[122]) ||
          !TestDate(data[123]) ||
          !TestDate(data[124]) ||
          !TestDate(data[127]) ||
          !TestDate(data[129]) ||
          !TestDate(data[135]) ||
          !TestDate(data[192])
        )
      {
        tw.WriteLine("date column failed - " + string.Join("|", data));
        return false;
      }

      return true;
    }

    private static bool CheckRecorderRecord(string[] data, TextWriter tw) {

      //column count check
      if (data.Length != 73) {
        lock (GlobalVariables.locker) {
          tw.WriteLine("incorrect column length - " + string.Join("|", data));
        }
        return false;
      }

      //NULLS and Empty Strings are for all columns because they are nullable.

      //check bit columns
      if (!TestBoolean(data[11]) ||
          !TestBoolean(data[12]) ||
          !TestBoolean(data[21]) ||
          !TestBoolean(data[33]) ||
          !TestBoolean(data[34]) ||
          !TestBoolean(data[35]) ||
          !TestBoolean(data[36]) ||
          !TestBoolean(data[62])) {
        tw.WriteLine("bit column failed - " + string.Join("|", data));
        return false;
      }

      //check int columns
      if (!TestInt(data[0]) ||
          !TestInt(data[1]) ||
          !TestInt(data[22]) ||
          !TestInt(data[25]) ||
          !TestInt(data[26]) ||
          !TestInt(data[41]) ||
          !TestInt(data[42])) {
        tw.WriteLine("int column failed - " + string.Join("|", data));
        return false;
      }

      //check decimal      
      if (!TestDecimal(data[28])) {
        tw.WriteLine("decimal column failed - " + string.Join("|", data));
        return false;
      }


      //check for date columns
      if (!TestDate(data[6]) ||
          !TestDate(data[7]) ||
          !TestDate(data[8]) ||
          !TestDate(data[29]) ||
          !TestDate(data[30]) ||
          !TestDate(data[31])) {
        tw.WriteLine("date column failed - " + string.Join("|", data));
        return false;
      }

      return true;
    }

    private static bool TestBoolean(string sIn) {
      sIn = sIn.Replace("\"", "");
      if (string.IsNullOrEmpty(sIn)) return true;

      try {
        bool bOut;
        return bool.TryParse(sIn, out bOut);
      }
      catch {
        return false;
      }
    }

    private static bool TestInt(string sIn) {
      sIn = sIn.Replace("\"", "");
      if (string.IsNullOrEmpty(sIn)) return true;

      try {
        int bOut;
        return int.TryParse(sIn, out bOut);
      }
      catch {
        return false;
      }
    }

    private static bool TestDecimal(string sIn) {
      sIn = sIn.Replace("\"", "");
      if (string.IsNullOrEmpty(sIn)) return true;

      try {
        decimal bOut;
        return decimal.TryParse(sIn, out bOut);
      }
      catch {
        return false;
      }
    }

    private static bool TestDate(string sIn) {
      sIn = sIn.Replace("\"", "");
      if (string.IsNullOrEmpty(sIn)) return true;

      try {
        DateTime bOut;
        return DateTime.TryParse(sIn, out bOut);
      }
      catch {
        return false;
      }
    }



  }
}
