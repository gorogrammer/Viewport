using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewPort.Functions
{
    public class UseInfomation
    {
        public string DeletePath { get; set; }
        public string Name { get; set; }
        public string EndWorker { get; set; }
        public string Authorization { get; set; }
        public string WorkTime { get; set; }
        public string EndTime { get; set; }
        public string TimeTaken { get; set; }
        public string LotImageCnt { get; set; }
        public string WorkImageCnt { get; set; }
        public string IdleWork { get; set; }
        public string IsFinallyWorker { get; set; }
        public bool OffLineMode { get; set; }
    }
    public class DeletePath
    {
        public string PathName { get; set; }
        public string MachineType { get; set; }
        public Enums.WORKTYPE WorkType { get; set; }
        [EditorAttribute(typeof(System.Windows.Forms.Design.FolderNameEditor), typeof(System.Drawing.Design.UITypeEditor))]   
        public string Path { get; set; }
    }
    public class User
    {
        public Enums.PERMISSION Authorization { get; set; }
    }
}
