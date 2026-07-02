

BEGIN TRAN T1;

update GlobalSettings
set Description = 'URL for logged-in MAT view'
where Description = 'Url for logged-in MAT view'

COMMIT TRAN T1;