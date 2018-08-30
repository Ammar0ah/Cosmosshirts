using System;
using System.Collections.Generic;
using System.Text;
using FreshMvvm;

namespace Cosmosshirts.Helpers
{
    public class FreshViewModelMapper : IFreshPageModelMapper
    {
        public string GetPageTypeName(Type pageModelType)
        {
            return pageModelType.AssemblyQualifiedName
                .Replace("ViewModel", "View");
        }
    }
}
