using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewPort.Models
{
    public class ImageInfo
    {
        string SDIP_NO, Delete_Check;
        string Lot_ID, Review_DefectName, SDIP_Result;
        string Equipment_DefectName, Image_name, xLocation, yLocation;
        string imageSize;
        int Frame_No, Camera_No;
        string changeCode;


        public ImageInfo(string LotID, string ImageName, int CamNo, int FrameNo, string Equipment_DF, string sdip_no, string sdip_result, string Review_DF, string DeleteNo, string XLocation, string YLocation, string imageSize, string Change_Code)
        {
           
            Lot_ID = LotID;
            //File_ID = FileID;
            Image_name = ImageName;
            Camera_No = CamNo;
            Frame_No = FrameNo;
            Delete_Check = DeleteNo;
            Equipment_DefectName = Equipment_DF;
            Review_DefectName = Review_DF;
                       
            SDIP_NO = sdip_no;
            SDIP_Result = sdip_result;
            xLocation = XLocation;
            yLocation = YLocation;
            ImageSize = imageSize;
            changeCode = Change_Code;

        }
        
        public string LotID { get { return Lot_ID; } set { Lot_ID = value; } }
       
        public string ReviewDefectName { get { return Review_DefectName; } set { Review_DefectName = value; } }

        public string Imagename { get { return Image_name; } set { Image_name = value; } }
        //public string FileID { get { return File_ID; } set { File_ID = value; } }
        public string EquipmentDefectName { get { return Equipment_DefectName; } set { Equipment_DefectName = value; } }

        public string sdip_no { get { return SDIP_NO; } set { SDIP_NO = value; } }
        public string sdip_result { get { return SDIP_Result; } set { SDIP_Result = value; } }
        public int FrameNo { get { return Frame_No; } set { Frame_No = value; } }
        public int CameraNo { get { return Camera_No; } set { Camera_No = value; } }

        public string DeleteCheck { get { return Delete_Check; } set { Delete_Check = value; } }

        public string X_Location { get { return xLocation; } set { xLocation = value; } }
        public string Y_Location { get { return yLocation; } set { yLocation = value; } }

        public string ImageSize { get { return imageSize; } set { imageSize = value; } }

        public string Change_Code { get { return changeCode; } set { changeCode = value; } }

    }
}
