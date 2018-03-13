using attomdataloader.Properties;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WinSCP;
using static System.Net.Mime.MediaTypeNames;


namespace attomdataloader
{
    class Program
    {
        public static string txtBatchSize = "";
        public static string txtNotifyAfter = "";
        public static string txtRootFolder = "";
        public static string txtTaxAssessorFile = "";
        public static string txtForeclosureFile = "";
        public static string txtRecorderFile = "";

        public static string txtAVMFile = "";
        public static string txtTaxAssessorTable = "";
        public static string txtForeclosureTable = "";
        public static string txtAVMTable = "";
        public static string txtRecorderTable = "";
        public static string txtSMTPAPIKey = "";
        public static List<string> lbFileList = new List<string>();
        public static List<string> lbNotProcessed = new List<string>();
        //public static System.Windows.Forms.CheckBox cbStopAfterFile = new System.Windows.Forms.CheckBox();
        public static bool cbStopAfterFile;


        static void Main()
        {
            try
            {
                //if(args== null) { }
            txtBatchSize = Properties.Settings.Default.BatchSize;
            txtNotifyAfter = Properties.Settings.Default.NotifyAfter;
            txtRootFolder = Properties.Settings.Default.RootFolderExt;
            txtTaxAssessorFile = Properties.Settings.Default.TaxAssessorPrefix;
            txtForeclosureFile = Properties.Settings.Default.ForeclosurePrefix;
            txtRecorderFile = Properties.Settings.Default.RecorderPrefix;
            txtAVMFile = Properties.Settings.Default.AVMPrefix;
            txtTaxAssessorTable = Properties.Settings.Default.TaxAssessorTable;
            txtForeclosureTable = Properties.Settings.Default.ForeclosureTable;
            txtRecorderTable = Properties.Settings.Default.RecorderTable;
            txtAVMTable = Properties.Settings.Default.AVMTable;
            txtSMTPAPIKey = Properties.Settings.Default.SMTPAPIKey;
            cbStopAfterFile = false;

                var d = new DirectoryInfo(txtRootFolder);
                var fArray = d.GetFiles("*.zip");

                foreach (FileInfo f in fArray)
                {
                    lbFileList.Add(f.Name);
                }

                lbNotProcessed.Clear();
                var filesToProcess = FileInloadStats.FilesToProcess();
                if (filesToProcess != null)
                {
                    foreach (var f in filesToProcess)
                    {
                        if (!f.FileProcessed) lbNotProcessed.Add(f.FileName + " - ALL");
                        if (!f.TaxAssessorSussessful) lbNotProcessed.Add(f.FileName + " - TaxAssessor");
                        if (!f.ForeclosureSussessful) lbNotProcessed.Add(f.FileName + " - Foreclosure");
                        if (!f.RecorderSussessful) lbNotProcessed.Add(f.FileName + " - Recorder");
                        if (!f.AVMLoaded) lbNotProcessed.Add(f.FileName + " - AVM");
                    }
                }

                var FilesToProcess = FTPData.CheckForFiles();
                if (FilesToProcess)
                {
                    ProcessFilesNow();
                }
            }catch(Exception ex)
            {
                EventLog.WriteEntry("Application", ex.Message,System.Diagnostics.EventLogEntryType.Error);
            }

        }

