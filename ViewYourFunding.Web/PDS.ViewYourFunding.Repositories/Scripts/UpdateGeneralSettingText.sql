

BEGIN TRAN T1;

update GlobalSettings
set Description = 'URL for logged-in provider view'
where Description = 'Url for logged-in provider view'

COMMIT TRAN T1;