﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameStore.Domain.Entities;
using GameStore.Domain.Abstract;
using Moq;
using GameStore.WebUI.Controllers;
using System.Web.Mvc;
using GameStore.WebUI.Models;

namespace GameStore.UnitTests
{
    [TestClass]
    public class CartTests
    {
        [TestMethod]
        public void Can_Add_New_Lines()
        {
            //организация-создание нескольких тестовых игр
            Game game1 = new Game { GameId=1,Name="Игра1"};
            Game game2 = new Game { GameId = 2, Name = "Игра2" };

            //организация-создание корзины
            Cart cart = new Cart();

            //действие
            cart.AddItem(game1, 1);
            cart.AddItem(game2, 1);
            List<CartLine> results = cart.Lines.ToList();

            //утверждение 
            Assert.AreEqual(results.Count(),2);
            Assert.AreEqual(results[0].Game,game1);
            Assert.AreEqual(results[1].Game,game2);
        }

        [TestMethod]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            //ораганизация-создание нескольких тестовых игр
            Game game1 = new Game { GameId = 1, Name = "Игра1" };
            Game game2 = new Game { GameId = 2, Name = "Игра2" };

            //организация-создание корзины
            Cart cart = new Cart();

            //действие
            cart.AddItem(game1, 1);
            cart.AddItem(game2, 1);
            cart.AddItem(game1,2);
            cart.AddItem(game2, 2);
            List<CartLine> results = cart.Lines.OrderBy(c => c.Game.GameId).ToList();

            //утверждение
            Assert.AreEqual(results.Count(),2);
            Assert.AreEqual(results[0].Quantity,3);
            Assert.AreEqual(results[1].Quantity, 3);
        }

        [TestMethod]
        public void Can_Remove_Line()
        {
            //орагнизация 
            Game game1 = new Game { GameId = 1, Name = "Игра1" };
            Game game2 = new Game { GameId = 2, Name = "Игра2" };
            Game game3 = new Game { GameId = 3, Name = "Игра3" };

            //организация - создание корзины
            Cart cart = new Cart();

            //орагнизация-добавление игр в корзину
            cart.AddItem(game1,1);
            cart.AddItem(game2,4);
            cart.AddItem(game3,2);
            cart.AddItem(game2,1);

            //действие
            cart.RemoveLine(game2);
            
            //утверждение
            Assert.AreEqual(cart.Lines.Where(g=>g.Game==game2).Count(),0);
            Assert.AreEqual(cart.Lines.Count(),2);
        }

        [TestMethod]
        public void Calculate_Cart_Total()
        {
            //организация-создание нескольких тестовых игр
            Game game1 = new Game { GameId = 1, Name = "Игра1", Price = 100 };
            Game game2 = new Game { GameId = 2, Name = "Игра2", Price = 50 };

            //организация-создание корзины
            Cart cart = new Cart();

            //действие
            cart.AddItem(game1,2);
            cart.AddItem(game2, 3);
            decimal result = cart.ComputeTotalValue();

            //утверждение
            Assert.AreEqual(result,350);
        }

        [TestMethod]
        public void Can_Clear_Contents()
        {
            //организация
            Game game1 = new Game { GameId = 1, Name = "Игра1" };
            Game game2 = new Game { GameId = 2, Name = "Игра2" };

            //организация-создание корзины
            Cart cart = new Cart();

            //действие
            cart.AddItem(game1,3);
            cart.AddItem(game2,3);
            cart.AddItem(game1,7);
            cart.Clear();

            //утверждение
            Assert.AreEqual(cart.Lines.Count(),0);
        }

        //проверям добавление в корзину
        [TestMethod]
        public void Can_Add_To_Cart()
        {
            //организация-создание имитированного хранилища
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>{
                new Game{GameId=1,Name="Игра1",Category="Кат1"}
            }.AsQueryable());

            //организация-создание корзины
            Cart cart = new Cart();

            //организация-создание контроллера
            CartController controller = new CartController(mock.Object);

            //act
            controller.AddToCart(cart,1,null);

            //assert
            Assert.AreEqual(cart.Lines.Count(),1);
            Assert.AreEqual(cart.Lines.ToList()[0].Game.GameId,1);
        }

        //после добавления игры в корзину,должно быть перенаправление на страницу корзины
        [TestMethod]
        public void Adding_Game_To_Cart_Goes_To_Cart_Screen()
        {
            //организация-создание имитированного хранилища
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>{
              new Game{GameId=1,Name="Игра1",Category="Кат1"},  
            }.AsQueryable());

            //организация создание корзины
            Cart cart = new Cart();

            //организация-создание контроллера
            CartController controller = new CartController(mock.Object);

            //действие-добавить игру в корзину
            RedirectToRouteResult result = controller.AddToCart(cart,2, "myUrl");

            //утверждение
            Assert.AreEqual(result.RouteValues["action"],"Index");
            Assert.AreEqual(result.RouteValues["returnUrl"],"myUrl");
        }

        //проверяем url
        public void Can_View_Cart_Contents()
        {
            //организация-создание корзины
            Cart cart = new Cart();

            //организация-создание контроллера
            CartController target = new CartController(null);

            //действие-вызов метода Index
            CartIndexViewModel result = (CartIndexViewModel)target.Index("myUrl", cart).ViewData.Model;

            //утверждение 
            Assert.AreSame(result.ReturnUrl,"myUrl");
            Assert.AreEqual(result.Cart,cart);
        }
    }
}