        private static void ProcessFilesNow()
        {
            //Process All Files that haven't already been processed
            var filesToProcess = FileInloadStats.FilesToProcess();
            foreach (var file in filesToProcess)
            {
                DateTime dOVerallStart = DateTime.Now;
                try
                {
                    //check that the file actually exists
                    if (File.Exists(Settings.Default.RootFolderExt + "\\" + file.FileName))
                    {
                        var sEmailBody = new StringBuilder();
                        //track time to process          
                        lock (GlobalVariables.locker)
                        {
                            GlobalVariables.sGlobalErrors = "";
                            GlobalVariables.iBatchSize = int.Parse(txtBatchSize);
                            GlobalVariables.iNotifyAfter = int.Parse(txtNotifyAfter);
                            GlobalVariables.iTaxAssessorRecords = 0;
                            GlobalVariables.iForeclosureRecords = 0;
                            GlobalVariables.iRecorderRecords = 0;
                            GlobalVariables.iAvmRecords = 0;
                            GlobalVariables.iNumberOfErrors = 0;
                            GlobalVariables.sTaxAssessorErrors = "";
                            GlobalVariables.sForeclosureErrors = "";
                            GlobalVariables.sRecorderErrors = "";
                            GlobalVariables.sAvmErrors = "";
                            GlobalVariables.bTaxAssessorProcessedOk = false;
                            GlobalVariables.bForeclosureProcessedOk = false;
                            GlobalVariables.bRecorderProcessedOk = false;
                            GlobalVariables.bAvmProcessedOk = false;
                        }
                        //first, unzip the file          
                        GlobalServices.SetText("frmMain", "lStatus", "Extracting: " + file.FileName);
                        var s = Unzip.Extract(Settings.Default.RootFolderExt + "\\" + file.FileName);
                        if (s.Length > 0)
                        {
                            GlobalServices.SetText("frmMain", "lStatus", "Extracting: " + " Truncating Import Tables");
                            LoadData.TruncateTaxAssessorImport();
                            LoadData.TruncateForeclosureImport();
                            LoadData.TruncateRecorderImport();
                            LoadData.DropTaxAssessorImportPrimaryKey();
                            GlobalServices.SetText("frmMain", "lStatus", "Processing Files");
                            if (!file.TaxAssessorSussessful)
                            {
                                GlobalVariables.bTaxAssessorProcessedOk = LoadTaxAssesssorFile();
                                //TEMPORARY DISABLE 0N 10/2 TO SEE PERFORMANCE OF MERGE STATEMENTIN BATCHES
                                ProcessTaxAssessorImport(file.FileInloadId);
                                WorkCompleted("TaxAssessor");
                                file.TaxAssessorRecordCount = GlobalVariables.iTaxAssessorRecords;
                                file.TaxAssessorSussessful = GlobalVariables.bTaxAssessorProcessedOk;
                                sEmailBody.AppendLine("Tax Assessor Record Count: " + file.TaxAssessorRecordCount.ToString("N0") + " (" + file.TaxAssessorSussessful.ToString() + ")");
                                if (!string.IsNullOrEmpty(GlobalVariables.sTaxAssessorErrors))
                                {
                                    sEmailBody.AppendLine("Tax Assessor ERROR: " + GlobalVariables.sTaxAssessorErrors);
                                }
                            }
                            else
                            {
                                //clean up extracted files that are not processed
                                lock (GlobalVariables.locker)
                                {
                                    var d = new DirectoryInfo(txtRootFolder);
                                    var fArray = d.GetFiles(txtTaxAssessorFile + "*.txt");
                                    foreach (FileInfo f in fArray)
                                    {
                                        if (File.Exists(txtRootFolder + "\\" + f.Name))
                                        {
                                            File.Delete(txtRootFolder + "\\" + f.Name);
                                        }
                                    }
                                }
                                sEmailBody.AppendLine("Tax Assessor: NOT PROCESSED");
                            }
                            if (!file.ForeclosureSussessful)
                            {
                                GlobalVariables.bForeclosureProcessedOk = LoadForeclosureFile();
                                ProcessForeclosureImport(file.FileInloadId);
                                WorkCompleted("Foreclosure");
                                file.ForeclosureRecordCount = GlobalVariables.iForeclosureRecords;
                                file.ForeclosureSussessful = GlobalVariables.bForeclosureProcessedOk;
                                sEmailBody.AppendLine("Foreclosure Record Count: " + file.ForeclosureRecordCount.ToString("N0") + " (" + file.ForeclosureSussessful.ToString() + ")");
                                if (!string.IsNullOrEmpty(GlobalVariables.sForeclosureErrors))
                                {
                                    sEmailBody.AppendLine("Foreclosure ERROR: " + GlobalVariables.sForeclosureErrors);
                                }
                            }
                            else
                            {
                                //clean up extracted files that are not processed
                                lock (GlobalVariables.locker)
                                {
                                    var d = new DirectoryInfo(txtRootFolder);
                                    var fArray = d.GetFiles(txtForeclosureFile + "*.txt");
                                    foreach (FileInfo f in fArray)
                                    {
                                        if (File.Exists(txtRootFolder + "\\" + f.Name))
                                        {
                                            File.Delete(txtRootFolder + "\\" + f.Name);
                                        }
                                    }
                                }
                                sEmailBody.AppendLine("Foreclosure: NOT PROCESSED");
                            }
                            if (!file.RecorderSussessful)
                            {
                                GlobalVariables.bRecorderProcessedOk = LoadRecorderFile();
                                ProcessRecorderImport(file.FileInloadId);
                                WorkCompleted("Recorder");
                                file.RecorderRecordCount = GlobalVariables.iRecorderRecords;
                                file.RecorderSussessful = GlobalVariables.bRecorderProcessedOk;
                                sEmailBody.AppendLine("Recorder Record Count: " + file.RecorderRecordCount.ToString("N0") + " (" + file.RecorderSussessful.ToString() + ")");
                                if (!string.IsNullOrEmpty(GlobalVariables.sRecorderErrors))
                                {
                                    sEmailBody.AppendLine("Recorder ERROR: " + GlobalVariables.sRecorderErrors);
                                }
                            }
                            else
                            {
                                //clean up extracted files that are not processed
                                lock (GlobalVariables.locker)
                                {
                                    var d = new DirectoryInfo(txtRootFolder);
                                    var fArray = d.GetFiles(txtRecorderFile + "*.txt");
                                    foreach (FileInfo f in fArray)
                                    {
                                        if (File.Exists(txtRootFolder + "\\" + f.Name))
                                        {
                                            File.Delete(txtRootFolder + "\\" + f.Name);
                                        }
                                    }
                                }
                                sEmailBody.AppendLine("Recorder: NOT PROCESSED");
                            }
                            if (!file.AVMLoaded)
                            {
                                GlobalVariables.bAvmProcessedOk = LoadAvmFile();
                                //Bo Making Change here to push processing to a database function.  This system will only load the avm import table starting 01/17/2016
                                //if (GlobalVariables.iAvmRecords > 0) ProcessAVMImport(file.FileInloadId);
                                //in this case, we only want to process the import table b/c the Stored Proc will truncate the main table.  Only do it if we had incoming data
                                WorkCompleted("AVM");
                                file.AVMRecordCount = GlobalVariables.iAvmRecords;
                                file.AVMLoaded = GlobalVariables.bAvmProcessedOk;
                                file.AVMSussessful = false; //forcing false here to database will pickup and process
                                sEmailBody.AppendLine("AVM Record Count: " + file.AVMRecordCount.ToString("N0") + " (" + file.AVMLoaded.ToString() + ")");
                                if (!string.IsNullOrEmpty(GlobalVariables.sAvmErrors))
                                {
                                    sEmailBody.AppendLine("AVM ERROR: " + GlobalVariables.sAvmErrors);
                                }
                            }
                            else
                            {
                                //clean up extracted files that are not processed
                                lock (GlobalVariables.locker)
                                {
                                    var d = new DirectoryInfo(txtRootFolder);
                                    var fArray = d.GetFiles(txtAVMFile + "*.txt");
                                    foreach (FileInfo f in fArray)
                                    {
                                        if (File.Exists(txtRootFolder + "\\" + f.Name))
                                        {
                                            File.Delete(txtRootFolder + "\\" + f.Name);
                                        }
                                    }
                                }
                                sEmailBody.AppendLine("AVM: NOT PROCESSED");
                            }
                        }
                        File.Move(Settings.Default.RootFolderExt + "\\" + file.FileName, Settings.Default.RootFolderExt + "\\Zips\\" + file.FileName);
                        //Finished.  
                        //update record to processed with status                                   
                        file.FileProcessed = true;
                        FileInloadStats.UpdateRecord(file);
                        DateTime dOVerallEnd = DateTime.Now;
                        TimeSpan span = dOVerallEnd - dOVerallStart;
                        double totalMinutes = span.TotalMinutes;
                        if (GlobalVariables.iNumberOfErrors > 0)
                        {
                            sEmailBody.AppendLine("ERRORS DID HAPPEN - CHECK ALL LOG FILES");
                        }
                        sEmailBody.AppendLine("===================================");
                        sEmailBody.AppendLine(file.FileName + " processed in " + totalMinutes.ToString("N2") + " minutes.");
                        string sBody = sEmailBody.ToString();
                        string sSubject = file.FileName + " processed in " + totalMinutes.ToString("N2") + " minutes.";
                        if (GlobalVariables.iNumberOfErrors > 0)
                        {
                            sSubject = file.FileName + " HAS ERRORS - processed in " + totalMinutes.ToString("N2") + " minutes.";
                        }
                        lock (GlobalVariables.locker)
                        {
                            GlobalServices.AppendText("frmMain", "txtTaxAssessor", "# Errors: " + GlobalVariables.iNumberOfErrors.ToString());
                            GlobalServices.AppendText("frmMain", "txtTaxAssessor", "About to send email");
                        }
                        GlobalServices.SendFileStats(sSubject, sBody);
                        lock (GlobalVariables.locker)
                        {
                            GlobalServices.AppendText("frmMain", "txtTaxAssessor", "Email Sent");
                        }
                    }
                    else
                    {
                        var sEmailBody = new StringBuilder();
                        sEmailBody.AppendLine(file.FileName + " MISSING ZIP FILE");
                        string sBody = sEmailBody.ToString();
                        string sSubject = file.FileName + " MISSING ZIP FILE";
                        GlobalServices.SendFileStats(sSubject, sBody);
                    }
                }
                catch (Exception ex)
                {
                    DateTime dOVerallEnd = DateTime.Now;
                    TimeSpan span = dOVerallEnd - dOVerallStart;
                    double totalMinutes = span.TotalMinutes;
                    //send email that file is done
                    //Send Email about Processed File
                    var sEmailBody = new StringBuilder();
                    sEmailBody.AppendLine(file.FileName + " processed in " + totalMinutes.ToString("N2") + " minutes.");
                    sEmailBody.AppendLine("===================================");
                    sEmailBody.AppendLine("Tax Assessor Record Count: " + file.TaxAssessorRecordCount.ToString("N0") + " (" +
                                          file.TaxAssessorSussessful.ToString() + ")");
                    sEmailBody.AppendLine("Foreclosure Record Count: " + file.ForeclosureRecordCount.ToString("N0") + " (" +
                                          file.ForeclosureSussessful.ToString() + ")");
                    sEmailBody.AppendLine("Recorder Record Count: " + file.RecorderRecordCount.ToString("N0") + " (" +
                                          file.RecorderSussessful.ToString() + ")");
                    sEmailBody.AppendLine("AVM Record Count: " + file.AVMRecordCount.ToString("N0") + " (" +
                                          file.AVMLoaded.ToString() + ")");
                    sEmailBody.AppendLine("===================================");
                    sEmailBody.AppendLine(ex.Message);

                    string sBody = sEmailBody.ToString();
                    string sSubject = file.FileName + " ERRORED in " + totalMinutes.ToString("N2") + " minutes.";
                    GlobalServices.SendFileStats(sSubject, sBody);
                    lock (GlobalVariables.locker)
                    {
                        GlobalServices.SetText("frmMain", "txtErrorFound", ex.Message);
                    }
                }

            }
            //Check for Dups table to be cleaned up.  This will keep notifying of dups in the dups table until extract and truncated.
            var iDupsRecordCount = FileInloadStats.GetTaxAssessorDupsCount();
            if (iDupsRecordCount != 0)
            {
                GlobalServices.SendFileStats("TAXASSESSOR DUPS FOUND", "There are " + iDupsRecordCount.ToString() + " duplicate in the TaxAssessorDups table.  Investigate Now.");
            }

            iDupsRecordCount = FileInloadStats.GetForeClosureDupsCount();
            if (iDupsRecordCount != 0)
            {
                GlobalServices.SendFileStats("FORECLOSURE DUPS FOUND", "There are " + iDupsRecordCount.ToString() + " duplicate in the ForeclosureDups table.  Investigate Now.");
            }

            var iNotProcessedCount = FileInloadStats.GetTaxAssessorNotProcessedCount();
            if (iNotProcessedCount != 0)
            {
                GlobalServices.SendFileStats("ProcessingBatchesTA BATCHES NOT PROCESSED", "There are " + iNotProcessedCount.ToString() + " rows not procssed in the ProcessingBatchesTA table.  Investigate Now.");
            }
            iNotProcessedCount = FileInloadStats.GetForeclosureNotProcessedCount();
            if (iNotProcessedCount != 0)
            {
                GlobalServices.SendFileStats("ProcessingBatchesFC BATCHES NOT PROCESSED", "There are " + iNotProcessedCount.ToString() + " rows not procssed in the ProcessingBatchesFC table.  Investigate Now.");
            }
            iNotProcessedCount = FileInloadStats.GetRecorderNotProcessedCount();
            if (iNotProcessedCount != 0)
            {
                GlobalServices.SendFileStats("ProcessingBatchesRC BATCHES NOT PROCESSED", "There are " + iNotProcessedCount.ToString() + " rows not procssed in the ProcessingBatchesRC table.  Investigate Now.");
            }
        }




