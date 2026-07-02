BEGIN TRAN T1;

update GlobalSettings
set Value = '/view-latest-funding/admin/home'
where Type = 1

update GlobalSettings
set Value = '/view-latest-funding/pre-16-16-19-statements'
where Type = 3


update GlobalSettings
set Value = '/view-latest-funding'
where Type = 5


update GlobalSettings
set Value = '/view-latest-funding/pre-16-16-19-statements/parent'
where Type = 7


COMMIT TRAN T1;