
using OfficeOpenXml;
using STD.Framework.Utils.NPOI.HSSF.UserModel;
using STD.Framework.Utils.NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;

namespace STD.MiniMES.Web
{
    public class ExcelHelper
    {
        public ExcelHelper()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }
        /// <summary>
        /// 读取EXCEL文件名、sheet，标题行索引
        /// </summary>
        /// <param name="excelFilePath">文件名</param>
        /// <param name="sheetName">sheet名</param>
        /// <param name="headerRowIndex">标题行索引</param>
        /// <returns>返回DataTable</returns>
        public static DataTable ImportDataTableFromExcel(string excelFilePath, string sheetName, int headerRowIndex)
        {
            using (System.IO.FileStream stream = System.IO.File.OpenRead(excelFilePath))
            {
                return ImportDataTableFromExcel(stream, sheetName, headerRowIndex);//调用重载的方法
            }
        }

        /// <summary>
        /// 读取EXCEL文件名、sheet，标题行索引
        /// </summary>
        /// <param name="excelFilePath">文件名</param>
        /// <param name="sheetName">sheet名</param>
        /// <param name="headerRowIndex">标题行索引</param>
        /// <returns>返回DataTable</returns>
        public static DataTable ImportDataTableFromExcel(string excelFilePath, int sheetIndex, int headerRowIndex)
        {
            using (System.IO.FileStream stream = System.IO.File.OpenRead(excelFilePath))
            {
                return ImportDataTableFromExcel(stream, "", null, sheetIndex, headerRowIndex);//调用重载的方法
            }
        }

