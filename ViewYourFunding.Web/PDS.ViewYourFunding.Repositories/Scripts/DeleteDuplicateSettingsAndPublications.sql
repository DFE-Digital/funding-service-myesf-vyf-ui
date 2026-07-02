--BEGIN TRANSACTION

--DECLARE @Counts TABLE (FundingStream NVARCHAR(max), Occurances int)
--DECLARE @Dupes TABLE (FundingStream NVARCHAR(max), Id int)

--INSERT INTO @Counts  
--SELECT FundingStreamCode, COUNT(*) FROM FundingStreams GROUP BY FundingStreamCode HAVING COUNT(*) > 1

--INSERT INTO @Dupes 
--SELECT FundingStreamCode, Id FROM FundingStreams WHERE (FundingStreamCode IN (SELECT FundingStream FROM @Counts))

--DELETE FROM @Dupes WHERE Id IN (
-- SELECT MIN(Id)
--        FROM @Dupes
--        GROUP BY FundingStream)

--SELECT * FROM @Dupes

--DELETE Publications WHERE FundingStreamId IN (SELECT Id FROM @Dupes)
--DELETE NextPayments WHERE NextPaymentTypeId IN (SELECT Id FROM NextPaymentTypes WHERE FundingStreamId in (SELECT Id FROM @Dupes))
--DELETE NextPaymentTypes WHERE FundingStreamId IN (SELECT Id FROM @Dupes)
--DELETE SettingValues WHERE FundingStreamId IN (SELECT Id FROM @Dupes)
--DELETE FundingStreams WHERE Id IN (SELECT Id FROM @Dupes)

--COMMIT TRANSACTION