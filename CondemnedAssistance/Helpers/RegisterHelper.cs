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

                allChildren.AddRange(tempChildren);
                allChildren.Add(parentId);
                allChildren.AddRange(children);

                foreach (int child in tempChildren) {
                    allChildren.AddRange(GetRegisterChildren(tempChildren, child));
                }
                return allChildren.Distinct().ToArray();
            }
        }

        public int[] GetRegisterLevelChildren(int[] children, int parentId) {
            if(!_db.RegisterLevelHierarchies.Any(r => r.ParentLevel == parentId)) {
                return children;
            }
            else {
                List<int> allChildren = new List<int>();
                int[] tempChildren = _db.RegisterLevelHierarchies.Where(r => r.ParentLevel == parentId).Select(r => r.ChildLevel).ToArray();

                allChildren.AddRange(tempChildren);
                allChildren.Add(parentId);
                allChildren.AddRange(children);

                foreach(int child in tempChildren) {
                    allChildren.AddRange(GetRegisterLevelChildren(tempChildren, child));
                }

                return allChildren.Distinct().ToArray();
            }
        }

        public int[] GetRegisterParents(int[] parents, int childId) {
            if (!_db.RegisterHierarchies.Any(r => r.ChildRegister == childId)) {
                return parents;
            } else {
                List<int> allParents = new List<int>();
                int tempParent = _db.RegisterHierarchies.Single(r => r.ChildRegister == childId).ParentRegister;
                allParents.AddRange(parents);
                allParents.Add(tempParent);
                allParents.Add(childId);
                allParents.AddRange(GetRegisterParents(allParents.ToArray(), tempParent));

                return allParents.Distinct().ToArray();
            }
        }

        public int[] GetRegisterLevelParents(int[] parents, int childId) {
            if(_db.RegisterLevelHierarchies.Any(r => r.ChildLevel == childId)) {
                return parents;
            }
            else {
                List<int> allParents = new List<int>();
                int tempParent = _db.RegisterLevelHierarchies.Single(r => r.ChildLevel == childId).ParentLevel;
                allParents.AddRange(parents);
                allParents.Add(tempParent);
                allParents.Add(childId);
                allParents.AddRange(GetRegisterParents(allParents.ToArray(), tempParent));

                return allParents.Distinct().ToArray();
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
                    RegisterParentId = (_db.RegisterHierarchies.FirstOrDefault(row => row.ChildRegister == r.Id) == null) ? 0 : _db.RegisterHierarchies.FirstOrDefault(row => row.ChildRegister == r.Id).ParentRegister,
                    RegisterLevels = new List<RegisterLevelModel> {
                        new RegisterLevelModel {
                            Id = registerLevel.Id,
                            Name = registerLevel.Name,
                            Description = registerLevel.Description,
                            IsFirstAncestor = registerLevel.IsFirstAncestor,
                            IsLastChild = registerLevel.IsLastChild
                        } },
                    RegisterLevelHierarchies = _db.RegisterLevelHierarchies.ToList()
                });
            });

            return registers;
        }
    }
}
