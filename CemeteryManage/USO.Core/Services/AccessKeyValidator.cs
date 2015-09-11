
namespace USO.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;


    public class AccessKeyValidator : IAccessKeyValidator
    {
        public bool IsValidAccessKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return false;
            }

            Guid checkedKey;
            return Guid.TryParse(key, out checkedKey);
        }
    }
}
