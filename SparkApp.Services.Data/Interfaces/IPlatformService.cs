using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SparkApp.Web.ViewModels.Platform;

namespace SparkApp.Services.Data.Interfaces
{
    public interface IPlatformService : IBaseService
    {
        Task AddPlatformAsync(AddPlatformInputModel model);
    }
}
