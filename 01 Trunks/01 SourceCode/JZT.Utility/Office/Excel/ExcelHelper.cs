using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Microsoft.Office.Interop.Excel;

namespace ExcelHelperTest
{
    /// <summary>
    /// 功能说明：套用模板输出Excel，并对数据进行分页
    /// 作    者：Lingyun_k
    /// 创建日期：2005-7-12
    /// </summary>
    public class ExcelHelper
    {
        private string _inputFilePath;

        private string _outFilePath;

        private int _rowIndex = 2;//行起始坐标
        private int _colIndex = 2;//列起始坐标

        private DataView dv;

        private List<string> _displayColList  ;


        private void CreateHeader(Worksheet sheet, DataView dataView)
        {
            int rowIndex = _rowIndex;
            int colIndex = _colIndex;
            //取得标题

            foreach (string colName in _displayColList)
            {
                DataColumn col = dv.Table.Columns[colName];
                FillCell(sheet, rowIndex, colIndex, col.ColumnName, XlVAlign.xlVAlignCenter, XlHAlign.xlHAlignCenter);

                colIndex++;
            }
        }

        private void FillCell(Worksheet sheet, int rowIndex, int colIndex, string text, XlVAlign vAlignment , XlHAlign hAlingment )
        {
            sheet.Cells[rowIndex, colIndex] = text;
            sheet.get_Range(sheet.Cells[rowIndex, colIndex], sheet.Cells[rowIndex, colIndex]).HorizontalAlignment = hAlingment;
            sheet.get_Range(sheet.Cells[rowIndex, colIndex], sheet.Cells[rowIndex, colIndex]).VerticalAlignment = vAlignment;
        }

        private void FillCell(Worksheet mySheet, int rowIndex ,int colIndex, DataRowView row, DataColumn col)
        {
            string text ;
            XlVAlign vAlignment = XlVAlign.xlVAlignCenter;
            XlHAlign hAlignment = XlHAlign.xlHAlignRight ;

            if (col.DataType == System.Type.GetType("System.DateTime"))
            {
                text = (Convert.ToDateTime(row[col.ColumnName].ToString())).ToString("yyyy-MM-dd");
            }
            else if (col.DataType == System.Type.GetType("System.String"))
            {
                text = "'" + row[col.ColumnName].ToString();
                hAlignment = XlHAlign.xlHAlignLeft;
            }
            else
            {
                text  = row[col.ColumnName].ToString();
            }

            FillCell(mySheet, rowIndex, colIndex, text, vAlignment, hAlignment);
        }

        private void FillRow(Worksheet mySheet, DataRowView row , int rowIndex)
        {
            int colIndex = _colIndex ;

            foreach (string colName in _displayColList)
            {
                DataColumn col = dv.Table.Columns[colName];
                FillCell(mySheet, rowIndex , colIndex++, row, col);
            }
        }

