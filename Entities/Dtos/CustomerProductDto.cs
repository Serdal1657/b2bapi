using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class CustomerProductDto
    {
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }

        


    }
}
