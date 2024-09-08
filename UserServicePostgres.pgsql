
CREATE TABLE "Users" (
    userId SERIAL PRIMARY KEY,
    nickname VARCHAR(255) NOT NULL,
    hashedpassword VARCHAR(255) NOT NULL,
    salt VARCHAR(255) NOT NULL,
    firstname VARCHAR(255),
    lastname VARCHAR(255),
    birthday DATE,
    profiledescription TEXT,
    imgurl VARCHAR(255),
    email VARCHAR(255) NOT NULL,
    status BOOLEAN NOT NULL DEFAULT FALSE
    );

CREATE TABLE "EmailVerification" (
    emailId SERIAL PRIMARY KEY,
    userId INTEGER NOT NULL REFERENCES "Users"(userId),
    email VARCHAR(2595) NOT NULL,
    verificationToken VARCHAR(255),
    tokenCreationDate TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);



ALTER TABLE "EmailVerification" add isUsed BOOLEAN NOT NULL DEFAULT FALSE;

ALTER TABLE "EmailVerification" add tokenType int not null;


ALTER TABLE "EmailVerification"
ALTER COLUMN email TYPE VARCHAR(254) ;
ALTER TABLE "EmailVerification"
ALTER COLUMN verificationToken TYPE VARCHAR(36);


ALTER TABLE "Users"
ALTER COLUMN email TYPE VARCHAR(254) ;
ALTER TABLE "Users"
ALTER COLUMN salt TYPE VARCHAR(32) ;
ALTER TABLE "Users"
ALTER COLUMN nickname TYPE VARCHAR(32) ;
ALTER TABLE "Users"
ALTER COLUMN firstname TYPE VARCHAR(32) ;
ALTER TABLE "Users"
ALTER COLUMN lastname TYPE VARCHAR(32) ;
ALTER TABLE "Users"
ALTER COLUMN hashedpassword TYPE VARCHAR(64) ;


CREATE INDEX idx_emailverification_userid ON "EmailVerification" (userId);
CREATE INDEX idx_emailverification_email ON "EmailVerification" (email);

SELECT * 
FROM pg_indexes 
WHERE tablename = 'EmailVerification';


CREATE INDEX idx_users_email ON "Users" (email);
CREATE INDEX idx_users_nickname ON "Users" (nickname);

SELECT * 
FROM pg_indexes 
WHERE tablename = 'Users';


SELECT * FROM "Users"

DELETE FROM "EmailVerification"
USING "Users"
WHERE "EmailVerification".userId = "Users".userId;


DELETE FROM "Users";