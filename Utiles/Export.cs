using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using ExcelExporter;

namespace Utiles
{
 public   class Export
    {
        public  void ToXMLfile(object fileOut, DataTable datos)
        {
            datos.WriteXml(fileOut.ToString());
        }
        public  void ToCSVfile(object fileOut, DataTable datos)
        {
            string separator = ";";
            using (StreamWriter sw = new StreamWriter(fileOut.ToString(), false, Encoding.UTF8))
            {

                string strRow; // represents a full row
                string nombreColumna = "";


                foreach (DataColumn columnName in datos.Columns)
                {

                    string name = columnName.ColumnName;
                    nombreColumna = nombreColumna + name + separator;
                }
                sw.WriteLine(nombreColumna.TrimEnd(';'));

                foreach (DataRow filas in datos.AsEnumerable())
                {
                    strRow = "";
                    for (int i = 0; i < filas.ItemArray.Count(); i++)
                    {
                        strRow += Convert.ToString(filas.ItemArray[i]) + separator;
                    }
                    if (separator.Length > 0)
                        strRow = strRow.Substring(0, strRow.LastIndexOf(separator));

                    sw.WriteLine(strRow);
                }
            }
        }
        public  void ToCSVfile(object fileOut, DataTable datos, string separator)
        {
            using (StreamWriter sw = new StreamWriter(fileOut.ToString(), false, Encoding.UTF8))
            {

                string strRow; // represents a full row
                string nombreColumna = "";


                foreach (DataColumn columnName in datos.Columns)
                {

                    string name = columnName.ColumnName;
                    nombreColumna = nombreColumna + name + separator;
                }
                sw.WriteLine(nombreColumna.TrimEnd(';'));

                foreach (DataRow filas in datos.AsEnumerable())
                {
                    strRow = "";
                    for (int i = 0; i < filas.ItemArray.Count(); i++)
                    {
                        strRow += Convert.ToString(filas.ItemArray[i]) + separator;
                    }
                    if (separator.Length > 0)
                        strRow = strRow.Substring(0, strRow.LastIndexOf(separator));

                    sw.WriteLine(strRow);
                }
            }
        }
        public  void ToExcelfile(object fileOut, DataTable datos)
        {
            if (File.Exists(fileOut.ToString())) {
                File.Delete(fileOut.ToString());
            }
            ExcelExport excel = new ExcelExport();
            excel.AddSheet(datos);
            excel.ExportTo(fileOut.ToString());
          
        }
    }  
}
