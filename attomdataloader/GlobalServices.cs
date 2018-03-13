using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Forms;
using RestSharp;
using System.Text;
using System.IO;

namespace attomdataloader
{
  public class GlobalServices
  {
    private delegate void SetTextCallback(string formName, string ctrlName, string text);

    //private delegate void SetProgressBarCallback(string formName, string ctrlName, int min, int max, int val);

    public static IRestResponse SendFileStats(string subject, string body)
    {
      try
      {
        RestClient client = new RestClient();
        client.BaseUrl = new Uri("https://api.mailgun.net/v2");
        client.Authenticator = new HttpBasicAuthenticator("api", Properties.Settings.Default.SMTPAPIKey);
        RestRequest request = new RestRequest();
        request.AddParameter("domain", "myreipro.com", ParameterType.UrlSegment);
        request.Resource = "{domain}/messages";
        request.AddParameter("from", "server@myreipro.com");
        request.AddParameter("to", "bmanry@myreipro.com");
        request.AddParameter("subject", subject);
        request.AddParameter("text", body);

        request.Method = Method.POST;
        return client.Execute(request);
      }
      catch
      {
        return null;
      }
    }

    public static void SetText(string formName, string ctrlName, string text)
    {
            Console.WriteLine(text);
            GlobalWriter(text);
            //var openForm = (frmMain) System.Windows.Forms.Application.OpenForms[formName];
            //if (openForm != null)
            //{
            //  var ctrl = openForm.Controls.Find(ctrlName, true);
            //  if (ctrl.Length == 1)
            //  {
            //    if (ctrl[0].InvokeRequired)
            //    {
            //      var d = new SetTextCallback(SetText);
            //      ctrl[0].Invoke(d, new object[] {formName, ctrlName, text});
            //    }
            //    else
            //    {
            //      ctrl[0] = text;
            //    }
            //  }
            //  else
            //  {
            //    var ctrl2 = openForm.statusStrip1.Items.Find(ctrlName, true);
            //    if (ctrl2.Length == 1) {
            //      if (openForm.statusStrip1.InvokeRequired) {
            //        var d = new SetTextCallback(SetText);
            //        openForm.statusStrip1.Invoke(d, new object[] { formName, ctrlName, text });
            //      }
            //      else {
            //        ctrl2[0] = text;
            //      }
            //    }
            //  }
            //}
        }

    public static void AppendText(string formName, string ctrlName, string text) {

      Console.WriteLine(text);

       GlobalWriter(text);

      //      var openForm = (frmMain)System.Windows.Forms.Application.OpenForms[formName];
      //if (openForm != null) {
      //  var ctrl = openForm.Controls.Find(ctrlName, true);
      //  if (ctrl.Length == 1) {
      //    if (ctrl[0].InvokeRequired) {
      //      var d = new SetTextCallback(AppendText);
      //      ctrl[0].Invoke(d, new object[] { formName, ctrlName, text });
      //    }
      //    else {
      //      ctrl[0] += Environment.NewLine + text;
      //    }
      //  }
      //  else {
      //    var ctrl2 = openForm.statusStrip1.Items.Find(ctrlName, true);
      //    if (ctrl2.Length == 1) {
      //      if (openForm.statusStrip1.InvokeRequired) {
      //        var d = new SetTextCallback(AppendText);
      //        openForm.statusStrip1.Invoke(d, new object[] { formName, ctrlName, text });
      //      }
      //      else {
      //        ctrl2[0] = text;
      //      }
      //    }
      //  }
      //}
        }

        //public static void SetProgressBar(string formName, string ctrlName, int min, int max, int val)
        //{
        //  var openForm = (frmMain) System.Windows.Forms.Application.OpenForms[formName];
        //  if (openForm != null)
        //  {
        //    var ctrl = openForm.Controls.Find(ctrlName, true);
        //    if (ctrl.Length == 1)
        //    {
        //      if (ctrl[0].InvokeRequired)
        //      {
        //        var d = new SetProgressBarCallback(SetProgressBar);
        //        ctrl[0].Invoke(d, new object[] {formName, ctrlName, min, max, val});
        //      }
        //      else
        //      {
        //        ((ProgressBar) ctrl[0]).Minimum = min;
        //        ((ProgressBar) ctrl[0]).Maximum = max;
        //        ((ProgressBar) ctrl[0]).Value = val;           
        //      }
        //    }
        //  }
        //}

        public static bool GlobalWriter(string text)
        {
            string GlobalLogFile = Properties.Settings.Default.GlobalLogFile;
            string fileDatePrefix = DateTime.Now.DayOfWeek.ToString() + "-"+ DateTime.Now.Day.ToString() + "-" + DateTime.Now.Year.ToString();
            GlobalVariables.iTotalRecords = 0;
            StreamWriter StrWrt;
            try
            { 
                if (!Directory.Exists(GlobalLogFile)) Directory.CreateDirectory(GlobalLogFile);
                string DailyFile = GlobalLogFile + "\\" + "logs-" + fileDatePrefix +".txt";
                FileInfo fi = new FileInfo(DailyFile);
                if (fi.Exists)
                {
                    StrWrt = new StreamWriter(DailyFile, true);
                    StrWrt.WriteLine("-----" + string.Format("{0:dd-MMM-yyyy hh:mm:ss tt}", DateTime.Now));
                    StrWrt.WriteLine(text);

                }
                else
                {
                    StrWrt = new StreamWriter(DailyFile);
                    StrWrt.WriteLine("-----" + string.Format("{0:dd-MMM-yyyy hh:mm:ss tt}", DateTime.Now));
                    StrWrt.WriteLine(text);
                }
                StrWrt.Flush();
                StrWrt.Close();
                }
                catch (Exception e)
                {
                    return false;
                }

            return true;
            }


        }
    

 

