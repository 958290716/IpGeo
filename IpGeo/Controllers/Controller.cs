using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using IpGeo;
using IpGeo.api.Models;

namespace IpGeo.Controllers
{
    public class Controller 
    {
        public IActionResult Index()
        {
            throw new NotImplementedException();
        }

        public async Task<IpGeoData> Create([FromBody] int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IpGeoData> Delete([FromBody] int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IpGeoData> Update([FromBody] int id)
        {
            throw new NotImplementedException();
        }
    }
}
