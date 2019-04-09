﻿
namespace APSIM.Shared.Utilities
{
    using System.Collections.Generic;
    using System;

    /// <summary>
    /// A database specific connection
    /// </summary>
    public interface IDatabaseConnection
    {
        /// <summary>Property to return true if the database is readonly.</summary>
        bool IsReadOnly { get; }

        /// <summary>Property to return true if the database is open.</summary>
        /// <value><c>true</c> if this instance is open; otherwise, <c>false</c>.</value>
        bool IsOpen { get; }

        /// <summary>Opens or creates Firebird database with the specified path</summary>
        /// <param name="path">Path to Firebird database</param>
        /// <param name="readOnly">if set to <c>true</c> [read only].</param>
        void OpenDatabase(string path, bool readOnly);

        /// <summary>Closes the Firebird database</summary>
        void CloseDatabase();

        /// <summary>Return a list of table names</summary>
        /// <returns>A list of table names in sorted order (upper case)</returns>
        List<string> GetTableNames();

        /// <summary>Does the specified table exist?</summary>
        /// <param name="tableName">The table name to look for</param>
        bool TableExists(string tableName);

        /// <summary>
        /// Run a query and return a data table of results
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        System.Data.DataTable ExecuteQuery(string query);

        /// <summary>
        /// Executes a query and return a single integer value to caller. Returns -1 if not found.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="columnNumber">The column number.</param>
        /// <returns>The integer for the column (0-n) for the first row</returns>
        int ExecuteQueryReturnInt(string query, int columnNumber = 0);

        /// <summary>
        /// Executes a query that returns no results
        /// </summary>
        /// <param name="query"></param>
        void ExecuteNonQuery(string query);

        /// <summary>Return a list of column names.</summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>A list of column names in column order (uppercase)</returns>
        List<string> GetColumnNames(string tableName);

        /// <summary>Return a list of column names with a data type of string.</summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        List<string> GetStringColumnNames(string tableName);

        /// <summary>Return a list of column names for the specified table</summary>
        /// <param name="tableName">The table name to get columns from.</param>
        List<string> GetTableColumns(string tableName);

        /// <summary>
        /// Drop (remove) columns from a table.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="colsToRemove"></param>
        void DropColumns(string tableName, IEnumerable<string> colsToRemove);

        /// <summary>
        /// Begin a transaction.
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// End a transaction.
        /// </summary>
        void EndTransaction();

        /// <summary>
        /// Do and ALTER on the db table and add a column
        /// </summary>
        /// <param name="tableName">The table name</param>
        /// <param name="columnName">The column to add</param>
        /// <param name="columnType">The db column type</param>
        void AddColumn(string tableName, string columnName, string columnType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnNames"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        int InsertRows(string tableName, List<string> columnNames, List<object[]> values);

        /// <summary>Convert .NET value into an SQLite type</summary>
        string GetDBDataTypeName(object value);

        /// <summary>Convert .NET type into an SQLite type</summary>
        string GetDBDataTypeName(Type type);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="colNames"></param>
        /// <param name="colTypes"></param>
        void CreateTable(string tableName, List<string> colNames, List<string> colTypes);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        string AsSQLString(DateTime value);
    }
}
