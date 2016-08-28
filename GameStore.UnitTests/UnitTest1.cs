using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using GameStore.WebUI.Controllers;
using Moq;
using GameStore.WebUI.HtmlHelpers;
using GameStore.WebUI.Models;
using System.Web.Mvc;

namespace GameStore.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            //организация-arrange
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
            {
                new Game{GameId=1,Name="Игра1"},
                new Game{GameId=2,Name="Игра2"},
                new Game{GameId=3,Name="Игра3"},
                new Game{GameId=4,Name="Игра4"},
                new Game{GameId=5,Name="Игра5"}
            });
            GameController controller = new GameController(mock.Object);
            controller.pageSize = 3;


            //Действие(act)
            GamesListViewModel result = (GamesListViewModel)controller.List(2).Model;

            //утверждение(assert)
            List<Game> games = result.Games.ToList();
            Assert.IsTrue(games.Count==2);
            Assert.AreEqual(games[0].Name, "Игра4");
            Assert.AreEqual(games[1].Name, "Игра5");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            //организация
            HtmlHelper myHelper = null;
            
            //организация создание объекта pagingInfo
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage=3,
                TotalItems=28,
                ItemsPerPage=10
            };

            //организация-настройка делегата с помощью лямбда-выражения
            Func<int, string> pageUrlDelegate = i => "Page" + i;

            //действие
            MvcHtmlString result = myHelper.PageLinks(pagingInfo,pageUrlDelegate);

            //утверждение
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
                +@"<a class=""btn btn-default"" href=""Page2"">2</a>"
                + @"<a class=""btn btn-default btn-primary selected"" href=""Page3"">3</a>",
                result.ToString());
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            //организация
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
            {
                new Game{GameId=1,Name="Игра1"},
                new Game{GameId=2,Name="Игра2"},
                new Game{GameId=3,Name="Игра3"},
                new Game{GameId=4,Name="Игра4"},
                new Game{GameId=5,Name="Игра5"}
            });
            GameController controller = new GameController(mock.Object);
            controller.pageSize = 3;

            //act
            GamesListViewModel result = (GamesListViewModel)controller.List(2).Model;

            //Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems,5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }
    }
}
