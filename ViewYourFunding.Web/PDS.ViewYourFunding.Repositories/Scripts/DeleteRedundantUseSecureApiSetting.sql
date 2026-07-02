BEGIN TRAN T1;
-- Delete the Use Secure Funding APi from the database
DELETE FROM [GlobalSettings]
WHERE Type = 1 
AND Description = 'Use secure OAuth View Your Funding API?'

COMMIT TRAN T1;