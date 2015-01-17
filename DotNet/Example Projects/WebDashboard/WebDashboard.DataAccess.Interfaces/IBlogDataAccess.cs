using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebDashboard.DataAccess.Interfaces
{
    public interface IBlogDataAccess
    {
        void SaveBlogEntry(string message);
    }
}