    public static class StringHelper {

    #region SplitQuotedString
    // used internally as a marker as to where the code is when splitting the text
    private enum LineIndexFieldState {
      StartField = 1,            // Start of a field 
      EndQuote = 2,            // We have a End Quote can only be found in InQuoteFieldDate
      Delimiter = 3,            // We are currently at a delimiter 
      InFiledData = 4,        // We are in copy of data and looking for a delimiter 
      InQuoteFieldDate = 5,    // We are in Quoted field and looking for an End Quote 
    }

    private static bool IsCurrentCharADelimiter(char testChar, char[] Delimiters) {
      bool result = false;
      foreach (char delimeter in Delimiters) {
        if (delimeter == testChar) {
          result = true;
          break;
        }
      }
      return result;
    }

    public static StringCollection SplitQuotedString(char[] Delimiters, char Quote, string input) {
      LineIndexFieldState state = LineIndexFieldState.StartField;
      System.Text.StringBuilder FieldData = new System.Text.StringBuilder();
      StringCollection result = new StringCollection();

      char[] datalineArray = input.ToCharArray();
      for (int i = 0; i < datalineArray.Length; i++) {
        char CurrentChar = datalineArray[i];
        switch (state) {
          #region LineIndexFieldState.StartField
          case LineIndexFieldState.StartField: {
              if (CurrentChar == Quote)
                state = LineIndexFieldState.InQuoteFieldDate;
              else if (IsCurrentCharADelimiter(CurrentChar, Delimiters))
                state = LineIndexFieldState.Delimiter;
              else {
                // it was not a quote or a delimiter so state is InFiledData
                state = LineIndexFieldState.InFiledData;
                FieldData.Append(CurrentChar);
              }
              break;
            }
          #endregion
          #region LineIndexFieldState.InFiledData
          case LineIndexFieldState.InFiledData: {
              // We are in the field data and looking for a delimiter 
              if (IsCurrentCharADelimiter(CurrentChar, Delimiters))
                state = LineIndexFieldState.Delimiter;
              else
                FieldData.Append(CurrentChar);
              break;
            }
          #endregion
          #region LineIndexFieldState.InQuoteFieldDate:
          case LineIndexFieldState.InQuoteFieldDate: {
              // We are in the field data and looking for an End Quote..
              if (CurrentChar == Quote) {
                if (i != datalineArray.Length - 1) // if true we are at end of line so ignore
                                 {
                  if (datalineArray[i + 1] == Quote) // if the next char is also a quote 
                                     {
                    // if should be a single quote so add a single quote and inc the counter
                    FieldData.Append(CurrentChar);
                    i++;
                  }
                  else // it is the end of the delimter 
                                     {
                    state = LineIndexFieldState.EndQuote;
                  }
                }
              }
              else {
                FieldData.Append(CurrentChar);
              }

              break;
            }
          #endregion
          #region LineIndexFieldState.EndQuote:
          case LineIndexFieldState.EndQuote: {
              // we have hit the end quote so look for the delimiter and ignore all until delimiter is hit..
              if (IsCurrentCharADelimiter(CurrentChar, Delimiters))
                state = LineIndexFieldState.Delimiter;
              break;
            }
          #endregion
        }
        #region LineIndexFieldState.Delimiter:
        if (state == LineIndexFieldState.Delimiter) {
          result.Add(FieldData.ToString());
          // Clear the existing Field and create a container to populate the data..
          FieldData = new System.Text.StringBuilder();
          state = LineIndexFieldState.StartField;
        }
        #endregion
      }
      // Automatic end of line code so add the last field.
      result.Add(FieldData.ToString());
      // Copies the collection to a new array starting at index 0.
      return result;
    }



    public static StringCollection SplitQuotedString(char Delimiter, char Quote, string input) {
      // create a single array to hold the single delimiter
      char[] Delimiters = new char[1];
      Delimiters[0] = Delimiter;
      // call the overloaded fucntion
      return SplitQuotedString(Delimiters, Quote, input);
    }

    public static StringCollection SplitQuotedString(char Delimiter, string input) {
      return SplitQuotedString(Delimiter, '"', input);
    }

    #endregion

    #region ParsePascalOrCamelString
    public static StringCollection ParsePascalOrCamelString(string input) {
      StringCollection result = new StringCollection();

      if (input != null) {
        if (input.Length == 0)
          result.Add(string.Empty);
        else {
          int wordStartIndex = 0;
          char[] letters = input.ToCharArray();
          // Skip the first letter. we don't care what case it is.
          for (int i = 1; i < letters.Length; i++) {
            if (char.IsUpper(letters[i])) {
              //Grab everything before the current index.
              result.Add(new string(letters, wordStartIndex, i - wordStartIndex));
              wordStartIndex = i;
            }
          }
          //We need to have the last word.
          result.Add(new string(letters, wordStartIndex, letters.Length - wordStartIndex));
        }
      }
      return result;
    }
    #endregion
  } 
}
