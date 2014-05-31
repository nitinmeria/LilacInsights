using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using System.Reflection;
using FluentNHibernate.Conventions.Helpers;

namespace TargetData.NHibernate
{
    public class SessionManager:IDisposable
    {
        private ISessionFactory SessionFactory { get; set; }

        public SessionManager(string connectionString)
        {
            CreateSessionFactory(connectionString);
        }

        private void CreateSessionFactory(string connectionString)
        {
            SessionFactory = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012
                .ConnectionString(connectionString))
                .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()))
                .BuildSessionFactory();
        }

        public  ISession Open()
        {
            return SessionFactory.OpenSession();
        }

        public void Dispose()
        {
            SessionFactory.Close();
            SessionFactory.Dispose();
        }
    }
}