BEGIN TRAN T1;

update GlobalSettings
set Value = '/single-funding-statement/latest/pre-16-16-19-statements'
where Type = 3

COMMIT TRAN T1;