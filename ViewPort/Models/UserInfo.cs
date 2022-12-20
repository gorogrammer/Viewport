using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewPort.Models
{
    public class UserInfo
    {
        public int SuLot;
        public int WorkTime;
        public int IdleTime;
        public int ImageSpeed;
        public int PassImage;
        public int ViewImage;
        
        public UserInfo(int suLot,int workTime,int idleTime,int imageSpeed,int passImage,int viewImage)
        {
            SuLot = suLot;
            WorkTime = workTime;
            IdleTime = idleTime;
            ImageSpeed = imageSpeed;
            PassImage = passImage;
            ViewImage = viewImage;
        }
    }
}
