-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [proc_SyncTombSimpleInfo]	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	delete  SubStone.dbo.SimpleTomb
	insert into  SubStone.dbo.SimpleTomb
	select * 
		from (
		select 
		(cast(C.Alias as nvarchar(20)) +cast(d.Alias as nvarchar(20))+cast(e.Alias as nvarchar(20))) as TombStoneId
		,b.Alias as TombStoneAlice
		, (select top 1 Applicanter from  SysLog where  ControllTid = b.Id order by Id desc) as Applicanter
		, STUFF((SELECT ', '  + CONVERT(VARCHAR,BuryMan)  from  SysLog where  ControllTid = b.Id order by Id  FOR XML PATH('')),1,1,'') as BuryMan 
		from Tombstone b
		left join SysLog a on a.ControllTid = b.Id
		left join CemeteryAreas c on b.AreaId=c.Id
		left join CemeteryRows d on b.RowId=d.Id
		left join CemeteryColumns e on b.ColumnId=e.Id
		) tt 
	group by tt.TombstoneId,TombStoneAlice,Applicanter,BuryMan
END
GO
