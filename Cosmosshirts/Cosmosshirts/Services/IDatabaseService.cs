using System;
using System.Collections.Generic;
using System.Text;
using Cosmosshirts.Models;

namespace Cosmosshirts.Services
{
    public interface IDatabaseService
    {
        List<MenuItem> GetMenuItems();
    }
}
