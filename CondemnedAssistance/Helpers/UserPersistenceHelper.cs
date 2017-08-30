using CondemnedAssistance.Models;
using CondemnedAssistance.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace CondemnedAssistance.Helpers {
    public class UserPersistenceHelper {

        private ClaimsPrincipal _identity;

        private UserContext _db;
        private PersistenceState _state;
        private PersistenceHelperMode _mode;
        private UserModelCreate _model;

        private User _user;
        private UserStaticInfo _userStaticInfo;
        private UserRole _userRole;
        private UserRegister _userRegister;
        private List<UserAddress> _userAddresses;
        private List<UserProfession> _userProfessions;
        private List<UserEvent> _userEvents;

        private IDbContextTransaction _transaction;
        private Transaction currentSession;
        private int currentUser;
        private DatabaseActionTypes currentAction;

        private UserHistory _userHist;
        private UserStaticInfoHistory _userStaticInfoHist;
        private UserRoleHistory _userRoleHist;
        private UserRegisterHistory _userRegisterHist;

        public UserPersistenceHelper(UserContext context, Transaction transaction, DatabaseActionTypes action, int userId) {
            _db = context;
            currentSession = transaction;
            currentAction = action;
            loadPersistenceState(userId);
            loadUserMode();
        }

        public UserPersistenceHelper(ClaimsPrincipal User, PersistenceHelperMode mode, UserContext context, PersistenceState state, UserModelCreate model = null) {
            _identity = User;
            _db = context;
            _state = state;
            _mode = mode;
            _model = model;

            currentUser = Convert.ToInt32(_identity.Identity.Name);

            switch (_mode) {
                case PersistenceHelperMode.Write:
                    currentAction = (_state == PersistenceState.Create) ? DatabaseActionTypes.Insert : DatabaseActionTypes.Update;
                    _db.Database.AutoTransactionsEnabled = false;
                    _transaction = _db.Database.BeginTransaction();

                    currentSession = new Transaction { TransactionGuid = _transaction.TransactionId };

                    _db.Transactions.Add(currentSession);
                    _db.SaveChanges();
                    break;
            }

            switch (state) {
                case PersistenceState.Create:
                    _user = new User();
                    _userStaticInfo = new UserStaticInfo();
                    _userRole = new UserRole();
                    _userRegister = new UserRegister();
                    _userAddresses = new List<UserAddress>();
                    _userProfessions = new List<UserProfession>();
                    _userEvents = new List<UserEvent>();
                    break;
                case PersistenceState.Update:
                    loadPersistenceState(model.UserId);
                    break;
            }

            switch (_mode) {
                case PersistenceHelperMode.Read:
                    break;
                case PersistenceHelperMode.Write:
                    loadUserMode();
                    break;
            }
        }

        private void loadPersistenceState(int userId) {
            _user = _db.Users.First(u => u.Id == userId);
            _userStaticInfo = _db.UserStaticInfo.First(u => u.UserId == userId);
            _userRole = _db.UserRoles.First(u => u.UserId == userId);
            _userRegister = _db.UserRegisters.First(u => u.UserId == userId);
            _userAddresses = _db.UserAddresses.Where(u => u.UserId == userId).ToList();
            _userProfessions = _db.UserProfessions.Where(u => u.UserId == userId).ToList();
            _userEvents = _db.UserEvents.Where(u => u.UserId == userId).ToList();
        }

        private void loadUserMode() {
            _userHist = new UserHistory { TransactionId = currentSession.TransactionId, ActionType = currentAction };
            _userStaticInfoHist = new UserStaticInfoHistory { TransactionId = currentSession.TransactionId, ActionType = currentAction };
            _userRoleHist = new UserRoleHistory { TransactionId = currentSession.TransactionId, ActionType = currentAction };
            _userRegisterHist = new UserRegisterHistory { TransactionId = currentSession.TransactionId, ActionType = currentAction };
        }

        public void LoadModel() {
            loadForUserStaticInfo();
            loadForUserRegisters();
            loadForUserRoles();
            loadForUserAddresses();
            loadForUserProfessions();
        }

        private void loadUser() {
            switch (_state) {
                case PersistenceState.Create:
                    _user.Login = _model.Login;
                    _user.Email = _model.Email;
                    _user.EmailConfirmed = false;
                    _user.NormalizedEmail = _model.Email.ToUpper();
                    _user.PhoneNumber = _model.PhoneNumber;
                    _user.PhoneNumberConfirmed = false;
                    _user.AccessFailedCount = 0;
                    _user.LockoutEnabled = false;
                    _user.PasswordHash = "123456";
                    break;
                case PersistenceState.Update:
                    if(_user.Email != _model.Email) {
                        _user.Email = _model.Email;
                        _user.NormalizedEmail = _model.Email.ToUpper();
                        _user.EmailConfirmed = false;
                    }
                    if(_user.PhoneNumber != _model.PhoneNumber) {
                        _user.PhoneNumber = _model.PhoneNumber;
                        _user.PhoneNumberConfirmed = false;
                    }
                    break;
            }

            _user.RequestDate = DateTime.Now;
            _user.RequestUser = currentUser;

            switch (_state) {
                case PersistenceState.Create:
                    _db.Users.Add(_user);
                    _db.SaveChanges();
                    break;
                case PersistenceState.Update:
                    _db.Users.Attach(_user);
                    _db.Entry(_user).State = EntityState.Modified;
                    break;
            }
        }

        private void loadUserStaticData() {
            switch (_state) {
                case PersistenceState.Create:
                    _userStaticInfo.UserId = _user.Id;
                    break;
                case PersistenceState.Update:
                    break;
            }
            _userStaticInfo.LastName = _model.LastName;
            _userStaticInfo.FirstName = _model.FirstName;
            _userStaticInfo.MiddleName = _model.MiddleName;
            _userStaticInfo.Xin = _model.Xin;
            _userStaticInfo.Birthdate = _model.Birthdate;
            _userStaticInfo.Gender = _model.Gender;
            _userStaticInfo.UserStatusId = _model.UserStatusId;
            _userStaticInfo.UserTypeId = _model.UserTypeId;
            _userStaticInfo.MainAddress = _model.MainAddress;
            _userStaticInfo.RequestUser = Convert.ToInt32(_identity.Identity.Name);
            _userStaticInfo.RequestDate = DateTime.Now;

            switch (_state) {
                case PersistenceState.Create:
                    _db.UserStaticInfo.Add(_userStaticInfo);
                    break;
                case PersistenceState.Update:
                    _db.UserStaticInfo.Attach(_userStaticInfo);
                    _db.Entry(_userStaticInfo).State = EntityState.Modified;
                    break;
            }
        }

        private void loadUserRole() {
            switch (_state) {
                case PersistenceState.Create:
                    _userRole.UserId = _user.Id;
                    break;
            }

            _userRole.RoleId = _model.RoleId;

            switch (_state) {
                case PersistenceState.Create:
                    _db.UserRoles.Add(_userRole);
                    break;
                case PersistenceState.Update:
                    _db.UserRoles.Attach(_userRole);
                    _db.Entry(_userRole).State = EntityState.Modified;
                    break;
            }
        }

        private void loadUserRegister() {
            switch (_state) {
                case PersistenceState.Create:
                    _userRegister.UserId = _user.Id;
                    break;
            }
            _userRegister.RegisterId = _model.UserRegisterId;

            switch (_state) {
                case PersistenceState.Create:
                    _db.UserRegisters.Add(_userRegister);
                    break;
                case PersistenceState.Update:
                    _db.UserRegisters.Attach(_userRegister);
                    _db.Entry(_userRegister).State = EntityState.Modified;
                    break;
            }
        }

        private void loadUserAddress() {
            switch (_state) {
                case PersistenceState.Create:
                    _userAddresses = new UserAddress[] {
                        new UserAddress { UserId = _user.Id, AddressId = _model.AddressLevelOneId},
                        new UserAddress { UserId = _user.Id, AddressId = _model.AddressLevelTwoId},
                        new UserAddress { UserId = _user.Id, AddressId = _model.AddressLevelThreeId}
                    }.ToList();

                    _db.UserAddresses.AddRange(_userAddresses);
                    break;
                case PersistenceState.Update:
                    foreach (UserAddress address in _userAddresses) {
                        switch (_db.Addresses.First(a => a.Id == address.AddressId).AddressLevelId) {
                            case 1:
                                address.AddressId = _model.AddressLevelOneId;
                                break;
                            case 2:
                                address.AddressId = _model.AddressLevelTwoId;
                                break;
                            case 3:
                                address.AddressId = _model.AddressLevelThreeId;
                                break;
                        }
                        _db.UserAddresses.Attach(address);
                        _db.Entry(address).State = EntityState.Modified;
                    }
                    break;
            }
        }

        private void loadUserEvents() {
            List<UserEventHistory> hist = new List<UserEventHistory>();
            _userEvents.ForEach(e => {
                hist.Add(new UserEventHistory {
                    EventId = e.Id,
                    ActionType = currentAction,
                    RequestDate = DateTime.Now,
                    RequestUser = currentUser,
                    TransactionId = currentSession.TransactionId,
                    UserId = _model.UserId
                });
            });

            if(hist.Count > 0)
                _db.UserEventHistory.AddRange(hist);
        }

        private void loadUserProfessions() {
            switch (_model.RoleId) {
                case 2:
                case 3:
                    return;
            }
            switch (_state) {
                case PersistenceState.Create:
                    loadUserProfessionsUpdateSort(new int[] { }, _model.ProfessionIds, EntityState.Added);
                    break;
                case PersistenceState.Update:
                    loadUserProfessionsUpdate();
                    break;
            }
        }

        private void loadUserProfessionsUpdate() {
            List<int> currentProfessionIds = _userProfessions.Select(p => p.ProfessionId).ToList();
            List<int> postedProfessionIds = _model.ProfessionIds.ToList();
            currentProfessionIds.Sort();
            postedProfessionIds.Sort();
            if (currentProfessionIds.SequenceEqual(postedProfessionIds)) return;

            int[] notIncluded = postedProfessionIds.Intersect(currentProfessionIds).ToArray();
            int[] newInPosted = postedProfessionIds.Except(notIncluded).ToArray();
            int[] notExistsInPosted = currentProfessionIds.Except(notIncluded).ToArray();

            if (newInPosted.Length == notExistsInPosted.Length) {
                loadUserProfessionsUpdateSort(newInPosted, notExistsInPosted, EntityState.Added);
            }
            else {
                if (newInPosted.Length < notExistsInPosted.Length)
                    loadUserProfessionsUpdateSort(newInPosted, notExistsInPosted, EntityState.Deleted);
                else
                    loadUserProfessionsUpdateSort(notExistsInPosted, newInPosted, EntityState.Added);
            }
        }
        
        private void loadUserProfessionsUpdateSort(int[] first, int[] second, EntityState entityState) {
            Queue<int> firstQueue = new Queue<int>();
            Queue<int> secondQueue = new Queue<int>();

            foreach(int f in first) {
                firstQueue.Enqueue(f);
            }
            foreach(int s in second) {
                secondQueue.Enqueue(s);
            }
            while(firstQueue.Count != 0 && secondQueue.Count != 0) {
                int professionId = (EntityState.Deleted == entityState) ? secondQueue.Dequeue() : firstQueue.Dequeue();
                UserProfession profession = _db.UserProfessions.First(p => p.ProfessionId == professionId && p.UserId == _user.Id);
                profession.ProfessionId = (EntityState.Deleted == entityState) ? firstQueue.Dequeue() : secondQueue.Dequeue();
                _db.UserProfessions.Attach(profession);
                _db.Entry(profession).State = EntityState.Modified;

                _db.UserProfessionHistory.Add(new UserProfessionHistory { ProfessionId = profession.ProfessionId,
                    RequestDate = DateTime.Now,
                    RequestUser = currentUser,
                    TransactionId = currentSession.TransactionId,
                    ActionType = DatabaseActionTypes.Update,
                    UserId = profession.UserId
                });
            }
            while (secondQueue.Count > 0) {
                int professionId = secondQueue.Dequeue();
                switch (entityState) {
                    case EntityState.Added:
                        _db.UserProfessions.Add(new UserProfession { UserId = _user.Id, ProfessionId = professionId });
                        _db.UserProfessionHistory.Add(new UserProfessionHistory {
                            UserId = _user.Id,
                            ProfessionId = professionId,
                            RequestDate = DateTime.Now,
                            RequestUser = currentUser,
                            ActionType = DatabaseActionTypes.Insert,
                            TransactionId = currentSession.TransactionId
                        });
                        break;
                    case EntityState.Deleted:
                        UserProfession profession = _db.UserProfessions.First(p => p.ProfessionId == professionId && p.UserId == _user.Id);
                        _db.UserProfessions.Remove(profession);
                        _db.UserProfessionHistory.Add(new UserProfessionHistory {
                            UserId = profession.UserId,
                            ProfessionId = profession.ProfessionId,
                            RequestDate = DateTime.Now,
                            RequestUser = currentUser,
                            ActionType = DatabaseActionTypes.Delete,
                            TransactionId = currentSession.TransactionId
                        });
                        break;
                }
            }
        }

        private void loadForUserStaticInfo() {
            switch (_state) {
                case PersistenceState.Update:
                    _model.UserId = _user.Id;
                    _model.Login = _user.Login;
                    _model.FirstName = _userStaticInfo.FirstName;
                    _model.LastName = _userStaticInfo.LastName;
                    _model.MiddleName = _userStaticInfo.MiddleName;
                    _model.PhoneNumber = _user.PhoneNumber;
                    _model.Email = _user.Email;
                    _model.Xin = _userStaticInfo.Xin;
                    _model.Birthdate = _userStaticInfo.Birthdate;
                    _model.Gender = _userStaticInfo.Gender;
                    _model.MainAddress = _userStaticInfo.MainAddress;
                    break;
            }
            _model.UserStatuses = _db.UserStatuses.ToList();
            _model.UserTypes = _db.UserTypes.ToList();
        }

        private void loadForUserRegisters() {
            RegisterHelper registerHelper = new RegisterHelper(_db);
            int operatorRegisterId = Convert.ToInt32(_identity.FindFirst(c => c.Type == "RegisterId").Value);
            int[] registerChildren = registerHelper.GetRegisterChildren(new int[] { }, operatorRegisterId);
            switch (_state) {
                case PersistenceState.Update:
                    _model.UserRegisterId = _db.UserRegisters.First(r => r.UserId == _user.Id).RegisterId;
                    break;
            }
            if (_identity.IsInRole("3")) {
                _model.UserRegisters = _db.Registers.ToList();
                return;
            }
            _model.UserRegisters = _db.Registers.Where(r => registerChildren.Contains(r.Id));
        }

        private void loadForUserRoles() {
            switch (_state) {
                case PersistenceState.Update:
                    _model.RoleId = _db.UserRoles.First(r => r.UserId == _user.Id).RoleId;
                    break;
            }
            if (_identity.IsInRole("3")) {
                _model.Roles = _db.Roles.ToList();
                return;
            }
            _model.Roles = _db.Roles.Where(r => r.Id != 3).ToList();
        }

        private void loadForUserAddresses() {
            switch (_state) {
                case PersistenceState.Update:
                    int[] addressIds = _db.UserAddresses.Where(a => a.UserId == _user.Id).Select(a => a.AddressId).ToArray();
                    foreach(int address in addressIds){
                        switch(_db.Addresses.First(a => a.Id == address).AddressLevelId) {
                            case 1:
                                _model.AddressLevelOneId = address;
                                break;
                            case 2:
                                _model.AddressLevelTwoId = address;
                                break;
                            case 3:
                                _model.AddressLevelThreeId = address;
                                break;
                        }
                    }
                    break;
            }

            _model.Addresses = _db.Addresses.ToList();
        }

        private void loadForUserProfessions() {
            switch (_userRole.RoleId) {
                case 2:
                case 3:
                    return;
            }
            switch (_state) {
                case PersistenceState.Update:
                    _model.ProfessionIds = _db.UserProfessions.Where(p => p.UserId == _user.Id).Select(p => p.ProfessionId).ToArray();
                    break;
            }

            _model.Professions = _db.Professions.ToList();
        }

        public bool Validate(out string ErrorMessage) {
            if (_state == PersistenceState.Create) {
                if(_db.Users.Any(u => u.Login == _model.Login)) {
                    ErrorMessage = "Already has such user";
                    return false;
                }
            }
            else {
                ErrorMessage = "Success";
                return true;
            }
            ErrorMessage = "Success";
            return true;
        }

        public bool Persist(out string message) {
            try {
                loadUser();
                loadUserStaticData();
                loadUserRole();
                loadUserRegister();
                loadUserEvents();
                loadUserAddress();
                loadUserProfessions();
                PersistHistory();
                _db.SaveChanges();
                message = "Success.";
                _transaction.Commit();
            } catch(Exception e) {
                _transaction.Rollback();
                _transaction.Dispose();
                message = "Failed. " + e.Message;
                return false;
            }
            _transaction.Dispose();
            return true;
        }

        public UserModelCreate GetModel() {
            return _model;
        }

        public void PersistHistory() {
            _userHist.Id = _user.Id;
            _userHist.Login = _user.Login;
            _userHist.Email = _user.Email;
            _userHist.EmailConfirmed = _user.EmailConfirmed;
            _userHist.NormalizedEmail = _user.NormalizedEmail;
            _userHist.PhoneNumber = _user.PhoneNumber;
            _userHist.PhoneNumberConfirmed = _user.PhoneNumberConfirmed;
            _userHist.AccessFailedCount = _user.AccessFailedCount;
            _userHist.LockoutEnabled = _user.LockoutEnabled;
            _userHist.PasswordHash = _user.PasswordHash;
            _userHist.RequestDate = _user.RequestDate;
            _userHist.RequestUser = _user.RequestUser;
            _db.UserHistory.Add(_userHist);

            _userStaticInfoHist.LastName = _userStaticInfo.LastName;
            _userStaticInfoHist.FirstName = _userStaticInfo.FirstName;
            _userStaticInfoHist.MiddleName = _userStaticInfo.MiddleName;
            _userStaticInfoHist.Xin = _userStaticInfo.Xin;
            _userStaticInfoHist.Birthdate = _userStaticInfo.Birthdate;
            _userStaticInfoHist.Gender = _userStaticInfo.Gender;
            _userStaticInfoHist.UserStatusId = _userStaticInfo.UserStatusId;
            _userStaticInfoHist.UserTypeId = _userStaticInfo.UserTypeId;
            _userStaticInfoHist.MainAddress = _userStaticInfo.MainAddress;
            _userStaticInfoHist.RequestUser = _userStaticInfo.RequestUser;
            _userStaticInfoHist.RequestDate = _userStaticInfo.RequestDate;
            _userStaticInfoHist.UserId = _userStaticInfo.UserId;
            _db.UserStaticInfoHistory.Add(_userStaticInfoHist);

            _userRoleHist.UserId = _userRole.UserId;
            _userRoleHist.RoleId = _userRole.RoleId;
            _db.UserRoleHistory.Add(_userRoleHist);

            _userRegisterHist.UserId = _userRegister.UserId;
            _userRegisterHist.RegisterId = _userRegister.RegisterId;
            _db.UserRegisterHistory.Add(_userRegisterHist);

            foreach (UserAddress address in _userAddresses) {
                _db.UserAddressHistory.Add(new UserAddressHistory {
                    RequestDate = DateTime.Now,
                    RequestUser = currentUser,
                    UserId = address.UserId,
                    AddressId = address.AddressId,
                    TransactionId = currentSession.TransactionId,
                    ActionType = currentAction
                });
            }

            foreach(UserProfession profession in _userProfessions) {
                _db.UserProfessionHistory.Add(new UserProfessionHistory {
                    ProfessionId = profession.ProfessionId,
                    RequestDate = DateTime.Now,
                    RequestUser = currentUser,
                    TransactionId = currentSession.TransactionId,
                    ActionType = DatabaseActionTypes.Update,
                    UserId = profession.UserId
                });
            }
        }
    }

    public enum PersistenceState {
        Create, Update, Null
    }

    public enum PersistenceHelperMode {
        Read, Write
    }
}