        private static bool LoadTaxAssesssorFile()
        {
            bool bReturn = true;
            lock (GlobalVariables.locker)
            {
                GlobalVariables.iBatchSize = int.Parse(txtBatchSize);
                GlobalVariables.iNotifyAfter = int.Parse(txtNotifyAfter);
                GlobalVariables.dtStartTaxAssessor = DateTime.Now;
            }
            //get a list of files in the folder with the Prefix in txtTaxassessorFile
            var d = new DirectoryInfo(txtRootFolder);
            var fArray = d.GetFiles(txtTaxAssessorFile + "*.txt");
            foreach (FileInfo f in fArray)
            {
                GlobalVariables.dtStartTaxAssessor = DateTime.Now;
                GlobalServices.AppendText("frmMain", "txtTaxAssessor", "Starting: " + GlobalVariables.dtStartTaxAssessor.ToString());
                GlobalServices.AppendText("frmMain", "txtTaxAssessor", "TA - Cleaning Quotes");
                int iCurrentFileCount = 0;
                bool b1 = LoadData.CleanFileOfQutoes(txtRootFolder, f.Name, out iCurrentFileCount);
                GlobalVariables.iTaxAssessorRecords += iCurrentFileCount;
                //move file if Successful
                if (b1)
                {
                    GlobalServices.AppendText("frmMain", "txtTaxAssessor", "TA - Checking for Errors");
                    bool b2 = LoadData.CheckTaxAssessorDataForErrors(txtRootFolder, Path.GetFileNameWithoutExtension(f.Name) + "_P.txt");
                    if (b2)
                    {
                        GlobalServices.AppendText("frmMain", "txtTaxAssessor", "TA - Loading File");
                        bool b = LoadData.LoadTaxAccessorFile(txtRootFolder, Path.GetFileNameWithoutExtension(f.Name) + "_P2.txt");
                        bReturn = b;
                        //delete file if Successful
                        f.Delete();

                        if (File.Exists(txtRootFolder + "\\" + Path.GetFileNameWithoutExtension(f.Name) + "_P.txt"))
                        {
                            File.Delete(txtRootFolder + "\\" + Path.GetFileNameWithoutExtension(f.Name) + "_P.txt");
                        }

                        if (File.Exists(txtRootFolder + "\\" + Path.GetFileNameWithoutExtension(f.Name) + "_P2.txt"))
                        {
                            File.Delete(txtRootFolder + "\\" + Path.GetFileNameWithoutExtension(f.Name) + "_P2.txt");
                        }

                        if (File.Exists(txtRootFolder + "\\" + Path.GetFileNameWithoutExtension(f.Name) + "_P_E.txt"))
                        {
                            var fCheck =
                              new FileInfo(txtRootFolder + "\\" + Path.GetFileNameWithoutExtension(f.Name) + "_P_E.txt");
                            if (fCheck.Length == 0)
                            {
                                fCheck.Delete();
                            }
                        }
                    }
                    else
                    {
                        f.Delete();
                        //Send Email about Processed File
                        string sBody = f.Name + " Error ";
                        string sSubject = f.Name;
                        sSubject += " - Error";
                        GlobalServices.SendFileStats(sSubject, sBody);
                    }
                }
                else
                {
                    f.Delete();
                    //Send Email about Processed File
                    string sBody = f.Name + " Error ";
                    string sSubject = f.Name;
                    sSubject += " - Error";
                    GlobalServices.SendFileStats(sSubject, sBody);
                }
                if (cbStopAfterFile) break;
            }
            return bReturn;
        }

