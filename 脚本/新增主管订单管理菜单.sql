 
insert into SystemMenus values('2005','���ܶ�������','2','2000','5','Order_DirectorOrders')
insert into SystemMenus values('4006','��Ʒ���ͳ��','2','4000','5','Report_DesginCostStatistics')

 
insert into Roles values('��������','orderDirector','2','������Ʋ��ŵĶ������','0',1)


update   OrderOperationLogs set Status = 100 where Status =18
update   OrderOperationLogs set Status = 101 where Status =19
update   OrderOperationLogs set Status = 102 where Status =20
 
 select * from SystemMenus
 