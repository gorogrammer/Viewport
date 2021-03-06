﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ViewPort.Functions
{
    public class MSG_STR
    {
        public const string NON_SELECTED = "파일이 선택되지 않았습니다";

        public const string LOAD_SUCCESS = "Load가 완료되었습니다.";

        public const string SUCCESS = "완료되었습니다.";

        public const string LOAD_ZIP = "zip files Loading...";

        public const string LOAD_SDIP_TXT = "SDIP txt files Loading...";

        public const string LOAD_ROWS = "Rows and JPG Loading...";

        public const string NONE_SDIP_TXT = "SDIP txt 파일이 없습니다.";

        public const string NONE_XY_TXT = "XY 좌표 txt 파일이 없습니다.";

        public const string NONE_MAP_TXT = "MAP txt 파일이 없습니다.";

        public const string NONE_DL_TXT = "DL txt 파일이 없습니다.";

    }

    public class FILTER_STR
    {
        public const string IMGSIZE_ALL = "전체";
    }

    public class COLUMN_STR
    {
        public const string GRID_IMGNAME = "이름";
        public const string GRID_STATE = "상태";
        public const string GRID_SELECT = "선택";
    }

    public class ZIP_STR
    {
        public const string EXETENSION = "Zip Files (*.zip)|*.zip";
    }
}
