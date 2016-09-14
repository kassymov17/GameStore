using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GameStore.Domain.Entities
{
    public class Game
    {
        [HiddenInput(DisplayValue=false)]
        public int GameId { get; set; }
        
        [Display(Name="Название")]
        [Required(ErrorMessage="Пожалуйста, введите название игры")]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name="Описания")]
        [Required(ErrorMessage="Пожалуйста, введите описание для игры")]
        public string Description { get; set; }

        [Display(Name="Категория")]
        [Required(ErrorMessage="Пожалуйста, укажите категорию для игры")]
        public string Category { get; set; }
        
        [Display(Name="Цена (руб)")]
        [Required]
        [Range(0.01,double.MaxValue,ErrorMessage="Пожалуйста, введите положительное значение для цены")]
        public decimal Price { get; set; }
    }
}