        private static bool LoadForeclosureFile()
        {
            bool bReturn = true;
            lock (GlobalVariables.locker)
            {
                GlobalVariables.iBatchSize = int.Parse(txtBatchSize);
                GlobalVariables.iNotifyAfter = int.Parse(txtNotifyAfter);
                GlobalVariables.dtStartForeclosure = DateTime.Now;
            }
            GlobalServices.AppendText("frmMain", "txtForeclosure", "Starting: " + GlobalVariables.dtStartForeclosure.ToString());
            //get a list of files in the folder with the Prefix in txtTaxassessorFile
            var d = new DirectoryInfo(txtRootFolder);
            var fArray = d.GetFiles(txtForeclosureFile + "*.txt");
            foreach (FileInfo f in fArray)
            {
                GlobalServices.AppendText("frmMain", "txtForeclosure", "FC - Cleaing Quotes");
                int iCurrentFileCount = 0;
                bool b1 = LoadData.CleanFileOfQutoes(txtRootFolder, f.Name, out GlobalVariables.iForeclosureRecords);
                GlobalVariables.iForeclosureRecords += iCurrentFileCount;
                //move file if Successful
                if (b1)
                {
                    GlobalServices.AppendText("frmMain", "txtForeclosure", "FC - Loading File");
                    bool b = LoadData.LoadForeclosureFile(txtRootFolder, f.Name);
                    bReturn = b;
                    //move file if Successful
                    f.Delete();

                    if (File.Exists(txtRootFolder + "\\" + Path.GetFileNameWithoutExtension(f.Name) + "_P.txt"))
                    {
                        File.Delete(txtRootFolder + "\\" + Path.GetFileNameWithoutExtension(f.Name) + "_P.txt");
                    }
                }
                else
                {
                    f.Delete();
                    //Send Email about Processed File
                    string sBody = f.Name + " Error ";
                    string sSubject = f.Name;
                    sSubject += " - Error";
                    GlobalServices.SendFileStats(sSubject, sBody);
                }
               if (cbStopAfterFile) break;
            }
            return bReturn;
        }
 
