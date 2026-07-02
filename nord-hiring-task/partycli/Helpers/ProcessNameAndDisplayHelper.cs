using System;

namespace partycli.Helpers
{
    public static class ProcessNameAndDisplayHelper
    {
        public static string ProccessName(string name)
        {
            name = name.Replace("-", string.Empty);
            return name;
        }

        public static void ToolTipDisplay()
        {
            Console.WriteLine("To get and save all servers, use command: partycli.exe server_list");
            Console.WriteLine("To get and save country servers, use command: partycli.exe server_list --country  <country name here>");
            Console.WriteLine("To get and save servers that support any network protocol, use command: partycli.exe server_list --protocol <protocol name here>");
            Console.WriteLine("To see saved list of servers, use command: partycli.exe server_list --local");
        }
    }
}
