using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewPort.Models
{
    public class txtInfo
    {
        public string _eq_Name;
        public string _sdip_No;
        public string _sdip_Result;
        public string _reviewDefectName;

        public txtInfo(string EqName, string SDIPNo, string SDIPResult, string ReviewDefectName)
        {
            _eq_Name = EqName;
            _sdip_No = SDIPNo;
            _sdip_Result = SDIPResult;
        }
        public string Eq_Name { get { return _eq_Name; } set { _eq_Name = value; } }
        public string SDIP_No { get { return _sdip_No; } set { _sdip_No = value; } }
        public string SDIP_Result { get { return _sdip_Result; } set { _sdip_Result = value; } }
    }
}
