using RoadFlow.Data.Factory;
using RoadFlow.Data.Interface;
using RoadFlow.Data.Model;
using RoadFlow.Utility;
using RoadFlow.Utility.New;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace RoadFlow.Platform
{
    /// <summary>
    /// 用户表业务逻辑层。
    /// </summary>
    public class UsersBLL
    {
        /// <summary>
        /// 用户表数据库访问基础对象。
        /// </summary>
        private IBase baseDb;

        /// <summary>
        /// 获取当前登录用户所在的部门ID。
        /// </summary>
        public static Guid CurrentDeptID
        {
            get
            {
                UsersBLL bll = new UsersBLL();
                Data.Model.Organize organize = bll.GetDeptByUserID(UsersBLL.CurrentUserID);
                return organize != null ? organize.ID : Guid.Empty;
            }
        }

        /// <summary>
        /// 获取当前登录用户所在部门的名称。
        /// </summary>
        public static string CurrentDeptName
        {
            get
            {
                UsersBLL bll = new UsersBLL();
                Data.Model.Organize organize = bll.GetDeptByUserID(UsersBLL.CurrentUserID);
                return organize != null ? organize.Name : string.Empty;
            }
        }

        /// <summary>
        /// 获取当前登录用户关系组织ID。
        /// </summary>
        public static Guid CurrentFirstRelationID
        {
            get
            {
                UsersRelation relation = new UsersRelation();
                List<Data.Model.UsersRelation> list = relation.GetAllByUserID(UsersBLL.CurrentUserID);
                return list.Count > 0 ? list.First().OrganizeID : Guid.Empty;
            }
        }

        /// <summary>
        /// 获取当前登录用户对象。
        /// </summary>
        public static UsersModel CurrentUser
        {
            get
            {
                object result = HttpContext.Current.Session[Keys.SessionKeys.User.ToString()];
                if (result != null)
                {
                    return result as UsersModel;
                }
                Guid guid = UsersBLL.CurrentUserID;
                if (Guid.Empty.Equals(guid))
                {
                    return null;
                }
                UsersBLL bll = new UsersBLL();
                UsersModel model = bll.Get(guid);
                if (model != null)
                {
                    HttpContext.Current.Session[Keys.SessionKeys.User.ToString()] = model;
                }
                return model;
            }
        }

        /// <summary>
        /// 获取当前登录用户的ID。
        /// </summary>
        public static Guid CurrentUserID
        {
            get
            {
                object result = HttpContext.Current.Session[Keys.SessionKeys.UserID.ToString()];
                return result != null ? result.Convert<Guid>() : Guid.Empty;
            }
        }

        /// <summary>
        /// 获取当前登录
        /// </summary>
        public static string CurrentUserName
        {
            get
            {
                UsersModel model = UsersBLL.CurrentUser;
                return model != null ? model.Name : string.Empty;
            }
        }

        /// <summary>
        /// 获取当前登录用户所有的角色。
        /// </summary>
        public static List<Guid> CurrentUserRoles
        {
            get
            {
                Guid guid = UsersBLL.CurrentUserID;
                if (Guid.Empty.Equals(guid))
                {
                    return new List<Guid>();
                }
                UsersRole role = new UsersRole();
                List<Data.Model.UsersRole> list = role.GetByUserIDFromCache(guid);
                return list.Select(r => r.RoleID).ToList();
            }
        }

        /// <summary>
        /// 排序字段名称。
        /// </summary>
        private const string Sort = "Sort";

        /// <summary>
        /// 用户表名称。
        /// </summary>
        private const string TableName = "Users";

        /// <summary>
        /// 用户前缀。
        /// </summary>
        public const string PREFIX = "u_";

        /// <summary>
        /// 初始化用户表业务逻辑层对象。
        /// </summary>
        public UsersBLL()
        {
            this.baseDb = Factory.GetBase(UsersBLL.TableName, UsersBLL.Sort);
        }

        /// <summary>
        /// 添加一个用户。
        /// </summary>
        /// <param name="model">需要添加的用户对象。</param>
        /// <returns>添加成功的用户数量。</returns>
        public int Add(UsersModel model)
        {
            return this.baseDb.Add<UsersModel>(model);
        }

        /// <summary>
        /// 更新一个用户信息。
        /// </summary>
        /// <param name="model">需要更新的用户对象。</param>
        /// <returns>更新成功的用户数量。</returns>
        public int Update(UsersModel model)
        {
            return this.baseDb.Update<UsersModel>(model, new KeyValuePair<string, object>("ID", model.ID));
        }

        /// <summary>
        /// 根据ID获取单个用户信息对象。
        /// </summary>
        /// <param name="id">用户ID。</param>
        /// <returns>若存在该ID，则为该ID对应的用户对象，否则为null。</returns>
        public UsersModel Get(Guid id)
        {
            return this.baseDb.Get<UsersModel>(new KeyValuePair<string, object>("ID", id));
        }

        /// <summary>
        /// 根据账号获取单个用户信息对象。
        /// </summary>
        /// <param name="account">需要获取用户信息的账号。</param>
        /// <returns>若存在该账号，则为账号对应的用户信息，否则为null。</returns>
        public UsersModel GetByAccount(string account)
        {
            if (string.IsNullOrEmpty(account))
            {
                return null;
            }
            return this.baseDb.Get<UsersModel>(new KeyValuePair<string, object>("Account", account));
        }

        /// <summary>
        /// 根据组装的ID列表，查询用户列表。
        /// </summary>
        /// <param name="ids">列表格式为：'guid','guid','guid',...（该格式是为了兼容老版本方法）。</param>
        /// <returns>查询的用户列表。</returns>
        public List<UsersModel> GetUsers(string ids)
        {
            if (string.IsNullOrEmpty(ids))
            {
                return new List<UsersModel>();
            }
            List<string> list = ids.Split(',').Select(s => s.Trim('\'')).ToList<string>();
            return this.baseDb.GetAll(0, new Dictionary<KeyValuePair<string, SQLFilterType>, object>()
            {
                { new KeyValuePair<string, SQLFilterType>("ID", SQLFilterType.IN), list }
            }).ToList<UsersModel>();
        }

        /// <summary>
        /// 将用户标记为删除状态。
        /// </summary>
        /// <param name="id">需要标记为删除状态的用户。</param>
        /// <returns>标记成功的用户数量。</returns>
        public int Delete(Guid id)
        {
            return this.baseDb.Delete(id);
        }

        /// <summary>
        /// 获取系统初始密码。
        /// </summary>
        /// <returns></returns>
        public string GetInitPassword()
        {
            return Config.SystemInitPassword;
        }

        /// <summary>
        /// 对密码进行加密。
        /// </summary>
        /// <param name="password">需要加密的明文密码。</param>
        /// <returns>加密后的密文密码。</returns>
        public string EncryptionPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return string.Empty;
            }
            string result = Encryption.ComputeMd5(password);
            return Encryption.ComputeMd5(password);
        }

        /// <summary>
        /// 获取一个用户加密后的密码。
        /// </summary>
        /// <param name="userId">用户ID字符串。</param>
        /// <param name="password">用户明文密码。</param>
        /// <returns>加密后的密码。</returns>
        public string GetUserEncryptionPassword(string userId, string password)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(password))
            {
                return string.Empty;
            }
            return this.EncryptionPassword(userId.Trim().ToLower() + password.Trim());
        }

        /// <summary>
        /// 初始化一个用户的密码。
        /// </summary>
        /// <param name="id">用户ID。</param>
        /// <returns>是否初始化成功。</returns>
        public bool InitPassword(Guid id)
        {
            UsersModel model = this.Get(id);
            if (model == null)
            {
                return false;
            }
            string initPwd = this.GetInitPassword();
            model.Password = this.GetUserEncryptionPassword(model.ID.ToString(), initPwd);
            return this.Update(model) > 0;
        }

        /// <summary>
        /// 查询一个岗位下的人员列表。
        /// </summary>
        /// <param name="organizeId">岗位ID。</param>
        /// <returns></returns>
        public List<UsersModel> GetAllByOrganizeID(Guid organizeId)
        {
            //ExecuteBLL bll = new ExecuteBLL();
            //StringBuilder builder = new StringBuilder();
            //builder.Append("select * from Users where ID in (select UserID from UsersRelation where OrganizeID=@OrganizeID)");
            //return bll.GetDataTable(builder.ToString(), new IDataParameter[]
            //{
            //    new SqlParameter("@OrganizeID", organizeId)
            //}).ToList<UsersModel>();
            IBase usersRelation = Factory.GetBase("UsersRelation", "Sort");
            List<Data.Model.UsersRelation> model = usersRelation.GetAll(0, new Dictionary<KeyValuePair<string, SQLFilterType>, object>()
            {
                { new KeyValuePair<string, SQLFilterType>("OrganizeID", SQLFilterType.EQUAL), organizeId }
            }).ToList<Data.Model.UsersRelation>();
            List<string> list = model.Select<Data.Model.UsersRelation, string>(ur => ur.UserID.ToString()).ToList<string>();
            return this.baseDb.GetAll(0, new Dictionary<KeyValuePair<string, SQLFilterType>, object>()
            {
                { new KeyValuePair<string, SQLFilterType>("ID", SQLFilterType.IN), list }
            }).ToList<UsersModel>();
        }

        /// <summary>
        /// 获取用户主要岗位的ID。
        /// </summary>
        /// <param name="id">用户ID。</param>
        /// <returns>用户对应的主要岗位的ID。</returns>
        public Guid GetMainStation(Guid id)
        {
            IBase usersRelation = Factory.GetBase("UsersRelation", "Sort");
            Data.Model.UsersRelation model = usersRelation.Get<Data.Model.UsersRelation>(
                new KeyValuePair<string, object>("UserID", id),
                new KeyValuePair<string, object>("IsMain", 1));
            return model != null ? model.OrganizeID : Guid.Empty;
        }

        /// <summary>
        /// 查询一组岗位下所有人员。
        /// </summary>
        /// <param name="organizeIds">岗位ID列表。</param>
        /// <returns></returns>
        public List<UsersModel> GetAllByOrganizeIDArray(IEnumerable<Guid> organizeIds)
        {
            if (organizeIds == null)
            {
                return new List<UsersModel>();
            }
            IBase usersRelation = Factory.GetBase("UsersRelation", "Sort");
            List<string> list = organizeIds.Select<Guid, string>(g => g.ToString()).ToList<string>();
            List<Data.Model.UsersRelation> model = usersRelation.GetAll(0, new Dictionary<KeyValuePair<string, SQLFilterType>, object>()
            {
                { new KeyValuePair<string, SQLFilterType>("OrganizeID", SQLFilterType.IN), list }
            }).ToList<Data.Model.UsersRelation>();
            list = model.Select<Data.Model.UsersRelation, string>(ur => ur.UserID.ToString()).ToList<string>();
            return this.baseDb.GetAll(0, new Dictionary<KeyValuePair<string, SQLFilterType>, object>()
            {
                { new KeyValuePair<string, SQLFilterType>("ID", SQLFilterType.IN), list }
            }).ToList<UsersModel>();
        }

        /// <summary>
        /// 获取一个用户所在的部门。
        /// </summary>
        /// <param name="userId">用户ID。</param>
        /// <returns></returns>
        public Data.Model.Organize GetDeptByUserID(Guid userId)
        {
            Guid organizeId = this.GetMainStation(userId);
            if (organizeId.Equals(Guid.Empty))
            {
                return null;
            }
            Organize organize = new Organize();
            List<Data.Model.Organize> list = organize.GetAllParent(organizeId);
            foreach (Data.Model.Organize current in list)
            {
                if (current.Type.Equals(1) || current.Type.Equals(2))
                {
                    return current;
                }
            }
            return null;
        }

        /// <summary>
        /// 检查账号是否重复。
        /// </summary>
        /// <param name="account">需要检查的账号。</param>
        /// <returns>重复则为true，否则为false。</returns>
        public bool HasAccount(string account, string userId = "")
        {
            if (string.IsNullOrEmpty(account))
            {
                return false;
            }
            UsersModel model = this.GetByAccount(account);
            if (model == null)
            {
                return false;
            }
            Guid id;
            if (Guid.TryParse(userId, out id))
            {
                return !model.ID.Equals(id);
            }
            return true;
        }

        /// <summary>
        /// 用户修改密码。
        /// </summary>
        /// <param name="password">新密码。</param>
        /// <param name="userId">用户ID。</param>
        /// <returns>是否修改成功。</returns>
        public bool UpdatePassword(string password, Guid userId)
        {
            UsersModel model = this.Get(userId);
            if (model == null)
            {
                return false;
            }
            model.Password = this.GetUserEncryptionPassword(userId.ToString(), password);
            return this.Update(model) > 0;
        }

        /// <summary>
        /// 获取一个不重复的账号。
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public string GetAccount(string account)
        {
            if (string.IsNullOrEmpty(account))
            {
                return string.Empty;
            }
            int index = 0;
            string result = account.Trim();
            while (this.HasAccount(result))
            {
                result += (++index).ToString();
            }
            return result;
        }

        /// <summary>
        /// 更新用户排序。
        /// </summary>
        /// <param name="userId">用户ID。</param>
        /// <param name="sort">排序编号。</param>
        /// <returns></returns>
        public int UpdateSort(Guid userId, int sort)
        {
            UsersModel model = this.Get(userId);
            if (model == null)
            {
                return 0;
            }
            model.Sort = sort;
            return this.Update(model);
        }

        /// <summary>
        /// 获取用户名称。
        /// </summary>
        /// <param name="userId">用户ID。</param>
        /// <returns></returns>
        public string GetName(Guid userId)
        {
            UsersModel model = this.Get(userId);
            return model != null ? model.Name : string.Empty;
        }

        /// <summary>
        /// 去掉ID前缀。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string RemovePrefix(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return string.Empty;
            }
            return id.Replace(UsersBLL.PREFIX, string.Empty);
        }

        /// <summary>
        /// 去掉ID前缀。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string RemovePrefix1(string id)
        {
            return UsersBLL.RemovePrefix(id);
        }

        /// <summary>
        /// 得到一个人员的主管。
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetLeader(Guid userId)
        {
            Guid organizeId = this.GetMainStation(userId);
            if (Guid.Empty.Equals(organizeId))
            {
                return string.Empty;
            }
            Organize organizeBll = new Organize();
            Data.Model.Organize organize = organizeBll.Get(organizeId);
            if (organize == null)
            {
                return string.Empty;
            }
            if (!string.IsNullOrEmpty(organize.Leader))
            {
                return organize.Leader;
            }
            List<Data.Model.Organize> list = organizeBll.GetAllParent(organize.Number);
            foreach (Data.Model.Organize current in list)
            {
                if (!string.IsNullOrEmpty(current.Leader))
                {
                    return current.Leader;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 得到一个人员的分管领导。
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetChargeLeader(Guid userId)
        {
            Guid organizeId = this.GetMainStation(userId);
            if (organizeId == null)
            {
                return string.Empty;
            }
            Organize organizeBll = new Organize();
            Data.Model.Organize organize = organizeBll.Get(organizeId);
            if (organize == null)
            {
                return string.Empty;
            }
            if (!string.IsNullOrEmpty(organize.ChargeLeader))
            {
                return organize.ChargeLeader;
            }
            var parents = organizeBll.GetAllParent(organize.Number);
            foreach (var current in parents)
            {
                if (!string.IsNullOrEmpty(current.ChargeLeader))
                {
                    return current.ChargeLeader;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 判断一个人员是否是部门主管。
        /// </summary>
        /// <param name="id">人员ID。</param>
        /// <returns></returns>
        public bool IsLeader(Guid id)
        {
            string leader = this.GetLeader(id);
            return leader.Contains(id.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 判断一个人员是否是部门分管领导。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsChargeLeader(Guid id)
        {
            string leader = this.GetChargeLeader(id);
            return leader.Contains(id.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 判断一个人员是否在一个组织机构字符串里。
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="memberString"></param>
        /// <returns></returns>
        public bool IsContains(Guid userID, string memberString)
        {
            if (string.IsNullOrEmpty(memberString))
            {
                return false;
            }
            Organize organizeBll = new Organize();
            List<UsersModel> users = organizeBll.GetAllUsers(memberString);
            return users.Any(u => u.ID.Equals(userID));
        }
    }

    public class UsersEqualityComparer : IEqualityComparer<RoadFlow.Data.Model.UsersModel>
    {
        public bool Equals(RoadFlow.Data.Model.UsersModel user1, RoadFlow.Data.Model.UsersModel user2)
        {
            return user1 == null || user2 == null || user1.ID == user2.ID;
        }
        public int GetHashCode(RoadFlow.Data.Model.UsersModel user)
        {
            return user.ToString().GetHashCode();
        }
    }
}