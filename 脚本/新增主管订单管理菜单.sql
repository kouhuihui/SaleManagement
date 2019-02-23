 
insert into SystemMenus values('2005','主管订单管理','2','2000','5','Order_DirectorOrders')
insert into SystemMenus values('4006','设计费用统计','2','4000','5','Report_DesginCostStatistics')

 
insert into Roles values('订单主管','orderDirector','2','负责设计部门的订单审核','0',1)


update   OrderOperationLogs set Status = 100 where Status =18
update   OrderOperationLogs set Status = 101 where Status =19
update   OrderOperationLogs set Status = 102 where Status =20
 
 select * from SystemMenus
 