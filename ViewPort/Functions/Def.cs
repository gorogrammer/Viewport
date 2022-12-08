using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ViewPort.Functions
{
    public class MachineInfo
    {
        public const string SOI_STR = "SOI";
        public const string TOI_STR = "TOI";
        public const string EAOI_STR = "EAOI";
        public const string FVI_STR = "FVI";

        public const string TOI_REEL = "T";
        public const string SOI_1M_STR = "SOI_1M";
        public const string SOI_2M_STR = "SOI_2M";

        #region SOI_1M
        public const string SOI_1M_01 = "2016005";
        public const string SOI_1M_02 = "2016011";
        public const string SOI_1M_03 = "2016010";
        public const string SOI_1M_04 = "2016009";
        public const string SOI_1M_05 = "2017023";
        public const string SOI_1M_06 = "2017030";
        public const string SOI_1M_07 = "2018058";
        public const string SOI_1M_08 = "2018065";
        public const string SOI_1M_09 = "2019001";
        public const string SOI_1M_10 = "2019004";
        public const string SOI_1M_11 = "2019005";
        public const string SOI_1M_12 = "2020017";
        public const string SOI_1M_01_STR = "SOI_1M_01";
        public const string SOI_1M_02_STR = "SOI_1M_02";
        public const string SOI_1M_03_STR = "SOI_1M_03";
        public const string SOI_1M_04_STR = "SOI_1M_04";
        public const string SOI_1M_05_STR = "SOI_1M_05";
        public const string SOI_1M_06_STR = "SOI_1M_06";
        public const string SOI_1M_07_STR = "SOI_1M_07";
        public const string SOI_1M_08_STR = "SOI_1M_08";
        public const string SOI_1M_09_STR = "SOI_1M_09";
        public const string SOI_1M_10_STR = "SOI_1M_10";
        public const string SOI_1M_11_STR = "SOI_1M_11";
        public const string SOI_1M_12_STR = "SOI_1M_12";
        #endregion

        #region SOI_2M
        public const string SOI_2M_01 = "2017017";
        public const string SOI_2M_02 = "2017036";
        public const string SOI_2M_01_STR = "SOI_2M_01";
        public const string SOI_2M_02_STR = "SOI_2M_02";
        #endregion

        #region EAOI_2M
        public const string EAOI_2M_01 = "2013001";
        public const string EAOI_2M_02 = "2015028";
        public const string EAOI_2M_04 = "2016001";
        public const string EAOI_2M_05 = "2018034";
        public const string EAOI_2M_01_STR = "EAOI_2M_01";
        public const string EAOI_2M_02_STR = "EAOI_2M_02";
        public const string EAOI_2M_04_STR = "EAOI_2M_04";
        public const string EAOI_2M_05_STR = "EAOI_2M_05";
        #endregion

        #region TOI_1M
        public const string TOI_1M_01_A = "2007010";
        public const string TOI_1M_01_B = "2007011";
        public const string TOI_1M_02_A = "2004078";
        public const string TOI_1M_02_B = "2004079";
        public const string TOI_1M_01_A_STR = "TOI_1M_01_A";
        public const string TOI_1M_01_B_STR = "TOI_1M_01_B";
        public const string TOI_1M_02_A_STR = "TOI_1M_02_A";
        public const string TOI_1M_02_B_STR = "TOI_1M_02_B";

        //20200103 異붽� 
        public const string TOI_1M_03_A = "2020001"; // 1.5
        public const string TOI_1M_03_B = "2020002"; // 2.0
        public const string TOI_1M_03_A_STR = "TOI_1M_03_A";
        public const string TOI_1M_03_B_STR = "TOI_1M_03_B";
        #endregion

        #region FVI_1M
        public const string FVI_1M_01 = "2007086";
        public const string FVI_1M_02 = "2007087";
        public const string FVI_1M_03 = "2007088";
        public const string FVI_1M_04 = "2008017";
        public const string FVI_1M_05 = "2008018";
        public const string FVI_1M_06 = "2008019";
        public const string FVI_1M_07 = "2008020";
        public const string FVI_1M_08 = "2008024";
        public const string FVI_1M_09 = "2008025";
        public const string FVI_1M_10 = "2008026";
        public const string FVI_1M_11 = "2008027";
        public const string FVI_1M_12 = "2008031";
        public const string FVI_1M_13 = "2008032";
        public const string FVI_1M_14 = "2008033";
        public const string FVI_1M_15 = "2009006";
        public const string FVI_1M_16 = "2009007";
        public const string FVI_1M_17 = "2009008";
        public const string FVI_1M_18 = "2009009";
        public const string FVI_1M_19 = "2009010";
        public const string FVI_1M_20 = "2009011";
        public const string FVI_1M_21 = "2009012";
        public const string FVI_1M_22 = "2010029";
        public const string FVI_1M_23 = "2010030";
        public const string FVI_1M_24 = "2010039";
        public const string FVI_1M_25 = "2010044";
        public const string FVI_1M_30 = "2011006";
        public const string FVI_1M_33 = "2012006";
        public const string FVI_1M_35 = "2014021";
        public const string FVI_1M_36 = "2014022";
        public const string FVI_1M_01_STR = "FVI_1M_01";
        public const string FVI_1M_02_STR = "FVI_1M_02";
        public const string FVI_1M_03_STR = "FVI_1M_03";
        public const string FVI_1M_04_STR = "FVI_1M_04";
        public const string FVI_1M_05_STR = "FVI_1M_05";
        public const string FVI_1M_06_STR = "FVI_1M_06";
        public const string FVI_1M_07_STR = "FVI_1M_07";
        public const string FVI_1M_08_STR = "FVI_1M_08";
        public const string FVI_1M_09_STR = "FVI_1M_09";
        public const string FVI_1M_10_STR = "FVI_1M_10";
        public const string FVI_1M_11_STR = "FVI_1M_11";
        public const string FVI_1M_12_STR = "FVI_1M_12";
        public const string FVI_1M_13_STR = "FVI_1M_13";
        public const string FVI_1M_14_STR = "FVI_1M_14";
        public const string FVI_1M_15_STR = "FVI_1M_15";
        public const string FVI_1M_16_STR = "FVI_1M_16";
        public const string FVI_1M_17_STR = "FVI_1M_17";
        public const string FVI_1M_18_STR = "FVI_1M_18";
        public const string FVI_1M_19_STR = "FVI_1M_19";
        public const string FVI_1M_20_STR = "FVI_1M_20";
        public const string FVI_1M_21_STR = "FVI_1M_21";
        public const string FVI_1M_22_STR = "FVI_1M_22";
        public const string FVI_1M_23_STR = "FVI_1M_23";
        public const string FVI_1M_24_STR = "FVI_1M_24";
        public const string FVI_1M_25_STR = "FVI_1M_25";
        public const string FVI_1M_30_STR = "FVI_1M_30";
        public const string FVI_1M_33_STR = "FVI_1M_33";
        public const string FVI_1M_35_STR = "FVI_1M_35";
        public const string FVI_1M_36_STR = "FVI_1M_36";
        #endregion

        #region FVI_2M
        public const string FVI_2M_37 = "2014040";
        public const string FVI_2M_38 = "2015001";
        public const string FVI_2M_39 = "2015030";
        public const string FVI_2M_37_STR = "FVI_2M_37";
        public const string FVI_2M_38_STR = "FVI_2M_38";
        public const string FVI_2M_39_STR = "FVI_2M_39";
        #endregion
    }
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
    public class EQ_STR
    {
        public const string DEFAULT = "Normal";
        public const string SHORT = "SHORT";
        public const string SPIN = "돌기";
        public const string OPEN = "OPEN";
        public const string MB = "MB";
        public const string DISCOLORATION = "변색";
        public const string TOP = "TOP결함";
        public const string SR = "SR";
    }
    public class FORM_STR
    {
        public const string DLForm = "DL";
        public const string ViewPort = "VP";
        public const string DBForm = "DB";
    }
   
}
