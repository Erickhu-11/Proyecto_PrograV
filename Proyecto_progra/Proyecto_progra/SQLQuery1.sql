CREATE TABLE USUARIO(
IDUSUARIO INT PRIMARY KEY IDENTITY(1,1),
CORREO VARCHAR(100),
CLAVE VARCHAR(500)
)

CREATE PROC SP_REGISTRARUSUARIO(
@CORREO VARCHAR(100),
@CLAVE VARCHAR(500),
@REGISTRADO BIT OUTPUT,
@MENSAJE VARCHAR(100) OUTPUT
)
AS
	BEGIN
		IF(not exists(SELECT * FROM USUARIO WHERE CORREO = @CORREO))
		BEGIN
			INSERT INTO USUARIO(CORREO,CLAVE) VALUES (@CORREO, @CLAVE)
			SET @REGISTRADO = 1
			SET @MENSAJE = 'USUARIO REGISTRADO'
		END
		ELSE
		BEGIN
			SET @REGISTRADO = 0
			SET @MENSAJE = 'CORREO YA EXISTE'
		END
END

ALTER PROC SP_VALIDARUSUARIO(
@CORREO VARCHAR(100),
@CLAVE VARCHAR(500)
)
AS
	BEGIN
		IF(exists(SELECT * FROM USUARIO WHERE CORREO = @CORREO AND CLAVE = @CLAVE))
			SELECT IDUSUARIO FROM USUARIO WHERE CORREO = @CORREO AND CLAVE = @CLAVE
		ELSE
			SELECT '0'
	END

DECLARE @REGISTRADO BIT, @MENSAJE VARCHAR(100)

EXEC SP_REGISTRARUSUARIO 'ADMINCONCIERTO@GMAIL.COM', 'TEST123', @REGISTRADO OUTPUT, @MENSAJE OUTPUT

SELECT @REGISTRADO
SELECT @MENSAJE

SELECT * FROM USUARIO

EXEC SP_VALIDARUSUARIO 'ADMINCONCIERTO@GMAIL.COM', '56a7010456b474aeee111f3b7336581fb0a99129d426cf51903efbdfd629f008'