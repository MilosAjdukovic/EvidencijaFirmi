CREATE DATABASE EvidencijaFirmi
GO

USE EvidencijaFirmi
GO

CREATE TABLE KORISNIK(
    JMBG nvarchar(13) PRIMARY KEY,
    Ime nvarchar(20) NOT NULL,
    Prezime nvarchar(40) NOT NULL,
    Email nvarchar(40) NOT NULL,
    Lozinka nvarchar(20) NOT NULL,
    TipKorisnika nvarchar(20) NOT NULL
)
GO

CREATE TABLE OBLAST(
    IDOblasti int IDENTITY(1,1) PRIMARY KEY,
    ImeOblasti nvarchar(30) NOT NULL
)
GO

CREATE TABLE StatusPrijave(
    IDStatusa int IDENTITY(1,1) PRIMARY KEY,
    Opis nvarchar(20) NOT NULL
)
GO

CREATE TABLE StatusFirme(
    IDStatusa int IDENTITY(1,1) PRIMARY KEY,
    Opis nvarchar(20) NOT NULL
)
GO

CREATE TABLE PRIJAVA(
    IDPrijave int IDENTITY(1,1) PRIMARY KEY,
    JMBGKorisnika nvarchar(13) NOT NULL,
    NazivFirme nvarchar(100) NOT NULL,
    IDOblasti int NOT NULL,
    StatusPrijaveID int NOT NULL,
    CONSTRAINT FK_PRIJAVA_KORISNIK FOREIGN KEY(JMBGKorisnika) REFERENCES KORISNIK(JMBG)
        ON UPDATE CASCADE
        ON DELETE CASCADE,
    CONSTRAINT FK_PRIJAVA_OBLAST FOREIGN KEY(IDOblasti) REFERENCES OBLAST(IDOblasti)
        ON UPDATE CASCADE
        ON DELETE CASCADE,
    CONSTRAINT FK_PRIJAVA_STATUS FOREIGN KEY(StatusPrijaveID) REFERENCES StatusPrijave(IDStatusa)
        ON UPDATE CASCADE
        ON DELETE CASCADE
)
GO

CREATE TABLE FIRMA(
    IDFirme int IDENTITY(1,1) PRIMARY KEY,
    PIBBroj nvarchar(9) NOT NULL,
    Naziv nvarchar(100) NOT NULL,
    IDOblasti int NOT NULL,
    JMBGKorisnika nvarchar(13) NOT NULL,
    StatusFirmeID int NOT NULL,
    CONSTRAINT FK_FIRMA_OBLAST FOREIGN KEY(IDOblasti) REFERENCES OBLAST(IDOblasti)
        ON UPDATE CASCADE
        ON DELETE CASCADE,
    CONSTRAINT FK_FIRMA_KORISNIK FOREIGN KEY(JMBGKorisnika) REFERENCES KORISNIK(JMBG)
        ON UPDATE CASCADE
        ON DELETE CASCADE,
    CONSTRAINT FK_FIRMA_STATUS FOREIGN KEY(StatusFirmeID) REFERENCES StatusFirme(IDStatusa)
        ON UPDATE CASCADE
        ON DELETE CASCADE
)
GO

USE EvidencijaFirmi
GO

INSERT INTO StatusPrijave (Opis) VALUES ('Na čekanju');
INSERT INTO StatusPrijave (Opis) VALUES ('Odbijena');
INSERT INTO StatusPrijave (Opis) VALUES ('Odobrena');
GO

INSERT INTO StatusFirme (Opis) VALUES ('Aktivna');
INSERT INTO StatusFirme (Opis) VALUES ('Neaktivna');
INSERT INTO StatusFirme (Opis) VALUES ('Ugašena');
INSERT INTO StatusFirme (Opis) VALUES ('U stečaju');
GO

INSERT INTO OBLAST (ImeOblasti) VALUES ('IT');
INSERT INTO OBLAST (ImeOblasti) VALUES ('Građevinarstvo');
INSERT INTO OBLAST (ImeOblasti) VALUES ('Poljoprivreda');
INSERT INTO OBLAST (ImeOblasti) VALUES ('Turizam');
INSERT INTO OBLAST (ImeOblasti) VALUES ('Finansije');
GO

CREATE VIEW FirmaView AS
SELECT 
    f.IDFirme,
    f.PIBBroj,
    f.Naziv AS NazivFirme,
    f.IDOblasti AS IDOblasti,
    o.ImeOblasti AS OblastDelatnosti,
    f.JMBGKorisnika AS JMBGKorisnika,
    k.Ime AS ImeKorisnika,
    k.Prezime AS PrezimeKorisnika,
    f.StatusFirmeID AS StatusFirmeID,
    sf.Opis AS StatusFirme
FROM 
    FIRMA f
JOIN 
    OBLAST o ON f.IDOblasti = o.IDOblasti
JOIN 
    KORISNIK k ON f.JMBGKorisnika = k.JMBG
JOIN 
    StatusFirme sf ON f.StatusFirmeID = sf.IDStatusa
GO

CREATE VIEW PrijavaView AS
SELECT 
    p.IDPrijave,
    p.NazivFirme AS NazivFirmePrijave,
    p.IDOblasti AS IDOblasti,
    o.ImeOblasti AS OblastDelatnosti,
    p.JMBGKorisnika AS JMBGKorisnika,
    k.Ime AS ImeKorisnika,
    k.Prezime AS PrezimeKorisnika,
    p.StatusPrijaveID AS StatusPrijaveID,
    sp.Opis AS StatusPrijave
