BEGIN TRAN T1;

DECLARE
    @now DATETIME = GETDATE(),
    @migrationUser nvarchar(128) = 'Migration',
    @fundingPeriod nvarchar(max) = 'AY-2021',
    @nmssPaymentTypeId int,
    @academyPaymentTypeId int,
    @msPaymentTypeId int,
    @psgFundingStreamId int

SELECT @nmssPaymentTypeId = Id
FROM [dbo].[NextPaymentTypes]
WHERE [TypeCode] = 'NMSS'

SELECT @academyPaymentTypeId = Id
FROM [dbo].[NextPaymentTypes]
WHERE [TypeCode] = 'AD'

SELECT @msPaymentTypeId = Id
FROM [dbo].[NextPaymentTypes]
WHERE [TypeCode] = 'MS'

SELECT @psgFundingStreamId = Id
FROM [dbo].[FundingStreams]
WHERE [FundingStreamCode] = 'PSG'

-- Maintained schools, 30 Oct 2020
INSERT INTO [dbo].[NextPayments] ([NextPaymentDate], [NextPaymentTypeId], [Active], [FundingStreamId], [CreatedAt], [LastUpdatedAt], [LastUpdatedBy], [FundingPeriodCode])
     VALUES ('2020-10-30', @msPaymentTypeId, 1, @psgFundingStreamId, @now, @now, @migrationUser, @fundingPeriod)

-- Maintained schools, 26 Feb 2021
INSERT INTO [dbo].[NextPayments] ([NextPaymentDate], [NextPaymentTypeId], [Active], [FundingStreamId], [CreatedAt], [LastUpdatedAt], [LastUpdatedBy], [FundingPeriodCode])
     VALUES ('2021-02-26', @msPaymentTypeId, 1, @psgFundingStreamId, @now, @now, @migrationUser, @fundingPeriod)

-- Maintained schools, 30 Apr 2021
INSERT INTO [dbo].[NextPayments] ([NextPaymentDate], [NextPaymentTypeId], [Active], [FundingStreamId], [CreatedAt], [LastUpdatedAt], [LastUpdatedBy], [FundingPeriodCode])
     VALUES ('2021-04-30', @msPaymentTypeId, 1, @psgFundingStreamId, @now, @now, @migrationUser, @fundingPeriod)

-- Academies, 2 Nov 2020
INSERT INTO [dbo].[NextPayments] ([NextPaymentDate], [NextPaymentTypeId], [Active], [FundingStreamId], [CreatedAt], [LastUpdatedAt], [LastUpdatedBy], [FundingPeriodCode])
     VALUES ('2020-11-02', @academyPaymentTypeId, 1, @psgFundingStreamId, @now, @now, @migrationUser, @fundingPeriod)

-- Academies, 1 March 2021
INSERT INTO [dbo].[NextPayments] ([NextPaymentDate], [NextPaymentTypeId], [Active], [FundingStreamId], [CreatedAt], [LastUpdatedAt], [LastUpdatedBy], [FundingPeriodCode])
     VALUES ('2021-03-01', @academyPaymentTypeId, 1, @psgFundingStreamId, @now, @now, @migrationUser, @fundingPeriod)

-- Academies, 4 May 2021
INSERT INTO [dbo].[NextPayments] ([NextPaymentDate], [NextPaymentTypeId], [Active], [FundingStreamId], [CreatedAt], [LastUpdatedAt], [LastUpdatedBy], [FundingPeriodCode])
     VALUES ('2021-05-04', @academyPaymentTypeId, 1, @psgFundingStreamId, @now, @now, @migrationUser, @fundingPeriod)

-- Non-maintained schools, 2 November 2020
INSERT INTO [dbo].[NextPayments] ([NextPaymentDate], [NextPaymentTypeId], [Active], [FundingStreamId], [CreatedAt], [LastUpdatedAt], [LastUpdatedBy], [FundingPeriodCode])
     VALUES ('2020-11-02', @nmssPaymentTypeId, 1, @psgFundingStreamId, @now, @now, @migrationUser, @fundingPeriod)

-- Non-maintained schools, 4 May 2021
INSERT INTO [dbo].[NextPayments] ([NextPaymentDate], [NextPaymentTypeId], [Active], [FundingStreamId], [CreatedAt], [LastUpdatedAt], [LastUpdatedBy], [FundingPeriodCode])
     VALUES ('2021-05-04', @nmssPaymentTypeId, 1, @psgFundingStreamId, @now, @now, @migrationUser, @fundingPeriod)

COMMIT TRAN T1;