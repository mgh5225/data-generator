using System;
using System.Data;
using System.IO;
using System.Text;

public static class Extensions
{
    public static string ToCSV(this DataTable table)
    {
        var result = new StringBuilder();
        for (int i = 0; i < table.Columns.Count; i++)
        {
            result.Append(table.Columns[i].ColumnName);
            result.Append(i == table.Columns.Count - 1 ? "\n" : ",");
        }

        foreach (DataRow row in table.Rows)
        {
            for (int i = 0; i < table.Columns.Count; i++)
            {
                result.Append(row[i].ToString());
                result.Append(i == table.Columns.Count - 1 ? "\n" : ",");
            }
        }

        return result.ToString();
    }
    public static void ToCSVFile(this DataTable table, string filePath)
    {
        if (File.Exists(filePath))
        {
            throw new ArgumentException();
        }

        string writeData = table.ToCSV();
        System.IO.File.WriteAllText(filePath, writeData);
    }
}