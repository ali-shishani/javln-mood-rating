using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewProjectTemplate.Config.Provider
{
    public interface IAppConfigurationProvider
    {
        string GetConnectionString();
    }
}
