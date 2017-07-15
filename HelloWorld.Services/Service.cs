using AutoMapper;
using HelloWorld.DataAccess;
using HelloWorld.Model;
using HelloWorld.ViewModel;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace HelloWorld.Services
{
    public class Service : IService
    {
        private readonly IRepository repository;
        private readonly ILogger<Service> logger;

        public Service(IRepository repository, ILogger<Service> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public IEnumerable<ResponseStatsViewModel> GetResponses()
        {
            this.logger.LogInformation("Getting all responses from Service.");

            ICollection<ResponseStatsViewModel> responses = new Collection<ResponseStatsViewModel>();

            try
            {
                var responseEntities = this.repository.GetResponses();
                responses = Mapper.Map<IEnumerable<ResponseEntity>, Collection<ResponseStatsViewModel>>(responseEntities);
            }
            catch(Exception ex)
            {
                logger.LogError("Exception while getting response. Returning empty response. Details:{0}", ex.Message);
            }

            return responses;
        }
    }
}