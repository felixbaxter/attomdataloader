using System;
using System.IO;
using Ionic.Zip;
using attomdataloader.Properties;

namespace attomdataloader
{
  class Unzip {
    public static string[] Extract(string args) {

      if (!File.Exists(args)) {
        return null;
      }
      string sFileNames = "";
      var fileInfo = new FileInfo(args);      
      string zipToUnpack = fileInfo.FullName;
      string unpackDirectory = fileInfo.DirectoryName;
      using (ZipFile zip1 = ZipFile.Read(zipToUnpack))
      {
        zip1.Password = Settings.Default.FTPPassword;
        // here, we extract every entry, but we could extract conditionally
        // based on entry name, size, date, checkbox status, etc.  
        foreach (ZipEntry e in zip1) {         
            sFileNames += Path.GetDirectoryName(args) + "\\" + e.FileName + ",";
            e.Extract(unpackDirectory, ExtractExistingFileAction.OverwriteSilently);         
        }
      }
      sFileNames += sFileNames.Remove(sFileNames.Length - 1, 1);
      return sFileNames.Split(',');
    }

    public static string[] ExtractForeclosureOnly(string args) {

      if (!File.Exists(args)) {
        return null;
      }
      string sFileNames = "";
      var fileInfo = new FileInfo(args);
      string zipToUnpack = fileInfo.FullName;
      string unpackDirectory = fileInfo.DirectoryName;
      using (ZipFile zip1 = ZipFile.Read(zipToUnpack)) {
        zip1.Password = Settings.Default.FTPPassword;
        // here, we extract every entry, but we could extract conditionally
        // based on entry name, size, date, checkbox status, etc.  
        foreach (ZipEntry e in zip1) {
          if (e.FileName.StartsWith("Foreclosure")) {
            sFileNames += Path.GetDirectoryName(args) + "\\" + e.FileName + ",";
            e.Extract(unpackDirectory, ExtractExistingFileAction.OverwriteSilently);
          }
        }
      }
      sFileNames += sFileNames.Remove(sFileNames.Length - 1, 1);
      return sFileNames.Split(',');
    }
  }
}
