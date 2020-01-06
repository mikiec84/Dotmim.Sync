﻿using Dotmim.Sync.Manager;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using Dotmim.Sync.Sqlite.Manager;

namespace Dotmim.Sync.Sqlite
{
    public class SqliteManager : DbManager
    {
        public SqliteManager(string tableName) : base(tableName, "")
        {

        }

        public override IDbManagerTable CreateManagerTable(DbConnection connection, DbTransaction transaction = null)
        {
            return new SqliteManagerTable(connection, transaction) { TableName = this.TableName };
        }


    }
}