        private static bool LoadRecorderFile()
        {
            bool bReturn = true;
            lock (GlobalVariables.locker)
            {
                GlobalVariables.iBatchSize = int.Parse(txtBatchSize);
                GlobalVariables.iNotifyAfter = int.Parse(txtNotifyAfter);
                GlobalVariables.dtStartRecorder = DateTime.Now;
            }
            GlobalServices.AppendText("frmMain", "txtRecorder", "Starting: " + GlobalVariables.dtStartRecorder.ToString());
            //get a list of files in the folder with the Prefix in txtRecorderFile
            var d = new DirectoryInfo(txtRootFolder);
            var fArray = d.GetFiles(txtRecorderFile + "*.txt");
            foreach (FileInfo f in fArray)
            {
                GlobalServices.AppendText("frmMain", "txtRecorder", "RC - Cleaning Quotes");
                int iCurrentFileCount = 0;
                bool b1 = LoadData.CleanFileOfQutoes(txtRootFolder, f.Name, out iCurrentFileCount);
                GlobalVariables.iRecorderRecords += iCurrentFileCount;
                //move file if Successful
                if (b1)
                {
                    GlobalServices.AppendText("frmMain", "txtRecorder", "RC - Checking for Errors");
                    bool b2 = LoadData.CheckRecorderDataForErrors(txtRootFolder, Path.GetFileNameWithoutExtension(f.Name) + "_P.txt");
                    if (b2)
                    {
                        GlobalServices.AppendText("frmMain", "txtRecorder", "RC - Loading File");
                        bool b = LoadData.LoadRecorderFile(txtRootFolder, Path.GetFileNameWithoutExtension(f.Name) + "_P2.txt");
                        bReturn = b;
                        //move file if Successful
                        f.Delete();

                        if (File.Exists(txtRootFolder + "\\" + Path.GetFileNameWithoutExtension(f.Name) + "_P.txt"))
                        {
                            File.Delete(txtRootFolder + "\\" + Path.GetFileNameWithoutExtension(f.Name) + "_P.txt");
                        }

                        if (File.Exists(txtRootFolder + "\\" + Path.GetFileNameWithoutExtension(f.Name) + "_P2.txt"))
                        {
                            File.Delete(txtRootFolder + "\\" + Path.GetFileNameWithoutExtension(f.Name) + "_P2.txt");
                        }

                        if (File.Exists(txtRootFolder + "\\" + Path.GetFileNameWithoutExtension(f.Name) + "_P_E.txt"))
                        {
                            var fCheck =
                              new FileInfo(txtRootFolder + "\\" + Path.GetFileNameWithoutExtension(f.Name) + "_P_E.txt");
                            if (fCheck.Length == 0)
                            {
                                fCheck.Delete();
                            }

                        }
                    }
                    else
                    {
                        f.Delete();
                        //Send Email about Processed File
                        string sBody = f.Name + " Error ";
                        string sSubject = f.Name;
                        sSubject += " - Error";
                        GlobalServices.SendFileStats(sSubject, sBody);
                    }
                }
                else
                {
                    f.Delete();
                    //Send Email about Processed File
                    string sBody = f.Name + " Error ";
                    string sSubject = f.Name;
                    sSubject += " - Error";
                    GlobalServices.SendFileStats(sSubject, sBody);
                }
                if (cbStopAfterFile) break;
                TextWriter tw = new StreamWriter(txtRootFolder + "\\Logs" + "\\recorder-errors-" + f.Name + ".txt");
                foreach (var s in GlobalVariables.errorList)
                {
                    tw.WriteLine(s);
                }
                tw.Close();
                tw.Dispose();
            }
            return bReturn;
        }

