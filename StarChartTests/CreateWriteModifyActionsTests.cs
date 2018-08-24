using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarChart.Data;
using Xunit;

namespace StarChartTests
{
    public class CreateWriteModifyActionsTests
    {
        [Fact(DisplayName = "Create Create Action @create-create-action")]
        public void CreateCreateActionTest()
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

            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseInMemoryDatabase("TestCreate");
            var context = new ApplicationDbContext(optionsBuilder.Options);

            var celestialController = Activator.CreateInstance(controller, new object[] { context });

            var method = controller.GetMethod("Create");
            Assert.True(method != null, "`CelestialObjectController` does not contain a `Create` action that accepts a `CelestialObject` parameter.");
            var postAttribute = method.GetCustomAttributes(typeof(HttpPostAttribute), false).FirstOrDefault();
            Assert.True(postAttribute != null, "`CelestialObjectController`'s `Create` action was found, but does not have an `HttpPost` attribute.");
            var okResults = method.Invoke(celestialController, new object[] { item }) as CreatedAtRouteResult;
            Assert.True(okResults != null, "`CelestialObjectController`'s `Create` action did not return a `CreatedAtRoute` with the new `CelestialObject`'s `Id` and the new `CelestialObject`.");
            var results = context.Find(model, 1);
            Assert.True(model.GetProperty("Name").GetValue(results) == model.GetProperty("Name").GetValue(item), "`CelestialObjectController`'s `Create` action did not add the provided `CelestialObject` to `_context.CelestialObjects` (Don't forget to call `SaveChanges` after adding it!).");
        }
    }
}
