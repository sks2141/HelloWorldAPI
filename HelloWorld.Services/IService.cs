using HelloWorld.ViewModel;
using System.Collections.Generic;


namespace HelloWorld.Services
{
    public interface IService
    {
        IEnumerable<ResponseStatsViewModel> GetResponses();
    }
}