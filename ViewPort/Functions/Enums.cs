using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewPort.Functions
{
    public class Enums
    {
        public  enum WORKTYPE
        {
            기본,
            미검,
            과검
        }
        public enum PERMISSION
        {
            None,
            admin,
            advanced,
            user
            
        }
        public enum DELETEPATHCOL
        {
            PathName,
            MachineType,
            WorkType,
            Path
        }
        public enum FILTERTYPE
        {
            NULL,
            XY,
            Frame,
            Camera,
            State,
            EQ
            
        }
    }
}
