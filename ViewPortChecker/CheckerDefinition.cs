using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewPortChecker
{
    public class CHECKER_STR
    {
        public const string CHECKER = "Checker";
        public const string INSTORAGE = @"C:\Program Files (x86)\Carlo\ViewPort";
        public const string PATH = @"C:\Program Files (x86)\Carlo\ViewPort\ViewPort.exe";
        public const string NAME = "ViewPort.exe";
        public const string PROC_NAME = "ViewPort";
    }

    public class MSG_STR
    {
        public const string PING_NAS = "CARLO NAS에 접속중입니다..";
        public const string PING_DB = "CARLO DB에 접속중입니다..";
        public const string CHECK_VER = "최신 버전을 확인중입니다..";
        public const string CHECK_VER_THIS = "현재 설치된 버전을 확인중입니다..";
        public const string UPDATE_VER = "업데이트에 성공했습니다.";
        public const string CHECK_VER_OK = "최신 버전입니다.";
        public const string EXCUTE_READY= "ViewPort 실행을 준비합니다.";


        public const string ERR_PING_NAS = "CARLO NAS에 접속할 수 없습니다. 이 PC의 네트워크 상태와 CARLO NAS 상태를 확인해주세요.";
        public const string ERR_PING_DB = "CARLO DB에 접속할 수 없습니다. 이 PC의 네트워크 상태와 CARLO DB의 상태를 확인해 주세요.";
        public const string ERR_CHECK_VER = "최신 버전 정보를 확인해 오는 데 실패했습니다.";
        public const string ERR_CHECK_VER_THIS = "현재 설치된 버전 정보를 확인해 오는 데 실패했습니다.";
        public const string ERR_UPDATE_VER = "업데이트에 실패했습니다.";
    }

}
