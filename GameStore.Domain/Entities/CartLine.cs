using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameStore.Domain.Entities
{
    public class CartLine
    {
        public Game Game { get; set; }
        public int Quantity { get; set; }
    }
}
