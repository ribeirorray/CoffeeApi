using Coffee.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Coffee.Controllers
{
    [ApiController]
    [Route("v1")]
    public class CoffeeController : ControllerBase
    {
        // Model para dados de consumo de café
        public class CoffeeConsumptionModel
        {
            public string Code { get; set; }
            public int Time { get; set; }
        }

        // Model para recomendações de café
        public class CoffeeRecommendationModel
        {
            public string Name { get; set; }
            public string Code { get; set; }
            public int Wait { get; set; }
        }
       
        [HttpGet("coffees")]
        public IActionResult GetCoffees()
        {
            var coffeeTypes = new List<CoffeeModel>
    {
        new CoffeeModel { Name = "Black Coffee", Code = "blk" },
        new CoffeeModel { Name = "Espresso", Code = "esp" },
        new CoffeeModel { Name = "Cappuccino", Code = "cap" },
        new CoffeeModel { Name = "Latte", Code = "lat" },
        new CoffeeModel { Name = "Flat White", Code = "wht" },
        new CoffeeModel { Name = "Cold Brew", Code = "cld" },
        new CoffeeModel { Name = "Decaf Coffee", Code = "dec" }
    };

            var response = new
            {
                Coffees = coffeeTypes.Select(coffee => new
                {
                    name = coffee.Name,
                    code = coffee.Code
                })
            };

            return Ok(response);
        }

        // Endpoint para calcular recomendações
        [HttpPost("calculate")]
        public IActionResult Calculate([FromBody] List<CoffeeConsumptionModel> consumptionData)
        {
            if (consumptionData == null || !consumptionData.Any())
            {
                return BadRequest("Dados de consumo de café não foram fornecidos.");
            }

            // Lógica para calcular recomendações com base nos dados de consumo
            var recommendations = new List<CoffeeRecommendationModel>();

            foreach (var consumption in consumptionData)
            {
                // Lógica para calcular o tempo de espera com base no consumo
                int waitTime = CalcularTempoDeEspera(consumption.Code, consumption.Time);

                recommendations.Add(new CoffeeRecommendationModel
                {
                    Name = GetCoffeeName(consumption.Code),
                    Code = consumption.Code,
                    Wait = waitTime
                });
            }

            return Ok(new { Recommendations = recommendations });
        }

        // Lógica para calcular o tempo de espera com base no histórico de consumo
        private int CalcularTempoDeEspera(string code, int time)
        {
            if (code == "blk")
            {
                return Math.Max(60 - time, 0); // Espere no mínimo 60 minutos para consumir Black Coffee
            }
            else if (code == "esp")
            {
                return Math.Max(60 - time, 0); // Espere no mínimo 60 minutos para consumir Espresso
            }
            else if (code == "cap")
            {
                return Math.Max(45 - time, 0); // Espere no mínimo 45 minutos para consumir Cappuccino
            }
            else if (code == "lat")
            {
                return Math.Max(45 - time, 0); // Espere no mínimo 45 minutos para consumir Latte
            }
            else if (code == "wht")
            {
                return Math.Max(45 - time, 0); // Espere no mínimo 45 minutos para consumir Flat White
            }
            else if (code == "cld")
            {
                return Math.Max(90 - time, 0); // Espere no mínimo 90 minutos para consumir Cold Brew
            }
            else if (code == "dec")
            {
                return Math.Max(15 - time, 0); // Espere no mínimo 120 minutos para consumir Decaf Coffee
            }

            // Se o código não corresponder a nenhum tipo de café conhecido, retorne 0
            return 0;
        }


        // Obtém o nome do café com base no código
        private string GetCoffeeName(string code)
        {
            if (code == "blk")
            {
                return "Black Coffee";
            }
            else if (code == "esp")
            {
                return "Espresso";
            }
            else if (code == "cap")
            {
                return "Cappuccino";
            }
            else if (code == "lat")
            {
                return "Latte";
            }
            else if (code == "wht")
            {
                return "Flat White";
            }
            else if (code == "cld")
            {
                return "Cold Brew";
            }
            else if (code == "dec")
            {
                return "Decaf Coffee";
            }

            // Se o código não corresponder a nenhum tipo de café conhecido, retorne o próprio código
            return code;
        }

    }
}
