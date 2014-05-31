using LilacInsight.Model;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LilacInsight.DataAccess
{
    public interface ILilacDao
    {
        bool GetLogin(float username, string password);
        IList<Testmaster> GetReport(ISession session);
    }
}
