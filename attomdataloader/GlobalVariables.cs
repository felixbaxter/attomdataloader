using System;
using System.Collections.Generic;

namespace attomdataloader
{
  class GlobalVariables
  {
    public static string DbConnection = "Server=.;Database=realtytrac;Trusted_Connection=True;";
    public static bool KeepGoing = true;
    public static DateTime dtStartTaxAssessor;
    public static DateTime dtEndTaxAssessor;
    public static DateTime dtStartForeclosure;
    public static DateTime dtEndForeclosure;
    public static DateTime dtStartRecorder;
    public static DateTime dtEndRecorder;
    public static DateTime dtStartAvm;
    public static DateTime dtEndAvm;
    public static long iTotalRecords;
    public static int iBatchSize;
    public static int iNotifyAfter;
    public static string sFileNameCheck;
    public static int iTaxAssessorRecords;
    public static int iForeclosureRecords;
    public static int iRecorderRecords;
    public static int iAvmRecords;
    public static int iNumberOfErrors;
    public static string sTaxAssessorErrors;
    public static string sForeclosureErrors;
    public static string sRecorderErrors;
    public static string sAvmErrors;
    public static string sGlobalErrors;
    public static bool bTaxAssessorProcessedOk;
    public static bool bForeclosureProcessedOk;
    public static bool bRecorderProcessedOk;
    public static bool bAvmProcessedOk;
    public static bool bProcessMT;
    public static Object locker = new Object();

    public static List<string> errorList = new List<string>(); 
  }
}
