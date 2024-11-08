using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SparkApp.Web.ViewModels.Game;

namespace SparkApp.Services.Data.Interfaces
{
    public interface IGameService : IBaseService
    {
        public Task<AddGameInputModel> GetInputGameModelAsync();

        public Task<AddGameInputModel> GetInputGameModelAsync(AddGameInputModel model);

        public Task AddGameAsync(AddGameInputModel model);
    }
}
