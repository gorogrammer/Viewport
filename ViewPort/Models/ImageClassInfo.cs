using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewPort.Models
{
    public class ImageListInfo
    {
        int Index;
        string Lot_ID, Verify_DefectName, DL_DefectName, Compare_DL_DefectName, Review_DefectName;
        string Image_name, Equipment_DefectName, Image_Size;
        int Frame_No, Camera_No;
        string File_Path;
        //테스트

        public ImageListInfo Clone()
        {
            return new ImageListInfo(Index, Lot_ID, Verify_DefectName, DL_DefectName, Compare_DL_DefectName, Review_DefectName, Image_name, Frame_No, Camera_No, Equipment_DefectName, Image_Size, File_Path);
        }

        public ImageListInfo(int _index, string LotID, string Veri_DF, string DL_DF, string Compare_DL_DF, string Review_DF, string ImageName, int FrameNo, int CamNo, string Equipment_DF, string ImageSize, string FilePath)
        {
            Index = _index;
            Lot_ID = LotID;
            Verify_DefectName = Veri_DF;
            DL_DefectName = DL_DF;
            Compare_DL_DefectName = Compare_DL_DF;
            Review_DefectName = Review_DF;
            Image_name = ImageName;
            Equipment_DefectName = Equipment_DF;
            Image_Size = ImageSize;
            Frame_No = FrameNo;
            Camera_No = CamNo;
            File_Path = FilePath;
        }
        public string[] GetData()
        {
            string[] Data = { Lot_ID, Verify_DefectName, DL_DefectName, Compare_DL_DefectName, Review_DefectName, Image_name, Frame_No.ToString(), Camera_No.ToString(), Equipment_DefectName, Image_Size };

            return Data;
        }
        public int index { get { return Index; } set { Index = value; } }
        public string LotID { get { return Lot_ID; } set { Lot_ID = value; } }
        public string VerifyDefectName { get { return Verify_DefectName; } set { Verify_DefectName = value; } }
        public string DLDefectName { get { return DL_DefectName; } set { DL_DefectName = value; } }
        public string CompareDLDefectName { get { return Compare_DL_DefectName; } set { Compare_DL_DefectName = value; } }
        public string ReviewDefectName { get { return Review_DefectName; } set { Review_DefectName = value; } }
        public string Imagename { get { return Image_name; } set { Image_name = value; } }
        public string EquipmentDefectName { get { return Equipment_DefectName; } set { Equipment_DefectName = value; } }
        public string ImageSize { get { return Image_Size; } set { Image_Size = value; } }
        public int FrameNo { get { return Frame_No; } set { Frame_No = value; } }
        public int CameraNo { get { return Camera_No; } set { Camera_No = value; } }
        public string FilePath { get { return File_Path; } set { File_Path = value; } }
    }
}