        private static bool LoadAvmFile()
        {
            LoadData.TruncateAVMEquityImport();
            bool bReturn = true;
            lock (GlobalVariables.locker)
            {
                GlobalVariables.iBatchSize = int.Parse(txtBatchSize);
                GlobalVariables.iNotifyAfter = int.Parse(txtNotifyAfter);
                GlobalVariables.dtStartAvm = DateTime.Now;
            }
            GlobalServices.AppendText("frmMain", "txtAvm", "Starting: " + GlobalVariables.dtStartAvm.ToString());
            //get a list of files in the folder with the Prefix in txtTaxassessorFile
            var d = new DirectoryInfo(txtRootFolder);
            var fArray = d.GetFiles(txtAVMFile + "*.txt");
            foreach (FileInfo f in fArray)
            {
                GlobalServices.AppendText("frmMain", "txtAvm", "AVM - Cleaning Quotes");
                int iCurrentFileCount = 0;
                bool b1 = LoadData.CleanFileOfQutoes(txtRootFolder, f.Name, out iCurrentFileCount);
                GlobalVariables.iAvmRecords += iCurrentFileCount;
                //move file if Successful
                if (b1)
                {
                    GlobalServices.AppendText("frmMain", "txtAvm", "AVM - Loading File");
                    bool b = LoadData.LoadAVMFile(txtRootFolder, Path.GetFileNameWithoutExtension(f.Name) + "_P.txt");
                    bReturn = b;
                    //move file if Successful
                    f.Delete();

                    if (File.Exists(txtRootFolder + "\\" + Path.GetFileNameWithoutExtension(f.Name) + "_P.txt"))
                    {
                        File.Delete(txtRootFolder + "\\" + Path.GetFileNameWithoutExtension(f.Name) + "_P.txt");
                    }
                }
                if (cbStopAfterFile) break;
            }
            return bReturn;
        }