FROM 
    PRIJAVA p
JOIN 
    OBLAST o ON p.IDOblasti = o.IDOblasti
JOIN 
    KORISNIK k ON p.JMBGKorisnika = k.JMBG
JOIN 
    StatusPrijave sp ON p.StatusPrijaveID = sp.IDStatusa
GO

CREATE PROCEDURE [DajSveFirme]
AS
select * from FirmaView
GO

CREATE PROCEDURE IzmeniFirmu
    @IDFirme INT,
    @NoviNaziv NVARCHAR(100),
    @NoviIDOblasti INT
AS
    UPDATE FIRMA
    SET Naziv = @NoviNaziv,
        IDOblasti = @NoviIDOblasti
    WHERE IDFirme = @IDFirme;
GO

CREATE PROCEDURE ObrisiFirmu
	@IDFirme INT
AS
	DELETE from FIRMA where IDFirme = @IDFirme;
GO

CREATE PROCEDURE [NoviKorisnik]
( 
@JMBG [nvarchar](13),
@Ime [nvarchar](20),
@Prezime [nvarchar](40),
@Email [nvarchar](20),
@Lozinka [nvarchar](20)
)
AS
BEGIN
Insert into KORISNIK(JMBG, Ime, Prezime, Email, Lozinka, TipKorisnika) values (@JMBG, @Ime, @Prezime, @Email, @Lozinka, 'obican_korisnik')
END
GO

CREATE PROCEDURE [ObrisiKorisnika](
@JMBG [nvarchar](13))
AS
BEGIN
Delete from KORISNIK where JMBG=@JMBG
END
GO

CREATE PROCEDURE [IzmeniKorisnika](
@StariJMBG [nvarchar](13),
@JMBG [nvarchar](13),
@Ime [nvarchar](20),
@Prezime [nvarchar](40),
@Email [nvarchar](20),
@Lozinka [nvarchar](20)
)
AS
BEGIN
Update KORISNIK set JMBG=@JMBG, Ime=@Ime, Prezime=@Prezime, Email=@Email, Lozinka=@Lozinka where JMBG=@StariJMBG
END
GO

CREATE PROCEDURE [DajSveKorisnike]
AS
select * from KORISNIK
GO

CREATE PROCEDURE [DajKorisnikaPoPrezimenu]
( @Prezime [nvarchar](40)
)
AS
SELECT * FROM KORISNIK WHERE Prezime LIKE '%' + @Prezime + '%';
GO

CREATE PROCEDURE [DajKorisnikaPoJMBG]
( @JMBG [nvarchar](40)
)
AS
SELECT * FROM KORISNIK WHERE JMBG=@JMBG;
GO

CREATE PROCEDURE [dbo].[PronadjiKorisnikaPoEmailu]
    @Email NVARCHAR(50)
AS
    SELECT * FROM KORISNIK WHERE Email = @Email;
GO

CREATE PROCEDURE [DajSveOblasti]
AS
select * from OBLAST
GO

CREATE PROCEDURE [DajSvePrijave]
AS
select * from PrijavaView
GO

CREATE PROCEDURE [DajSvePrijavePoKorisniku]
(
@JMBGKorisnika [nvarchar](13)
)
AS
select * from PrijavaView where JMBGKorisnika=@JMBGKorisnika
GO

CREATE PROCEDURE [OdbijPrijavu]
(
@IDPrijave [int]
)
AS
BEGIN
Update PRIJAVA set StatusPrijaveID=2 where IDPrijave=@IDPrijave
END
GO

CREATE PROCEDURE OdobriPrijavuIKreirajFirmu
    @IDPrijave INT,
    @PIBBroj NVARCHAR(9)
AS
BEGIN
    BEGIN TRANSACTION;

    BEGIN TRY
        UPDATE PRIJAVA
        SET StatusPrijaveID = 3
        WHERE IDPrijave = @IDPrijave;

        DECLARE @NazivFirme NVARCHAR(100);
        DECLARE @IDOblasti INT;
        DECLARE @JMBGKorisnika NVARCHAR(13);

        SELECT 
            @NazivFirme = NazivFirme,
            @IDOblasti = IDOblasti,
            @JMBGKorisnika = JMBGKorisnika
        FROM PRIJAVA
        WHERE IDPrijave = @IDPrijave;

        INSERT INTO FIRMA (PIBBroj, Naziv, IDOblasti, JMBGKorisnika, StatusFirmeID)
        VALUES (@PIBBroj, @NazivFirme, @IDOblasti, @JMBGKorisnika, 1);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO


INSERT Into KORISNIK (JMBG, Ime, Prezime, Email, Lozinka, TipKorisnika) values ('1111111111111', 'admin', 'admin', 'admin@admin.com', 'lozinka', 'admin');
GO
CREATE PROCEDURE [NovaPrijava]
( 
@JMBGKorisnika [nvarchar](13),
@NazivFirme [nvarchar](100),
@IDOblasti INT
)
AS
BEGIN
Insert into Prijava(JMBGKorisnika, NazivFirme, IDOblasti, StatusPrijaveID) values (@JMBGKorisnika, @NazivFirme, @IDOblasti, 1)
END
GO

CREATE PROCEDURE [DajFirmuPoKorisniku]
( @JMBGKorisnika [nvarchar](40)
)
AS
SELECT * FROM FirmaView WHERE JMBGKorisnika=@JMBGKorisnika;
GO