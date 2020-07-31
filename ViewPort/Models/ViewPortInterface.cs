using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewPort.Models
{
    public interface MyInterface
    {
        int GetLoad_State();

        void GetFilterList(List<ImageListInfo> OutputData);
    }
}
