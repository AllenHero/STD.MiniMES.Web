﻿using STD.Framework.Core;
using STD.Framework.Data;

namespace STD.MiniMES.Web
{
    public class FrameworkConfig
    {
        public static void Register()
        {
            APP.DB_DEFAULT_CONN_NAME = "Sys";
            APP.OnDbExecuting = OnDbExecuting;
            APP.Init();
        }

        public static void OnDbExecuting(CommandEventArgs args)
        {
            var sql = args.Command.CommandText;
        }

        /*public static void LogDatabase(object sysLog,IDbContext db = null) 
        {
            var log = sysLog as sys_log;
            if (log==null) return;

            log.Date = DateTime.Now;

            if (db == null)
            {
                using (db = Db.Context())
                {
                    db.Insert<sys_log>("sys_log", log).AutoMap(x => x.ID).Execute();
                }
            }
            else
            {
                db.Insert<sys_log>("sys_log", log).AutoMap(x => x.ID).Execute();
            }
        }*/
    }
}