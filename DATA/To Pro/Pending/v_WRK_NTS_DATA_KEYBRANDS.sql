USE [godzilla]
GO

/****** Object:  View [dbo].[v_WRK_NTS_DATA_KEYBRANDS]    Script Date: 11/10/2016 19:17:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[v_WRK_NTS_DATA_KEYBRANDS]
AS
  WITH DATA AS(SELECT        bst.CHANNEL, bst.MARKET, bst.ID AS BRAND, data.[SELLIN COL1], data.[YEAR PERIOD], data.[MONTH PERIOD], data.TYPE
FROM            dbo.ROSETTA_LOADER AS mstr INNER JOIN
                         dbo.BRAND_MASTER AS bst ON bst.MARKET = mstr.[MARKET ID] AND bst.ID = mstr.[BRAND ID] INNER JOIN
                         dbo.WRK_BOY_DATA AS data ON data.MARKET = bst.MARKET AND data.BRAND = bst.ID AND data.CHANNEL = bst.CHANNEL
						 ), MSTR AS
    (SELECT ROW_ID, ID, MARKET, BRAND, [CONFIG MARKET] AS CONFIG, [GROUP]
      FROM            dbo.v_KEYBRANDS_MASTER)
    SELECT        ROW_NUMBER() OVER (ORDER BY MSTR_1.ID, MSTR_1.[GROUP], DATA_1.[YEAR PERIOD], DATA_1.[MONTH PERIOD]) AS ROW_ID, MSTR_1.[GROUP] AS [GROUP], MSTR_1.ID, 
SUM(DATA_1.[SELLIN COL1] * MSTR_1.CONFIG) AS AMOUNT, DATA_1.[YEAR PERIOD], DATA_1.[MONTH PERIOD], DATA_1.TYPE
FROM            DATA AS DATA_1 INNER JOIN
                         MSTR AS MSTR_1 ON MSTR_1.MARKET = DATA_1.MARKET AND MSTR_1.BRAND = DATA_1.BRAND
GROUP BY MSTR_1.ID, MSTR_1.[GROUP], DATA_1.[YEAR PERIOD], DATA_1.[MONTH PERIOD], DATA_1.TYPE

GO