        private static void ProcessTaxAssessorImport(int iFileId)
        {
            lock (GlobalVariables.locker)
            {
                GlobalVariables.iBatchSize = int.Parse(txtBatchSize);
                GlobalVariables.iNotifyAfter = int.Parse(txtNotifyAfter);
            }
            GlobalServices.AppendText("frmMain", "txtTaxAssessor", "TA - Processing File");
            bool b = LoadData.ProcessTaxAccessorImport(iFileId);
            if (!b)
            {
                //Send Email about Processed File
                string sBody = "Process TaxAssessor Import Failed";
                string sSubject = "Process TaxAssessor Import Failed";
                GlobalServices.SendFileStats(sSubject, sBody);
            }
        }

        private static void ProcessForeclosureImport(int iFileId)
        {
            lock (GlobalVariables.locker)
            {
                GlobalVariables.iBatchSize = int.Parse(txtBatchSize);
                GlobalVariables.iNotifyAfter = int.Parse(txtNotifyAfter);
            }
            GlobalServices.AppendText("frmMain", "txtForeclosure", "FC - Processing File");
            bool b = LoadData.ProcessForeclosureImport(iFileId);
            if (!b)
            {
                //Send Email about Processed File
                string sBody = "Process Foreclosure Import Failed";
                string sSubject = "Process Foreclosure Import Failed";
                GlobalServices.SendFileStats(sSubject, sBody);
            }
        }

        private static void ProcessRecorderImport(int iFileId)
        {
            lock (GlobalVariables.locker)
            {
                GlobalVariables.iBatchSize = int.Parse(txtBatchSize);
                GlobalVariables.iNotifyAfter = int.Parse(txtNotifyAfter);
            }
            GlobalServices.AppendText("frmMain", "txtRecorder", "RC - Processing File");
            bool b = LoadData.ProcessRecorderImport(iFileId);
            if (!b)
            {
                //Send Email about Processed File
                string sBody = "Process Recorder Import Failed";
                string sSubject = "Process Recorder Import Failed";
                GlobalServices.SendFileStats(sSubject, sBody);
            }
        }

        private static void ProcessAVMImport(int iFileId)
        {
            lock (GlobalVariables.locker)
            {
                GlobalVariables.iBatchSize = int.Parse(txtBatchSize);
                GlobalVariables.iNotifyAfter = int.Parse(txtNotifyAfter);
            }
            GlobalServices.AppendText("frmMain", "txtAvm", "AVM - Processing File");
            bool b = LoadData.ProcessAVMImport(iFileId);
            if (!b)
            {
                //Send Email about Processed File
                string sBody = "Process AVM Import Failed";
                string sSubject = "Process AVM Import Failed";
                GlobalServices.SendFileStats(sSubject, sBody);
            }
        }

