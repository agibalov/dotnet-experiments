using System.Collections.Generic;
using SophisticatedDataValidationExperiment.Service.DTO;

namespace SophisticatedDataValidationExperiment.Service
{
    public class MegaService
    {
        public OrderDTO GetOrder()
        {
            return new OrderDTO
                {
                    Id = 0,
                    Customer = new CustomerDTO
                        {
                            Id = 0,
                            Person = new PersonDTO
                                {
                                    Id = 0,
                                    Name = null
                                }
                        },
                    Items = new List<ItemDTO>
                        {
                            new ItemDTO { ItemId = 0, ItemName = "beer" },
                            new ItemDTO { ItemId = 1, ItemName = null },
                            new ItemDTO { ItemId = 333, ItemName = "vodka" },
                            new ItemDTO { ItemId = 0, ItemName = null }
                        }
                };
        }
    }
}