        /// <summary>
        /// 从EXCEL文件导入到DataTable
        /// </summary>
        /// <param name="excelFileStream">EXCEL文件流</param>
        /// <param name="sheetName">sheet名</param>
        /// <param name="headerRowIndex">标题行索引</param>
        /// <returns>返回DataTable</returns>
        public static DataTable ImportDataTableFromExcel(System.IO.Stream excelFileStream, string sheetName, int headerRowIndex)
        {
            Framework.Utils.NPOI.HSSF.UserModel.HSSFWorkbook workbook = new Framework.Utils.NPOI.HSSF.UserModel.HSSFWorkbook(excelFileStream);//创建工作簿对象
            ISheet sheet = workbook.GetSheet(sheetName);//获取参数指定的sheet
            DataTable table = new DataTable();
            IRow headerRow = sheet.GetRow(headerRowIndex);
            int cellCount = headerRow.LastCellNum;
            //读取并生成标题行，这里能成功执行
            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
            {
                DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                table.Columns.Add(column);
            }
            //逐个读取单元格                                  
            for (int i = (sheet.FirstRowNum + headerRowIndex + 1); i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                DataRow dataRow = table.NewRow();
                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (row.GetCell(j) != null)
                    {
                        dataRow[j] = row.GetCell(j).ToString();
                    }
                }
                table.Rows.Add(dataRow);
            }
            excelFileStream.Close();
            workbook = null;
            sheet = null;
            return table;
        }

        /// <summary>
        /// 从EXCEL文件导入到DataTable
        /// </summary>
        /// <param name="excelFileStream">EXCEL文件流</param>
        /// <param name="sheetName">sheet名</param>
        /// <param name="headerRowIndex">标题行索引</param>
        /// <returns>返回DataTable</returns>
        public static DataTable ImportDataTableFromExcel(System.IO.Stream excelFileStream, string filePath, string[] columnName, int sheetIndex, int headerRowIndex)
        {
            DataTable table = new DataTable();

            string fileType = System.IO.Path.GetExtension(filePath);

            if (fileType == ".xls")
            {
                IWorkbook workbook = null;//创建工作簿对象
                workbook = new HSSFWorkbook(excelFileStream);

                if (workbook == null) return null;
                ISheet sheet = workbook.GetSheetAt(sheetIndex);//获取参数指定的sheet

                IRow headerRow = sheet.GetRow(headerRowIndex);
                int cellCount = headerRow.LastCellNum;
                //读取并生成标题行，这里能成功执行
                for (int i = headerRow.FirstCellNum; i < cellCount; i++)
                {
                    if (headerRow.GetCell(i) != null)
                    {
                        DataColumn column = new DataColumn();
                        if (i < columnName.Length)
                        {
                            column = new DataColumn(columnName[i]);
                        }
                        else
                            column = new DataColumn(headerRow.GetCell(i).ToString());//
                        table.Columns.Add(column);
                    }
                }
                //逐个读取单元格                                  
                for (int i = (sheet.FirstRowNum + headerRowIndex + 1); i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null)
                        continue;
                    DataRow dataRow = table.NewRow();
                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        if (row.GetCell(j) != null)
                        {
                            dataRow[j] = row.GetCell(j).ToString();
                        }
                    }
                    table.Rows.Add(dataRow);
                }
                workbook = null;
                sheet = null;
            }
            else if (fileType == ".xlsx")
            {
                //table = EPPlusHelper.ReadByExcelLibrary(excelFileStream, columnName, sheetIndex + 1, headerRowIndex);
                using (var package = new ExcelPackage(excelFileStream))
                {
                    ExcelWorksheet sheet = package.Workbook.Worksheets[sheetIndex + 1];

                    var colCount = sheet.Dimension.End.Column;
                    var rowCount = sheet.Dimension.End.Row;

                    for (ushort j = 0; j < columnName.Length; j++)
                    {
                        //table.Columns.Add(new DataColumn(sheet.Cells[1, j].Value.ToString()));

                        DataColumn column = new DataColumn();
                        //if (j < columnName.Length)
                        //{
                        //    column = new DataColumn(columnName[j]);
                        //}
                        //else
                        //{
                        //    ExcelRange cell = sheet.Cells[headerRowIndex, j];
                        //    column = new DataColumn(cell.Value.ToString());
                        //}
                        column = new DataColumn(columnName[j]);

                        table.Columns.Add(column);
                    }

                    for (int i = headerRowIndex + 2; i <= rowCount; i++)
                    {
                        var row = table.NewRow();
                        for (int j = 1; j <= columnName.Length; j++)
                        {
                            row[j - 1] = sheet.Cells[i, j].Value;
                        }
                        table.Rows.Add(row);
                    }
                }
            }

            excelFileStream.Close();
            return table;
        }
        public static DataTable ImportDataTableFromExcel(string excelFilePath, string filePath, string[] columnName, int sheetIndex, int headerRowIndex)
        {
            DataTable table = new DataTable();

            string fileType = System.IO.Path.GetExtension(filePath);
            FileStream excelFileStream = new FileStream(excelFilePath, FileMode.Open);

            if (fileType == ".xls")
            {
                IWorkbook workbook = null;//创建工作簿对象

                workbook = new HSSFWorkbook(excelFileStream);

                if (workbook == null) return null;
                ISheet sheet = workbook.GetSheetAt(sheetIndex);//获取参数指定的sheet

                IRow headerRow = sheet.GetRow(headerRowIndex);
                int cellCount = headerRow.LastCellNum;
                //读取并生成标题行，这里能成功执行
                for (int i = headerRow.FirstCellNum; i < cellCount; i++)
                {
                    if (headerRow.GetCell(i) != null)
                    {
                        DataColumn column = new DataColumn();
                        if (i < columnName.Length)
                        {
                            column = new DataColumn(columnName[i]);
                        }
                        else
                            column = new DataColumn(headerRow.GetCell(i).ToString());//
                        table.Columns.Add(column);
                    }
                }
                //逐个读取单元格                                  
                for (int i = (sheet.FirstRowNum + headerRowIndex + 1); i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    DataRow dataRow = table.NewRow();
                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        if (row.GetCell(j) != null)
                        {
                            dataRow[j] = row.GetCell(j).ToString();
                        }
                    }
                    table.Rows.Add(dataRow);
                }

                workbook = null;
                sheet = null;
            }
            else if (fileType == ".xlsx")
            {
                //table = EPPlusHelper.ReadByExcelLibrary(excelFileStream, columnName, sheetIndex + 1,headerRowIndex);
                using (var package = new ExcelPackage(excelFileStream))
                {
                    ExcelWorksheet sheet = package.Workbook.Worksheets[sheetIndex];

                    var colCount = sheet.Dimension.End.Column;
                    var rowCount = sheet.Dimension.End.Row;

                    for (ushort j = 0; j < colCount; j++)
                    {
                        //table.Columns.Add(new DataColumn(sheet.Cells[1, j].Value.ToString()));

                        DataColumn column = new DataColumn();
                        if (j < columnName.Length)
                        {
                            column = new DataColumn(columnName[j]);
                        }
                        else
                        {
                            ExcelRange cell = sheet.Cells[headerRowIndex, j];
                            column = new DataColumn(cell.Value.ToString());
                        }

                        table.Columns.Add(column);
                    }

                    for (int i = headerRowIndex + 2; i <= rowCount; i++)
                    {
                        var row = table.NewRow();
                        for (int j = 1; j <= colCount; j++)
                        {
                            row[j - 1] = sheet.Cells[i, j].Value;
                        }
                        table.Rows.Add(row);
                    }
                }
            }

            excelFileStream.Close();

            return table;
        }
        public static DataTable GetDataFromExcelByConn(string filePath, bool hasTitle = false)
        {
            string fileType = System.IO.Path.GetExtension(filePath);
            if (string.IsNullOrEmpty(fileType)) return null;

            using (DataSet ds = new DataSet())
            {
                string strCon = string.Format("Provider=Microsoft.ACE.OLEDB.{0}.0;" +
                                "Extended Properties=\"Excel {1}.0;HDR={2};IMEX=1;\";" +
                                "data source={3};",
                                (fileType == ".xls" ? 4 : 12), (fileType == ".xls" ? 8 : 12), (hasTitle ? "Yes" : "NO"), filePath);
                string strCom = " SELECT * FROM [Sheet1$]";
                using (OleDbConnection myConn = new OleDbConnection(strCon))
                using (OleDbDataAdapter myCommand = new OleDbDataAdapter(strCom, myConn))
                {
                    myConn.Open();
                    myCommand.Fill(ds);
                }
                if (ds == null || ds.Tables.Count <= 0) return null;
                return ds.Tables[0];
            }
        }
    }
}