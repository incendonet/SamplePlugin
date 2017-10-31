DROP TABLE tblCustomers;

CREATE TABLE tblCustomers
(
    CustKey        SERIAL UNIQUE PRIMARY KEY,
    CustID         text NOT NULL DEFAULT '',
    CustType       integer DEFAULT 0,
    SSN            text NOT NULL DEFAULT '',
    NameFirst      text NOT NULL DEFAULT '',
    NameMiddle     text NOT NULL DEFAULT '',
    NameLast       text NOT NULL DEFAULT '',
	NamePrefix     text NOT NULL DEFAULT '',
	NameSuffix     text NOT NULL DEFAULT '',
	HomeStreet     text NOT NULL DEFAULT '',
	HomeState      text NOT NULL DEFAULT '',
	HomeZip        text NOT NULL DEFAULT '',
	PhoneHome      text NOT NULL DEFAULT '',
	PhoneMobile    text NOT NULL DEFAULT '',
	PhoneWork      text NOT NULL DEFAULT '',
	PhoneWorkExt   text NOT NULL DEFAULT '',
	Balance        money NOT NULL DEFAULT 0,
	DueDate        date NOT NULL DEFAULT CURRENT_DATE,
	CardType       integer DEFAULT 0,
	CardNumber     text NOT NULL DEFAULT '',
	CardExp        text NOT NULL DEFAULT '',
	CardCvv        text NOT NULL DEFAULT ''
);

INSERT INTO tblCustomers (NameFirst, PhoneWorkExt, Balance, DueDate) VALUES ('Bryan', '1003', '111.11', '1999/1/1');