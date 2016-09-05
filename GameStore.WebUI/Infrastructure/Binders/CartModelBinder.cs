using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GameStore.Domain.Entities;

namespace GameStore.WebUI.Infrastructure.Binders
{
    public class CartModelBinder:IModelBinder
    {
        private const string sessionKey = "Cart";

        public object BindModel(ControllerContext controllerContext,
            ModelBindingContext bindingContext)
        {
            //получить объект Cart из сеанса
            Cart cart = null;
            if (controllerContext.HttpContext.Session != null)
            {
                cart=(Cart)controllerContext.HttpContext.Session[sessionKey];
            }

            //создать объект Cart если он не обнаружен в сеансе
            if (cart==null)
            {
                cart = new Cart();
                if (controllerContext.HttpContext.Session != null)
                {
                    controllerContext.HttpContext.Session[sessionKey] = cart;
                }
            }

            //возвратить объект Cart
            return cart;
        }
    }
}

