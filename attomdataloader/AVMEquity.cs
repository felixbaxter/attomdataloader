using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace attomdataloader {
  public class AVMEquity
  {
    public int RTPropertyUniqueIdentifier { get; set; }
    public int EstimatedValue { get; set; }
    public int EstimatedValueLow { get; set; }
    public int EstimatedValueHigh { get; set; }
    public int OpenLoanAmount1 { get; set; }
    public string OpenLoanDocNumber1 { get; set; }
    public int OpenLoanAmount2 { get; set; }
    public string OpenLoanDocNumber2 { get; set; }
    public int OpenLoanAmount3 { get; set; }
    public string OpenLoanDocNumber3 { get; set; }
    public int Equity { get; set; }
    public DateTime CalculationDate { get; set; }
  }
}
