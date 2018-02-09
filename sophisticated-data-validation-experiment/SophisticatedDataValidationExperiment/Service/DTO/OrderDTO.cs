using System.Collections.Generic;

namespace SophisticatedDataValidationExperiment.Service.DTO
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public CustomerDTO Customer { get; set; }
        public List<ItemDTO> Items { get; set; }
    }
}