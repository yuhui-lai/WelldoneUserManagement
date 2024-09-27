using Microsoft.Extensions.Logging;
using System.Text.Encodings.Web;
using WUM.Lib.Interfaces;
using WUM.Lib.Models.Common;
using WUM.Lib.Models.DB_Context;
using WUM.Lib.Models.Role;
using WUM.Lib.Utilities;

namespace WUM.Lib.Services
{
    public class RoleService : IRoleService
    {
        private readonly HtmlEncoder encoder;
        private readonly ManagementContext dbContext;

        public RoleService(HtmlEncoder encoder, ManagementContext dbContext)
        {
            this.encoder = encoder;
            this.dbContext = dbContext;
        }

        public async Task<CommAPIModel<string>> CreateRoleAsync(RoleCreateReq req)
        {
            await LogUtil.SaveLog(req, req.OperatorId, "RoleService CreateRole()", LogLevel.Information.ToString(), dbContext);

            return new CommAPIModel<string>
            {
                Data = ""
            };
        }

        public async Task<CommAPIModel<string>> DeleteRoleAsync(RoleDeleteReq req)
        {
            await LogUtil.SaveLog(req, req.OperatorId, "RoleService DeleteRole()", LogLevel.Information.ToString(), dbContext);

            return new CommAPIModel<string>
            {
                Data = ""
            };
        }

        public async Task<CommAPIModel<string>> EditRoleBasicInfoAsync(RoleBasicInfoEditReq req)
        {
            await LogUtil.SaveLog(req, req.OperatorId, "RoleService EditRoleBasicInfo()", LogLevel.Information.ToString(), dbContext);

            return new CommAPIModel<string>
            {
                Data = ""
            };
        }

        public async Task<CommAPIModel<string>> EditRolePermissionAsync(RolePermissionEditReq req)
        {
            await LogUtil.SaveLog(req, req.OperatorId, "RoleService EditRolePermission()", LogLevel.Information.ToString(), dbContext);

            return new CommAPIModel<string>
            {
                Data = ""
            };
        }

        public async Task<CommAPIModel<RoleBasicInfoVM>> GetRoleBasicInfoAsync(int id)
        {
            return new CommAPIModel<RoleBasicInfoVM>
            {
                Data = GetFakeRoleBasicInfo(id)
            };
        }

        public async Task<CommAPIModel<List<ProductClassRes>>> GetProductClassesAsync()
        {
            return new CommAPIModel<List<ProductClassRes>>
            {
                Data = GetFakeProductClasses()
            };
        }

        public async Task<CommAPIModel<RolePermissionVM>> GetRolePermissionAsync(int id)
        {
            return new CommAPIModel<RolePermissionVM>
            {
                Data = GetFakeRolePermission(id)
            };
        }

        public async Task<CommAPIModel<List<RoleVM>>> GetRolesAsync(RoleListReq req)
        {
            var cPClass = req.ProductCode.Purify(encoder);
            var data = GetFakeRoles();
            var res = data.Where(x =>
                (string.IsNullOrEmpty(cPClass) || x.ProductCode == cPClass)).ToList();

            return new CommAPIModel<List<RoleVM>>
            {
                Data = res
            };
        }

