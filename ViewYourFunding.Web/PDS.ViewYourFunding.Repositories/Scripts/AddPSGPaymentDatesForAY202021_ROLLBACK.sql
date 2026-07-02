BEGIN TRAN T1;

DECLARE @psgFundingStreamId int

SELECT @psgFundingStreamId = Id
FROM [dbo].[FundingStreams]
WHERE [FundingStreamCode] = 'PSG'

DELETE FROM [dbo].[NextPayments]
      WHERE [FundingStreamId] = @psgFundingStreamId
        AND [FundingPeriodCode] = 'AY-2021'

COMMIT TRAN T1;