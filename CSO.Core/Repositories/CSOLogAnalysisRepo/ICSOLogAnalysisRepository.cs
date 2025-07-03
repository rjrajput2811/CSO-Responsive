using CSO.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSO.Core.Repositories.CSOLogAnalysisRepo
{
    public interface ICSOLogAnalysisRepository
    {
        Task<List<CSOLogGridModel>> GetCSOLogListAsync();
    }
}
