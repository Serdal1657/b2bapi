﻿using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class basketListDto: Basket
    {
        public String ProductName { get; set; }
        public decimal Total { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }


    }
}
