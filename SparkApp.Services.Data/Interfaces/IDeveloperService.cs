﻿
using SparkApp.Web.ViewModels.Developer;
using SparkApp.Web.ViewModels.Director;

namespace SparkApp.Services.Data.Interfaces
{
    public interface IDeveloperService : IBaseService
    {
        Task AddDeveloperAsync(AddDeveloperInputModel model);
    }
}