        private List<RoleVM> GetFakeRoles()
        {
            List<RoleVM> roleVMs =
                [
                    new RoleVM{
                        Id = 1,
                        ProductCode = "QPAY_CUSTOMER_SERVICE",
                        ProductName = "QPAY客服",
                        RoleName = "客服內部人員"
                    },
                    new RoleVM{
                        Id = 2,
                        ProductCode = "QPAY_BACKEND",
                        ProductName = "QPAY後台",
                        RoleName = "行銷人員 (越南)"
                    },
                    new RoleVM{
                        Id = 3,
                        ProductCode = "QPAY_BACKEND",
                        ProductName = "QPAY後台",
                        RoleName = "Manager"
                    },
                    new RoleVM{
                        Id = 4,
                        ProductCode = "QPAY_BACKEND",
                        ProductName = "QPAY後台",
                        RoleName = "KYC/AML內部人員"
                    },
                    new RoleVM{
                        Id = 5,
                        ProductCode = "QPay",
                        ProductName = "QPay",
                        RoleName = "KYC/AML內部人員"
                    },
                    new RoleVM{ Id = 6, ProductCode = "QPAY_CUSTOMER_SERVICE", ProductName = "QPAY客服", RoleName = "客服主管" },
        new RoleVM{ Id = 7, ProductCode = "QPAY_BACKEND", ProductName = "QPAY後台", RoleName = "技術支援" },
        new RoleVM{ Id = 8, ProductCode = "QPAY_BACKEND", ProductName = "QPAY後台", RoleName = "產品經理" },
        new RoleVM{ Id = 9, ProductCode = "QPAY_BACKEND", ProductName = "QPAY後台", RoleName = "數據分析師" },
        new RoleVM{ Id = 10, ProductCode = "QPay", ProductName = "QPay", RoleName = "財務分析師" },
        new RoleVM{ Id = 11, ProductCode = "QPAY_CUSTOMER_SERVICE", ProductName = "QPAY客服", RoleName = "客服代表" },
        new RoleVM{ Id = 12, ProductCode = "QPAY_BACKEND", ProductName = "QPAY後台", RoleName = "行銷經理" },
        new RoleVM{ Id = 13, ProductCode = "QPAY_BACKEND", ProductName = "QPAY後台", RoleName = "開發工程師" },
        new RoleVM{ Id = 14, ProductCode = "QPAY_BACKEND", ProductName = "QPAY後台", RoleName = "測試工程師" },
        new RoleVM{ Id = 15, ProductCode = "QPay", ProductName = "QPay", RoleName = "運營經理" },
        new RoleVM{ Id = 16, ProductCode = "QPAY_CUSTOMER_SERVICE", ProductName = "QPAY客服", RoleName = "客服專員" },
        new RoleVM{ Id = 17, ProductCode = "QPAY_BACKEND", ProductName = "QPAY後台", RoleName = "數據科學家" },
        new RoleVM{ Id = 18, ProductCode = "QPAY_BACKEND", ProductName = "QPAY後台", RoleName = "系統架構師" },
        new RoleVM{ Id = 19, ProductCode = "QPAY_BACKEND", ProductName = "QPAY後台", RoleName = "安全專家" },
        new RoleVM{ Id = 20, ProductCode = "QPay", ProductName = "QPay", RoleName = "法務顧問" },
        new RoleVM{ Id = 21, ProductCode = "QPAY_CUSTOMER_SERVICE", ProductName = "QPAY客服", RoleName = "客戶經理" },
        new RoleVM{ Id = 22, ProductCode = "QPAY_BACKEND", ProductName = "QPAY後台", RoleName = "產品設計師" },
        new RoleVM{ Id = 23, ProductCode = "QPAY_BACKEND", ProductName = "QPAY後台", RoleName = "運營專員" },
        new RoleVM{ Id = 24, ProductCode = "QPAY_BACKEND", ProductName = "QPAY後台", RoleName = "數據工程師" },
        new RoleVM{ Id = 25, ProductCode = "QPay", ProductName = "QPay", RoleName = "風控專員" }
                ];
            return roleVMs;
        }

        private List<ProductClassRes> GetFakeProductClasses()
        {
            List<ProductClassRes> res =
            [
                new ProductClassRes
                {
                    ProductCode = "QPAY_CUSTOMER_SERVICE",
                    ProductName = "QPAY客服",
                },
                new ProductClassRes {
                    ProductCode = "QPAY_BACKEND",
                    ProductName = "QPAY後台",
                },
            ];
            return res;
        }

        private RolePermissionVM GetFakeRolePermission(int id)
        {
            return new RolePermissionVM
            {
                Id = id,
                RoleName = "測試角色",
                MainItems = ["APP"],
                Items =
                [
                    new RoleItemPermission
                    {
                        MainItem = "APP",
                        SubItem = "Users",
                        Read = true,
                        Write = false,
                        Delete = false,
                        Export = false
                    },
                    new RoleItemPermission
                    {
                        MainItem = "APP",
                        SubItem = "Order",
                        Read = true,
                        Write = true,
                        Delete = true,
                        Export = true
                    },
                    new RoleItemPermission
                    {
                        MainItem = "APP",
                        SubItem = "Test",
                        Read = true,
                        Write = true,
                        Delete = false,
                        Export = false
                    },
                    new RoleItemPermission
                    {
                        MainItem = "Web",
                        SubItem = "Users",
                        Read = false,
                        Write = true,
                        Delete = true,
                        Export = false
                    },
                    new RoleItemPermission
                    {
                        MainItem = "Web",
                        SubItem = "Order",
                        Read = false,
                        Write = false,
                        Delete = false,
                        Export = false
                    }
                ]
            };
        }

        private RoleBasicInfoVM GetFakeRoleBasicInfo(int id)
        {
            return new RoleBasicInfoVM
            {
                Id = id,
                RoleName = "行銷人員",
                ProductCode = "QPAY_BACKEND",
                Products =
                [
                    new RoleBasicProduct
                    {
                        ProductCode = "QPAY_BACKEND",
                        ProductName = "QPAY後台",
                    },
                    new RoleBasicProduct
                    {
                        ProductCode = "QPAY_CUSTOMER_SERVICE",
                        ProductName = "QPAY客服",
                    }
                ]
            };
        }
    }
}
