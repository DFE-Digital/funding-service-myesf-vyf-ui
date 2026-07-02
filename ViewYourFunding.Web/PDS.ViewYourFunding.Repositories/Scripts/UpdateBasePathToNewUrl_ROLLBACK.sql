BEGIN TRAN T1;

update GlobalSettings
set Value = '/single-funding-statement/latest/admin/home'
where Type = 1

update GlobalSettings
set Value = '/single-funding-statement/latest/pre-16-16-19-statements'
where Type = 3


update GlobalSettings
set Value = '/single-funding-statement/latest/start'
where Type = 5


update GlobalSettings
set Value = '/single-funding-statement/latest/pre-16-16-19-statements/parent'
where Type = 7

COMMIT TRAN T1;