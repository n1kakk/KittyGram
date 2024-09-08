CREATE TABLE "User"(
    Nickname varchar(32) PRIMARY KEY,
    LastActive timestamp
)


CREATE TABLE "Chat" (
    ChatId SERIAL PRIMARY KEY,
    ChatName VARCHAR(32) NOT NULL,
    ChatType INT NOT NULL,
    Created TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    Deleted INT DEFAULT 0,
    Updated INT DEFAULT 0
);

CREATE TABLE "Message" (
    MessageId UUID PRIMARY KEY,
    ChatId INT,
    SenderNickname VARCHAR(32) NOT NULL,
    MessageType INT NOT NULL,
    MessageContent TEXT,
    Created TIMESTAMP NOT NULL,
    Deleted INT DEFAULT 0,
    Updated INT DEFAULT 0,
    Status INT,
    FOREIGN KEY (ChatId) REFERENCES "Chat"(ChatId),
    FOREIGN KEY (SenderNickname) REFERENCES "User"(nickname)
);

CREATE TABLE "UserChat" (
    ChatId INT,
    SenderNickname VARCHAR(32),
    UserRole INT,
    FOREIGN KEY (ChatId) REFERENCES "Chat"(ChatId),
    FOREIGN KEY (SenderNickname) REFERENCES "User"(Nickname)
);


INSERT INTO "User" (Nickname)
VALUES 
    ('nika')

INSERT INTO "Chat" (ChatName)
VALUES 
    ('Public Chat'),
