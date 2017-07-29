using CondemnedAssistance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.ViewModels {
    public class AddressModel {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int AddressLevelId { get; set; }
        public List<AddressLevelModel> AddressLevels { get; set; }
        public int AddressParentId { get; set; }
        public Address AddressParent { get; set; }

        public AddressModel() {
            AddressLevels = new List<AddressLevelModel>();
        }
    }

    public class AddressLevelModel {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
