using System;
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
    public class AdminTests
    {
        [TestMethod]
        public void Index_Contains_All_Games()
        {
            //arrange a mock
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
            {
                new Game {GameId=1,Name="Игра1"},
                new Game {GameId=2,Name="Игра2"},
                new Game {GameId=3,Name="Игра3"},
                new Game {GameId=4,Name="Игра4"},
                new Game {GameId=5,Name="Игра5"}
            });

            //arrange controller
            AdminController controller = new AdminController(mock.Object);

            //act
            List<Game> result = ((IEnumerable<Game>)controller.Index().ViewData.Model).ToList();

            //assert
            Assert.AreEqual(result.Count(),5);
            Assert.AreEqual(result[0].Name,"Игра1");
            Assert.AreEqual(result[1].Name, "Игра2");
            Assert.AreEqual(result[2].Name, "Игра3");
        }

        [TestMethod]
        public void Can_Edit_Game()
        {
            //организация-создание имитированного хранилища
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
            {
                new Game{GameId=1,Name="Игра1"},
                new Game{GameId=2,Name="Игра2"},
                new Game{GameId=3,Name="Игра3"},
                new Game{GameId=4,Name="Игра4"},
                new Game{GameId=5,Name="Игра5"}
            });

            //организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);
            
            //действие
            Game game1 = controller.Edit(1).ViewData.Model as Game;
            Game game2 = controller.Edit(2).ViewData.Model as Game;
            Game game3 = controller.Edit(3).ViewData.Model as Game;

            // утверждение
            Assert.AreEqual(1, game1.GameId);
            Assert.AreEqual(2, game2.GameId);
            Assert.AreEqual(3, game3.GameId);
        }

        [TestMethod]
        public void Cannot_Edit_Nonexistent_Game()
        {
            // организация - создание имитированного хранилища
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
            {
                new Game {GameId=1,Name="Игра1"},
                new Game {GameId=2,Name="Игра2"},
                new Game {GameId=3,Name="Игра3"},
                new Game {GameId=4,Name="Игра4"},
                new Game {GameId=5,Name="Игра5"}
            });

            // организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            //действие
            Game result = controller.Edit(6).ViewData.Model as Game;

            // assert
        }

        [TestMethod]
        public void Can_Save_Valid_Changes()
        {
            //организация - создание имитированного хранилища
            Mock<IGameRepository> mock = new Mock<IGameRepository>();

            //организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            //Организация - создание объекта Game
            Game game = new Game { Name = "Test" };

            //действие попытка сохранения товара
            ActionResult result = controller.Edit(game);

            // утверждение - проверка того, что к хранилищу производится обращение
            mock.Verify(m=>m.SaveGame(game));

            // утверждение - проверка типа результата метода
            Assert.IsInstanceOfType(result,typeof(RedirectToRouteResult));
        }

        [TestMethod]
        public void Cannot_Save_Invalid_Changes()
        {
            //организация - создание имитированного хранилища
            Mock<IGameRepository> mock = new Mock<IGameRepository>();

            //организация - созданик контроллера
            AdminController controller = new AdminController(mock.Object);

            // организация - создание объекта Game
            Game game = new Game { Name = "Test" };

            // организация - добавление ошибки в состояние модели
            controller.ModelState.AddModelError("error", "error");

            // действие - попытка сохранения товара
            ActionResult result = controller.Edit(game);

            // утверждение - проверка того, что к хранилищу обращение не производится
            mock.Verify(m=>m.SaveGame(It.IsAny<Game>()),Times.Never());

            // утверждение - проверка типа результата метода
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Can_Delete_Valid_Games()
        {
            //организация - создание объекта game
            Game game = new Game { GameId = 2, Name = "Игра2" };

            // организация - создание имитированного хранилища
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
            {
                new Game{GameId=1,Name="Игра1"},
                new Game{GameId=2,Name="Игра2"},
                new Game{GameId=3,Name="Игра3"},
                new Game{GameId=4,Name="Игра4"},
                new Game{GameId=5,Name="Игра5"}
            });

            // организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // действие - удаление игры
            controller.Delete(game.GameId);

            // утверждение - проверка того, что метод удаления в хранилище
            // вызывается для корректного объекта game
            mock.Verify(m => m.DeleteGame(game.GameId));
        }
    }
}
