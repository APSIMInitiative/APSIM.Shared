// -----------------------------------------------------------------------
// <copyright file="DataTableUtilties.cs" company="APSIM Initiative">
//     Copyright (c) APSIM Initiative
// </copyright>
//-----------------------------------------------------------------------
namespace APSIM.Shared.Utilities
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;

    /// <summary>
    /// Some utilities for manipulating a data table.
    /// </summary>
    public class DataTableUtilities
    {
        /// <summary>
        /// Add a value to the specified data table
        /// </summary>
        static public void AddValue(System.Data.DataTable Table, string ColumnName, string Value, int StartRow, int Count)
        {
            string[] Values = new string[Count];
            for (int i = 0; i != Count; i++)
                Values[i] = Value;
            AddColumn(Table, ColumnName, Values, StartRow, Count);
        }

        /// <summary>
        /// Add a value to the specified data table
        /// </summary>
        static public void AddValue(System.Data.DataTable Table, string ColumnName, double Value, int StartRow, int Count)
        {
            string[] Values = new string[Count];
            for (int i = 0; i != Count; i++)
            {
                if (Value == MathUtilities.MissingValue)
                    Values[i] = "";
                else
                    Values[i] = Value.ToString();
            }
            AddColumn(Table, ColumnName, Values, StartRow, Count);
        }


        /// <summary>
        /// Add a column of values to the specified data table
        /// </summary>
        static public void AddColumn(System.Data.DataTable Table, string ColumnName, double[] Values, int StartRow, int Count)
        {
            if (Table.Columns.IndexOf(ColumnName) == -1)
                Table.Columns.Add(ColumnName, typeof(double));

            if (Values == null)
                return;
		
            // Make sure there are enough values in the table.
            while (Table.Rows.Count < Values.Length + StartRow)
                Table.Rows.Add(Table.NewRow());

            int Row = StartRow;
            for (int Index = 0; Index != Values.Length; Index++)
            {
                if (Values[Index] != MathUtilities.MissingValue)
                    Table.Rows[Row][ColumnName] = Values[Index];
                else
                    Table.Rows[Row][ColumnName] = DBNull.Value;
                Row++;
            }
        }

        /// <summary>
        /// Add a column of values to the specified data table
        /// </summary>
        static public void AddColumn(System.Data.DataTable Table, string ColumnName, double[] Values)
        {
            int Count = 0;
            if (Values != null)
                Count = Values.Length;
            AddColumn(Table, ColumnName, Values, 0, Count);
        }

        /// <summary> 
        /// Add a column of values to the specified data table
        /// </summary>
        static public void AddColumn(System.Data.DataTable Table, string ColumnName, string[] Values)
        {
            int Count = 0;
            if (Values != null)
                Count = Values.Length;
            AddColumn(Table, ColumnName, Values, 0, Count);
        }

        /// <summary>
        /// Add a column of values to the specified data table
        /// </summary>
        static public void AddColumn(System.Data.DataTable Table, string ColumnName, string[] Values, int StartRow, int Count)
        {
            if (Table.Columns.IndexOf(ColumnName) == -1)
                Table.Columns.Add(ColumnName, typeof(string));

            if (Values == null)
                return;
			
            // Make sure there are enough values in the table.
            while (Table.Rows.Count < Values.Length + StartRow)
                Table.Rows.Add(Table.NewRow());

            int Row = StartRow;
            for (int Index = 0; Index != Values.Length; Index++)
            {
                if (Values[Index] != "")
                    Table.Rows[Row][ColumnName] = Values[Index];
                Row++;
            }
        }

        /// <summary>
        /// Add a column of values to the specified data table
        /// </summary>
        /// <param name="table">The table to add values to</param>
        /// <param name="columnName">The name of the column</param>
        /// <param name="values">The values to add to the table.</param>
        static public void AddColumnOfObjects(System.Data.DataTable table, string columnName, IEnumerable values)
        {
            // Make sure the table has the specified column
            if (!table.Columns.Contains(columnName))
            {
                table.Columns.Add(columnName, values.GetType().GetElementType());
            }

            if (values != null)
            {
                int row = 0;
                foreach (object value in values)
                {
                    // Make sure we have enough rows.
                    if (table.Rows.Count <= row)
                    {
                        table.Rows.Add(table.NewRow());
                    }

                    // Determine if this value should be put into the table.
                    // If the value is a double.NaN then don't put into table.
                    // All other values do get inserted.
                    bool insertValue = true;
                    if (value is double && (double.IsNaN((double) value) || (double) value == MathUtilities.MissingValue))
                    {
                        insertValue = false;
                    }

                    // Set the cell value in table.
                    if (insertValue)
                    {
                        table.Rows[row][columnName] = value;
                    }

                    row++;
                }
            }
        }

        /// <summary>
        /// Get a column of values from the specified data table
        /// </summary>
        static public double[] GetColumnAsDoubles(System.Data.DataTable Table, string ColumnName)
        {
            return GetColumnAsDoubles(Table, ColumnName, Table.Rows.Count);
        }

        /// <summary>
        /// Get a column as doubles
        /// </summary>
        /// <param name="Table"></param>
        /// <param name="ColumnName"></param>
        /// <param name="NumValues"></param>
        /// <returns></returns>
        static public double[] GetColumnAsDoubles(System.Data.DataTable Table, string ColumnName, int NumValues)
        {
            double[] Values = new double[NumValues];
            for (int Row = 0; Row != Table.Rows.Count && Row != NumValues; Row++)
            {
                if (Table.Rows[Row][ColumnName].ToString() == "")
                    Values[Row] = double.NaN;
                else
                    Values[Row] = Convert.ToDouble(Table.Rows[Row][ColumnName]);
            }
            return Values;
        }

        /// <summary>
        /// Get a column as doubles
        /// </summary>
        /// <param name="Table"></param>
        /// <param name="ColumnName"></param>
        /// <returns></returns>
        static public double[] GetColumnAsDoubles(DataView Table, string ColumnName)
        {
            int NumValues = Table.Count;
            double[] Values = new double[NumValues];
            for (int Row = 0; Row != Table.Count; Row++)
            {
                if (Table[Row][ColumnName].ToString() == "")
                    Values[Row] = double.NaN;
                else
                    Values[Row] = Convert.ToDouble(Table[Row][ColumnName]);
            }
            return Values;
        }

        /// <summary>
        /// Get a column as doubles.
        /// </summary>
        /// <param name="Table"></param>
        /// <param name="ColumnName"></param>
        /// <param name="NumValues"></param>
        /// <param name="StartRow"></param>
        /// <returns></returns>
        static public double[] GetColumnAsDoubles(System.Data.DataTable Table, string ColumnName, int NumValues, int StartRow)
        {
            double[] Values = new double[NumValues];
            int Index = 0;
            for (int Row = StartRow; Row != Table.Rows.Count && Index != NumValues; Row++)
            {
                if (Table.Rows[Row][ColumnName].ToString() == "")
                    Values[Index] = MathUtilities.MissingValue;
                else
                {
                    try
                    {
                        Values[Index] = Convert.ToDouble(Table.Rows[Row][ColumnName]);
                    }
                    catch (Exception)
                    {
                        throw new Exception("Invalid number found: " + Table.Rows[Row][ColumnName].ToString() +
                                       ". Row: " + Row.ToString() + ". Column name: " + ColumnName);
                    }
                }
                Index++;
            }
            return Values;
        }

        /// <summary>
        /// Get a column of values from the specified data table
        /// </summary>
        static public string[] GetColumnAsStrings(System.Data.DataTable Table, string ColumnName)
        {
            return GetColumnAsStrings(Table, ColumnName, Table.Rows.Count);
        }
        
        /// <summary>
        /// Get a column as strings
        /// </summary>
        /// <param name="Table"></param>
        /// <param name="ColumnName"></param>
        /// <param name="NumValues"></param>
        /// <returns></returns>
        static public string[] GetColumnAsStrings(System.Data.DataTable Table, string ColumnName, int NumValues)
        {
            string[] Values = new string[NumValues];
            for (int Row = 0; Row != Table.Rows.Count && Row != NumValues; Row++)
                Values[Row] = Convert.ToString(Table.Rows[Row][ColumnName]);
            return Values;
        }
        
        /// <summary>
        /// Get a column as strings.
        /// </summary>
        /// <param name="Table"></param>
        /// <param name="ColumnName"></param>
        /// <param name="NumValues"></param>
        /// <param name="StartRow"></param>
        /// <returns></returns>
        static public string[] GetColumnAsStrings(System.Data.DataTable Table, string ColumnName, int NumValues, int StartRow)
        {
            string[] Values = new string[NumValues];
            int Index = 0;
            for (int Row = StartRow; Row != Table.Rows.Count && Index != NumValues; Row++)
            {
                Values[Index] = Convert.ToString(Table.Rows[Row][ColumnName]);
                Index++;
            }
            return Values;
        }

        /// <summary>
        /// Get a column as dates.
        /// </summary>
        /// <param name="Table"></param>
        /// <param name="ColumnName"></param>
        /// <returns></returns>
        static public DateTime[] GetColumnAsDates(System.Data.DataTable Table, string ColumnName)
        {
            DateTime[] Values = new DateTime[Table.Rows.Count];
            for (int Row = 0; Row != Table.Rows.Count; Row++)
                Values[Row] = Convert.ToDateTime(Table.Rows[Row][ColumnName]);
            return Values;
        }

        /// <summary>
        /// Get a list of column names
        /// </summary>
        static public string[] GetColumnNames(System.Data.DataTable Table)
        {
            if (Table != null)
            {
                string[] ColumnNames = new string[Table.Columns.Count];
                for (int Col = 0; Col != Table.Columns.Count; Col++)
                    ColumnNames[Col] = Table.Columns[Col].ColumnName;
                return ColumnNames;
            }
            else
                return new string[0];
        }

        /// <summary>
        /// Get number of non blank values in column of the specified data table
        /// </summary>
        static public int GetNumberOfNonBlankRows(System.Data.DataTable Table, string ColumnName)
        {
            for (int Row = Table.Rows.Count - 1; Row >= 0; Row--)
            {
                if (Table.Rows[Row][ColumnName].ToString() != "")
                    return Row + 1;
            }
            return Table.Rows.Count;
        }

        /// <summary>
        /// Get a date from the specified row
        /// </summary>
        /// <param name="Row"></param>
        /// <returns></returns>
        static public DateTime GetDateFromRow(System.Data.DataRow Row)
        {
            // ---------------------------------------------------------------------
            // Try and return a date for the specified row in the specified table.
            // Will throw if there is no date found.
            // ---------------------------------------------------------------------
            int Year = 0;
            int Month = 0;
            int Day = 0;
            for (int Col = 0; Col != Row.Table.Columns.Count; Col++)
            {
                string ColumnName = Row.Table.Columns[Col].ColumnName.ToLower();
                if (ColumnName == "date")
                {
                    if (Row.Table.Columns[Col].DataType == typeof(DateTime))
                        return (DateTime)Row[Col];
                    else
                        return DateTime.Parse(Row[Col].ToString());
                }
                else if (ColumnName == "year")
                    Year = Convert.ToInt32(Row[Col]);
                else if (ColumnName == "month")
                    Month = Convert.ToInt32(Row[Col]);
                else if (ColumnName == "day")
                    Day = Convert.ToInt32(Row[Col]);
            }
            if (Year > 0)
            {
                if (Day > 0)
                    return new DateTime(Year, 1, 1).AddDays(Day - 1);
                else
                    Day = 1;
                if (Month == 0)
                    Month = 1;
                return new DateTime(Year, Month, Day);
            }
            throw new Exception("Cannot find a date columns. " +
                                "There must be one of the following combinations of columns: " +
                                "[a date column] OR " +
                                "[a year and day column] OR" +
                                "[a year, month and day column]");
        }

        /// <summary>
        /// Filter the specified table for the given date range.
        /// </summary>
        /// <param name="Table"></param>
        /// <param name="StartYear"></param>
        /// <param name="EndYear"></param>
        /// <returns></returns>
        static public DataView FilterTableForYear(System.Data.DataTable Table, int StartYear, int EndYear)
        {
            // ---------------------------------------------------------------------
            // Filter the specified data table for the specified year range.
            // ---------------------------------------------------------------------
            DataView View = new DataView();
            View.Table = Table;
            if (Table.Columns.IndexOf("year") != -1)
                View.RowFilter = "Year >= " + StartYear.ToString() + " and Year <= " + EndYear;

            else if (Table.Columns.IndexOf("date") != -1)
            {
                // This uses system locale to decode a date string, we should really
                // be using the units attribute instead.
                DateTime d1 = new DateTime(StartYear, 1, 1);
                string filter = string.Format(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat, "Date >= #{0}#", d1);
                DateTime d2 = new DateTime(EndYear, 12, 31);
                filter += string.Format(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat, "AND Date <= #{0}#", d2);
                View.RowFilter = filter;
            }
            else
                throw new Exception("Cannot find a date column in data");
            return View;
        }

        /// <summary>
        /// Get the distinct rows from the specified table using the values in the specified column
        /// </summary>
        /// <param name="Table"></param>
        /// <param name="ColumnName"></param>
        /// <returns></returns>
        static public List<string> GetDistinctValues(System.Data.DataTable Table, string ColumnName)
        {
            // ---------------------------------------------------------------------
            // Return a list of unique values for the specified column in the
            // specified table.
            // ---------------------------------------------------------------------
            List<string> Values = new List<string>();

            foreach (DataRow Row in Table.Rows)
            {
                if (Values.IndexOf(Row[ColumnName].ToString()) == -1)
                    Values.Add(Row[ColumnName].ToString());
            }
            return Values;
        }

        /// <summary>
        /// Get a list of monthly sums for the specified data view.
        /// </summary>
        /// <param name="View"></param>
        /// <returns></returns>
        static public System.Data.DataTable MonthlySums(DataView View)
        {
            // ----------------------------------------------------------------------------------
            // From the daily data in the Metfile object, calculate monthly sums of all variables
            // ----------------------------------------------------------------------------------
            System.Data.DataTable MonthlyData = new System.Data.DataTable();
            MonthlyData.TableName = "MonthlyData";

            if (View.Table.Columns.IndexOf("Date") == -1)
                MonthlyData.Columns.Add("Date", Type.GetType("System.DateTime"));

            foreach (DataColumn Column in View.Table.Columns)
                MonthlyData.Columns.Add(Column.ColumnName, Column.DataType);

            int PreviousMonth = 0;
            DataRow MonthRow = null;
            for (int Row = 0; Row != View.Count; Row++)
            {
                DateTime RowDate = DataTableUtilities.GetDateFromRow(View[Row].Row);
                if (PreviousMonth != RowDate.Month)
                {
                    MonthRow = MonthlyData.NewRow();
                    MonthlyData.Rows.Add(MonthRow);
                    MonthRow["Date"] = RowDate;
                    PreviousMonth = RowDate.Month;
                }

                foreach (DataColumn Column in View.Table.Columns)
                {
                    if (Convert.IsDBNull(MonthRow[Column.ColumnName]))
                        MonthRow[Column.ColumnName] = View[Row][Column.ColumnName];
                    else if (Column.DataType.ToString() == "System.Single" || Column.DataType.ToString() == "System.Double")
                        MonthRow[Column.ColumnName] = Convert.ToDouble(MonthRow[Column.ColumnName]) +
                                                      Convert.ToDouble(View[Row][Column.ColumnName]);
                    else
                        MonthRow[Column.ColumnName] = View[Row][Column.ColumnName];

                }
            }
            return MonthlyData;
        }

        /// <summary>
        /// Write the specified DataTable to a CSV string, excluding the specified column names.
        /// </summary>
        static public string DataTableToText(System.Data.DataTable data, int startColumnIndex, string delimiter, bool showHeadings)
        {
            // Convert the data table to a table of strings. This will make it easier for
            // calculating widths.
            DataTable stringTable = new DataTable();
            foreach (DataColumn col in data.Columns)
                stringTable.Columns.Add(col.ColumnName, typeof(string));
            foreach (DataRow row in data.Rows)
            {
                DataRow newRow = stringTable.NewRow();
                foreach (DataColumn column in data.Columns)
                    newRow[column.Ordinal] = ConvertObjectToString(row[column]);
                stringTable.Rows.Add(newRow);
            }

            // Need to work out column widths
            List<int> columnWidths = new List<int>();
            foreach (DataColumn column in stringTable.Columns)
            {
                int width = column.ColumnName.Length;
                foreach (DataRow row in stringTable.Rows)
                    width = System.Math.Max(width, row[column].ToString().Length);
                columnWidths.Add(width);
            }

            // Write out column headings.
            StringBuilder st = new StringBuilder(100000);
            if (showHeadings)
            {
                for (int i = startColumnIndex; i < stringTable.Columns.Count; i++)
                {
                    if (i > startColumnIndex) 
                        st.Append(delimiter);
                    st.AppendFormat("{0," + columnWidths[i] + "}", stringTable.Columns[i].ColumnName);
                }
                st.Append(Environment.NewLine);
            }

            // Write out each row.
            foreach (DataRow row in stringTable.Rows)
            {
                for (int i = startColumnIndex; i < stringTable.Columns.Count; i++)
                {
                    if (i > startColumnIndex) 
                        st.Append(delimiter);
                    st.AppendFormat("{0," + columnWidths[i] + "}", row[i]);
                }
                st.Append(Environment.NewLine);
            }
            return st.ToString();
        }

        /// <summary>
        /// Convert the specified object to a string.
        /// </summary>
        private static string ConvertObjectToString(object obj)
        {
            if (obj is DateTime)
            {
                DateTime D = Convert.ToDateTime(obj);
                return D.ToString("yyyy-MM-dd");
            }
            else if (obj is float || obj is double)
                return string.Format("{0:F3}", obj);
            else
                return obj.ToString();
        }

        /// <summary>Merges the columns and rows from one specified table to another.</summary>
        /// <remarks>The builtin DataTable.merge needs the fields to be the same type.
        /// This method will instead try and conver the fields.</remarks>
        /// <param name="from">The from table</param>
        /// <param name="to">The destination table.</param>
        public static void CopyRows(DataTable from, DataTable to)
        {
            foreach (DataRow row in from.Rows)
            {
                DataRow newRow = to.NewRow();
                foreach (DataColumn column in from.Columns)
                {
                    if (!Convert.IsDBNull(row[column]))
                    {
                        if (to.Columns.Contains(column.ColumnName))
                        {
                            Type toDataType = to.Columns[column.ColumnName].DataType;
                            bool conversionNeeded = column.DataType != toDataType;
                            if (conversionNeeded)
                            {
                                if (toDataType == typeof(float))
                                    newRow[column.ColumnName] = Convert.ToSingle(row[column]);
                                else if (toDataType == typeof(double))
                                    newRow[column.ColumnName] = Convert.ToDouble(row[column]);
                                else if (toDataType == typeof(int))
                                    newRow[column.ColumnName] = Convert.ToInt32(row[column]);
                                else
                                    throw new Exception("Cannot convert from type '" + column.DataType.ToString() +
                                                        "' to type '" + toDataType.ToString() + "'");
                            }
                            else
                                newRow[column.ColumnName] = row[column];
                        }
                    }
                }
                to.Rows.Add(newRow);
            }
        }
    }
}