        private void FillData(Worksheet mySheet, DataView dataView)
        {
            try
            {
                int rowIndex = _rowIndex + 1;
                //取得表格中的数据
                foreach (DataRowView row in dv)
                {
                    FillRow(mySheet, row, rowIndex++);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SetStyle(Worksheet mySheet)
        {



            ////加载一个合计行
            //int rowSum = _rowIndex + 1;
            //int colSum = 2;
            //mySheet.Cells[rowSum, 2] = "合计";
            //mySheet.get_Range(mySheet.Cells[rowSum, 2], mySheet.Cells[rowSum, 2]).HorizontalAlignment = XlHAlign.xlHAlignCenter;

            ////设置选中的部分的颜色
            //mySheet.get_Range(mySheet.Cells[rowSum, colSum], mySheet.Cells[rowSum, colIndex]).Select();
            //mySheet.get_Range(mySheet.Cells[rowSum, colSum], mySheet.Cells[rowSum, colIndex]).Interior.ColorIndex = 19;//设置为浅黄色，共计有56种

            ////取得整个报表的标题
            //mySheet.Cells[2, 2] = "title";

            ////设置整个报表的标题格式
            //mySheet.get_Range(mySheet.Cells[2, 2], mySheet.Cells[2, 2]).Font.Bold = true;
            //mySheet.get_Range(mySheet.Cells[2, 2], mySheet.Cells[2, 2]).Font.Size = 22;

            ////设置报表表格为最适应宽度
            //mySheet.get_Range(mySheet.Cells[4, 2], mySheet.Cells[rowSum, colIndex]).Select();
            //mySheet.get_Range(mySheet.Cells[4, 2], mySheet.Cells[rowSum, colIndex]).Columns.AutoFit();

            ////设置整个报表的标题为跨列居中
            //mySheet.get_Range(mySheet.Cells[2, 2], mySheet.Cells[2, colIndex]).Select();
            //mySheet.get_Range(mySheet.Cells[2, 2], mySheet.Cells[2, colIndex]).HorizontalAlignment = XlHAlign.xlHAlignCenterAcrossSelection;

            ////绘制边框
            //mySheet.get_Range(mySheet.Cells[4, 2], mySheet.Cells[rowSum, colIndex]).Borders.LineStyle = 1;
            //mySheet.get_Range(mySheet.Cells[4, 2], mySheet.Cells[rowSum, 2]).Borders[XlBordersIndex.xlEdgeLeft].Weight = XlBorderWeight.xlThick;//设置左边线加粗
            //mySheet.get_Range(mySheet.Cells[4, 2], mySheet.Cells[4, colIndex]).Borders[XlBordersIndex.xlEdgeTop].Weight = XlBorderWeight.xlThick;//设置上边线加粗
            //mySheet.get_Range(mySheet.Cells[4, colIndex], mySheet.Cells[rowSum, colIndex]).Borders[XlBordersIndex.xlEdgeRight].Weight = XlBorderWeight.xlThick;//设置右边线加粗
            //mySheet.get_Range(mySheet.Cells[rowSum, 2], mySheet.Cells[rowSum, colIndex]).Borders[XlBordersIndex.xlEdgeBottom].Weight = XlBorderWeight.xlThick;//设置下边线加粗


        }

        public void CreateExcel(string outFilePath, DataView dataView)
        {
            List<string> displayColList = new List<string>();

            foreach (DataColumn col in dataView.Table.Columns)
            {
                displayColList.Add(col.ColumnName);
            }

            CreateExcel(outFilePath, dataView, displayColList);
        }

       


        public void CreateExcel(string outFilePath, DataView dataView , List<string> displayColList)
        {
            _displayColList = displayColList;
            _outFilePath = outFilePath; //设置显示列表。
            dv = dataView;

            ApplicationClass excelApplication = null;
            Workbook workBook = null;
            Worksheet sheet = null;

            bool newFileFlag = !File.Exists(outFilePath);

            //如果文件不存在，则将模板文件拷贝一份作为输出文件
            if (!File.Exists(outFilePath))
            {
                //File.Copy(_inputFilePath, outFilePath, true);
            }

            excelApplication = new ApplicationClass();
            excelApplication.Visible = false;
            object missingValue = System.Reflection.Missing.Value;

            //打开文件
            if (!newFileFlag)
            {
                excelApplication.Workbooks.Open(outFilePath, missingValue, false, missingValue, missingValue,
                    missingValue, missingValue, missingValue, missingValue, missingValue, missingValue, missingValue,
                    missingValue, missingValue, missingValue);
            }

            workBook = excelApplication.Workbooks.Add(true);//[1];
            sheet = (Worksheet)workBook.ActiveSheet;

            //创建标题。
            CreateHeader(sheet, dv);

            //填充数据
            FillData(sheet, dv);

            //设置格式 
            //SetStyle(sheet);

            try
            {
                if (newFileFlag)
                {
                    workBook.SaveAs(outFilePath, missingValue, missingValue, missingValue,   //Excel.XlSaveAsAccessMode.xlShared 
    missingValue, missingValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlShared,
    missingValue, missingValue, missingValue,
    missingValue, missingValue);
                }
                else
                {
                    //保存
                    workBook.Save();
                }
                workBook.Close(true, outFilePath, true);
                excelApplication.Quit();

                System.Runtime.InteropServices.Marshal.ReleaseComObject(sheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workBook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApplication);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


 
    }
}