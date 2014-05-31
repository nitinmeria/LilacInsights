using LilacInsight.Model;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TargetData.NHibernate;

namespace LilacInsight.DataAccess
{
    public class LilacDao:ILilacDao
    {
         protected string _connectionString;

        public LilacDao(string connectionString)
        {
            _connectionString = connectionString;
        }
        public bool GetLogin(float username, string password)
        {
            
            using (ISession session =new SessionManager(_connectionString).Open())
            {
                Refmaster lgobj = new Refmaster();
                lgobj = session.QueryOver<Refmaster>().Where(x => x.refid == username).Where(x => x.password == password).Where(x => x.user_type == "1").List().FirstOrDefault();

                if (lgobj == null)
                {
                    return false;
                }
                else
                    //if (lgobj.refid == username && lgobj.password == password && lgobj.user_type == "1")
                    return true;
            }
        }

        public IList<Testmaster> GetReport(ISession sessions)
        {
       
            using (ISession session =sessions)
            {
                //IList<Testmaster> items = session.QueryOver<Testmaster>()
                //    .JoinQueryOver(testmaster => testmaster.Patientmaster)
                //    .List();
                Refmaster refmaster=null;
                Patientmaster pat=null;
                IList<Testmaster> items = session.QueryOver<Testmaster>()
                    .JoinAlias(effort => effort.Patientmaster, () => pat)
                    .JoinAlias(effort => effort.Refmaster, () => refmaster)
                    .Select(effort=>pat.name,effort=>refmaster.name,effort=>effort.rptname,effort=>effort.rptintime,effort=>effort.rptouttime,effort=>effort.uploadtime)
                    .List<Testmaster>();
                    
                return items??new List<Testmaster>();
                                    
            }
        }
    }
}
