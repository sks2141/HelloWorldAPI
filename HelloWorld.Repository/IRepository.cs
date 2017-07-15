using HelloWorld.Model;
using System.Collections.Generic;

namespace HelloWorld.DataAccess
{
    public interface IRepository
    {
        IEnumerable<ResponseEntity> GetResponses();
    }
}