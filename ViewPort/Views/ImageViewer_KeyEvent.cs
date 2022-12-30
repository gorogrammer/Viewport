using ViewPort.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ViewPort.Models;
using ViewPort.Functions;
using System.IO.Compression;
using System.IO;
using System.Diagnostics;

namespace ViewPort.Views
{
    public partial class ImageViewer : UserControl
    {
        private void ImageViewer_KeyDown(object sender, KeyEventArgs e)
        {
            this.Focus();
            if (Main != null)
            {
                if (e.Control)
                {
                    Main.InputKey += 1;
                    if (Main.S_Page_TB.Text == "" || int.Parse(Main.S_Page_TB.Text) <= 1)
                    {
                        //MessageBox.Show("첫 페이지 입니다.");
                        return;
                    }
                    else
                    {
                        Application.DoEvents();
                        Last_Picture_Selected_Index = -1;
                        Current_PageNum = int.Parse(Main.S_Page_TB.Text) - 1;
                        Main.S_Page_TB.Text = Current_PageNum.ToString();

                        if (!Main.EngrMode)
                        {
                            if (Main.Frame_View_CB.Checked)
                                Frame_Set_Image();
                            else
                                Set_Image();
                        }
                        else if (Main.EngrMode)
                        {

                            if (Main.Frame_View_CB.Checked)
                                Frame_Set_Image();
                            else if (Main.EngrMode && !normalCheck)
                                Set_Image_Eng();
                            else
                                Set_Image();

                        }

                        //this.Refresh();
                    }
                }
                else if (e.KeyCode == Keys.Enter)
                {
                    Main.InputKey += 1;
                    if (Main.ZipFilePath != null)
                    {
                        if (Main.ZipFilePath != "")
                            Main.Filter_Check();
                    }
                }
                //else if (e.Alt)
                //{
                //    e.Handled = true;

                //    if (Main.S_Page_TB.Text == "" || int.Parse(Main.S_Page_TB.Text) >= int.Parse(Main.E_Page_TB.Text))
                //    {
                //        MessageBox.Show("마지막 페이지 입니다.");
                //    }
                //    else
                //    {
                //        Last_Picture_Selected_Index = -1;
                //        Current_PageNum = int.Parse(Main.S_Page_TB.Text) + 1;
                //        Main.S_Page_TB.Text = Current_PageNum.ToString();

                //        //Set_PictureBox();

                //        if (Main.Frame_View_CB.Checked)
                //            Frame_Set_Image();
                //        else
                //            Set_Image();

                //    }

                //}

                else if (e.Shift && e.KeyCode == Keys.Delete)
                {
                    shift_del = 1;
                    if (Main.View_Mode_RB.Checked == true)
                    {
                        if (ViewMode_PSW_Check == 0)
                        {
                            ViewModePassword psw = new ViewModePassword(this);
                            psw.ShowDialog();

                        }
                        else
                        {
                            Key_shift_del();
                        }
                    }
                    else
                    {
                        Key_shift_del();
                    }

                    shift_del = 0;
                }

                else if (e.KeyCode == Keys.Delete)
                {
                    Main.InputKey += 1;
                    if (Main.View_Mode_RB.Checked == true)
                    {
                        if (ViewMode_PSW_Check == 0)
                        {
                            ViewModePassword psw = new ViewModePassword(this);
                            psw.ShowDialog();

                        }
                        else
                        {
                            Key_only_del();

                        }
                    }
                    else
                    {
                        Key_only_del();

                    }

                }

                else if (e.KeyCode == Keys.Z)
                {
                    Main.InputKey += 1;
                    if (Main.Frame_View_CB.Checked)
                    {
                        e.Handled = true;
                        if (Main.Frame_S_Page_TB.Text == "" || int.Parse(Main.Frame_S_Page_TB.Text) <= 1)
                        {
                            //MessageBox.Show("첫 페이지 입니다.");
                        }
                        else
                        {


                            Last_Picture_Selected_Index = -1;
                            Current_Frame_PageNum = int.Parse(Main.Frame_S_Page_TB.Text) - 1;
                            Main.Frame_S_Page_TB.Text = Current_Frame_PageNum.ToString();

                            Frame_Set_Image();



                        }

                    }
                    else
                        MessageBox.Show("Frame 별 체크 후에 사용 부탁드립니다.");
                    Main.List_Count_TB.Text = String.Format("{0:#,##0}", frame_dicInfo_Filter.Count);
                }

                else if (e.KeyCode == Keys.X)
                {
                    Main.InputKey += 1;
                    if (Main.Frame_View_CB.Checked)
                    {
                        e.Handled = true;
                        if (Main.Frame_S_Page_TB.Text == "" || int.Parse(Main.Frame_S_Page_TB.Text) >= int.Parse(Main.Frame_E_Page_TB.Text))
                        {
                            // MessageBox.ShASXDZZow("마지막 페이지 입니다.");
                        }
                        else
                        {

                            Current_PageNum = 1;
                            Last_Picture_Selected_Index = -1;
                            Current_Frame_PageNum = int.Parse(Main.Frame_S_Page_TB.Text) + 1;
                            Main.Frame_S_Page_TB.Text = Current_Frame_PageNum.ToString();

                            Frame_Set_Image();



                        }
                        Main.List_Count_TB.Text = String.Format("{0:#,##0}", frame_dicInfo_Filter.Count);
                    }
                    else
                        MessageBox.Show("Frame 별 체크 후에 사용 부탁드립니다.");

                }

                else if (e.KeyCode == Keys.L)
                {
                    Main.InputKey += 1;
                    if (Main.Dl_List_Main.Count == 0)
                    {
                        MessageBox.Show("텍스트 파일이 없습니다.");
                        return;
                    }
                    else
                    {
                        DL_ViewFrom DL = new DL_ViewFrom(this, Main);
                        DL.Dl_LIst_ADD(Main.Dl_List_Main);
                        DL.ShowDialog();

                    }



                }

                else if (e.KeyCode == Keys.F11)
                {
                    Main.InputKey += 1;
                    if (MessageBox.Show("[SDIP] 양품 판정된 이미지를 필터링합니다.", "알림", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        try
                        {
                            ProgressBar1 progressBar = new ProgressBar1();
                            progressBar.Text = "Filtering...";
                            progressBar.Show();
                            progressBar.SetProgressBarMaxSafe(DicInfo_Filtered.Count);
                            Before_No1_Filter_dicInfo = new Dictionary<string, ImageInfo>(DicInfo_Filtered);
                            DicInfo_Filtered = new Dictionary<string, ImageInfo>(Main.DicInfo);
                            foreach (string no in DicInfo_Filtered.Keys.ToList())
                            {
                                if (DicInfo_Filtered[no].sdip_no == "1")
                                {

                                }
                                else
                                {
                                    DicInfo_Filtered.Remove(no);
                                }
                                progressBar.AddProgressBarValueSafe(1);
                            }

                            if (DicInfo_Filtered.Count > 0)
                            {
                                Filter_NO_1 = 1;
                                Set_View();
                                Main.Print_List();
                                progressBar.ExitProgressBarSafe();
                            }
                            else
                            {
                                progressBar.ExitProgressBarSafe();
                                DicInfo_Filtered = Before_No1_Filter_dicInfo;
                                MessageBox.Show("[SDIP] 양품 판정된 이미지가 없습니다.");
                                Set_View();
                                Main.Print_List();

                            }

                        }
                        catch { }

                    }
                    else
                    {

                    }




                }



                else if (e.Shift && e.KeyCode == Keys.A)
                {
                    try
                    {
                        Main.InputKey += 1;
                        ProgressBar1 progressBar = new ProgressBar1();
                        progressBar.Text = "Loading...";
                        progressBar.Show();
                        progressBar.SetProgressBarMaxSafe(dicInfo_Filter.Count);
                        // waitform.Show();
                        if (Main.ViewType == "FrameSetView" || Main.ViewType == "DLFrameSetView")
                        {
                            foreach (KeyValuePair<string, ImageInfo> pair in Frame_dicInfo_Filter)
                            {
                                pair.Value.ReviewDefectName = "선택";
                            }
                            Select_Pic_List = Frame_dicInfo_Filter.Keys.ToList();


                            Frame_Set_View();
                            progressBar.AddProgressBarValueSafe(1);
                        }
                        else if (Main.EngrMode)
                        {
                            foreach (KeyValuePair<string, ImageInfo> pair in Main.Eng_dicinfo)
                            {
                                pair.Value.ReviewDefectName = "선택";
                                progressBar.AddProgressBarValueSafe(1);
                            }
                            //foreach(string pair in dicInfo_Filter.Keys.ToList())
                            //{
                            //    dicInfo_Filter[pair].ReviewDefectName = "선택";
                            //}
                            Select_Pic_List = dicInfo_Filter.Keys.ToList();

                            Set_Image_Eng();
                        }
                        else
                        {
                            foreach (KeyValuePair<string, ImageInfo> pair in dicInfo_Filter)
                            {
                                pair.Value.ReviewDefectName = "선택";
                                progressBar.AddProgressBarValueSafe(1);
                            }
                            //foreach(string pair in dicInfo_Filter.Keys.ToList())
                            //{
                            //    dicInfo_Filter[pair].ReviewDefectName = "선택";
                            //}
                            Select_Pic_List = dicInfo_Filter.Keys.ToList();

                            Set_Image();

                        }

                        Main.ALL_Changeed_State();
                        progressBar.ExitProgressBarSafe();
                    }
                    catch { }

                }



                else if (e.KeyCode == Keys.A)
                {
                    Main.InputKey += 1;
                    Select_Pic_List.Clear();
                    Change_state_List.Clear();

                    int index = ((Current_PageNum - 1) * (cols * rows));
                    Selected_Picture_Index.Clear();


                    if (Main.ViewType == "FrameSetView" || Main.ViewType == "DLFrameSetView")
                    {
                        for (int i = 0; i < (cols * rows); i++)
                        {
                            if ((index + i) >= frame_dicInfo_Filter.Count)
                                break;
                            Selected_Picture_Index.Add(index + i);
                        }

                        for (int p = 0; p < Selected_Picture_Index.Count; p++)
                        {
                            frame_dicInfo_Filter[frame_dicInfo_Filter.ElementAt(Selected_Picture_Index[p]).Key].ReviewDefectName = "선택";
                            Select_Pic_List.Add(frame_dicInfo_Filter.ElementAt(Selected_Picture_Index[p]).Key);
                            Change_state_List.Add(frame_dicInfo_Filter.ElementAt(Selected_Picture_Index[p]).Key);
                        }
                        Frame_Set_Image();
                    }
                    else
                    {
                        if (Main.EngrMode)
                        {
                            for (int i = 0; i < (cols * rows); i++)
                            {
                                if ((index + i) >= Main.Eng_dicinfo.Count)
                                    break;
                                Selected_Picture_Index.Add(index + i);
                            }

                            for (int p = 0; p < Selected_Picture_Index.Count; p++)
                            {
                                Main.Eng_dicinfo[Main.Eng_dicinfo.ElementAt(Selected_Picture_Index[p]).Key].ReviewDefectName = "선택";
                                Select_Pic_List.Add(Main.Eng_dicinfo.ElementAt(Selected_Picture_Index[p]).Key);
                                Change_state_List.Add(Main.Eng_dicinfo.ElementAt(Selected_Picture_Index[p]).Key);
                            }

                            Set_Image_Eng();
                        }
                        else
                        {
                            for (int i = 0; i < (cols * rows); i++)
                            {
                                if ((index + i) >= dicInfo_Filter.Count)
                                    break;
                                Selected_Picture_Index.Add(index + i);
                            }

                            for (int p = 0; p < Selected_Picture_Index.Count; p++)
                            {
                                dicInfo_Filter[dicInfo_Filter.ElementAt(Selected_Picture_Index[p]).Key].ReviewDefectName = "선택";
                                Select_Pic_List.Add(dicInfo_Filter.ElementAt(Selected_Picture_Index[p]).Key);
                                Change_state_List.Add(dicInfo_Filter.ElementAt(Selected_Picture_Index[p]).Key);
                            }

                            Set_Image();
                        }
                    }


                    


                }
                else if (e.KeyCode == Keys.S)
                {
                    Main.InputKey += 1;
                    if (MessageBox.Show("현재 이미지들을 파일로 저장 하시겠습니까?", "알림", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        string fileName;

                        string save_path = "";
                        Bitmap tmp_Img = null;
                        Dictionary<string, ImageInfo> copy_IMG = new Dictionary<string, ImageInfo>();
                        FolderBrowserDialog dialog = new FolderBrowserDialog();
                        dialog.ShowDialog();
                        save_path = dialog.SelectedPath;

                        if (Main.ViewType == "FrameSetView" || Main.ViewType == "DLFrameSetView")
                        {
                            copy_IMG = new Dictionary<string, ImageInfo>(Frame_dicInfo_Filter);
                        }
                        else if (Main.EngrMode)
                        {
                            copy_IMG = new Dictionary<string, ImageInfo>(Main.Eng_dicinfo);
                        }
                        else
                        {
                            copy_IMG = new Dictionary<string, ImageInfo>(DicInfo_Filtered);
                        }
                        copy_IMG = copy_IMG.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);

                        if (save_path != "")
                        {
                            List<int> frame_copy = new List<int>();
                            foreach (string pair in copy_IMG.Keys.ToList())
                            {
                                if (frame_copy.Contains(copy_IMG[pair].FrameNo))
                                { }
                                else
                                {
                                    frame_copy.Add(copy_IMG[pair].FrameNo);
                                }
                            }


                            if (Main.ZipFilePath != "")
                            {
                                zip = ZipFile.Open(Main.ZipFilePath, ZipArchiveMode.Read);       // Zip파일(Lot) Load
                                string Open_ZipName;
                                ZipArchiveEntry sortzip;
                                System.Text.Encoding euckr = System.Text.Encoding.GetEncoding(949);

                                foreach (ZipArchiveEntry entry in zip.Entries.OrderBy(x => x.Name))
                                {
                                    Open_ZipName = entry.Name.Split('.')[0];
                                    if (Open_ZipName[0].Equals('R'))
                                        Open_ZipName = Open_ZipName.Substring(1, Open_ZipName.Length - 1);

                                    if (entry.Name.ToUpper().IndexOf(".ZIP") != -1 && frame_copy.Contains(int.Parse(entry.Name.ToString().Substring(0, 5))))
                                    {
                                        MemoryStream subEntryMS = new MemoryStream();           // 2중 압축파일을 MemoryStream으로 읽는다.
                                        entry.Open().CopyTo(subEntryMS);

                                        ZipArchive subZip = new ZipArchive(subEntryMS, ZipArchiveMode.Read, false, euckr);         // MemoryStream으로 읽은 파일(2중 압축파일) 각각을 ZipArchive로 읽는다.


                                        var sub =
                                                            from ent in subZip.Entries
                                                            orderby ent.Name
                                                            select ent;


                                        foreach (ZipArchiveEntry subEntry in sub)       // 2중 압축파일 내에 있는 파일을 탐색
                                        {

                                            foreach (string pair in copy_IMG.Keys.ToList())
                                            {
                                                if (subEntry.Name.Equals(dicInfo_Filter[pair].Imagename + ".jpg"))  // jpg 파일이 있을 경우 ( <= 각 이미지 파일에 대한 처리는 여기서... )
                                                {
                                                    tmp_Img = new Bitmap(subEntry.Open());



                                                    tmp_Img.Save(save_path + "\\" + copy_IMG[pair].Imagename + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);

                                                    copy_IMG.Remove(pair);
                                                }
                                            }



                                        }
                                        subZip.Dispose();
                                    }

                                }
                                zip.Dispose();

                            }
                        }
                        MessageBox.Show("   이미지 추출 완료 \n Lot Name : " + Main.LotName + "\n 위치 : " + save_path);
                    }
                    else
                    {

                    }

                }
                else if (e.KeyCode == Keys.T)
                {
                    Main.InputKey += 1;
                    if (MessageBox.Show("현재 삭제대기 정보를 저장하시겠습니까? \r 주의: Zip 파일의 이미지를 삭제 한 후에는 저장이 되지 않습니다", "알림", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        Main.F12_del_list_main = new List<string>(F12_del_list);
                        Func.SaveDelFileID(Main.Waiting_Del, Main.F12_del_list_main);
                    }
                    else
                    {

                    }

                }

                else if (e.KeyCode == Keys.C) // 
                {
                    Main.InputKey += 1;
                    Expand_Find_Contain_PB(A_Mouse_XY, A_Mouse_XY);
                    if (expand_img != null)
                        Clipboard.SetImage(expand_img);
                }

                else if (e.KeyCode == Keys.I)  // 현재파일 위치
                {
                    Main.InputKey += 1;
                    string FileName = string.Empty;
                    FileName = Util.GetFileName();
                    Clipboard.SetText(FileName);
                    MessageBox.Show("Lot Name: " + FileName + "\r" + "위치: " + Main.ZipFilePath, "알림");

                }
                else if (e.Shift && e.KeyCode == Keys.G)
                {
                    Main.InputKey += 1;
                    Select_Pic_List.Clear();
                    if (Main.EngrMode)
                    {
                        foreach (KeyValuePair<string, ImageInfo> pair in Main.Eng_dicinfo)
                        {
                            pair.Value.ReviewDefectName = "양품";
                        }
                    }
                    
                    foreach (KeyValuePair<string, ImageInfo> pair in dicInfo_Filter)
                    {
                        pair.Value.ReviewDefectName = "양품";
                    }


                    if (Main.EngrMode)
                        Set_Image_Eng();
                    else
                        Set_Image();


                    Main.ALL_Changeed_State();


                }

                else if (e.KeyCode == Keys.G)
                {
                    Main.InputKey += 1;
                    //Select_Pic_List.Clear();
                    Change_state_List.Clear();
                    int index = ((Current_PageNum - 1) * (cols * rows));
                    Selected_Picture_Index.Clear();



                    if (Main.ViewType == "FrameSetView" || Main.ViewType == "DLFrameSetView")
                    {
                        for (int i = 0; i < (cols * rows); i++)
                        {
                            if ((index + i) >= frame_dicInfo_Filter.Count)
                                break;
                            Selected_Picture_Index.Add(index + i);
                        }

                        for (int p = 0; p < Selected_Picture_Index.Count; p++)
                        {
                            frame_dicInfo_Filter[frame_dicInfo_Filter.ElementAt(Selected_Picture_Index[p]).Key].ReviewDefectName = "양품";

                            Change_state_List.Add(frame_dicInfo_Filter.ElementAt(Selected_Picture_Index[p]).Key);
                        }
                        Frame_Set_View();
                    }
                    else
                    {
                        for (int i = 0; i < (cols * rows); i++)
                        {
                            if ((index + i) >= dicInfo_Filter.Count)
                                break;
                            Selected_Picture_Index.Add(index + i);
                        }

                        for (int p = 0; p < Selected_Picture_Index.Count; p++)
                        {
                            if (Main.EngrMode)
                            {
                                Main.Eng_dicinfo[Main.Eng_dicinfo.ElementAt(Selected_Picture_Index[p]).Key].ReviewDefectName = "양품";
                                Change_state_List.Add(Main.Eng_dicinfo.ElementAt(Selected_Picture_Index[p]).Key);
                            }
                            else
                            {
                                dicInfo_Filter[dicInfo_Filter.ElementAt(Selected_Picture_Index[p]).Key].ReviewDefectName = "양품";
                                Change_state_List.Add(dicInfo_Filter.ElementAt(Selected_Picture_Index[p]).Key);
                            }
                            
                        }
                        if (Main.EngrMode)
                            Set_Image_Eng();
                        else
                            Set_Image();
                       
                    }


                    Main.Changeed_State();


                }
                else if (e.KeyCode == Keys.F2)
                {
                    //Main.InputKey += 1;

                    Main.deleteSavePathToolStripMenuItem_Click(null, null);
                }
                else if (e.KeyCode == Keys.F3)
                {
                    //Main.InputKey += 1;
                    Main.engrModeToolStripMenuItem_Click(null, null);
                }
                else if (e.KeyCode == Keys.End)
                {
                    // Main.InputKey += 1;
                    Main.완전종료ToolStripMenuItem_Click(null, null);
                }
                else if (e.KeyCode == Keys.F9)
                {
                    if (MessageBox.Show("[SDIP] 강제불량 처리 대상(Limit 미초과)인 Frame에 대한 이미지를 필터링합니다.\r (단, 강제불량 처리된 해당 이미지는 미표시)", "알림", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {

                        Dictionary<string, ImageInfo> Before_Dic = new Dictionary<string, ImageInfo>(DicInfo_Filtered);
                        Dictionary<string, ImageInfo> F9_Dic = new Dictionary<string, ImageInfo>(Main.F9_Find_in_Dicinfo());
                        DicInfo_Filtered = F9_Dic;
                        Main.Filter_CheckEQ_Dic = F9_Dic;
                        if (Main.F9_code_dicInfo.Count > 0)
                        {
                            Filter_F9 = 1;
                            Main.FI_RE_B.Enabled = true;
                            Main.Equipment_DF_CLB.SelectedValueChanged -= Main.Equipment_DF_CLB_ItemCheck;
                            Main.EQ_Data_Update(DicInfo_Filtered);
                            Set_View();
                            Main.Equipment_DF_CLB.SelectedValueChanged += Main.Equipment_DF_CLB_ItemCheck;
                            Main.Print_List();
                            Main.List_Count_TB.Text = String.Format("{0:#,##0}", dicInfo_Filter.Count);

                        }

                        else
                        {
                            MessageBox.Show("Limit 아래 부품이 없습니다.");
                            DicInfo_Filtered = Before_Dic;
                        }
                    }
                    else
                    {

                    }




                }

                else if (e.KeyCode == Keys.F10)
                {
                    if (MessageBox.Show("[SDIP] 강제불량 처리 해제된(Limit 초과) Frame에 대한 이미지를 필터링합니다.\r (단, 강제불량 처리된 해당 이미지는 미표시)", "알림", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        Dictionary<string, ImageInfo> Before_Dic = new Dictionary<string, ImageInfo>(DicInfo_Filtered);
                        Dictionary<string, ImageInfo> F10_Dic = new Dictionary<string, ImageInfo>(Main.F10_Find_in_Dicinfo());
                        DicInfo_Filtered = Main.F10_Find_in_Dicinfo();
                        Main.Filter_CheckEQ_Dic = F10_Dic;
                        if (Main.F10_code_dicInfo.Count > 0)
                        {

                            Filter_F10 = 1;
                            Main.FI_RE_B.Enabled = true;
                            Main.Equipment_DF_CLB.SelectedValueChanged -= Main.Equipment_DF_CLB_ItemCheck;
                            Main.EQ_Data_Update(DicInfo_Filtered);
                            Set_View();
                            Main.Equipment_DF_CLB.SelectedValueChanged += Main.Equipment_DF_CLB_ItemCheck;
                            Main.Print_List();
                            Main.List_Count_TB.Text = String.Format("{0:#,##0}", dicInfo_Filter.Count);
                            Main.FI_RE_B.Enabled = true;
                        }
                        else
                        {
                            MessageBox.Show("Limit 초과 부품이 없습니다.");
                            DicInfo_Filtered = Before_Dic;
                        }

                    }
                    else
                    {

                    }


                }

                else if (e.KeyCode == Keys.R)
                {

                    //A_Mouse_XY = this.PointToClient(new Point(MousePosition.X, MousePosition.Y));
                    Main.InputKey += 1;
                    ExpandImage expand = new ExpandImage(this);
                    Expand_Find_Contain_PB(A_Mouse_XY, A_Mouse_XY);
                    expand.Expand_ImgInfo.Add(expand_ImgInfo.Keys.ElementAt(0), expand_ImgInfo[expand_ImgInfo.Keys.ElementAt(0)]);
                    expand.Set_Expand_Img(expand_img);


                    expand.ShowDialog();

                }

                else if (e.KeyCode == Keys.F)
                {
                    //Filter_F = 1;
                    Main.InputKey += 1;
                    Main.OLD_XY_X.Text = Main.MouseXY_FT_X.Text;
                    Main.OLD_XY_Y.Text = Main.MouseXY_FT_Y.Text;

                    /*
                    XYLocationFilter xyFilter = new XYLocationFilter(this,Befroe_X,Before_Y);
                    Expand_Find_Contain_PB(A_Mouse_XY, A_Mouse_XY);
                    if (expand_ImgInfo.Count > 0)
                    {

                        xyFilter.XY_Location.Add(expand_ImgInfo.Keys.ElementAt(0), expand_ImgInfo[expand_ImgInfo.Keys.ElementAt(0)]);

                        if (Main.ViewType == "FrameSetView" || Main.ViewType == "DLFrameSetView")
                        {
                            xyFilter.DicInfo_XY_filter = new Dictionary<string, ImageInfo>(frame_dicInfo_Filter);
                        }
                        else
                        {
                            if (Main.Sdip_200_code_dicInfo.ContainsKey(DicInfo_Filtered.ElementAt(0).Key))
                            {
                                xyFilter.DicInfo_XY_filter = new Dictionary<string, ImageInfo>(Main.Sdip_200_code_dicInfo);
                            }
                            else
                            {
                                Dictionary<string, ImageInfo> eq_filter_dic = new Dictionary<string, ImageInfo>(DicInfo_Filtered);

                                xyFilter.DicInfo_XY_filter = new Dictionary<string, ImageInfo>(eq_filter_dic);
                            }

                        }

                        xyFilter.Set_XY_TB();
                        xyFilter.ShowDialog();

                }
                else
                    {
                        MessageBox.Show("Image XY Error");
                    }
                     */
                }

                else if (e.KeyCode == Keys.F5)
                {
                    Main.InputKey += 1;
                    if (MessageBox.Show("[SDIP] SR Bleed/Lack 로 자동불량 판정된 이미지를 필터링합니다.", "알림", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        if (Main.F5_code_dicInfo.Count > 0)
                        {
                            DicInfo_Filtered = Main.F5_code_dicInfo;

                            Filter_F5 = 1;
                            Set_View();
                            Main.Print_List();
                            Main.List_Count_TB.Text = String.Format("{0:#,##0}", dicInfo_Filter.Count);
                            Main.FI_RE_B.Enabled = true;
                        }
                        else
                            MessageBox.Show("이미지가 없습니다.");
                    }
                    else
                    {

                    }



                }


                else if (e.KeyCode == Keys.F1)
                {
                    Main.InputKey += 1;
                    //Get_Delete_IMG();
                    Select_Pic_List.Clear();

                    if (MessageBox.Show("선택한 이미지 List에서 제거 \r (※ 실제 파일에서 Image 삭제는 안함!!)", "알림", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        if (Main.Frame_View_CB.Checked)
                        {
                            foreach (string pair in frame_dicInfo_Filter.Keys.ToList())
                            {
                                if (frame_dicInfo_Filter[pair].ReviewDefectName == "선택")
                                {
                                    Select_Pic_List.Add(pair);
                                }
                                else
                                {

                                }
                            }

                            for (int i = 0; i < Select_Pic_List.Count; i++)
                            {
                                if (frame_dicInfo_Filter.ContainsKey(Select_Pic_List[i]))
                                {
                                    f12_del_list.Add(Select_Pic_List[i]);
                                    frame_dicInfo_Filter.Remove(Select_Pic_List[i]);
                                }

                            }
                            Main.Dl_PrintList();
                            DL_Frame_Set_View();


                            Main.List_Count_TB.Text = String.Format("{0:#,##0}", frame_dicInfo_Filter.Count);

                            Eq_cb_need_del = new List<string>(Select_Pic_List);
                        }
                        else
                        {
                            foreach (string pair in dicInfo_Filter.Keys.ToList())
                            {
                                if (dicInfo_Filter[pair].ReviewDefectName == "선택")
                                {
                                    Select_Pic_List.Add(pair);
                                }
                                else
                                {

                                }
                            }
                            for (int i = 0; i < Select_Pic_List.Count; i++)
                            {
                                if (dicInfo_Filter.ContainsKey(Select_Pic_List[i]))
                                {
                                    f12_del_list.Add(Select_Pic_List[i]);
                                    dicInfo_Filter.Remove(Select_Pic_List[i]);
                                }

                            }

                            Main.Dl_PrintList();
                            Del_Set_View();



                            Main.List_Count_TB.Text = String.Format("{0:#,##0}", dicInfo_Filter.Count);

                            Eq_cb_need_del = new List<string>(Select_Pic_List);
                        }

                        Select_Pic_List.Clear();
                    }
                    else
                    {

                    }

                }
                else if (e.KeyCode == Keys.F5)
                {

                }

            }
        }

        private void ImageViewer_KeyUp(object sender, KeyEventArgs e)
        {
            if (Main != null)
            {
                Main.InputKey += 1;
                if (e.KeyCode == Keys.Menu)
                {
                    e.Handled = true;
                    Main.InputKey += 1;
                    if (Main.S_Page_TB.Text == "" || int.Parse(Main.S_Page_TB.Text) >= int.Parse(Main.E_Page_TB.Text))
                    {
                        // MessageBox.Show("마지막 페이지 입니다.");
                        return;
                    }
                    else
                    {

                        Last_Picture_Selected_Index = -1;
                        Current_PageNum = int.Parse(Main.S_Page_TB.Text) + 1;
                        Main.S_Page_TB.Text = Current_PageNum.ToString();

                        //Set_PictureBox();
                        if (!Main.EngrMode)
                        {
                            if (Main.Frame_View_CB.Checked)
                                Frame_Set_Image();
                            else
                                Set_Image();
                        }
                        else if (Main.EngrMode)
                        {

                            if (Main.Frame_View_CB.Checked)
                                Frame_Set_Image();
                            else if (Main.EngrMode && !normalCheck)
                                Set_Image_Eng();
                            else
                                Set_Image();

                        }

                    }

                }
            }
        }


    }
}