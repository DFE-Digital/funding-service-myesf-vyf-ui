BEGIN TRAN T1;

update GlobalSettings
set Value = '/single-funding-statement/latest/logged-in-provider-statement'
where Type = 3

COMMIT TRAN T1;