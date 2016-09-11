using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameStore.Domain.Entities;

namespace GameStore.Domain.Abstract
{
    public interface IOrderProcessor
    {
        void ProcessOrder(Cart cart,ShippingDetails shippingDetails);
    }
}
