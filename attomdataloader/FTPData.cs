using System;
using System.IO;
using System.Linq;
using System.Threading;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using WinSCP;

namespace attomdataloader
{
    public class FTPData
    {



        public static bool CheckForFiles()
        {
            string remoteDirectory = "/" + Properties.Settings.Default.FTPDownloadDir + "/";
            string localDirectory = Properties.Settings.Default.RootFolderExt;
            bool bReturn = false;



            try
            {
                // Setup session options
                SessionOptions sessionOptions = new SessionOptions
                {
                    Protocol = Protocol.Sftp,
                    HostName = Properties.Settings.Default.FTPAddress,
                    UserName = Properties.Settings.Default.FTPUserName,
                    Password = Properties.Settings.Default.FTPPassword,
                    SshHostKeyFingerprint = Properties.Settings.Default.SshHostKeyFingerprint
                };


                using (WinSCP.Session session = new WinSCP.Session())
                {

                    // Connect
                    session.Open(sessionOptions);

                    //List files
                    TransferOptions transferOptions = new TransferOptions();
                    transferOptions.TransferMode = TransferMode.Binary;

                    RemoteDirectoryInfo files = session.ListDirectory(remoteDirectory);


                    foreach (RemoteFileInfo file in files.Files)
                    {
                        if (file.Name.ToLower().Contains(".zip"))
                        {

                            GlobalServices.SetText("frmMain", "lFTPStatus", "Checking " + file.Name);


                            if (!FileInloadStats.CheckRecordExists(file.Name))
                            {

                                long total = file.Length > 0 ? file.Length : 1;
                                int i = 0;
                                GlobalServices.SetText("frmMain", "lFTPStatus", "Checking " + file.Name);
                                //GlobalServices.SetProgressBar("frmMain", "pbProgress", 0, Convert.ToInt32(total), 0);
                                GlobalServices.SetText("frmMain", "lFTPStatus", "Downloading File: " + file.Name);

                                Console.WriteLine("Starting Download of file - " + file.Name + " Size - " + file.Length.ToString() + " Time - " + DateTime.Now.ToString());
                                // Download files
                                TransferOperationResult transferOperationResult = session.GetFiles(session.EscapeFileMask(remoteDirectory + file.Name), localDirectory);


                                // Check and throw if there are any errors with the transfer operation.
                                transferOperationResult.Check();

                                if (transferOperationResult.IsSuccess)
                                {
                                    var newFile = new FileInloadStats()
                                    {
                                        FileName = file.Name
                                    };
                                    FileInloadStats.InsertRecord(newFile);
                                    Console.WriteLine("Completed Download of file - " + file.Name + " Size - " + file.Length.ToString() + " Time - " + DateTime.Now.ToString());

                                }

                            }
                        }

                    }
                    bReturn = true;
                }
            }
            catch (Exception e)
            {

                bReturn = false;
                return bReturn;
            }
            return bReturn;
        }
    }
                    
}
