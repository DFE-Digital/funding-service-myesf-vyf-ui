

BEGIN TRAN T1;

update GlobalSettings
set Description = 'Url for logged-in MAT view'
where Description = 'URL for logged-in MAT view'

COMMIT TRAN T1;