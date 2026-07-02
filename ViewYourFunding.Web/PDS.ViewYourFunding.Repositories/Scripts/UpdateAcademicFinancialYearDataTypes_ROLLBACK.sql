BEGIN TRAN T1;

update Settings
set ValueDataType = '1'
where SettingName in ('AcademicYear', 'FinancialYear')

COMMIT TRAN T1;