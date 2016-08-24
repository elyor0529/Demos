USE [HRM]
GO
/****** Object:  StoredProcedure [dbo].[sp_SalaryReportNotAssigned]    Script Date: 24.08.2016 19:03:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[sp_SalaryReportNotAssigned]
	-- Add the parameters for the stored procedure here
	@Date nvarchar(50),
	@PageNumber INT = 1,
	@PageSize   INT = 100
AS
BEGIN

DECLARE @StartRow int;
DECLARE @EndRow int;
DECLARE @Dt date;

-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
SET NOCOUNT ON;

-- Insert statements for procedure here
SET @StartRow = (@PageNumber -1) * @PageSize + 1;
SET @EndRow = @PageNumber * @PageSize;
SET @Dt=convert(date, @Date, 101);

WITH report AS
    (
	SELECT d.Name,
			s.Price,
			s.Date,
			ROW_NUMBER() OVER(ORDER BY s.Id DESC) as Row, 
			COUNT(s.Id) OVER() AS Total 
	FROM [dbo].[Salary] AS s
		INNER JOIN  [dbo].[Developer] AS d ON s.WorkerId=d.Id 
	WHERE YEAR(@Dt)=YEAR(s.Date) AND MONTH(@Dt)=MONTH(s.Date) AND s.IsGiven is null
	)
SELECT r.Name,r.Price,r.Date, r.Total FROM report as r
WHERE r.Row BETWEEN @StartRow AND @EndRow;

END
