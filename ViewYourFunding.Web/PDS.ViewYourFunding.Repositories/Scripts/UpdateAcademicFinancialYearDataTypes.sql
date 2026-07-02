BEGIN TRAN T1;

update Settings
set ValueDataType = '0'
where SettingName in ('AcademicYear', 'FinancialYear')

COMMIT TRAN T1;