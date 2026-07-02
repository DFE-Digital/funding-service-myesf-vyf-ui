BEGIN TRAN T1;

update GlobalSettings
set Value = '/single-funding-statement/latest/start'
where Value = 'https://myesf.local:44316/single-funding-statement/latest/start'

update GlobalSettings
set Value = '/single-funding-statement/latest/admin/home'
where Value = 'https://myesf.local:44316/single-funding-statement/latest/admin/home'

COMMIT TRAN T1;