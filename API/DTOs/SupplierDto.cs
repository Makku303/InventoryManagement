using Core.Models;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class SupplierDto
    {
        public string Name { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string Address { get; set; }
        public string Notes { get; set; }
    }
}