        private static void WorkCompleted(string whichone)
        {
            switch (whichone)
            {
                case "TaxAssessor":
                    lock (GlobalVariables.locker)
                    {
                        GlobalVariables.dtEndTaxAssessor = DateTime.Now;
                        TimeSpan span = GlobalVariables.dtEndTaxAssessor - GlobalVariables.dtStartTaxAssessor;
                        double totalMinutes = span.TotalMinutes;
                        GlobalServices.AppendText("frmMain", "txtTaxAssessor", "End Time: " + GlobalVariables.dtEndTaxAssessor.ToString());
                        GlobalServices.AppendText("frmMain", "txtTaxAssessor", "Elapsed Time: " + totalMinutes.ToString("N2") + " minutes");
                        GlobalServices.AppendText("frmMain", "txtTaxAssessor", "Record Counnt: " + GlobalVariables.iTaxAssessorRecords.ToString("N0"));
                        GlobalServices.AppendText("frmMain", "txtTaxAssessor", "Records Per Min: " + (GlobalVariables.iTaxAssessorRecords / totalMinutes).ToString("N0"));
                        GlobalServices.AppendText("frmMain", "txtTaxAssessor", "Records Per Sec: " + ((GlobalVariables.iTaxAssessorRecords / totalMinutes) / 60).ToString("N0"));
                        GlobalServices.AppendText("frmMain", "txtTaxAssessor", "Completed");
                    }
                    break;
                case "Foreclosure":
                    lock (GlobalVariables.locker)
                    {
                        GlobalVariables.dtEndForeclosure = DateTime.Now;
                        TimeSpan span = GlobalVariables.dtEndForeclosure - GlobalVariables.dtStartForeclosure;
                        double totalMinutes = span.TotalMinutes;
                        GlobalServices.AppendText("frmMain", "txtForeclosure", "End Time: " + GlobalVariables.dtEndForeclosure.ToString());
                        GlobalServices.AppendText("frmMain", "txtForeclosure", "Elapsed Time: " + totalMinutes.ToString("N2") + " minutes");
                        GlobalServices.AppendText("frmMain", "txtForeclosure", "Record Counnt: " + GlobalVariables.iForeclosureRecords.ToString("N0"));
                        GlobalServices.AppendText("frmMain", "txtForeclosure", "Records Per Min: " + (GlobalVariables.iForeclosureRecords / totalMinutes).ToString("N0"));
                        GlobalServices.AppendText("frmMain", "txtForeclosure", "Records Per Sec: " + ((GlobalVariables.iForeclosureRecords / totalMinutes) / 60).ToString("N0"));
                        GlobalServices.AppendText("frmMain", "txtForeclosure", "Completed");
                    }
                    break;
                case "Recorder":
                    lock (GlobalVariables.locker)
                    {
                        GlobalVariables.dtEndRecorder = DateTime.Now;
                        TimeSpan span = GlobalVariables.dtEndRecorder - GlobalVariables.dtStartRecorder;
                        double totalMinutes = span.TotalMinutes;
                        GlobalServices.AppendText("frmMain", "txtRecorder", "End Time: " + GlobalVariables.dtEndRecorder.ToString());
                        GlobalServices.AppendText("frmMain", "txtRecorder", "Elapsed Time: " + totalMinutes.ToString("N2") + " minutes");
                        GlobalServices.AppendText("frmMain", "txtRecorder", "Record Counnt: " + GlobalVariables.iRecorderRecords.ToString("N0"));
                        GlobalServices.AppendText("frmMain", "txtRecorder", "Records Per Min: " + (GlobalVariables.iRecorderRecords / totalMinutes).ToString("N0"));
                        GlobalServices.AppendText("frmMain", "txtRecorder", "Records Per Sec: " + ((GlobalVariables.iRecorderRecords / totalMinutes) / 60).ToString("N0"));
                        GlobalServices.AppendText("frmMain", "txtRecorder", "Completed");
                    }
                    break;
                case "AVM":
                    lock (GlobalVariables.locker)
                    {
                        GlobalVariables.dtEndAvm = DateTime.Now;
                        TimeSpan span = GlobalVariables.dtEndAvm - GlobalVariables.dtStartAvm;
                        double totalMinutes = span.TotalMinutes;
                        if (totalMinutes == 0) totalMinutes = 1;
                        GlobalServices.AppendText("frmMain", "txtAvm", "End Time: " + GlobalVariables.dtEndAvm.ToString());
                        GlobalServices.AppendText("frmMain", "txtAvm", "Elapsed Time: " + totalMinutes.ToString("N2") + " minutes");
                        GlobalServices.AppendText("frmMain", "txtAvm", "Record Counnt: " + GlobalVariables.iAvmRecords.ToString("N0"));
                        GlobalServices.AppendText("frmMain", "txtAvm", "Records Per Min: " + (GlobalVariables.iAvmRecords / totalMinutes).ToString("N0"));
                        GlobalServices.AppendText("frmMain", "txtAvm", "Records Per Sec: " + ((GlobalVariables.iAvmRecords / totalMinutes) / 60).ToString("N0"));
                        GlobalServices.AppendText("frmMain", "txtAvm", "Completed");
                    }
                    break;
                case "FTP":
                case "ProcessFiles":
                    lock (GlobalVariables.locker)
                    {
                        if (GlobalVariables.bTaxAssessorProcessedOk && GlobalVariables.bForeclosureProcessedOk &&
                            GlobalVariables.bRecorderProcessedOk && GlobalVariables.bAvmProcessedOk)
                        {
                            GlobalServices.SetText("frmMain", "lStatus", "Completed");
                        }
                        else
                        {
                            GlobalServices.SetText("frmMain", "lStatus", "Errored");
                        }
                    }

                    break;
            }

        }

    }
}
