using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameStore.WebUI.Models
{
    public class PagingInfo
    {
        //кол-во товаров
        public int TotalItems { get; set; }

        //кол-во товаров на одной странице
        public int ItemsPerPage { get; set; }

        //номер текущей страницы
        public int CurrentPage { get; set; }

        //общее количество страниц
        public int TotalPages
        {
            get { return (int)Math.Ceiling((decimal)TotalItems/ItemsPerPage); }
        }
    }
}