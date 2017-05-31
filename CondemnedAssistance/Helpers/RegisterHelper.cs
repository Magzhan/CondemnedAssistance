using CondemnedAssistance.Models;
using CondemnedAssistance.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace CondemnedAssistance.Helpers
{
    public class RegisterHelper {

        private UserContext _db;

        public RegisterHelper(UserContext context) {
            _db = context;
        }

        public int[] GetRegisterChildren(int[] children, int parentId) {
            if (!_db.RegisterHierarchies.Any(r => r.ParentRegister == parentId)) {
                return children;
            } else {
                List<int> allChildren = new List<int>();
                int[] tempChildren = _db.RegisterHierarchies.Where(r => r.ParentRegister == parentId).Select(r => r.ChildRegister).ToArray();
                
                foreach (int child in tempChildren) {
                    allChildren.AddRange(GetRegisterChildren(tempChildren, child));
                }
                return allChildren.Distinct().ToArray();
            }
        }

        public List<RegisterModel> GetUserRegisterModels(int userId, int userRegisterId) {
            int[] registerChildren = GetRegisterChildren(new int[] { }, userRegisterId);
            List<RegisterModel> registers = new List<RegisterModel>();

            _db.Registers.Where(r => registerChildren.Contains(r.Id)).Select(r => r).ToList().ForEach(r => {
                RegisterLevel registerLevel = _db.RegisterLevels.FirstOrDefault(row => row.Id == r.RegisterLevelId);
                registers.Add(new RegisterModel {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    RegisterLevelId = r.RegisterLevelId,
                    RegisterParentId = _db.RegisterHierarchies.FirstOrDefault(row => row.ChildRegister == r.Id).ParentRegister,
                    RegisterLevels = new List<RegisterLevelModel> { new RegisterLevelModel { Id = registerLevel.Id, Name = registerLevel.Name, Description = registerLevel.Description } }
                });
            });

            return registers;
        }
    }
}
