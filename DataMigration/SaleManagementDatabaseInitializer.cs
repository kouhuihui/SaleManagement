﻿using Microsoft.AspNet.Identity;
using SaleManagement.Core;
using SaleManagement.Core.Models;
using SaleManagement.Core.Store;
using SaleManagement.Managers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DataMigration
{
    public class SaleManagementDatabaseInitializer : DropCreateDatabaseAlways<SaleManagementDbContext>
    {
        public SaleManagementDatabaseInitializer()
        {

        }
        protected override void Seed(SaleManagementDbContext context)
        {
            DepartmentSeed(context);
            RoleSeed(context);
            CompanySeed(context);
            UserSeed(context);
            CustomerSeed(context);
            SystemMenuSeed(context);
            GemCategorysSeed(context);
            MatchStoneSeed(context);
            ColorFormSeed(context);
            ProductCategorySeed(context);
            AdminRoleMenuSeed(context);
        }

        private void CompanySeed(SaleManagementDbContext context)
        {
            var company = new Company
            {
                Name = "18K珠宝有限公司",
                ContactPerson = "谭泓浩",
                Address = "深圳罗湖区xxx",
                ShortName = "18K",
                Status = CompanyStatus.Normal
            };
            context.Companys.Add(company);
            context.SaveChanges();
        }

        private void UserSeed(SaleManagementDbContext context)
        {
            var userManager = new UserManager();
            var roles = context.Roles.ToList();
            var company = context.Companys.FirstOrDefault();
            var password = "123456";
            foreach (var role in roles)
            {
                var user = new SaleUser
                {
                    Name = role.Code,
                    UserName = role.Code,
                    RoleId = role.Id,
                    EmailConfirmed = true,
                    Status = UserStatus.Normal,
                    CompanyId = company.Id,
                    IdentityType = IdentityType.Employee
                };
                userManager.Create(user, password);
            }
        }

        private void CustomerSeed(SaleManagementDbContext context)
        {
            var userManager = new UserManager();
            var company = context.Companys.FirstOrDefault();
            var customerRole = context.Roles.FirstOrDefault(r => r.Code == SaleManagentConstants.SystemRole.CommonUser);
            var customer1 = new SaleUser
            {
                Name = "testCustomer",
                UserName = "testCustomer",
                RoleId = customerRole.Id,
                Status = UserStatus.Normal,
                EmailConfirmed = true,
                CompanyId = company.Id,
                IdentityType = IdentityType.Customer
            };
            userManager.Create(customer1, "123456");

            var customerInfo = new CustomerInfo
            {
                Id = Guid.NewGuid().ToString(),
                UserId = customer1.Id,
                Address = "深圳市南山区白石洲"
            };
            context.CustomerInfos.Add(customerInfo);
            context.SaveChanges();
        }

        private void DepartmentSeed(SaleManagementDbContext context)
        {
            var department = new Department()
            {
                Name = "公司总部",
                order = 1,
                ParentId = null,
                Created = DateTime.Now
            };
            context.Departments.Add(department);
            context.SaveChanges();
        }

        private void RoleSeed(SaleManagementDbContext context)
        {
            var commonUserRole = new Role()
            {
                Name = "普通用户",
                Code = SaleManagentConstants.SystemRole.CommonUser,
                Description = "普通用户，较低权限的用户",
                Type = RoleType.Production,
                Deleted = false,
                IsSystemRole = true
            };

            var adminRole = new Role()
            {
                Name = "管理员",
                Code = SaleManagentConstants.SystemRole.Admin,
                Description = "负责公司日常运营管理",
                Type = RoleType.Admin,
                Deleted = false,
                IsSystemRole = true
            };

            var customerServiceRole = new Role()
            {
                Name = "客服",
                Description = "负责公司日常订单流程操作",
                Type = RoleType.Production,
                Deleted = false,
                Code = SaleManagentConstants.SystemRole.CustomerService,
                IsSystemRole = true
            };

            var outputWaxRole = new Role
            {
                Name = "出蜡",
                Description = "负责订单的蜡具模型设计",
                Type = RoleType.Production,
                Deleted = false,
                Code = SaleManagentConstants.SystemRole.OutputWax,
                IsSystemRole = true
            };

            var moduleRole = new Role
            {
                Name = "执模",
                Description = "负责订单的蜡具模型设计",
                Type = RoleType.Production,
                Deleted = false,
                Code = SaleManagentConstants.SystemRole.Module,
                IsSystemRole = true
            };

            var designRole = new Role
            {
                Name = "设计师",
                Description = "负责订单的款式设计",
                Type = RoleType.Production,
                Deleted = false,
                Code = SaleManagentConstants.SystemRole.Design,
                IsSystemRole = true
            };
            var sendAndReceiveRole = new Role
            {
                Name = "总收发",
                Description = "负责订单各工序的材料打的收发",
                Type = RoleType.Production,
                Deleted = false,
                Code = SaleManagentConstants.SystemRole.SendAndReceive,
                IsSystemRole = true
            };
            var financeRole = new Role
            {
                Name = "财务",
                Description = "负责订单的发货及对账",
                Type = RoleType.Production,
                Deleted = false,
                Code = SaleManagentConstants.SystemRole.Finance,
                IsSystemRole = true
            };
            var assistantStoneRole = new Role
            {
                Name = "配石",
                Description = "负责订单的配石的数据填充",
                Type = RoleType.Production,
                Deleted = false,
                Code = SaleManagentConstants.SystemRole.AssistantStone,
                IsSystemRole = true
            };
            var packRole = new Role
            {
                Name = "打包",
                Description = "负责订单的打包",
                Type = RoleType.Production,
                Deleted = false,
                Code = SaleManagentConstants.SystemRole.Pack,
                IsSystemRole = true
            };

            var withTheHandRole = new Role
            {
                Name = "手镶",
                Description = "手镶",
                Type = RoleType.Production,
                Deleted = false,
                Code = SaleManagentConstants.SystemRole.WithTheHand,
                IsSystemRole = true
            };

            var microInsertRole = new Role
            {
                Name = "微镶",
                Description = "微镶",
                Type = RoleType.Production,
                Deleted = false,
                Code = SaleManagentConstants.SystemRole.MicroInsert,
                IsSystemRole = true
            };

            var polishingRole = new Role
            {
                Name = "抛光",
                Description = "抛光",
                Type = RoleType.Production,
                Deleted = false,
                Code = SaleManagentConstants.SystemRole.Polishing,
                IsSystemRole = true
            };

            var direktorRole = new Role
            {
                Name = "厂长",
                Description = "负责工厂管理",
                Type = RoleType.Production,
                Deleted = false,
                Code = SaleManagentConstants.SystemRole.Direktor,
                IsSystemRole = true
            };
            var orderViewRole = new Role
            {
                Name = "订单查看",
                Description = "订单查看",
                Type = RoleType.Production,
                Deleted = false,
                Code = SaleManagentConstants.SystemRole.OrderView,
                IsSystemRole = true
            };

            context.Roles.Add(commonUserRole);
            context.Roles.Add(adminRole);
            context.Roles.Add(customerServiceRole);
            context.Roles.Add(outputWaxRole);
            context.Roles.Add(designRole);
            context.Roles.Add(sendAndReceiveRole);
            context.Roles.Add(financeRole);
            context.Roles.Add(assistantStoneRole);
            context.Roles.Add(moduleRole);
            context.Roles.Add(packRole);
            context.Roles.Add(withTheHandRole);
            context.Roles.Add(microInsertRole);
            context.Roles.Add(polishingRole);
            context.Roles.Add(direktorRole);
            context.Roles.Add(orderViewRole);
            context.SaveChanges();
        }

        private void SystemMenuSeed(SaleManagementDbContext context)
        {
            #region
            var menu = new SystemMenu
            {
                Id = 1000,
                Level = 1,
                Name = "系统管理",
                parentId = null,
                Order = 1,
                ControllerAction = null
            };

            var menu1 = new SystemMenu
            {
                Id = 1001,
                Level = 2,
                Name = "员工管理",
                parentId = 1000,
                Order = 1,
                ControllerAction = "User_List"
            };

            var menu2 = new SystemMenu
            {
                Id = 1002,
                Level = 2,
                Name = "部门管理",
                parentId = 1000,
                Order = 2,
                ControllerAction = null
            };

            var menu3 = new SystemMenu
            {
                Id = 1003,
                Level = 2,
                Name = "角色管理",
                parentId = 1000,
                Order = 3,
                ControllerAction = "Role_List"
            };

            var menu18 = new SystemMenu
            {
                Id = 1004,
                Level = 2,
                Name = "客户管理",
                parentId = 1000,
                Order = 4,
                ControllerAction = "Crm_List"
            };

            var menu4 = new SystemMenu
            {
                Id = 2000,
                Level = 1,
                Name = "生产管理",
                parentId = null,
                Order = 2,
                ControllerAction = null
            };
            var menu5 = new SystemMenu
            {
                Id = 2001,
                Level = 2,
                Name = "生产单",
                parentId = 2000,
                Order = 1,
                ControllerAction = "Order_List"
            };
            var menu6 = new SystemMenu
            {
                Id = 2002,
                Level = 2,
                Name = "待设计的订单",
                parentId = 2000,
                Order = 2,
                ControllerAction = "Order_Myorders"
            };

            var menu13 = new SystemMenu
            {
                Id = 2003,
                Level = 3,
                Name = "下单",
                parentId = 2000,
                Order = 3,
                ControllerAction = ""
            };
            var menu14 = new SystemMenu
            {
                Id = 2004,
                Level = 3,
                Name = "出货单",
                parentId = 2000,
                Order = 4,
                ControllerAction = "Shipment_List"
            };

            var menu7 = new SystemMenu
            {
                Id = 3000,
                Level = 1,
                Name = "基础信息",
                parentId = null,
                Order = 1
            };

            var menu8 = new SystemMenu
            {
                Id = 3001,
                Level = 2,
                Name = "宝石信息",
                parentId = 3000,
                Order = 1
            };

            var menu9 = new SystemMenu
            {
                Id = 3002,
                Level = 2,
                Name = "成色信息",
                parentId = 3000,
                Order = 2
            };

            var menu10 = new SystemMenu
            {
                Id = 3003,
                Level = 2,
                Name = "配石信息",
                parentId = 3000,
                Order = 3
            };

            var menu11 = new SystemMenu
            {
                Id = 3004,
                Level = 2,
                Name = "品类信息",
                parentId = 3000,
                Order = 4
            };
            var menu12 = new SystemMenu
            {
                Id = 3005,
                Level = 2,
                Name = "金价管理",
                parentId = 3000,
                Order = 5
            };
            var menu15 = new SystemMenu
            {
                Id = 3006,
                Level = 2,
                Name = "公告管理",
                parentId = 3000,
                Order = 6
            };

            var menu19 = new SystemMenu
            {
                Id = 3007,
                Level = 2,
                Name = "工期设置",
                parentId = 3000,
                Order = 7
            };

            var menu16 = new SystemMenu
            {
                Id = 4000,
                Level = 1,
                Name = "财务管理",
                parentId = null,
                Order = 1
            };

            var menu17 = new SystemMenu
            {
                Id = 4001,
                Level = 2,
                Name = "对账",
                parentId = 4000,
                Order = 1,
                ControllerAction = "Reconciliation_List"
            };
            var menu20 = new SystemMenu
            {
                Id = 4002,
                Level = 2,
                Name = "账目报表",
                parentId = 4000,
                Order = 2,
                ControllerAction = "Report_AccountStatistics"
            };

            var menu21 = new SystemMenu
            {
                Id = 4003,
                Level = 2,
                Name = "出货统计",
                parentId = 4000,
                Order = 3,
                ControllerAction = "Report_shipmentStatistics"
            };

            var menu22 = new SystemMenu
            {
                Id = 4004,
                Level = 2,
                Name = "配石报表",
                parentId = 4000,
                Order = 4,
                ControllerAction = "Report_OrderSetStoneStatistics"
            };

            context.Menus.Add(menu);
            context.Menus.Add(menu1);
            context.Menus.Add(menu2);
            context.Menus.Add(menu3);
            context.Menus.Add(menu4);
            context.Menus.Add(menu5);
            context.Menus.Add(menu6);
            context.Menus.Add(menu7);
            context.Menus.Add(menu8);
            context.Menus.Add(menu9);
            context.Menus.Add(menu10);
            context.Menus.Add(menu11);
            context.Menus.Add(menu12);
            context.Menus.Add(menu13);
            context.Menus.Add(menu14);
            context.Menus.Add(menu15);
            context.Menus.Add(menu16);
            context.Menus.Add(menu17);
            context.Menus.Add(menu18);
            context.Menus.Add(menu19);
            context.Menus.Add(menu20);
            context.Menus.Add(menu21);
            context.Menus.Add(menu22);
            context.SaveChanges();
            #endregion
        }

        private void GemCategorysSeed(SaleManagementDbContext context)
        {
            var categorys = new List<GemCategory>
            {
                new GemCategory { Name = "钻石"},
                new GemCategory { Name = "彩宝"},
                new GemCategory { Name = "无石"},
                new GemCategory { Name = "玉石"},
                new GemCategory { Name = "珍珠"},
                new GemCategory { Name = "半宝"},
            };
            categorys.ForEach(e => context.GemCategorys.Add(e));
            context.SaveChanges();
        }

        private void MatchStoneSeed(SaleManagementDbContext context)
        {
            var matchStones = new List<MatchStone>
            {
                new MatchStone { Name = "高品质黄钻" , Price = 5000.00, WorkingCost = 5 },
                new MatchStone { Name = "黄钻" , Price =  3800.00 , WorkingCost = 5 },
                new MatchStone { Name = "1-5分T方" , Price = 4500.00 , WorkingCost = 5 },
                new MatchStone { Name = "8-12分马眼" , Price =  6200.00 , WorkingCost = 5  },
                new MatchStone { Name = "7-8分马眼" , Price =  6000.00 , WorkingCost = 5 },
                new MatchStone { Name= "1-5分马眼" , Price =  4800.00 , WorkingCost = 5 },
                new MatchStone { Name= "18-22分水滴" , Price =  8400.00  , WorkingCost = 5 },
                new MatchStone { Name= "13-17分水滴" , Price =  7400.00 , WorkingCost = 5  },
            };
            matchStones.ForEach(e => context.MatchStones.Add(e));
            context.SaveChanges();
        }

        private void ColorFormSeed(SaleManagementDbContext context)
        {
            var colorForms = new List<ColorForm>
            {    new ColorForm { Name = "PT950" , Purity = "0.950"},
                new ColorForm { Name = "18K分色" , Purity = "0.750"},
                new ColorForm { Name = "18K黄" , Purity = "0.750"},
                new ColorForm { Name = "18K红" , Purity = "0.750"},
                new ColorForm { Name = "18K白" , Purity = "0.750"},
            };
            colorForms.ForEach(e => context.ColorForms.Add(e));
            context.SaveChanges();
        }

        private void ProductCategorySeed(SaleManagementDbContext context)
        {
            var productCategories = new List<ProductCategory>
            {
                new ProductCategory { Name = "女戒" },
                new ProductCategory { Name = "吊坠" },
                new ProductCategory { Name = "男戒" },
                new ProductCategory { Name = "耳饰" },
                new ProductCategory { Name = "手链" },
                new ProductCategory { Name = "手镯" },
                new ProductCategory { Name = "其他" },
            };
            productCategories.ForEach(e => context.ProductCategorys.Add(e));
            context.SaveChanges();
        }

        private void AdminRoleMenuSeed(SaleManagementDbContext context)
        {
            var role = context.Roles.FirstOrDefault(r => r.Code == SaleManagentConstants.SystemRole.Admin);
            var sysmenus = context.Menus.Where(r => r.parentId != null).ToList();
            sysmenus.ForEach(s =>
            {
                context.RoleMenus.Add(new RoleMenu
                {
                    RoleId = role.Id,
                    SystemMenuId = s.Id
                });
            });

            context.SaveChanges();
        }
    }
}

