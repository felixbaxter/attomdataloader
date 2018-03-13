using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace attomdataloader
{
  public class FileInloadStats {
    public int FileInloadId { get; set; }
    public string FileName { get; set; }
    public bool FileRemoteDeleted { get; set; }
    public int TaxAssessorRecordCount { get; set; }
    public bool TaxAssessorSussessful { get; set; }
    public int ForeclosureRecordCount { get; set; }
    public bool ForeclosureSussessful { get; set; }
    public int RecorderRecordCount { get; set; }
    public bool RecorderSussessful { get; set; }
    public int AVMRecordCount { get; set; }
    public bool AVMSussessful { get; set; }
    public bool AVMLoaded { get; set; }
    public bool FileProcessed { get; set; }
    public DateTime CreatedOn { get; set; }

    public FileInloadStats() {
      FileRemoteDeleted = false;
      TaxAssessorRecordCount = 0;
      TaxAssessorSussessful = false;
      ForeclosureRecordCount = 0;
      ForeclosureSussessful = false;
      RecorderRecordCount = 0;
      RecorderSussessful = false;
      AVMRecordCount = 0;      
      AVMSussessful = false;
      AVMLoaded = false;
      FileProcessed = false;
    }

    public static bool CheckRecordExists(string fileName) {
      try {
        using (var destinationConnection = new SqlConnection(GlobalVariables.DbConnection)) {
          destinationConnection.Open();
          var sql = "select count(1) from FileInloadStats where FileName = '{0}'";
          sql = String.Format(sql, fileName);
          var command = new SqlCommand(sql, destinationConnection);
          var output = (int)command.ExecuteScalar();
          destinationConnection.Close();
          if (output == 1) return true;
          return false;
        }
      }
      catch (Exception ex) {
        return false;
      }
    }

    public static bool InsertRecord(FileInloadStats filestats) {
      try {
                Console.WriteLine("Insert Routine started time - " + DateTime.Now);
        using (var destinationConnection = new SqlConnection(GlobalVariables.DbConnection)) {
          destinationConnection.Open();
          var sql =
            "insert into FileInloadStats (FileName, FileRemoteDeleted, TaxAssessorRecordCount, TaxAssessorSussessful, ForeclosureRecordCount, ForeclosureSussessful, RecorderRecordCount, RecorderSussessful, AVMRecordCount, AVMSussessful, AVMLoaded, FileProcessed, CreatedOn) values ('{0}',{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},'{11}','{12}')";
          sql = String.Format(sql, filestats.FileName, filestats.FileRemoteDeleted ? 1 : 0, filestats.TaxAssessorRecordCount,
            filestats.TaxAssessorSussessful ? 1 : 0, filestats.ForeclosureRecordCount, filestats.ForeclosureSussessful ? 1 : 0,
            filestats.RecorderRecordCount, filestats.RecorderSussessful ? 1 : 0, filestats.AVMRecordCount,
            filestats.AVMSussessful ? 1 : 0, filestats.AVMLoaded ? 1 : 0, filestats.FileProcessed ? 1 : 0, DateTime.Now);
          var command = new SqlCommand(sql, destinationConnection);
          command.ExecuteNonQuery();
          destinationConnection.Close();
        }
                Console.WriteLine("Insert Routine end time - " + DateTime.Now);
                return true;
      }
      catch (Exception ex) {
        return false;
      }
    }

    public static void UpdateRecord(FileInloadStats filestats) {
      try {
        using (var destinationConnection = new SqlConnection(GlobalVariables.DbConnection)) {
          destinationConnection.Open();
          var sql =
            "update FileInloadStats set TaxAssessorRecordCount={0}, TaxAssessorSussessful={1}, ForeclosureRecordCount={2}, ForeclosureSussessful={3}, RecorderRecordCount={4}, RecorderSussessful={5}, AVMRecordCount={6}, AVMSussessful={7}, AVMLoaded={8}, FileProcessed={9} where FileInloadId={10}";
          sql = String.Format(sql, filestats.TaxAssessorRecordCount, filestats.TaxAssessorSussessful ? 1 : 0,
            filestats.ForeclosureRecordCount, filestats.ForeclosureSussessful ? 1 : 0,
            filestats.RecorderRecordCount, filestats.RecorderSussessful ? 1 : 0, filestats.AVMRecordCount,
            filestats.AVMSussessful ? 1 : 0, filestats.AVMLoaded ? 1 : 0, filestats.FileProcessed ? 1 : 0, filestats.FileInloadId);
          var command = new SqlCommand(sql, destinationConnection);
          command.ExecuteNonQuery();
          destinationConnection.Close();
        }   
      }
      catch (Exception ex) {        
      }
    }

    public static List<FileInloadStats> FilesToProcess() {
      try {
        using (var destinationConnection = new SqlConnection(GlobalVariables.DbConnection)) {
          destinationConnection.Open();
          var sql = "select FileInloadId, FileName, FileRemoteDeleted, TaxAssessorRecordCount, TaxAssessorSussessful, ForeclosureRecordCount, ForeclosureSussessful, RecorderRecordCount, RecorderSussessful, AVMRecordCount, AVMSussessful, AVMLoaded, FileProcessed, CreatedOn from FileInloadStats where FileProcessed = 0 OR TaxAssessorSussessful = 0 OR ForeclosureSussessful = 0 OR RecorderSussessful = 0 OR AVMLoaded = 0 Order By FileInloadId";
          var command = new SqlCommand(sql, destinationConnection);
          var outputList = new List<FileInloadStats>();


          using (var reader = command.ExecuteReader()) {
            while (reader.Read()) {
              var r = new FileInloadStats() {
                FileInloadId = reader.GetInt32(0),
                FileName = reader.GetString(1),
                FileRemoteDeleted = reader.GetBoolean(2),
                TaxAssessorRecordCount = reader.GetInt32(3),
                TaxAssessorSussessful = reader.GetBoolean(4),
                ForeclosureRecordCount = reader.GetInt32(5),
                ForeclosureSussessful = reader.GetBoolean(6),
                RecorderRecordCount = reader.GetInt32(7),
                RecorderSussessful = reader.GetBoolean(8),
                AVMRecordCount = reader.GetInt32(9),
                AVMSussessful = reader.GetBoolean(10),
                AVMLoaded = reader.GetBoolean(11),
                FileProcessed = reader.GetBoolean(12),
                CreatedOn = reader.GetDateTime(13)
              };
              outputList.Add(r);
            }
          }
          destinationConnection.Close();
          return outputList;
        }
      }
      catch (Exception ex) {
        return null;
      }
    }

    public static int GetTaxAssessorDupsCount() {
      try {
        using (var destinationConnection = new SqlConnection(GlobalVariables.DbConnection)) {
          destinationConnection.Open();
          var sql = "select count(*) from TaxAssessorDups";
          var command = new SqlCommand(sql, destinationConnection);
          var outputList = new List<FileInloadStats>();

          int iRecordCount = 0;
          using (var reader = command.ExecuteReader()) {
            while (reader.Read())
            {
              iRecordCount = reader.GetInt32(0);
            }
          }
          destinationConnection.Close();
          return iRecordCount;
        }
      }
      catch (Exception ex) {
        return -1;
      }
    }

    public static int GetTaxAssessorNotProcessedCount() {
      try {
        using (var destinationConnection = new SqlConnection(GlobalVariables.DbConnection)) {
          destinationConnection.Open();
          var sql = "select count(*) from ProcessingBatchesTA where Processed=0";
          var command = new SqlCommand(sql, destinationConnection);
          var outputList = new List<FileInloadStats>();

          int iRecordCount = 0;
          using (var reader = command.ExecuteReader()) {
            while (reader.Read()) {
              iRecordCount = reader.GetInt32(0);
            }
          }
          destinationConnection.Close();
          return iRecordCount;
        }
      }
      catch (Exception ex) {
        return -1;
      }
    }

    public static int GetForeClosureDupsCount()
    {
      try
      {
        using (var destinationConnection = new SqlConnection(GlobalVariables.DbConnection))
        {
          destinationConnection.Open();
          var sql = "select count(*) from ForeclosureDups";
          var command = new SqlCommand(sql, destinationConnection);
          var outputList = new List<FileInloadStats>();

          int iRecordCount = 0;
          using (var reader = command.ExecuteReader())
          {
            while (reader.Read())
            {
              iRecordCount = reader.GetInt32(0);
            }
          }
          destinationConnection.Close();
          return iRecordCount;
        }
      }
      catch (Exception ex)
      {
        return -1;
      }
    }

    public static int GetForeclosureNotProcessedCount() {
      try {
        using (var destinationConnection = new SqlConnection(GlobalVariables.DbConnection)) {
          destinationConnection.Open();
          var sql = "select count(*) from ProcessingBatchesFC where Processed=0";
          var command = new SqlCommand(sql, destinationConnection);
          var outputList = new List<FileInloadStats>();

          int iRecordCount = 0;
          using (var reader = command.ExecuteReader()) {
            while (reader.Read()) {
              iRecordCount = reader.GetInt32(0);
            }
          }
          destinationConnection.Close();
          return iRecordCount;
        }
      }
      catch (Exception ex) {
        return -1;
      }
    }

    public static int GetRecorderNotProcessedCount() {
      try {
        using (var destinationConnection = new SqlConnection(GlobalVariables.DbConnection)) {
          destinationConnection.Open();
          var sql = "select count(*) from ProcessingBatchesRC where Processed=0";
          var command = new SqlCommand(sql, destinationConnection);
          var outputList = new List<FileInloadStats>();

          int iRecordCount = 0;
          using (var reader = command.ExecuteReader()) {
            while (reader.Read()) {
              iRecordCount = reader.GetInt32(0);
            }
          }
          destinationConnection.Close();
          return iRecordCount;
        }
      }
      catch (Exception ex) {
        return -1;
      }
    }

  }
}
