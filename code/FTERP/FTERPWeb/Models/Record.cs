using System;
using System.Collections.Generic;
using PetaPoco;

namespace FTERPWeb.Models
{
    [Serializable]
    public class Record<T> where T : new()
    {
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

        public static DefaultConnectionDB repo
        {
            get
            {
                return DefaultConnectionDB.GetInstance();
            }
        }

        public bool IsNew()
        {
            try
            {
                return repo.IsNew(this);
            }
            catch (Exception e)
            {
                log.Error("{0}:{1}", DateTime.Now, e.Message);
                return false;
            }
        }

        public object Insert()
        {
            try
            {
                return repo.Insert(this);
            }
            catch (Exception e)
            {
                log.Error("{0}:{1}", DateTime.Now, e.Message);
                return null;
            }
        }

        public void Save()
        {
            try
            {
                repo.Save(this);
            }
            catch (Exception e)
            {
                log.Error("{0}:{1}", DateTime.Now, e.Message);
            }
        }

        public int Update()
        {
            try
            {
                return repo.Update(this);
            }
            catch (Exception e)
            {
                log.Error("{0}:{1}", DateTime.Now, e.Message);
                return 0;
            }
        }

        public int Update(IEnumerable<string> columns)
        {
            try
            {
                return repo.Update(this, columns);
            }
            catch (Exception e)
            {
                log.Error("{0}:{1}", DateTime.Now, e.Message);
                return 0;
            }
        }

        public static int Update(string sql, params object[] args)
        {
            try
            {
                return repo.Update<T>(sql, args);
            }
            catch (Exception e)
            {
                log.Error("{0}:{1}", DateTime.Now, e.Message);
                return 0;
            }
        }

        public static int Update(Sql sql)
        {
            try
            {
                return repo.Update<T>(sql);
            }
            catch (Exception e)
            {
                log.Error("{0}:{1}", DateTime.Now, e.Message);
                return 0;
            }
        }

        public int Delete()
        {
            try
            {
                return repo.Delete(this);
            }
            catch (Exception e)
            {
                log.Error("{0}:{1}", DateTime.Now, e.Message);
                return 0;
            }
        }

        public static int Delete(string sql, params object[] args)
        {
            try
            {
                return repo.Delete<T>(sql, args);
            }
            catch (Exception e)
            {
                log.Error("{0}:{1}", DateTime.Now, e.Message);
                return 0;
            }
        }

        public static int Delete(Sql sql)
        {
            try
            {
                return repo.Delete<T>(sql);
            }
            catch (Exception e)
            {
                log.Error("{0}:{1}", DateTime.Now, e.Message);
                return 0;
            }
        }

        public static int Delete(object primaryKey)
        {
            try
            {
                return repo.Delete<T>(primaryKey);
            }
            catch (Exception e)
            {
                log.Error("{0}:{1}", DateTime.Now, e.Message);
                return 0;
            }
        }

        public static bool Exists(object primaryKey)
        {
            try
            {
                return repo.Exists<T>(primaryKey);
            }
            catch (Exception e)
            {
                log.Error("{0}:{1}", DateTime.Now, e.Message);
                return false;
            }
        }

        public static bool Exists(string sql, params object[] args)
        {
            try
            {
                return repo.Exists<T>(sql, args);
            }
            catch (Exception e)
            {
                log.Error("{0}:{1}", DateTime.Now, e.Message);
                return false;
            }
        }

        public static T SingleOrDefault(object primaryKey)
        {
            try
            {
                return repo.SingleOrDefault<T>(primaryKey);
            }
            catch (Exception e)
            {
                log.Error("{0}:{1}", DateTime.Now, e.Message);
                return default(T);
            }
        }

        public static T SingleOrDefaultBySql(string sql, params object[] args)
        {
            try
            {
                return repo.SingleOrDefault<T>(sql, args);
            }
            catch (Exception e)
            {
                log.Error("{0}:{1}", DateTime.Now, e.Message);
                return default(T);
            }
        }

        public static T SingleOrDefaultBySql(Sql sql)
        {
            try
            {
                return repo.SingleOrDefault<T>(sql);
            }
            catch (Exception e)
            {
                log.Error("{0}:{1}", DateTime.Now, e.Message);
                return default(T);
            }
        }

        public static T FirstOrDefault(string sql, params object[] args)
        {
            try
            {
                return repo.FirstOrDefault<T>(sql, args);
            }
            catch (Exception e)
            {
                log.Error("{0}:{1}", DateTime.Now, e.Message);
                return default(T);
            }
        }

