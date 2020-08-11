using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewPort.Models
{
    public class ImageInfo
    {
        private string _eq_Name;
        int _sdip_No;
        string _sdip_Result;

        public ImageInfo(string EqName, int SDIPNo, string SDIPResult)
        {
            _eq_Name = EqName;
            _sdip_No = SDIPNo;
            _sdip_Result = SDIPResult;

        }
        public string Eq_Name { get { return _eq_Name; } }
        public int SDIP_No { get { return _sdip_No; } set { _sdip_No = value; } }
        public string SDIP_Result { get { return _sdip_Result; } set { _sdip_Result = value; } }

    }
}
