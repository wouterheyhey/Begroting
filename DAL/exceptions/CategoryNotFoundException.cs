using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.exceptions
{
    class CategoryNotFoundException : NullReferenceException
    {
        public CategoryNotFoundException(string message) : base(message)
        {
            System.Diagnostics.Debug.WriteLine(this.Message);
        }
    }
}
