using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartridgeManagementSystem.Models
{
    public class Cartridge
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }
        public string InstallationDate { get; set; }
        public string Status { get; set; }
        public string Comment { get; set; }
    }
}
