

BEGIN TRAN T1;

update GlobalSettings
set Description = 'Url for logged-in provider view'
where Description = 'URL for logged-in provider view'

COMMIT TRAN T1;