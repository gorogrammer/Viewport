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
        public string _x_Location;
        public string _y_Location;

        public txtInfo(string EqName, string SDIPNo, string SDIPResult, string ReviewDefectName, string XLocation, string YLocation)
        {
            _eq_Name = EqName;
            _sdip_No = SDIPNo;
            _sdip_Result = SDIPResult;
            _x_Location = XLocation;
            _y_Location = YLocation;
        }
        public string Eq_Name { get { return _eq_Name; } set { _eq_Name = value; } }
        public string SDIP_No { get { return _sdip_No; } set { _sdip_No = value; } }
        public string SDIP_Result { get { return _sdip_Result; } set { _sdip_Result = value; } }

        public string X_Location { get { return _x_Location; } set { _x_Location = value; } }

        public string Y_Location { get { return _y_Location; } set { _y_Location = value; } }
    }
}
