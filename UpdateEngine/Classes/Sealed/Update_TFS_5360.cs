using UpdateEngine.Classes.Abstract;
using GPL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace UpdateEngine.Classes.Sealed
{
    internal sealed class Update_TFS_5360 : BaseUpdate
    {
        private const string _AR_ID = "1427763";
        private const string _TFS_ID = "5360";
        private const string _UpdateDescription = @"Bulk inactivation of MedSuite Carrier records.";

        private Dictionary<Enums.UpdateEnvironment, string> _EnvironmentsToApply = new Dictionary<Enums.UpdateEnvironment, string> {
        {Enums.UpdateEnvironment.Unknow,@"Unknow"},
        {Enums.UpdateEnvironment.Development,@"develop\develop_ans"},
        {Enums.UpdateEnvironment.Test,@"3s3kMedSQL01"},
        //{Enums.UpdateEnvironment.Stage,@"4s3kMedSQL01"},
        {Enums.UpdateEnvironment.Production,@"1s3kMedSQL01"}
        };

        private const string USER_INPUT_CSV_FILE = @"DATA_001.csv";
        private const string BACKUP_DATABASE_NAME = @"dbrollback";
        private const string TABLE_DATABASE = @"Americananes";
        private const string TABLE_SCHEMA = @"dbo";
        private const string TABLE_NAME = @"CARRIER";

        private string SQLcs = string.Empty;
        private System.Data.DataTable UserInputdt;
        private string DataPath;
        private string FULL_BACKUP_TABLE_NAME = @"[" + BACKUP_DATABASE_NAME + @"].[" + TABLE_SCHEMA + @"].[" + TABLE_NAME + @"_TFS" + _TFS_ID + @"]";
        private string FULL_UPDATE_TABLE_NAME = @"[" + TABLE_DATABASE + @"].[" + TABLE_SCHEMA + @"].[" + TABLE_NAME + @"]";
        private string FULL_BACKUP_TABLE_NAME_VERSIONED;

        public Update_TFS_5360()
        {
            SendMessage(string.Format("Starting Executing method:'{0}' on Class:'{1}'", Utility.GetCurrentMethodName(), this.GetType().Name));

            GetConfigValues();

            SendMessage(string.Format("Ending Executing method:'{0}' on Class:'{1}'", Utility.GetCurrentMethodName(), this.GetType().Name));
        }

        protected override void ExecuteBackUp()
        {
            SendMessage(string.Format("Starting Executing method:'{0}' on Class:'{1}'", Utility.GetCurrentMethodName(), this.GetType().Name));

            SendMessage(@"Executing the backup...");

            // if the backup table exist rename
            using (var dbh = new DBHelper(false))
            {
                dbh.CreateDBObjects(SQLcs, DBHelper.Providers.SqlServer, null);

                // Check if the backup table exist.
                var ct = @"select case when object_id('{0}') is null then cast(0 as bit) else cast(1 as bit) end".FormatString(FULL_BACKUP_TABLE_NAME);

                var RetVal = dbh.ExecuteScalar(ct, CommandType.Text, ConnectionState.Open);

                if (RetVal == null || RetVal.ToString().Parse<bool>())
#if DEBUG
                {
                    // Drop the table.
                    SendMessage(@"Dropping Table:'{0}'.".FormatString(FULL_BACKUP_TABLE_NAME));
                    ct = @"drop table {0}".FormatString(FULL_BACKUP_TABLE_NAME);
                    var RetVal1 = dbh.ExecuteNonQuery(ct, CommandType.Text, ConnectionState.Open);
                }
#else
                    throw new Exception(@"ERROR Table {0} exist, can not perform the backup twice.".FormatString(FULL_BACKUP_TABLE_NAME));

#endif
                //Copy the afected rows.
                for (int i = 0; i < UserInputdt.Rows.Count; i++)
                {
                    var r = UserInputdt.Rows[i];

                    string Carrier_ID = r["Carrier_ID"].ToString();

                    if (i == 0)
                        ct =
@"SELECT * 
INTO {0}  
FROM {1} with (nolock) 
WHERE 1=0 
UNION  
SELECT * FROM {1} with (nolock) 
WHERE [CARRIER_ID] = '{2}';".FormatString(FULL_BACKUP_TABLE_NAME, FULL_UPDATE_TABLE_NAME, Carrier_ID);
                    else
                        ct =
@"INSERT
INTO {0}  
SELECT *
FROM {1} with (nolock) 
WHERE [CARRIER_ID] = '{2}';
".FormatString(FULL_BACKUP_TABLE_NAME, FULL_UPDATE_TABLE_NAME, Carrier_ID);

                    // Backup the record.
                    SendMessage(@"Executing SQL Command =" + Environment.NewLine + @"{0}".FormatString(ct));

                    var rowsUpdated = dbh.ExecuteNonQuery(ct, CommandType.Text, ConnectionState.Open);

                    SendMessage(@"Rows Affected = " + @"{0}".FormatString(rowsUpdated));

                    var p = (((i + 1) * 100) / UserInputdt.Rows.Count);

                    SendProgress(p);
                }

                // Validate the Backup.
                // Get the total rows count of the backup table
                ct = @"select count(*) from {0};".FormatString(FULL_BACKUP_TABLE_NAME);

                RetVal = dbh.ExecuteScalar(ct, CommandType.Text, ConnectionState.Closed).ToString().Parse<int>();

                if (!RetVal.Equals(UserInputdt.Rows.Count))
                    throw new Exception(@"Not all records where backed up, Table {0} has {1} rows and must be {2}.".FormatString(FULL_BACKUP_TABLE_NAME, RetVal, UserInputdt.Rows.Count));

                SendMessage(@"Backup completed.");

                SendMessage(@"{0} Records were backed up on the {1} table.".FormatString(RetVal, FULL_BACKUP_TABLE_NAME));
            }

            SendMessage(string.Format("Ending Executing method:'{0}' on Class:'{1}'", Utility.GetCurrentMethodName(), this.GetType().Name));
        }

        protected override void ExecuteUpdate()
        {
            SendMessage(string.Format("Starting Executing method:'{0}' on Class:'{1}'", Utility.GetCurrentMethodName(), this.GetType().Name));

            SendMessage(@"Applying the Update...");
            using (var dbh = new DBHelper(false))
            {
                dbh.CreateDBObjects(SQLcs, DBHelper.Providers.SqlServer, null);

                for (int i = 0; i < UserInputdt.Rows.Count; i++)
                {
                    var r = UserInputdt.Rows[i];

                    string Carrier_ID = r["Carrier_ID"].ToString();
                    string Is_Active = r["Is_Active"].ToString();

                    var ct = @"update {0} set [IS_ACTIVE] = {1} where [CARRIER_ID] = '{2}'".FormatString(FULL_UPDATE_TABLE_NAME, Is_Active, Carrier_ID);

                    SendMessage(@"Executing SQL Command =" + Environment.NewLine + @"{0}".FormatString(ct));

                    var rowsUpdated = dbh.ExecuteNonQuery(ct, CommandType.Text, ConnectionState.Open);

                    SendMessage(@"Rows Affected = " + @"{0}".FormatString(rowsUpdated));

                    var p = (((i + 1) * 100) / UserInputdt.Rows.Count);

                    SendProgress(p);
                }

                _UpdateStatus = Enums.UpdateStatus.Applied;
            }

            SendMessage(Environment.NewLine + @"Applying the Update Completed." + Environment.NewLine);


            SendMessage(string.Format("Ending Executing method:'{0}' on Class:'{1}'", Utility.GetCurrentMethodName(), this.GetType().Name));
        }

        protected override void ExecuteRollBack()
        {
            SendMessage(string.Format("Starting Executing method:'{0}' on Class:'{1}'", Utility.GetCurrentMethodName(), this.GetType().Name));

            // Check if backup table exist.
            if (!Utility.TableExist(SQLcs, BACKUP_DATABASE_NAME, TABLE_SCHEMA, TABLE_NAME + @"_TFS" + TFS_ID))
            {
                _UpdateStatus = Enums.UpdateStatus.Unknow;

                throw new Exception(@"Trying to roll back the Update but the backup table {0} does not exist.".FormatString(FULL_BACKUP_TABLE_NAME));
            }

            SendMessage(@"Executing the rollback...");

            SendProgress(0);

            using (var dbh = new DBHelper(false))
            {
                dbh.CreateDBObjects(SQLcs, DBHelper.Providers.SqlServer, null);

                dbh.BeginTransaction();

                var ct =
@"-- Rollback is a delicated issue so with (nolock) can NOT be used here to avoid read or update uncomitten rows.
update t 
set    t.[is_active] = b.[is_active] 
from   {0} as b --with (nolock) no used for security.
       inner join {1} as t -- with (nolock) no used for security.
               on b.[carrier_id] = t.[carrier_id] ".FormatString(FULL_BACKUP_TABLE_NAME, FULL_UPDATE_TABLE_NAME);

                SendMessage(@"Executing SQL Command =" + Environment.NewLine + @"{0}".FormatString(ct));

                var rowsUpdated = dbh.ExecuteNonQuery(ct, CommandType.Text, ConnectionState.Open);

                SendMessage(@"Rows Affected = " + @"{0}".FormatString(rowsUpdated));

                dbh.CommitTransaction();
            }

            FULL_BACKUP_TABLE_NAME_VERSIONED = @"[" + BACKUP_DATABASE_NAME + @"].[" + TABLE_SCHEMA + @"].[" + TABLE_NAME + @"_TFS" + TFS_ID + @"_" + DateTime.Now.ToString("yyyyMMddHmmss" + @"]");

            SendProgress(50);

            Thread.Sleep(6000);

            ExecuteVersionBackUp();

            SendProgress(100);

            Thread.Sleep(6000);

            SendMessage(@"Rollback completed." + Environment.NewLine);

            SendMessage(string.Format("Ending Executing method:'{0}' on Class:'{1}'", Utility.GetCurrentMethodName(), this.GetType().Name));
        }

        private void ExecuteVersionBackUp()
        {
            SendMessage(string.Format("Starting Executing method:'{0}' on Class:'{1}'", Utility.GetCurrentMethodName(), this.GetType().Name));

            SendMessage(@"Versioning the backup table...");

            using (var dbh = new DBHelper(false))
            {
                dbh.CreateDBObjects(SQLcs, DBHelper.Providers.SqlServer, null);

                var db = @"[" + BACKUP_DATABASE_NAME + @"]";
                var bt = FULL_BACKUP_TABLE_NAME.ToString().Replace(db + @".", @"");
                var vt = FULL_BACKUP_TABLE_NAME_VERSIONED.ToString().Replace(db + @".[" + TABLE_SCHEMA + @"].[", @"").Replace(@"]", @"");

                var ct = @"USE {0}; execute sp_rename '{1}', '{2}'".FormatString(db, bt, vt);

                SendMessage(@"Executing SQL Command =" + Environment.NewLine + @"{0}".FormatString(ct));

                var rowsUpdated = dbh.ExecuteNonQuery(ct, CommandType.Text, ConnectionState.Open);

                SendMessage(@"Rows Affected = " + @"{0}".FormatString(rowsUpdated));
            }

            SendMessage(@"Versioning backup table completed.");


            SendMessage(string.Format("Ending Executing method:'{0}' on Class:'{1}'", Utility.GetCurrentMethodName(), this.GetType().Name));

        }

        protected override bool IsTimeToRun()
        {
            SendMessage(string.Format("Starting Executing method:'{0}' on Class:'{1}'", Utility.GetCurrentMethodName(), this.GetType().Name));

            bool retVal = true;

            SendMessage(string.Format("Ending Executing method:'{0}' on Class:'{1}'", Utility.GetCurrentMethodName(), this.GetType().Name));

            return retVal;
        }



        protected override void RefreshUpdateStatus()
        {
            SendMessage(string.Format("Starting Executing method:'{0}' on Class:'{1}'", Utility.GetCurrentMethodName(), this.GetType().Name));

            _UpdateStatus = Enums.UpdateStatus.Unknow;

            try
            {

                /*
                    NeverApplied = no backup tables,x
                    Applied      = the backup exist,x
                    Rollbacked   =  backup table versioned exist,
                    Unknow       = none of above. 
                 */

                if (!UpdateEnvironment.Equals(Enums.UpdateEnvironment.Unknow))
                {
                    GetPreExecutionData();

                    // Check if is  NeverApplied = no backup tables.
                    if (!Utility.TableExist(SQLcs, BACKUP_DATABASE_NAME, TABLE_SCHEMA, @"%_TFS" + TFS_ID + @"%"))
                    {
                        _UpdateStatus = Enums.UpdateStatus.NeverApplied;
                        return;
                    }

                    // Check if is   Applied      = the backup exist,
                    if (Utility.TableExist(SQLcs, BACKUP_DATABASE_NAME, TABLE_SCHEMA, TABLE_NAME + @"_TFS" + TFS_ID))
                    {
                        _UpdateStatus = Enums.UpdateStatus.Applied;
                        return;
                    }

                    // Check if is   backup table versioned exist,
                    if (Utility.TableExist(SQLcs, BACKUP_DATABASE_NAME, TABLE_SCHEMA, TABLE_NAME + @"_TFS" + TFS_ID + @"%"))
                    {
                        _UpdateStatus = Enums.UpdateStatus.Rollbacked;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                SendMessage(string.Format("An ERROR has occoured on method:'{0}' on Class:'{1}':" + Environment.NewLine + "'{2}' ", Utility.GetCurrentMethodName(), this.GetType().Name, ex.Message));
            }
            finally
            {
                SendMessage(string.Format("Ending Executing method:'{0}' on Class:'{1}'", Utility.GetCurrentMethodName(), this.GetType().Name));

            }

        }

        /// <summary>
        /// Gets the pre execution data.
        /// </summary>
        protected override void GetPreExecutionData()
        {
            SendMessage(string.Format("Starting Executing method:'{0}' on Class:'{1}'", Utility.GetCurrentMethodName(), this.GetType().Name));

            // Get the SQL connection strings by Environtment To Apply  The Update.
            SQLcs = @"Server={0};Database=master;Trusted_Connection=True;";

            // search the value in the EnvironmentsToApply Dictionary to replace the Server.
            string value = Enums.UpdateEnvironment.Unknow.ToString();

            EnvironmentsToApply.TryGetValue(UpdateEnvironment, out value);

            SQLcs = SQLcs.FormatString(value);

            SendMessage("SQLcs = {0}.".FormatString(SQLcs));

            DataPath = Path.Combine(CurrentExecutablePath, @"App_Data");
          
            SendMessage(string.Format("DataPath='{0}' on Class:'{1}'", DataPath, this.GetType().Name));

            LoadInputData();

            SendMessage(string.Format("Ending Executing method:'{0}' on Class:'{1}'", Utility.GetCurrentMethodName(), this.GetType().Name));
        }

        private void LoadInputData()
        {
            SendMessage(string.Format("Starting Executing method:'{0}' on Class:'{1}'", Utility.GetCurrentMethodName(), this.GetType().Name));


            //using (var dbh = new DBHelper(false))
            //{
            //    string Excelcs = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0 Xml;HDR=YES';".FormatString(UserDataFilePath);

            //    dbh.CreateDBObjects(Excelcs, DBHelper.Providers.OleDB, null);

            //    // Get data from Excel worksheet.
            //    UserInputdt = dbh.GetDataSet("SELECT * FROM [Records$]", CommandType.Text, ConnectionState.Closed).Tables[0];

            //    if (UserInputdt == null || UserInputdt.Rows.Count == 0)
            //        throw new DataException(@"Error: null ,empty ,Invalid data or invalid schema returned.");

            //    // Set the returned data to Settings DataTable.
            //    UserInputdt.TableName = "Records";

            //    SendMessage(log4net.Appender.RemoteSyslogAppender.SyslogSeverity.Debug, "{0} Records loaded from {1}".FormatString(UserInputdt.Rows.Count, UserDataFilePath));
            //}

            //UserInputdt = Utility.GetDataTableFromDelimitedFile(Path.Combine(DataPath, USER_INPUT_CSV_FILE));
            UserInputdt = Utility.GetDataTableFromDelimitedFile(Path.Combine(DataPath, USER_INPUT_CSV_FILE),true,true);

            SendMessage(string.Format("Ending Executing method:'{0}' on Class:'{1}'", Utility.GetCurrentMethodName(), this.GetType().Name));
        }

        public override string AR_ID
        {
            get { return _AR_ID; }
        }

        public override string TFS_ID
        {
            get { return _TFS_ID; }
        }

        public override string UpdateDescription
        {
            get { return _UpdateDescription; }
        }

      public override  Dictionary<Enums.UpdateEnvironment,string> EnvironmentsToApply
        {
            get { return _EnvironmentsToApply; }
        }
    }
}
