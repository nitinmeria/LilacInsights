using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Automapping;
using FluentNHibernate.Conventions.Helpers;
using System.Reflection;

namespace LilacInsight.NHibernate
{
    public class SessionFactory:IHttpModule
    {
        private static readonly ISessionFactory _SessionFactory;
        static SessionFactory()
        {
            _SessionFactory = CreateSessionFactory();
        }

        private static void BeginRequest(object sender, EventArgs e)
        {
            ISession session = _SessionFactory.OpenSession();
            session.BeginTransaction();
            CurrentSessionContext.Bind(session);
        }

        private static void EndRequest(object sender, EventArgs e)
        {
            ISession session = CurrentSessionContext.Unbind(_SessionFactory);
            if (session == null)
                return;
            try
            {
                session.Transaction.Commit();
            }
            catch (Exception)
            {

                session.Transaction.Rollback();
            }
            finally
            {
                session.Close();
                session.Dispose();
            }
        }

        private static ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure().Database(MsSqlConfiguration.MsSql2012
                .ConnectionString(c => c.FromConnectionStringWithKey("lilacDataConnection")))
                .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()))
                .CurrentSessionContext<WebSessionContext>()
                .BuildSessionFactory();
        }

        //private static AutoPersistenceModel CreateMappings()
        //{
        //    return AutoMap
        //       .Assembly(System.Reflection.Assembly.GetCallingAssembly())
        //       .Where(t => t.Namespace != "LilacInsight" && t.Namespace.EndsWith("Model"))
        //       .Conventions.Setup(c => c.Add(DefaultCascade.SaveUpdate()));
        //}

        public void Dispose()
        {
            
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += BeginRequest;
            context.EndRequest += EndRequest;
        }

        public static ISession GetCurrentSession()
        {
            return _SessionFactory.GetCurrentSession();
        }
 
    }
}
