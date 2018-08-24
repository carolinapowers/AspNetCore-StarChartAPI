using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using StarChart.Data;
using Xunit;

namespace StarChartTests
{
    public class CreateGetActionsTests
    {
        [Fact(DisplayName = "Create GetById Action @create-getbyid-action")]
        public void CreateGetByIdActionTest()
        {
            var filePath = ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + "StarChart" + Path.DirectorySeparatorChar + "Controllers" + Path.DirectorySeparatorChar + "CelestialObjectController.cs";
            Assert.True(File.Exists(filePath), "`CelestialObjectController.cs` was not found in the `Controllers` directory.");

            var controller = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                              from type in assembly.GetTypes()
                              where type.FullName == "StarChart.Controllers.CelestialObjectController"
                              select type).FirstOrDefault();
            Assert.True(controller != null, "A `public` class `CelestialObjectController` was not found in the `StarChart.Controllers` namespace.");

            var model = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                              from type in assembly.GetTypes()
                              where type.FullName == "StarChart.Models.CelestialObject"
                              select type).FirstOrDefault();

            var item = Activator.CreateInstance(model);
            model.GetProperty("Id").SetValue(item, 1);
            model.GetProperty("Name").SetValue(item, "Sun");
            var item2 = Activator.CreateInstance(model);
            model.GetProperty("Id").SetValue(item2, 2);
            model.GetProperty("Name").SetValue(item2, "Earth");
            model.GetProperty("OrbitedObject").SetValue(item2, item);

            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseInMemoryDatabase("Test");
            var context = new ApplicationDbContext(optionsBuilder.Options);

            var celestialController = Activator.CreateInstance(controller, new object[] { context });

            context.Add(item);
            context.Add(item2);
            context.SaveChanges();

            var method = controller.GetMethod("GetById", new Type[] { typeof(int) });
            Assert.True(method != null, "`CelestialObjectController` does not contain a `GetById` action that accepts an `int` parameter.");
            var notFoundResults = method.Invoke(celestialController, new object[] { 3 }) as NotFoundResult;
            Assert.True(notFoundResults != null, "`CelestialObjectController`'s `GetById` action did not return the `NotFound` when no `CelestialObject` with a matching `Id` was found.");
            var okResults = method.Invoke(celestialController, new object[] { 1 }) as OkObjectResult;
            Assert.True(okResults != null && okResults.Value != null, "`CelestialObjectController`'s `GetById` action did not return an `Ok` with the `CelestialObject` that has a matching `Id` when one was found.");
            Assert.True((int)model.GetProperty("Id")?.GetValue(okResults.Value) == 1, "`CelestialObjectController`'s `GetById` action returned an `Ok` with a `CelestialObject`, however; the `Id` does not appear to match the one provided by the parameter.");
            Assert.True(model.GetProperty("Satellites")?.GetValue(okResults.Value) != null, "`CelestialObjectController`'s `GetById` action returned an `Ok` with a `CelestialObject`, however; the `Satellites` property was not set.");
        }

        [Fact(DisplayName = "Create GetByName Action @create-getbyname-action")]
        public void CreateGetByNameActionTest()
        {
            var filePath = ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + "StarChart" + Path.DirectorySeparatorChar + "Controllers" + Path.DirectorySeparatorChar + "CelestialObjectController.cs";
            Assert.True(File.Exists(filePath), "`CelestialObjectController.cs` was not found in the `Controllers` directory.");

            var controller = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                              from type in assembly.GetTypes()
                              where type.FullName == "StarChart.Controllers.CelestialObjectController"
                              select type).FirstOrDefault();
            Assert.True(controller != null, "A `public` class `CelestialObjectController` was not found in the `StarChart.Controllers` namespace.");

            var model = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                         from type in assembly.GetTypes()
                         where type.FullName == "StarChart.Models.CelestialObject"
                         select type).FirstOrDefault();

            var item = Activator.CreateInstance(model);
            model.GetProperty("Id").SetValue(item, 1);
            model.GetProperty("Name").SetValue(item, "Sun");
            var item2 = Activator.CreateInstance(model);
            model.GetProperty("Id").SetValue(item2, 2);
            model.GetProperty("Name").SetValue(item2, "Earth");
            model.GetProperty("OrbitedObject").SetValue(item2, item);

            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseInMemoryDatabase("Test2");
            var context = new ApplicationDbContext(optionsBuilder.Options);

            var celestialController = Activator.CreateInstance(controller, new object[] { context });

            context.Add(item);
            context.Add(item2);
            context.SaveChanges();

            var method = controller.GetMethod("GetByName", new Type[] { typeof(string) });
            Assert.True(method != null, "`CelestialObjectController` does not contain a `GetByName` action that accepts a `string` parameter.");
            var notFoundResults = method.Invoke(celestialController, new object[] { "Bob" }) as NotFoundResult;
            Assert.True(notFoundResults != null, "`CelestialObjectController`'s `GetByName` action did not return the `NotFound` when no `CelestialObject` with a matching `Name` was found.");
            var okResults = method.Invoke(celestialController, new object[] { "Sun" }) as OkObjectResult;
            Assert.True(okResults != null && okResults.Value != null, "`CelestialObjectController`'s `GetByName` action did not return an `Ok` with the `CelestialObject` that has a matching `Name` when one was found.");
            Assert.True((okResults.Value.GetType().ToString().Contains("System.Collections.Generic.List") || okResults.Value.GetType().ToString().Contains("Microsoft.EntityFrameworkCore.Query.Internal.EntityQueryable"))  && okResults.Value.GetType().ToString().Contains("StarChart.Models.CelestialObject"), "`CelestialObjectController`'s `GetByName` action returned an `Ok` with a `CelestialObject`, however; the `Name` does not appear to match the one provided by the parameter.");
        }
    }
}
