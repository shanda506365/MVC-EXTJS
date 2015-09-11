/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP 1000 [Id]
      ,[Name]
      ,[Code]
      ,[Url]
      ,[ParentId]
  FROM [CemeteryDB].[dbo].[Function]
  
  select * from Tombstone
  select * from TombstoneType
  select * from PaymentStatus
    select * from CemeteryAreas
  select * from CemeteryColumns
    

  
declare @Count int
set @Count = 23
while (@Count < 100)
begin
	insert  CemeteryColumns values(CONVERT(varchar(50),@Count,21)+'号',@Count,null)	
	set @Count = @Count+1		
end  

  select * from CemeteryRows

declare @Count1 int
set @Count1 = 50
while (@Count1 < 51)
begin
	insert  CemeteryRows values(CONVERT(varchar(50),@Count1,21)+'排',@Count1,null)	
	set @Count1 = @Count1+1		
end  