        public static T FirstOrDefault(Sql sql)
        {
            try
            {
                return repo.FirstOrDefault<T>(sql);
            }
            catch (Exception e)
            {
                log.Error("{0}:{1}", DateTime.Now, e.Message);
                return default(T);
            }
        }

        public static T Single(object primaryKey)
        {
            try
            {
                return repo.Single<T>(primaryKey);
            }
            catch (Exception e)
            {
                log.Error("{0}:{1}", DateTime.Now, e.Message);
                return default(T);
            }
        }

        public static T SingleBySql(string sql, params object[] args)
        {
            try
            {
                return repo.Single<T>(sql, args);
            }
            catch (Exception e)
            {
                log.Error("{0}:{1}", DateTime.Now, e.Message);
                return default(T);
            }
        }

        public static T SingleBySql(Sql sql)
        {
            try
            {
                return repo.Single<T>(sql);
            }
            catch (Exception e)
            {
                log.Error("{0}:{1}", DateTime.Now, e.Message);
                return default(T);
            }
        }

        public static T First(string sql, params object[] args)
        {
            try
            {
                return repo.First<T>(sql, args);
            }
            catch (Exception e)
            {
                log.Error("{0}:{1}", DateTime.Now, e.Message);
                return default(T);
            }
        }

        public static T First(Sql sql)
        {
            try
            {
                return repo.First<T>(sql);
            }
            catch (Exception e)
            {
                log.Error("{0}:{1}", DateTime.Now, e.Message);
                return default(T);
            }
        }

        public static List<T> Fetch(string sql, params object[] args)
        {
            try
            {
                return repo.Fetch<T>(sql, args);
            }
            catch (Exception e)
            {
                log.Error("{0}:{1}", DateTime.Now, e.Message);
                return null;
            }
        }

        public static List<T> Fetch(Sql sql)
        {
            try
            {
                return repo.Fetch<T>(sql);
            }
            catch (Exception e)
            {
                log.Error("{0}:{1}", DateTime.Now, e.Message);
                return null;
            }
        }

        public static List<T> Fetch(long page, long itemsPerPage, string sql, params object[] args)
        {
            try
            {
                return repo.Fetch<T>(page, itemsPerPage, sql, args);
            }
            catch (Exception e)
            {
                log.Error("{0}:{1}", DateTime.Now, e.Message);
                return null;
            }
        }

        public static List<T> Fetch(long page, long itemsPerPage, Sql sql)
        {
            try
            {
                return repo.Fetch<T>(page, itemsPerPage, sql);
            }
            catch (Exception e)
            {
                log.Error("{0}:{1}", DateTime.Now, e.Message);
                return null;
            }
        }

        public static List<T> SkipTake(long skip, long take, string sql, params object[] args)
        {
            try
            {
                return repo.SkipTake<T>(skip, take, sql, args);
            }
            catch (Exception e)
            {
                log.Error("{0}:{1}", DateTime.Now, e.Message);
                return null;
            }
        }

        public static List<T> SkipTake(long skip, long take, Sql sql)
        {
            try
            {
                return repo.SkipTake<T>(skip, take, sql);
            }
            catch (Exception e)
            {
                log.Error("{0}:{1}", DateTime.Now, e.Message);
                return null;
            }
        }

        public static Page<T> Page(long page, long itemsPerPage, string sql, params object[] args)
        {
            try
            {
                return repo.Page<T>(page, itemsPerPage, sql, args);
            }
            catch (Exception e)
            {
                log.Error("{0}:{1}", DateTime.Now, e.Message);
                return null;
            }
        }

        public static Page<T> Page(long page, long itemsPerPage, Sql sql)
        {
            try
            {
                return repo.Page<T>(page, itemsPerPage, sql);
            }
            catch (Exception e)
            {
                log.Error("{0}:{1}", DateTime.Now, e.Message);
                return null;
            }
        }

        public static IEnumerable<T> Query(string sql, params object[] args)
        {
            try
            {
                return repo.Query<T>(sql, args);
            }
            catch (Exception e)
            {
                log.Error("{0}:{1}", DateTime.Now, e.Message);
                return null;
            }
        }

        public static IEnumerable<T> Query(Sql sql)
        {
            try
            {
                return repo.Query<T>(sql);
            }
            catch (Exception e)
            {
                log.Error("{0}:{1}", DateTime.Now, e.Message);
                return null;
            }
        }

    }
}