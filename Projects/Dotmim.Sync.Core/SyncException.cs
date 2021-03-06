﻿using Dotmim.Sync.Enumerations;
using Dotmim.Sync.Serialization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Dotmim.Sync
{

    /// <summary>
    /// Exception
    /// </summary>
    public class SyncException : Exception
    {
        public SyncException(string message) : base(message)
        {

        }

        public SyncException(Exception exception, SyncStage stage = SyncStage.None) : base(exception.Message, exception)
        {
            this.SyncStage = stage;

            if (exception is null)
                return;

            this.TypeName = exception.GetType().Name;
        }

        /// <summary>
        /// Gets or Sets type name of exception
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// Sync stage when exception occured
        /// </summary>
        public SyncStage SyncStage { get; set; }

        /// <summary>
        /// Data source error number if available
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Gets or Sets data source if available
        /// </summary>
        public string DataSource { get; set; }

        /// <summary>
        /// Gets or Sets initial catalog if available
        /// </summary>
        public string InitialCatalog { get; set; }

        /// <summary>
        /// Gets or Sets if error is Local or Remote side
        /// </summary>
        public SyncExceptionSide Side { get; set; }

    }


    /// <summary>
    /// Defines where occured the exception
    /// </summary>
    public enum SyncExceptionSide
    {
        /// <summary>
        /// Occurs when error comes from LocalOrchestrator
        /// </summary>
        ClientSide,

        /// <summary>
        /// Occurs when error comes from RemoteOrchestrator
        /// </summary>
        ServerSide
    }

    /// <summary>
    /// Rollback Exception
    /// </summary>
    public class RollbackException : Exception
    {
        public RollbackException(string message) : base(message) { }
    }

    /// <summary>
    /// Occurs when trying to launch another sync during an in progress sync.
    /// </summary>
    public class AlreadyInProgressException : Exception
    {
        const string message = "Synchronization already in progress";

        public AlreadyInProgressException() : base(message) { }
    }

    /// <summary>
    /// Occurs when trying to launch another sync during an in progress sync.
    /// </summary>
    public class FormatTypeException : Exception
    {
        const string message = "The type {0} is not supported ";

        public FormatTypeException(Type type) : base(string.Format(message, type.Name)) { }
    }



    /// <summary>
    /// Occurs when a file is missing
    /// </summary>
    public class MissingFileException : Exception
    {
        const string message = "File {0} doesn't exist.";

        public MissingFileException(string fileName) : base(string.Format(message, fileName)) { }
    }


    /// <summary>
    /// Occurs when primary key is missing in the table schema
    /// </summary>
    public class MissingPrimaryKeyException : Exception
    {
        const string message = "Table {0} does not have any primary key.";

        public MissingPrimaryKeyException(string tableName) : base(string.Format(message, tableName)) { }
    }

    /// <summary>
    /// Setup table exception. Used when a setup table is defined that does not exist in the data source
    /// </summary>
    public class MissingTableException : Exception
    {
        const string message = "Table {0} does not exists.";

        public MissingTableException(string tableName) : base(string.Format(message, tableName)) { }
    }

    /// <summary>
    /// Setup column exception. Used when a setup column  is defined that does not exist in the data source table
    /// </summary>
    public class MissingColumnException : Exception
    {
        const string message = "Column {0} does not exists in the table {1}.";

        public MissingColumnException(string columnName, string sourceTableName) : base(string.Format(message, columnName, sourceTableName)) { }
    }


    /// <summary>
    /// Setup table exception. Used when a your setup does not contains any table
    /// </summary>
    public class MissingTablesException : Exception
    {
        const string message = "Your setup does not contains any table.";

        public MissingTablesException() : base(message) { }
    }

    /// <summary>
    /// Metadata exception.
    /// </summary>
    public class MetadataException : Exception
    {
        const string message = "No metadatas rows found for table {0}.";

        public MetadataException(string tableName) : base(string.Format(message, tableName)) { }
    }


    /// <summary>
    /// Occurs when a row is too big for download batch size
    /// </summary>
    public class RowOverSizedException : Exception
    {
        const string message = "Row is too big ({0} kb.) for the current DownloadBatchSizeInKB.";

        public RowOverSizedException(string finalFieldSize) : base(string.Format(message, finalFieldSize)) { }
    }

    /// <summary>
    /// Occurs when a command is missing
    /// </summary>
    public class MissingCommandException : Exception
    {
        const string message = "Missing command {0}.";

        public MissingCommandException(string commandType) : base(string.Format(message, commandType)) { }
    }

    /// <summary>
    /// Occurs when we use change tracking and it's not enabled on the source database
    /// </summary>
    public class MissingChangeTrackingException : Exception
    {
        const string message = "Change Tracking is not activated for database {0}. Please execute this statement : Alter database {0} SET CHANGE_TRACKING = ON (CHANGE_RETENTION = 14 DAYS, AUTO_CLEANUP = ON)";

        public MissingChangeTrackingException(string databaseName) : base(string.Format(message, databaseName)) { }
    }

    /// <summary>
    /// Occurs when we check database existence
    /// </summary>
    public class MissingDatabaseException : Exception
    {

        const string message = "Database {0} does not exist";

        public MissingDatabaseException(string databaseName) : base(string.Format(message, databaseName)) { }
    }


    /// <summary>
    /// Occurs when a column is not supported by the Dotmim.Sync framework
    /// </summary>
    public class UnsupportedColumnTypeException : Exception
    {
        const string message = "The Column {0} of type {1} from provider {2} is not currently supported.";

        public UnsupportedColumnTypeException(string columnName, string columnType, string provider) : base(string.Format(message, columnName, columnType, provider)) { }
    }



    /// <summary>
    /// [Not Used] Occurs when sync metadatas are out of date
    /// </summary>
    public class OutOfDateException : Exception
    {
        const string message = "The provider is out of date ! Try to make a Reinitialize sync.";

        public OutOfDateException() : base(message) { }
    }

    /// <summary>
    /// Http empty response exception.
    /// </summary>
    public class HttpEmptyResponseContentException : Exception
    {
        const string message = "The reponse has an empty body.";

        public HttpEmptyResponseContentException() : base(message) { }
    }

    /// <summary>
    /// Http response exception.
    /// </summary>
    public class HttpResponseContentException : Exception
    {
        public HttpResponseContentException(string content) : base(content) { }
    }

    /// <summary>
    /// Occurs when a header is missing in the http request
    /// </summary>
    public class HttpHeaderMissingExceptiopn : Exception
    {
        const string message = "Header {0} is missing.";

        public HttpHeaderMissingExceptiopn(string header) : base(string.Format(message, header)) { }
    }

    /// <summary>
    /// Occurs when a cache is not set on the server
    /// </summary>
    public class HttpCacheNotConfiguredException : Exception
    {
        const string message = "Cache is not configured! Please add memory cache (distributed or not). See https://docs.microsoft.com/en-us/aspnet/core/performance/caching/response?view=aspnetcore-2.2).";

        public HttpCacheNotConfiguredException() : base(message) { }
    }

    /// <summary>
    /// Occurs when a Serializer is not available on the server side
    /// </summary>
    public class HttpSerializerNotConfiguredException : Exception
    {
        const string message = "Unexpected value for serializer. Available serializers on the server: {0}";
        const string messageEmpty = "Unexpected value for serializer. Server has not any serializer registered";

        public HttpSerializerNotConfiguredException(IEnumerable<string> serializers) :
            base(
                serializers.Count() > 0 ?
                    string.Format(message, string.Join(".", serializers))
                    : messageEmpty
                )
        { }
    }
    /// <summary>
    /// Occurs when a Serializer is not available on the server side
    /// </summary>
    public class HttpConverterNotConfiguredException : Exception
    {
        const string message = "Unexpected value for converter. Available converters on the server: {0}";
        const string messageEmpty = "Unexpected value for converter. Server has not any converter registered";


        public HttpConverterNotConfiguredException(IEnumerable<string> converters) :
            base(
                converters.Count() > 0 ?
                    string.Format(message, string.Join(".", converters))
                    : messageEmpty
                )
        { }
    }


    /// <summary>
    /// Occurs when a parameter has been already added in a filter parameter list
    /// </summary>
    public class FilterParameterAlreadyExistsException : Exception
    {
        const string message = "The parameter {0} has been already added for the {1} changes stored procedure";

        public FilterParameterAlreadyExistsException(string parameterName, string tableName) : base(string.Format(message, parameterName, tableName)) { }
    }

    /// <summary>
    /// Occurs when a filter already exists for a named table
    /// </summary>
    public class FilterAlreadyExistsException : Exception
    {
        const string message = "The filter for the {0} changes stored procedure already exists";

        public FilterAlreadyExistsException(string tableName) : base(string.Format(message, tableName)) { }
    }


    /// <summary>
    /// Occurs when a filter column used as a filter for a tracking table, has not been added to the column parameters list
    /// </summary>
    public class FilterTrackingWhereException : Exception
    {
        const string message = "The column {0} does not exist in the columns parameters list, so can't be add as a where filter clause to the tracking table";

        public FilterTrackingWhereException(string columName) : base(string.Format(message, columName)) { }
    }


    /// <summary>
    /// Occurs when a filter column used as a filter for a tracking table, but not exists
    /// </summary>
    public class FilterParamColumnNotExistsException : Exception
    {
        const string message = "The parameter {0} does not exist as a column in the table {1}";

        public FilterParamColumnNotExistsException(string columName, string tableName) : base(string.Format(message, columName, tableName)) { }
    }

    /// <summary>
    /// Occurs when a filter column used as a filter for a tracking table, but not exists
    /// </summary>
    public class FilterParamTableNotExistsException : Exception
    {
        const string message = "The table {0} does not exist";

        public FilterParamTableNotExistsException(string tableName) : base(string.Format(message, tableName)) { }
    }

    /// <summary>
    /// Occurs when a parameter has been already added to the parameter collection
    /// </summary>
    public class SyncParameterAlreadyExistsException : Exception
    {
        const string message = "The parameter {0} already exists in the parameter list.";

        public SyncParameterAlreadyExistsException(string parameterName) : base(string.Format(message, parameterName)) { }
    }


    /// <summary>
    /// Occurs when trying to apply a snapshot that does not exists
    /// </summary>
    public class SnapshotNotExistsException : Exception
    {
        const string message = "The snapshot {0} does not exists.";

        public SnapshotNotExistsException(string directoryName) : base(string.Format(message, directoryName)) { }
    }

}
