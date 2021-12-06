using System;
using System.Collections.Generic;
using System.Text;

namespace DependencyInjection
{
    class InputController
    {
        readonly ILogger logger;

        public InputController(ILogger logger)
        {
            this.logger = logger;
        }

        public string Input()
        {
            Console.WriteLine(">");
            var input = Console.ReadLine();
            logger.Log($"Ввод {input}");
            return input;
        }
    }
}
