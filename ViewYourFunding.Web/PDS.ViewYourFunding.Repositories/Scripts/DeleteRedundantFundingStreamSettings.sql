BEGIN TRAN T1;
-- Delete the Setting Values from the [SettingValues] table associated with these Ids
DELETE FROM [SettingValues]
WHERE SettingId in (SELECT Id FROM [Settings] 
WHERE SettingName in ('CutOffDate', 'NextAllocationPaymentDate', 'NextPaymentDateMaintained', 'NextPaymentDateAcademies', 'NextPaymentDateNMSS'));

-- Delete the Settings from the [Settings] table
DELETE FROM [Settings] 
WHERE SettingName in ('CutOffDate', 'NextAllocationPaymentDate', 'NextPaymentDateMaintained', 'NextPaymentDateAcademies', 'NextPaymentDateNMSS');

COMMIT TRAN T1;